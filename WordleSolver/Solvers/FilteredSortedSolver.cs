using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Models;

namespace WordleSolver.Solvers
{
    internal class FilteredSortedSolver : LazySortedSolver
    {
        public override string SolverIdentifier { get; } = "Solver3, filters guesses based on constraints, guesses words in order of literature usage";
        protected virtual List<Constraint> Constraints { get; private set; } = [];

        public override string GetFirstGuess()
        {
            return "SALET";
        }

        public override void AddResponse(WordleResponse response)
        {
            var results = response.LetterResults;

            for (int i = 0; i < results.Count; i++)
            {
                var (letter, correctness) = results[i];

                Constraints.Add(new Constraint(letter, correctness, i));
            }
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
            foreach (var constraint in Constraints)
            {
                var (letter, correctness, position) = constraint;

                if (correctness == Correctness.Absent && word.Contains(letter)) return false;
                if (correctness == Correctness.Correct && word[position] != letter) return false;
                if (correctness == Correctness.Present && !word.Remove(position, 1).Contains(letter)) return false;
            }
            return true;
        }

        public override void Reset()
        {
            Constraints = [];
            base.Reset();
        }
    }
}
