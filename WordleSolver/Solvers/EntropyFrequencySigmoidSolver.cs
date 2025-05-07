using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropyFrequencySigmoidSolver : EntropyFrequencySolver
    {
        public EntropyFrequencySigmoidSolver(
            IFirstGuessProvider firstGuessProvider,
            IConstraintManager constraintManager,
            IPatternsProvider patternsProvider,
            Dictionary<string, double> wordOccurrences,
            string[] words,
            string identifier)
            : base(firstGuessProvider, constraintManager, patternsProvider, wordOccurrences, words, identifier) { }
    }
}
