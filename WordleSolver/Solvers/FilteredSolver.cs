using WordleCore.Models;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class FilteredSolver : LazySolver, IReactiveSolver
    {
        protected IConstraintManager Constraints { get; protected private set; }
        protected string FirstGuess { get; protected private set; }

        public FilteredSolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, string[] words, string identifier) : base(words, identifier)
        {
            FirstGuess = firstGuessProvider.Value;
            Constraints = constraintManager;
        }

        public override string GetFirstGuess()
        {
            return FirstGuess;
        }

        public virtual void AddResponse(WordleResponse response)
        {
            Constraints.AddConstraints(response.LetterResults);
        }

        public override string GetNextGuess()
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

        protected virtual bool FitsConstraints(string word)
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
