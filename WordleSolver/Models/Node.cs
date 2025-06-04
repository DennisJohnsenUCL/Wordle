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

		public Node(int steps)
		{
			Steps = steps;
			IsLeaf = true;
		}
	}
}
