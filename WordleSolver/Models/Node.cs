namespace WordleSolver.Models
{
	internal class Node
	{
		public string Guess { get; set; } = "";
		public Dictionary<string, Node> Nodes { get; set; } = [];
		public int Steps { get; set; } = 0;
		public bool IsLeaf { get; set; } = false;

		public Node(string guess, Dictionary<string, Node> nodes, int steps)
		{
			Guess = guess;
			Nodes = nodes;
			Steps = steps;
		}

		public Node(string guess, int steps)
		{
			Guess = guess;
			Nodes = new() { { "CCCCC", new Node(steps) } };
			Steps = steps;
		}

		public Node(int steps)
		{
			Steps = steps;
			IsLeaf = true;
		}

		public Node() { }
	}
}
