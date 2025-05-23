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
			_root = new(BuildTree());
		}

		private Node BuildTree()
		{
			return GetSubTree(_firstGuess, [.. _wordles], 1);
		}

		private Node GetSubTree(string guess, List<string> possibleWords, int steps)
		{
			return new Node();
		}

		private int CountGuesses()
		{
			return 0;
		}

		public void AddResponse(WordleResponse response)
		{
			var pattern = string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
			if (_patternsProvider.Patterns == Patterns.Simple) pattern = pattern.Replace('O', 'A');

			_node = _node?.Nodes[pattern];
		}

		public string GetNextGuess()
		{
			if (_node == null) return _root.Value.Guess;
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
	}
}
