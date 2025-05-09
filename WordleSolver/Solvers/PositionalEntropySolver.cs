using System.Collections.Concurrent;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class PositionalEntropySolver : EntropySolver
	{
		public PositionalEntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<string, double> wordFrequencies, int limit, string identifier)
			: base(firstGuessProvider, constraintManager, patternsProvider, wordFrequencies, limit, identifier) { }

		protected override ConcurrentDictionary<string, double> GetEntropies(List<string> possibleWords)
		{
			var normalizedFrequencies = GetNormalizedFrequencies(possibleWords);
			ConcurrentDictionary<string, double> entropies = [];

			Parallel.ForEach(Words, word =>
			{
				if (GuessedWords.Contains(word)) return;

				var totalPositionalEntropy = GetPositionalEntropy([.. normalizedFrequencies.Keys]);

				var (patternGroupWords, patternGroupProbabilities) = GetPatternGroupWords(word, normalizedFrequencies);

				var expectedPositionalEntropy = GetExpectedPositionalEntropy(patternGroupWords, patternGroupProbabilities);

				var positionalInformationGain = totalPositionalEntropy - expectedPositionalEntropy;

				entropies.TryAdd(word, positionalInformationGain);
			});
			return entropies;
		}

		private double GetPositionalEntropy(string[] possibleWords)
		{
			var positionalEntropy = 0d;

			for (int i = 0; i < 5; i++)
			{
				var positionEntropy = GetPositionEntropy(i, possibleWords);
				positionalEntropy += positionEntropy;
			}
			return positionalEntropy;
		}

		private double GetPositionEntropy(int i, string[] words)
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

		private (Dictionary<string, List<string>>, Dictionary<string, double>) GetPatternGroupWords(string guess, Dictionary<string, double> frequencies)
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

		private double GetExpectedPositionalEntropy(Dictionary<string, List<string>> patternGroupWords, Dictionary<string, double> patternGroupProbabilities)
		{
			var expectedPositionalEntropy = 0d;
			foreach (var pair in patternGroupWords)
			{
				var pattern = pair.Key;
				var words = pair.Value;
				var probability = patternGroupProbabilities[pattern];

				var totalPositionalEntropy = GetPositionalEntropy([.. words]);
				expectedPositionalEntropy += probability * totalPositionalEntropy;
			}
			return expectedPositionalEntropy;
		}
	}
}
