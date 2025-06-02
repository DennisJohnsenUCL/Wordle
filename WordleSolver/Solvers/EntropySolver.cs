using System.Collections.Concurrent;
using WordleSolver.Enums;
using WordleSolver.Services;

namespace WordleSolver.Solvers
{
	internal class EntropySolver : FilteredSolver
	{
		private readonly Dictionary<string, string> _cachedGuesses = [];
		private readonly int _limit;
		private Dictionary<string, double> _possibleWords;
		private readonly Dictionary<string, double> _possibleWordsPool;

		public EntropySolver(SolverContext context, Frequencies frequencies, int limit, string identifier)
			: base(context, identifier)
		{
			var _wordFrequencies = frequencies switch
			{
				Frequencies.Flat => context.WordFrequenciesFlat,
				Frequencies.Weighted => context.WordFrequenciesWeighted,
				Frequencies.Sigmoid => context.WordFrequenciesSigmoid,
				Frequencies.Log => context.WordFrequenciesLog,
				_ => throw new Exception(),
			};
			_limit = limit;

			if (context.AnswerPools == AnswerPools.AllWords) _possibleWordsPool = _wordFrequencies;
			else _possibleWordsPool = _wordFrequencies.Where(x => context.Wordles.Contains(x.Key)).ToDictionary();
			_possibleWords = _possibleWordsPool;
		}

		public override string GetNextGuess()
		{
			if (TryGetFirstGuess(out var firstGuess)) return firstGuess;

			SetPossibleWords();

			var normalizedFrequencies = GetNormalizedFrequencies();

			if (TryGetThresholdGuess(normalizedFrequencies, out var thresholdGuess)) return thresholdGuess;

			if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;

			var guess = GetStrategyGuess(normalizedFrequencies);

			_cachedGuesses.Add(GameKey, guess);
			GameKey += guess;
			return guess;
		}

		protected virtual bool TryGetFirstGuess(out string firstGuess)
		{
			if (GameKey.Length == 0)
			{
				GameKey += FirstGuess;
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
			var guess = entropies.MaxBy(x => x.Value).Key;
			return guess;
		}

		protected void SetPossibleWords()
		{
			var possibleWords = new Dictionary<string, double>();
			var guess = GameKey[^10..^5];
			var guessIndex = PatternsProvider.GetWordIndex(guess);
			var pattern = GameKey[^5..^0];
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
				GameKey += guess;
				return true;
			}
			guess = "";
			return false;
		}

		protected virtual bool TryGetCachedGuess(out string cachedGuess)
		{
			if (_cachedGuesses.TryGetValue(GameKey, out var value))
			{
				GameKey += value;
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
			_possibleWords = _possibleWordsPool;
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
