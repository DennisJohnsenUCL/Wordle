using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class LazySolver : ISolver
	{
		protected virtual string[] Words { get; }
		protected int Index { get; set; } = 0;
		public virtual string Identifier { get; }

		public LazySolver(string[] words, string identifier)
		{
			Words = words;
			Identifier = identifier;
		}

		public virtual string GetFirstGuess()
		{
			return GetNextGuess();
		}

		public virtual string GetNextGuess()
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
