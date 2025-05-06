using System.Collections.Concurrent;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropyFrequencySolver : EntropySolver
    {
        public override string Identifier { get; } = "EntropyFrequencySolver, uses information theory to calculate the guess with most information and weighs each words value by their frequency in literature";
        protected override int Limit { get; protected private set; } = 21;
        private readonly Dictionary<string, double> _wordFrequencies;

        public EntropyFrequencySolver(
            IFirstGuessProvider firstGuessProvider,
            IConstraintManager constraintManager,
            IPatternsProvider patternsProvider,
            Dictionary<string, double> wordFrequencies)
            : base(firstGuessProvider, constraintManager, patternsProvider)
        {
            _wordFrequencies = wordFrequencies;
        }

        protected override ConcurrentDictionary<string, double> GetEntropies(List<string> possibleWords)
        {
            var normalizedFrequencies = GetNormalizedFrequencies(possibleWords);

            ConcurrentDictionary<string, double> entropies = [];

            Parallel.For(0, Words.Count, i =>
            {
                var word = Words[i];

                if (GuessedWords.Contains(word)) return;

                Dictionary<string, double> patternGroups = [];

                foreach (var possibleWord in normalizedFrequencies.Keys)
                {
                    var pattern = PatternsProvider.GetPattern(i, possibleWord);
                    var frequency = normalizedFrequencies[possibleWord];

                    if (!patternGroups.TryAdd(pattern, frequency)) patternGroups[pattern] += frequency;
                }

                var entropy = patternGroups.Sum(probability => probability.Value * Math.Log2(1 / probability.Value));

                entropies.TryAdd(word, entropy);
            });

            return entropies;
        }

        protected virtual Dictionary<string, double> GetNormalizedFrequencies(List<string> possibleWords)
        {
            var possibleSet = possibleWords.ToHashSet();
            var possibleFrequencies = _wordFrequencies
                .Where(pair => possibleSet.Contains(pair.Key))
                .ToDictionary();

            var totalFreq = possibleFrequencies.Sum(pair => pair.Value);
            var normalizedFrequencies = possibleFrequencies.ToDictionary(x => x.Key, x => x.Value / totalFreq);

            return normalizedFrequencies;
        }
    }
}
