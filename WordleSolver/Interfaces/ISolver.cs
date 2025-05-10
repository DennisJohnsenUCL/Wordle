using WordleSolver.Models;

namespace WordleSolver.Interfaces
{
	internal interface ISolver
	{
		string Identifier { get; }
		Word GetFirstGuess();
		Word GetNextGuess();
		void Reset();
	}
}
