using System.Collections.Concurrent;
using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Solvers
{
	internal class EntropySolver : FilteredSolver
	{
		protected Word? LastPattern { get; protected private set; }
		protected HashSet<Word> GuessedWords { get; protected private set; } = [];
		protected Dictionary<Word, Word> CachedBestSecond { get; protected private set; } = [];
		protected IPatternsProvider PatternsProvider { get; }
		private readonly int _limit;
		private readonly Dictionary<Word, double> _wordFrequencies;

		public EntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<Word, double> wordFrequencies, int limit, string identifier)
			: base(firstGuessProvider, constraintManager, [.. wordFrequencies.Keys], identifier)
		{
			PatternsProvider = patternsProvider;
			_wordFrequencies = wordFrequencies;
			_limit = limit;
		}

		public override void AddResponse(WordleResponse response)
		{
			LastPattern = new Word(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
			base.AddResponse(response);
		}

		public override Word GetNextGuess()
		{
			var possibleWords = GetPossibleWords();

			var normalizedFrequencies = GetNormalizedFrequencies(possibleWords);

			if (TryGetThresholdGuess(normalizedFrequencies, out var value)) return value;

			if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;

			var entropies = GetEntropies(normalizedFrequencies);

			var guess = entropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;

			if (GuessedWords.Count == 1) CachedBestSecond.Add((Word)LastPattern!, guess);

			GuessedWords.Add(guess);
			return guess;
		}

		protected virtual Dictionary<Word, double> GetPossibleWords()
		{
			var possibleWords = new Dictionary<Word, double>();

			foreach (var pair in _wordFrequencies)
			{
				var word = pair.Key;
				if (GuessedWords.Contains(word)) continue;

				if (FitsConstraints(word))
				{
					possibleWords.Add(word, pair.Value);
				}
			}
			return possibleWords;
		}

		protected virtual bool TryGetThresholdGuess(Dictionary<Word, double> normalizedFrequencies, out Word guess)
		{
			if (normalizedFrequencies.Count < _limit)
			{
				guess = normalizedFrequencies.First().Key;
				return true;
			}
			guess = Word.Empty;
			return false;
		}

		protected virtual bool TryGetCachedGuess(out Word cachedGuess)
		{
			if (GuessedWords.Count == 1 && CachedBestSecond.TryGetValue((Word)LastPattern!, out var value))
			{
				GuessedWords.Add(value);
				cachedGuess = value;
				return true;
			}
			cachedGuess = Word.Empty;
			return false;
		}

		protected virtual ConcurrentDictionary<Word, double> GetEntropies(Dictionary<Word, double> possibleWords)
		{
			ConcurrentDictionary<Word, double> entropies = [];

			Parallel.ForEach(Words, word =>
			{
				if (GuessedWords.Contains(word)) return;

				var patternGroups = GetPatternGroups(word, possibleWords);

				var entropy = patternGroups.Sum(pattern => pattern.Value * Math.Log2(1 / pattern.Value));

				entropies.TryAdd(word, entropy);
			});

			return entropies;
		}

		protected virtual Dictionary<Word, double> GetPatternGroups(Word word, Dictionary<Word, double> normalizedFrequencies)
		{
			Dictionary<Word, double> patternGroups = [];

			foreach (var possibleWord in normalizedFrequencies.Keys)
			{
				var pattern = PatternsProvider.GetPattern(word, possibleWord);
				var frequency = normalizedFrequencies[possibleWord];

				if (!patternGroups.TryAdd(pattern, frequency)) patternGroups[pattern] += frequency;
			}
			return patternGroups;
		}

		public override Word GetFirstGuess()
		{
			GuessedWords.Add(FirstGuess);
			return base.GetFirstGuess();
		}

		public override void Reset()
		{
			LastPattern = null;
			GuessedWords = [];
			base.Reset();
		}

		protected virtual Dictionary<Word, double> GetNormalizedFrequencies(Dictionary<Word, double> possibleWords)
		{
			var totalFreq = possibleWords.Values.Sum();
			var normalizedFrequencies = possibleWords.ToDictionary(x => x.Key, x => x.Value / totalFreq);
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
