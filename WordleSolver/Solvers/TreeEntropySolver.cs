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
						nodes.Add(patternGroup.Key, new Node()
						{
							Guess = patternGroup.Value[0],
							Steps = steps + 1,
							IsLeaf = false
						});
						nodes[patternGroup.Key].Nodes.Add(_patternsProvider.GetPattern(guess, patternGroup.Value[0]), new Node(steps + 1));
					}
				}
				else if (patternGroup.Value.Count == 2)
				{
					nodes.Add(patternGroup.Key, new Node()
					{
						Guess = patternGroup.Value[0],
						Steps = steps + 1,
						IsLeaf = false
					});
					nodes[patternGroup.Key].Nodes.Add("CCCCC", new Node(steps + 1));
					nodes[patternGroup.Key].Nodes.Add(_patternsProvider.GetPattern(patternGroup.Value[0], patternGroup.Value[1]), new Node()
					{
						Guess = patternGroup.Value[1],
						Steps = steps + 2,
						IsLeaf = false,
					});
					nodes[patternGroup.Key].Nodes[_patternsProvider.GetPattern(patternGroup.Value[0], patternGroup.Value[1])].Nodes.Add("CCCCC", new Node(steps + 2));
				}
				else
				{
					var entropies = new Dictionary<string, double>();
					foreach (var word in _words)
					{
						var patternGroups = new Dictionary<string, List<string>>();
						foreach (var possibleWord in sortedWords[patternGroup.Key])
						{
							var pattern = _patternsProvider.GetPattern(word, possibleWord);
							if (!patternGroups.TryAdd(pattern, [possibleWord])) patternGroups[pattern].Add(possibleWord);
						}

						var entropy = patternGroups.Sum(pattern => pattern.Value.Count * Math.Log2(1d / pattern.Value.Count));

						entropies.Add(word, entropy);
					}
					var bestGuess = entropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;
					var bestNode = GetSubTree(bestGuess, sortedWords[patternGroup.Key], steps + 1);

					nodes.Add(patternGroup.Key, bestNode);
				}
			}

			var node = new Node()
			{
				Guess = guess,
				Nodes = nodes,
				Steps = steps,
				IsLeaf = false
			};

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

			if (!(pattern == "CCCCC")) _node = _node?.Nodes[pattern];
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
		public string Guess { get; set; } = "";
		public Dictionary<string, Node> Nodes { get; set; } = [];
		public int Steps { get; set; } = 0;
		public bool IsLeaf { get; set; } = false;

		public Node() { }

		public Node(int steps)
		{
			Steps = steps;
			IsLeaf = true;
		}
	}
}
