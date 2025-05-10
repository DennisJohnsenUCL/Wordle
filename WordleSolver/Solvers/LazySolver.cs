using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Solvers
{
	internal class LazySolver : ISolver
	{
		protected virtual Word[] Words { get; }
		protected int Index { get; set; } = 0;
		public virtual string Identifier { get; }

		public LazySolver(Word[] words, string identifier)
		{
			Words = words;
			Identifier = identifier;
		}

		public virtual Word GetFirstGuess()
		{
			return GetNextGuess();
		}

		public virtual Word GetNextGuess()
		{
			var guess = Words[Index];
			Index++;
			return guess;
		}

		public virtual void Reset()
		{
			Index = 0;
		}
	}
}
