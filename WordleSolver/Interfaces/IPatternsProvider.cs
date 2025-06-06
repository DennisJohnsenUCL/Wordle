﻿namespace WordleSolver.Interfaces
{
	internal interface IPatternsProvider
	{
		string GetPattern(string guess, string wordle);
		int GetWordIndex(string word);
		int GetPatternIndex(string pattern);
		bool FitsPattern(int guessIndex, int wordleIndex, int patternIndex);
	}
}
