using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropyFrequencyLogSolver : EntropyFrequencySolver
    {
        public EntropyFrequencyLogSolver(
            IFirstGuessProvider firstGuessProvider,
            IConstraintManager constraintManager,
            IPatternsProvider patternsProvider,
            Dictionary<string, double> wordOccurrences,
            string[] words,
            string identifier)
            : base(firstGuessProvider, constraintManager, patternsProvider, wordOccurrences, words, identifier) { }
    }
}
