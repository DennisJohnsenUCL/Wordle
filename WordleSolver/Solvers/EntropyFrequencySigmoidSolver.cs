using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropyFrequencySigmoidSolver : EntropyFrequencySolver
    {
        public EntropyFrequencySigmoidSolver(
            IFirstGuessProvider firstGuessProvider,
            IConstraintManager constraintManager,
            IPatternsProvider patternsProvider,
            Dictionary<string, long> wordOccurrences)
            : base(firstGuessProvider, constraintManager, patternsProvider, ApplySigmoid(wordOccurrences)) { }

        private static Dictionary<string, double> ApplySigmoid(Dictionary<string, long> wordOccurrences)
        {
            int m = 5000000;
            int s = 1000000;
            var wordSigmoidFrequency = new Dictionary<string, double>();

            foreach (var key in wordOccurrences.Keys)
            {
                var value = wordOccurrences[key];

                var exponent = -(double)(value - m) / s;
                var newValue = 1.0 / (1.0 + Math.Exp(exponent));

                wordSigmoidFrequency[key] = newValue;
            }

            double total = wordSigmoidFrequency.Values.Sum();

            var normalizedWordSigmoidFrequency = wordSigmoidFrequency.ToDictionary(x => x.Key, x => x.Value / total);

            return normalizedWordSigmoidFrequency;
        }
    }
}
