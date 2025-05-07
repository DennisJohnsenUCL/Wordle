using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropyFrequencySigmoidSolver : EntropyFrequencySolver
    {
        public override string Identifier { get; } = "EntropyFrequencySigmoidSolver, applies a sigmoid curve to word frequencies";

        public EntropyFrequencySigmoidSolver(
            IFirstGuessProvider firstGuessProvider,
            IConstraintManager constraintManager,
            IPatternsProvider patternsProvider,
            Dictionary<string, double> wordOccurrences,
            string[] words)
            : base(firstGuessProvider, constraintManager, patternsProvider, wordOccurrences, words) { }
    }
}
