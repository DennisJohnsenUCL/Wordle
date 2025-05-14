using System.Collections.Concurrent;
using WordleSolver.Enums;
using WordleSolver.Services;

namespace WordleSolver.Solvers
{
	internal class PositionalEntropySolver : EntropySolver
	{
		public PositionalEntropySolver(SolverContext context, Frequencies frequencies, int limit, string identifier)
			: base(context, frequencies, limit, identifier) { }

		protected override string GetStrategyGuess(Dictionary<string, double> possibleWords)
		{
			return GetPositionalEntropyGuess(possibleWords);
		}

		protected virtual string GetPositionalEntropyGuess(Dictionary<string, double> possibleWords)
		{
			var positionalEntropies = GetPositionalEntropies(possibleWords);
			var guess = positionalEntropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;
			return guess;
		}

		protected virtual ConcurrentDictionary<string, double> GetPositionalEntropies(Dictionary<string, double> possibleWords)
		{
			ConcurrentDictionary<string, double> entropies = [];

			Parallel.ForEach(Words, word =>
			{
				var totalPositionalEntropy = GetPositionalEntropy([.. possibleWords.Keys]);

				var (patternGroupWords, patternGroupProbabilities) = GetPatternBuckets(word, possibleWords);

				var expectedPositionalEntropy = GetExpectedPositionalEntropy(patternGroupWords, patternGroupProbabilities);

				var positionalInformationGain = totalPositionalEntropy - expectedPositionalEntropy;

				entropies.TryAdd(word, positionalInformationGain);
			});
			return entropies;
		}

		private static double GetPositionalEntropy(string[] possibleWords)
		{
			var positionalEntropy = 0d;

			for (int i = 0; i < 5; i++)
			{
				var positionEntropy = GetPositionEntropy(i, possibleWords);
				positionalEntropy += positionEntropy;
			}
			return positionalEntropy;
		}

		private static double GetPositionEntropy(int i, string[] words)
		{
			var letterCounts = new Dictionary<char, int>();
			foreach (var word in words)
			{
				if (!letterCounts.TryAdd(word[i], 1)) letterCounts[word[i]] += 1;
			}
			var total = letterCounts.Values.Sum();
			var letterFrequencies = letterCounts.ToDictionary(x => x.Key, x => (double)x.Value / total);
			var entropy = letterFrequencies.Sum(letter => letter.Value * Math.Log2(1 / letter.Value));
			return entropy;
		}

		protected virtual (Dictionary<string, List<string>>, Dictionary<string, double>) GetPatternBuckets(string guess, Dictionary<string, double> frequencies)
		{
			var patternGroupWords = new Dictionary<string, List<string>>();
			var patternGroupProbabilities = new Dictionary<string, double>();
			foreach (var word in frequencies.Keys)
			{
				var frequency = frequencies[word];

				var pattern = PatternsProvider.GetPattern(guess, word);
				if (!patternGroupWords.TryAdd(pattern, [word])) patternGroupWords[pattern].Add(word);
				if (!patternGroupProbabilities.TryAdd(pattern, frequency)) patternGroupProbabilities[pattern] += frequency;
			}
			return (patternGroupWords, patternGroupProbabilities);
		}

		private static double GetExpectedPositionalEntropy(Dictionary<string, List<string>> patternGroupWords, Dictionary<string, double> patternGroupProbabilities)
		{
			var expectedPositionalEntropy = 0d;
			foreach (var pair in patternGroupWords)
			{
				var pattern = pair.Key;
				var words = pair.Value;
				var probability = patternGroupProbabilities[pattern];

				var patternPositionalEntropy = GetPositionalEntropy([.. words]);
				expectedPositionalEntropy += probability * patternPositionalEntropy;
			}
			return expectedPositionalEntropy;
		}
	}
}
