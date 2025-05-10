using WordleCore.Models;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Solvers
{
	internal class FilteredSolver : LazySolver, IReactiveSolver
	{
		protected IConstraintManager Constraints { get; protected private set; }
		protected Word FirstGuess { get; protected private set; }

		public FilteredSolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, Word[] words, string identifier) : base(words, identifier)
		{
			FirstGuess = firstGuessProvider.Value;
			Constraints = constraintManager;
		}

		public override Word GetFirstGuess()
		{
			return FirstGuess;
		}

		public virtual void AddResponse(WordleResponse response)
		{
			Constraints.AddConstraints(response.LetterResults);
		}

		public override Word GetNextGuess()
		{
			for (int i = Index; i < Words.Length; i++)
			{
				var word = Words[i];

				if (FitsConstraints(word))
				{
					Index = i + 1;
					return word;
				}
			}
			throw new Exception();
		}

		protected virtual bool FitsConstraints(Word word)
		{
			return Constraints.FitsConstraints(word);
		}

		public override void Reset()
		{
			Constraints.Clear();
			base.Reset();
		}
	}
}
