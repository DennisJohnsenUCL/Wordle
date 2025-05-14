using WordleSolver.Enums;

namespace WordleSolver.Interfaces
{
	internal interface IPatternsProvider
	{
		Patterns Patterns { get; }
		string GetPattern(string guess, string wordle);
		int GetWordIndex(string word);
		int GetPatternIndex(string pattern);
		bool FitsPattern(int guessIndex, int wordleIndex, int patternIndex);
	}
}
