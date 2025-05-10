using System.Collections.Concurrent;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Solvers
{
	internal class PositionalEntropySolver : EntropySolver
	{
		public PositionalEntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<Word, double> wordFrequencies, int limit, string identifier)
			: base(firstGuessProvider, constraintManager, patternsProvider, wordFrequencies, limit, identifier) { }

		protected override ConcurrentDictionary<Word, double> GetEntropies(Dictionary<Word, double> possibleWords)
		{
			ConcurrentDictionary<Word, double> entropies = [];

			Parallel.ForEach(Words, word =>
			{
				if (GuessedWords.Contains(word)) return;

				var totalPositionalEntropy = GetPositionalEntropy([.. possibleWords.Keys]);

				var (patternGroupWords, patternGroupProbabilities) = GetPatternGroups(word, possibleWords);

				var expectedPositionalEntropy = GetExpectedPositionalEntropy(patternGroupWords, patternGroupProbabilities);

				var positionalInformationGain = totalPositionalEntropy - expectedPositionalEntropy;

				entropies.TryAdd(word, positionalInformationGain);
			});
			return entropies;
		}

		private double GetPositionalEntropy(Word[] possibleWords)
		{
			var positionalEntropy = 0d;

			for (int i = 0; i < 5; i++)
			{
				var positionEntropy = GetPositionEntropy(i, possibleWords);
				positionalEntropy += positionEntropy;
			}
			return positionalEntropy;
		}

		private static double GetPositionEntropy(int i, Word[] words)
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

		protected virtual new (Dictionary<Word, List<Word>>, Dictionary<Word, double>) GetPatternGroups(Word guess, Dictionary<Word, double> frequencies)
		{
			var patternGroupWords = new Dictionary<Word, List<Word>>();
			var patternGroupProbabilities = new Dictionary<Word, double>();
			foreach (var word in frequencies.Keys)
			{
				var frequency = frequencies[word];

				var pattern = PatternsProvider.GetPattern(guess, word);
				if (!patternGroupWords.TryAdd(pattern, [word])) patternGroupWords[pattern].Add(word);
				if (!patternGroupProbabilities.TryAdd(pattern, frequency)) patternGroupProbabilities[pattern] += frequency;
			}
			return (patternGroupWords, patternGroupProbabilities);
		}

		private double GetExpectedPositionalEntropy(Dictionary<Word, List<Word>> patternGroupWords, Dictionary<Word, double> patternGroupProbabilities)
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
