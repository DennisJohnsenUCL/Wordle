using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Enums;
using WordleSolver.Interfaces;
using WordleSolver.Services;

namespace WordleSolver.Solvers
{
	internal class TreeEntropySolver : ISolver, IReactiveSolver
	{
		public string Identifier { get; }
		private readonly IPatternsProvider _patternsProvider;
		private readonly string _firstGuess;
		private readonly string[] _words;
		private readonly string[] _wordles;
		private readonly Lazy<Node> _root;
		private Node? _node;

		public TreeEntropySolver(SolverContext context, string identifier)
		{
			Identifier = identifier;
			_patternsProvider = context.PatternsProvider;
			_firstGuess = context.FirstGuessProvider.Value;
			_words = context.Words;
			_wordles = context.Wordles;
			_root = new(() => BuildTree());
		}

		private Node BuildTree()
		{
			var root = GetSubTree(_firstGuess, [.. _wordles], 1);

			Console.WriteLine(CountGuesses(root));

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
			foreach (var patternGroup in sortedWords)
			{
				if (patternGroup.Value.Count == 1)
				{
					if (patternGroup.Key == "CCCCC")
					{
						nodes.Add(patternGroup.Key, new Node(steps));
					}
					else
					{
						nodes.Add(patternGroup.Key, new Node(patternGroup.Value[0], steps + 1));
					}
				}
				else if (patternGroup.Value.Count == 2)
				{
					nodes.Add(patternGroup.Key, new Node(patternGroup.Value[0], steps + 1));
					nodes[patternGroup.Key].Nodes.Add(_patternsProvider.GetPattern(patternGroup.Value[0], patternGroup.Value[1]), new Node(patternGroup.Value[1], steps + 2));
				}
				else if (patternGroup.Value.Count == 3)
				{
					var words = patternGroup.Value.ToList();
					string? splitter = null;
					foreach (var word1 in words)
					{
						var check = new List<string>();
						foreach (var word2 in words)
						{
							if (word1 == word2) continue;
							var pattern = _patternsProvider.GetPattern(word1, word2);
							check.Add(pattern);
						}
						if (check[0] != check[1]) { splitter = word1; break; }
					}
					if (splitter != null)
					{
						words.Remove(splitter);
						nodes.Add(patternGroup.Key, new Node(splitter, steps + 1));
						nodes[patternGroup.Key].Nodes.Add(_patternsProvider.GetPattern(splitter, words[0]), new Node(words[0], steps + 2));
						nodes[patternGroup.Key].Nodes.Add(_patternsProvider.GetPattern(splitter, words[1]), new Node(words[1], steps + 2));
					}
					else
					{
						nodes.Add(patternGroup.Key, new Node(patternGroup.Value[0], steps + 1));
						nodes[patternGroup.Key].Nodes.Add(_patternsProvider.GetPattern(patternGroup.Value[0], patternGroup.Value[1]), new Node(patternGroup.Value[1], steps + 2));
						nodes[patternGroup.Key].Nodes[_patternsProvider.GetPattern(patternGroup.Value[0], patternGroup.Value[1])].Nodes.Add(_patternsProvider.GetPattern(patternGroup.Value[1], patternGroup.Value[2]), new Node(patternGroup.Value[2], steps + 3));
					}
				}
				else
				{
					var entropies = new Dictionary<string, double>();
					foreach (var word in _words)
					{
						var patternGroups = new Dictionary<string, List<string>>();
						foreach (var possibleWord in patternGroup.Value)
						{
							var pattern = _patternsProvider.GetPattern(word, possibleWord);
							if (!patternGroups.TryAdd(pattern, [possibleWord])) patternGroups[pattern].Add(possibleWord);
						}

						var entropy = patternGroups.Sum(pattern => pattern.Value.Count * Math.Log2(1d / pattern.Value.Count));

						entropies.Add(word, entropy);
					}

					foreach (var entropy in entropies)
					{
						if (patternGroup.Value.Contains(entropy.Key)) entropies[entropy.Key] += 1d / patternGroup.Value.Count;
					}

					int tries = 8;
					if (steps > 2) tries = 1;

					entropies = entropies.OrderByDescending(x => x.Value).ToDictionary();

					Node bestNode = new();
					var bestCount = int.MaxValue;

					foreach (var entropy in entropies)
					{
						var possibleGuess = entropy.Key;
						var possibleNode = GetSubTree(possibleGuess, patternGroup.Value, steps + 1);
						var possibleCount = CountGuesses(possibleNode);

						if (possibleCount < bestCount)
						{
							bestCount = possibleCount;
							bestNode = possibleNode;
						}

						tries -= 1;
						if (tries == 0) break;
					}
					nodes.Add(patternGroup.Key, bestNode);
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

		public void AddResponse(WordleResponse response)
		{
			var pattern = string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
			if (_patternsProvider.Patterns == Patterns.Simple) pattern = pattern.Replace('O', 'A');

			_node = _node?.Nodes[pattern];
		}

		public string GetNextGuess()
		{
			if (_node == null)
			{
				_node = _root.Value;
				return _root.Value.Guess;
			}
			else return _node.Guess;
		}

		public void Reset()
		{
			_node = null;
		}

		private static readonly Dictionary<Correctness, char> CorrectnessMappings = new()
		{
			{ Correctness.Absent, 'A' },
			{ Correctness.Present, 'P' },
			{ Correctness.Correct, 'C' },
			{ Correctness.OverCount, 'O' },
		};
	}

	//>> Move this out
	internal class Node
	{
		public string Guess { get; set; }
		public Dictionary<string, Node> Nodes { get; set; }
		public int Steps { get; set; }
		public bool IsLeaf { get; set; }

		public Node(string guess, Dictionary<string, Node> nodes, int steps)
		{
			Guess = guess;
			Nodes = nodes;
			Steps = steps;
			IsLeaf = false;
		}

		public Node(string guess, int steps)
		{
			Guess = guess;
			Nodes = new() { { "CCCCC", new Node(steps) } };
			Steps = steps;
			IsLeaf = false;
		}

		public Node(int steps)
		{
			Guess = "";
			Nodes = [];
			Steps = steps;
			IsLeaf = true;
		}

		public Node()
		{
			Guess = "";
			Nodes = [];
			Steps = 0;
			IsLeaf = false;
		}
	}
}
