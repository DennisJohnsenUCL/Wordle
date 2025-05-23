using WordleCore.Models;
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
			return new Node();
		}

		private int CountGuesses()
		{
			return 0;
		}

		public void AddResponse(WordleResponse response)
		{

		}

		public string GetNextGuess()
		{
			return "";
		}

		public void Reset()
		{

		}
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
