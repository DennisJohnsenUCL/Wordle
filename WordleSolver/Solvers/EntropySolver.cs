using System.Collections.Concurrent;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class EntropySolver : FilteredSolver
	{
		private readonly Dictionary<string, string> _cachedGuesses = [];
		private readonly int _limit;
		private readonly Dictionary<string, double> _wordFrequencies;
		private Dictionary<string, double> _possibleWords;

		public EntropySolver(IFirstGuessProvider firstGuessProvider, IPatternsProvider patternsProvider, Dictionary<string, double> wordFrequencies, int limit, string identifier)
			: base(firstGuessProvider, patternsProvider, [.. wordFrequencies.Keys], identifier)
		{
			_wordFrequencies = wordFrequencies;
			_limit = limit;
			_possibleWords = _wordFrequencies;
		}

		public override string GetNextGuess()
		{
			if (TryGetFirstGuess(out var firstGuess)) return firstGuess;

			SetPossibleWords();

			var normalizedFrequencies = GetNormalizedFrequencies();

			if (TryGetThresholdGuess(normalizedFrequencies, out var thresholdGuess)) return thresholdGuess;

			if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;

			var guess = GetStrategyGuess(normalizedFrequencies);

			_cachedGuesses.Add(CacheKey, guess);
			CacheKey += guess;
			return guess;
		}

		protected virtual bool TryGetFirstGuess(out string firstGuess)
		{
			if (CacheKey.Length == 0)
			{
				CacheKey += FirstGuess;
				firstGuess = FirstGuess;
				return true;
			}
			firstGuess = string.Empty;
			return false;
		}

		protected virtual string GetStrategyGuess(Dictionary<string, double> possibleWords)
		{
			return GetEntropyGuess(possibleWords);
		}

		protected virtual string GetEntropyGuess(Dictionary<string, double> possibleWords)
		{
			var entropies = GetEntropies(possibleWords);
			var guess = entropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;
			return guess;
		}

		protected void SetPossibleWords()
		{
			var possibleWords = new Dictionary<string, double>();
			var guess = CacheKey[^10..^5];
			var guessIndex = PatternsProvider.GetWordIndex(guess);
			var pattern = CacheKey[^5..^0];
			var patternIndex = PatternsProvider.GetPatternIndex(pattern);

			foreach (var pair in _possibleWords)
			{
				var word = pair.Key;
				var wordIndex = PatternsProvider.GetWordIndex(word);

				if (PatternsProvider.FitsPattern(guessIndex, wordIndex, patternIndex))
				{
					possibleWords.Add(word, pair.Value);
				}
			}
			_possibleWords = possibleWords;
		}

		protected virtual bool TryGetThresholdGuess(Dictionary<string, double> normalizedFrequencies, out string guess)
		{
			if (normalizedFrequencies.Count < _limit)
			{
				guess = normalizedFrequencies.First().Key;
				CacheKey += guess;
				return true;
			}
			guess = "";
			return false;
		}

		protected virtual bool TryGetCachedGuess(out string cachedGuess)
		{
			if (_cachedGuesses.TryGetValue(CacheKey, out var value))
			{
				CacheKey += value;
				cachedGuess = value;
				return true;
			}
			cachedGuess = string.Empty;
			return false;
		}

		protected virtual ConcurrentDictionary<string, double> GetEntropies(Dictionary<string, double> possibleWords)
		{
			ConcurrentDictionary<string, double> entropies = [];

			Parallel.ForEach(Words, word =>
			{
				var patternGroups = GetPatternFrequencies(word, possibleWords);

				var entropy = patternGroups.Sum(pattern => pattern.Value * Math.Log2(1 / pattern.Value));

				entropies.TryAdd(word, entropy);
			});

			return entropies;
		}

		protected virtual Dictionary<string, double> GetPatternFrequencies(string word, Dictionary<string, double> normalizedFrequencies)
		{
			Dictionary<string, double> patternGroups = [];

			foreach (var possibleWord in normalizedFrequencies.Keys)
			{
				var pattern = PatternsProvider.GetPattern(word, possibleWord);
				var frequency = normalizedFrequencies[possibleWord];

				if (!patternGroups.TryAdd(pattern, frequency)) patternGroups[pattern] += frequency;
			}
			return patternGroups;
		}

		public override void Reset()
		{
			_possibleWords = _wordFrequencies;
			base.Reset();
		}

		protected virtual Dictionary<string, double> GetNormalizedFrequencies()
		{
			var totalFreq = _possibleWords.Values.Sum();
			var normalizedFrequencies = _possibleWords.ToDictionary(x => x.Key, x => x.Value / totalFreq);
			return normalizedFrequencies;
		}
	}
}
