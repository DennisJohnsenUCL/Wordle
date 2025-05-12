using System.Collections.Concurrent;
using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class EntropySolver : FilteredSolver
	{
		protected string? LastPattern { get; protected private set; }
		protected HashSet<string> GuessedWords { get; protected private set; } = [];
		protected Dictionary<string, string> CachedBestSecond { get; protected private set; } = [];
		protected IPatternsProvider PatternsProvider { get; }
		protected int Limit { get; }
		private readonly Dictionary<string, double> _wordFrequencies;
		private Dictionary<string, double> _possibleWords;

		public EntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<string, double> wordFrequencies, int limit, string identifier)
			: base(firstGuessProvider, constraintManager, [.. wordFrequencies.Keys], identifier)
		{
			PatternsProvider = patternsProvider;
			_wordFrequencies = wordFrequencies;
			Limit = limit;
			_possibleWords = _wordFrequencies;
		}

		public override void AddResponse(WordleResponse response)
		{
			LastPattern = string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
			base.AddResponse(response);
		}

		public override string GetNextGuess()
		{
			if (GuessedWords.Count == 0)
			{
				GuessedWords.Add(FirstGuess);
				return FirstGuess;
			}

			SetPossibleWords();

			var normalizedFrequencies = GetNormalizedFrequencies();

			if (TryGetThresholdGuess(normalizedFrequencies, out var value)) return value;

			if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;

			var guess = GetStrategyGuess(normalizedFrequencies);

			if (GuessedWords.Count == 1) CachedBestSecond.Add(LastPattern!, guess);

			GuessedWords.Add(guess);
			return guess;
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

		protected virtual void SetPossibleWords()
		{
			var possibleWords = new Dictionary<string, double>();

			foreach (var pair in _possibleWords)
			{
				var word = pair.Key;
				if (GuessedWords.Contains(word)) continue;

				if (FitsConstraints(word))
				{
					possibleWords.Add(word, pair.Value);
				}
			}
			_possibleWords = possibleWords;
		}

		protected virtual bool TryGetThresholdGuess(Dictionary<string, double> normalizedFrequencies, out string guess)
		{
			if (normalizedFrequencies.Count < Limit)
			{
				guess = normalizedFrequencies.First().Key;
				return true;
			}
			guess = "";
			return false;
		}

		protected virtual bool TryGetCachedGuess(out string cachedGuess)
		{
			if (GuessedWords.Count == 1 && CachedBestSecond.TryGetValue(LastPattern!, out var value))
			{
				GuessedWords.Add(value);
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
				if (GuessedWords.Contains(word)) return;

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
			LastPattern = null;
			GuessedWords = [];
			_possibleWords = _wordFrequencies;
			base.Reset();
		}

		protected virtual Dictionary<string, double> GetNormalizedFrequencies()
		{
			var totalFreq = _possibleWords.Values.Sum();
			var normalizedFrequencies = _possibleWords.ToDictionary(x => x.Key, x => x.Value / totalFreq);
			return normalizedFrequencies;
		}

		private static readonly Dictionary<Correctness, char> CorrectnessMappings = new()
		{
			{ Correctness.Absent, 'A' },
			{ Correctness.Present, 'P' },
			{ Correctness.Correct, 'C' },
			{ Correctness.OverCount, 'O' },
		};
	}
}
