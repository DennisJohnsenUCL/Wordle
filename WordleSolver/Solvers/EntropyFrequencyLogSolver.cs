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
            Dictionary<string, long> wordOccurrences)
            : base(firstGuessProvider, constraintManager, patternsProvider, ApplyLog(wordOccurrences)) { }

        private static Dictionary<string, double> ApplyLog(Dictionary<string, long> wordOccurrences)
        {
            var wordLogFrequency = new Dictionary<string, double>();

            foreach (var key in wordOccurrences.Keys)
            {
                var value = wordOccurrences[key];
                var newValue = Math.Log(1 + value);

                wordLogFrequency[key] = newValue;
            }

            double total = wordLogFrequency.Values.Sum();

            var normalizedWordLogFrequency = wordLogFrequency.ToDictionary(x => x.Key, x => x.Value / total);

            return normalizedWordLogFrequency;
        }
    }
}
