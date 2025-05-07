using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropyFrequencyLogSolver : EntropyFrequencySolver
    {
        public override string Identifier { get; } = "EntropyFrequencyLogSolver, applies a log(1+x) curve to word frequencies";

        public EntropyFrequencyLogSolver(
            IFirstGuessProvider firstGuessProvider,
            IConstraintManager constraintManager,
            IPatternsProvider patternsProvider,
            Dictionary<string, double> wordOccurrences,
            string[] words)
            : base(firstGuessProvider, constraintManager, patternsProvider, wordOccurrences, words) { }
    }
}
