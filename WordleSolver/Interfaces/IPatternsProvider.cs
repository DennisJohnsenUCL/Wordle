namespace WordleSolver.Interfaces
{
	internal interface IPatternsProvider
	{
		string GetPattern(string guess, string wordle);
	}
}
