using WordleSolver.Models;

namespace WordleSolver.Interfaces
{
	internal interface IPatternsProvider
	{
		Word GetPattern(Word guess, Word wordle);
	}
}
