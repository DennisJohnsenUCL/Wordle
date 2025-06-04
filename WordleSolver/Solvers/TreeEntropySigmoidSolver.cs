using System.Collections.Concurrent;
using WordleSolver.Interfaces;
using WordleSolver.Models;
using WordleSolver.Services;

namespace WordleSolver.Solvers
{
	internal class TreeEntropySigmoidSolver : ISolver, IReactiveSolver
	{
		public string Identifier { get; }
		private readonly IPatternsProvider _patternsProvider;
		private readonly string _firstGuess;
		private readonly Dictionary<string, double> _words;
		private readonly Lazy<Node> _root;
		private Node? _node;

		public TreeEntropySigmoidSolver(SolverContext context, string identifier)
		{
			Identifier = identifier;
			_patternsProvider = context.PatternsProvider;
			_firstGuess = context.FirstGuessProvider.Value;
			_words = context.WordFrequenciesSigmoid;
			_root = new(() => BuildTree());
		}

		private Node BuildTree()
		{
			var root = GetSubTree(_firstGuess, _words, 1);
			return root;
		}

		private Node GetSubTree(string guess, Dictionary<string, double> possibleWords, int steps)
		{
			var sortedWords = new Dictionary<string, Dictionary<string, double>>();
			foreach (var possibleWord in possibleWords)
			{
				var pattern = _patternsProvider.GetPattern(guess, possibleWord.Key);
				if (!sortedWords.TryAdd(pattern, new() { { possibleWord.Key, possibleWord.Value } })) sortedWords[pattern].Add(possibleWord.Key, possibleWord.Value);
			}

			var nodes = new Dictionary<string, Node>();
			foreach (var kvp in sortedWords)
			{
				var pattern = kvp.Key;
				var group = kvp.Value.Keys.ToList();
				var count = group.Count;
				var totalFreq = kvp.Value.Values.Sum();
				var normalized = kvp.Value.ToDictionary(x => x.Key, x => x.Value / totalFreq);

				if (pattern == "CCCCC") nodes.Add(pattern, new Node(steps));
				else if (count < 3) nodes.Add(pattern, GetSubTree(group[0], normalized, steps + 1));
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
					if (splitter != null) nodes.Add(pattern, GetSubTree(splitter, normalized, steps + 1));
					else nodes.Add(pattern, GetSubTree(group[0], normalized, steps + 1));
				}
				else if (normalized[group[0]] > 0.85) nodes.Add(pattern, GetSubTree(group[0], normalized, steps + 1));
				else
				{
					var entropies = new Dictionary<string, double>();

					foreach (var word in _words.Keys)
					{
						var patternGroups = new Dictionary<string, double>();
						foreach (var possibleWord in group)
						{
							var patternKey = _patternsProvider.GetPattern(word, possibleWord);
							if (!patternGroups.TryAdd(patternKey, normalized[possibleWord])) patternGroups[patternKey] += normalized[possibleWord];
						}

						var entropy = patternGroups.Sum(pattern =>
						{
							var probability = pattern.Value;
							return probability * Math.Log2(1 / probability);
						});

						if (group.Contains(word)) entropy += normalized[word] / 3;

						entropies.Add(word, entropy);
					}

					int tries = steps < 3 ? 16 : 9;

					var bestEntropies = entropies.OrderByDescending(x => x.Value).Take(tries).ToDictionary();

					var bestCounts = new ConcurrentDictionary<Node, double>();

					Parallel.ForEach(bestEntropies.Keys, candidate =>
					{
						var possibleNode = GetSubTree(candidate, kvp.Value, steps + 1);
						var possibleCount = CountGuesses(possibleNode);
						var possibleSum = possibleCount.Select((x, i) => x * i).Sum();

						bestCounts.TryAdd(possibleNode, possibleSum);
					});

					var bestNode = bestCounts
						.OrderBy(x => x.Value)
						.ThenByDescending(x => bestEntropies[x.Key.Guess])
						.ThenBy(x => x.Key.Guess)
						.First().Key;

					nodes.Add(pattern, bestNode);
				}
			}

			var node = new Node(guess, nodes, steps);
			return node;
		}

		private double[] CountGuesses(Node node, double[]? count = null)
		{
			count ??= new double[100];

			if (node.Nodes.ContainsKey("CCCCC")) count[node.Steps] += _words[node.Guess];
			foreach (var subNode in node.Nodes)
			{
				CountGuesses(subNode.Value, count);
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
