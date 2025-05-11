namespace WordleSolver.Interfaces
{
	internal interface ISolver
	{
		string Identifier { get; }
		string GetNextGuess();
		void Reset();
	}
}
