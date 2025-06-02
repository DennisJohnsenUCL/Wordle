using System.Collections.Concurrent;
using WordleSolver.Enums;
using WordleSolver.Models;
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

				var patternGroups = GetPatternBuckets(word, possibleWords);

				var expectedPositionalEntropy = GetExpectedPositionalEntropy(patternGroups);

				var positionalInformationGain = totalPositionalEntropy - expectedPositionalEntropy;

				entropies.TryAdd(word, positionalInformationGain);
			});
			return entropies;
		}

		private static double GetPositionalEntropy(List<string> possibleWords)
		{
			var positionalEntropy = 0d;

			for (int i = 0; i < 5; i++)
			{
				var positionEntropy = GetPositionEntropy(i, possibleWords);
				positionalEntropy += positionEntropy;
			}
			return positionalEntropy;
		}

		private static double GetPositionEntropy(int i, List<string> words)
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

		protected virtual Dictionary<string, PatternGroup> GetPatternBuckets(string guess, Dictionary<string, double> frequencies)
		{
			var patternGroups = new Dictionary<string, PatternGroup>();
			foreach (var (word, frequency) in frequencies)
			{
				var pattern = PatternsProvider.GetPattern(guess, word);
				if (!patternGroups.TryGetValue(pattern, out var group))
				{
					group = new PatternGroup();
					patternGroups[pattern] = group;
				}
				group.Words.Add(word);
				group.Frequency += frequency;
			}
			return patternGroups;
		}

		private static double GetExpectedPositionalEntropy(Dictionary<string, PatternGroup> patternGroups)
		{
			var expectedPositionalEntropy = 0d;
			foreach (var group in patternGroups.Values)
			{
				var words = group.Words;
				var probability = group.Frequency;

				var patternPositionalEntropy = GetPositionalEntropy(words);
				expectedPositionalEntropy += probability * patternPositionalEntropy;
			}
			return expectedPositionalEntropy;
		}
	}
}
