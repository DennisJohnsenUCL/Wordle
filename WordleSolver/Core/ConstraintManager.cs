using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Models;

namespace WordleSolver.Core
{
    internal class ConstraintManager
    {
        public HashSet<Constraint> Constraints { get; private set; } = [];

        public void AddConstraints(IReadOnlyList<LetterResult> results)
        {
            for (int i = 0; i < results.Count; i++)
            {
                var (letter, correctness) = results[i];

                var constraint = new Constraint(letter, correctness, i);

                Constraints.Add(constraint);
            }
        }

        public bool FitsConstraints(string word)
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

        public void Clear()
        {
            Constraints = [];
        }
    }
}
