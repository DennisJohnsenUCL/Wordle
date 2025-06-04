using System.Collections.Concurrent;
using WordleSolver.Interfaces;
using WordleSolver.Models;
using WordleSolver.Services;

namespace WordleSolver.Solvers
{
	internal class TreeEntropySolver : ISolver, IReactiveSolver
	{
		public string Identifier { get; }
		private readonly IPatternsProvider _patternsProvider;
		private readonly string _firstGuess;
		private readonly string[] _words;
		private readonly List<string> _wordles;
		private readonly Lazy<Node> _root;
		private Node? _node;

		public TreeEntropySolver(SolverContext context, string identifier)
		{
			Identifier = identifier;
			_patternsProvider = context.PatternsProvider;
			_firstGuess = context.FirstGuessProvider.Value;
			_words = context.UnsortedWords;
			_wordles = [.. context.Wordles];
			_root = new(() => BuildTree());
		}

		private Node BuildTree()
		{
			var root = GetSubTree(_firstGuess, _wordles, 1);
			return root;
		}

		private Node GetSubTree(string guess, List<string> possibleWords, int steps)
		{
			var sortedWords = new Dictionary<string, List<string>>();
			foreach (var possibleWord in possibleWords)
			{
				var pattern = _patternsProvider.GetPattern(guess, possibleWord);
				if (!sortedWords.TryAdd(pattern, [possibleWord])) sortedWords[pattern].Add(possibleWord);
			}

			var nodes = new Dictionary<string, Node>();
			foreach (var kvp in sortedWords)
			{
				var pattern = kvp.Key;
				var group = kvp.Value;
				var count = group.Count;

				if (pattern == "CCCCC") nodes.Add(pattern, new Node(steps));
				else if (count < 3) nodes.Add(pattern, GetSubTree(group[0], group, steps + 1));
				else if (count == 3)
				{
					var words = group.ToList();
					string? splitter = null;
					foreach (var word1 in words)
					{
						var check = new List<string>();
						foreach (var word2 in words)
						{
							if (word1 == word2) continue;
							check.Add(_patternsProvider.GetPattern(word1, word2));
						}
						if (check[0] != check[1]) { splitter = word1; break; }
					}
					if (splitter != null) nodes.Add(pattern, GetSubTree(splitter, group, steps + 1));
					else nodes.Add(pattern, GetSubTree(group[0], group, steps + 1));
				}
				else
				{
					var entropies = new Dictionary<string, double>();
					foreach (var word in _words)
					{
						var patternGroups = new Dictionary<string, int>();
						foreach (var possibleWord in group)
						{
							var patternKey = _patternsProvider.GetPattern(word, possibleWord);
							if (!patternGroups.TryAdd(patternKey, 1)) patternGroups[patternKey] += 1;
						}

						var total = (double)count;
						var entropy = patternGroups.Sum(pattern =>
						{
							var probability = pattern.Value / total;
							return probability * Math.Log2(1 / probability);
						});

						if (group.Contains(word)) entropy += 1 / total;

						entropies.Add(word, entropy);
					}

					int tries = steps < 3 ? 8 : 1;

					var bestEntropies = entropies.OrderByDescending(x => x.Value).Take(tries).ToDictionary();

					var bestCounts = new ConcurrentDictionary<Node, int>();

					Parallel.ForEach(bestEntropies.Keys, candidate =>
					{
						var possibleNode = GetSubTree(candidate, group, steps + 1);
						var possibleCount = CountGuesses(possibleNode);
						bestCounts.TryAdd(possibleNode, possibleCount);
					});

					var bestNode = bestCounts.OrderBy(x => x.Value).ThenByDescending(x => bestEntropies[x.Key.Guess]).First().Key;

					nodes.Add(pattern, bestNode);
				}
			}

			var node = new Node(guess, nodes, steps);
			return node;
		}

		private static int CountGuesses(Node node)
		{
			var count = 0;

			if (node.IsLeaf == true) count += node.Steps;
			else
			{
				foreach (var subNode in node.Nodes)
				{
					count += CountGuesses(subNode.Value);
				}
			}
			return count;
		}

		public void AddResponse(string pattern)
		{
			_node = _node?.Nodes[pattern];
		}

		public string GetNextGuess()
		{
			_node ??= _root.Value;
			return _node.Guess;
		}

		public void Reset()
		{
			_node = _root.Value;
		}
	}
}
