using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Services
{
	internal class StaticFirstGuessProvider : IFirstGuessProvider
	{
		public Word Value { get; }

		public StaticFirstGuessProvider(Word firstGuess)
		{
			Value = firstGuess;
		}
	}
}
