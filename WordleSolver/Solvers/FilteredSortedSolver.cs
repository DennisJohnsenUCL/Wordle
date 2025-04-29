using WordleCore.Models;
using WordleSolver.Core;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class FilteredSortedSolver : LazySortedSolver
    {
        public override string SolverIdentifier { get; } = "Solver3, filters guesses based on constraints, guesses words in order of literature usage";
        protected ConstraintManager Constraints { get; } = new();
        protected string FirstGuess { get; private set; }

        public FilteredSortedSolver(IFirstGuessProvider firstGuessProvider)
        {
            FirstGuess = firstGuessProvider.Value;
        }

        public override string GetFirstGuess()
        {
            return FirstGuess;
        }

        public override void AddResponse(WordleResponse response)
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
