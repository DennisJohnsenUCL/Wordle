using WordleCore.Models;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class FilteredSortedSolver : LazySortedSolver, IReactiveSolver
    {
        public override string SolverIdentifier { get; } = "Solver3, filters guesses based on constraints, guesses words in order of literature usage";
        protected IConstraintManager Constraints { get; }
        protected string FirstGuess { get; private set; }

        public FilteredSortedSolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager)
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
            for (int i = Index; i < Words.Count; i++)
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
