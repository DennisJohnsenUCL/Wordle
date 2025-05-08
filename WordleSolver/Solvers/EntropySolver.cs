using System.Collections.Concurrent;
using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class EntropySolver : FilteredSolver
	{
		private string? _lastPattern;
		protected HashSet<string> GuessedWords { get; protected private set; } = [];
		private readonly Dictionary<string, string> CachedBestSecond = [];
		protected IPatternsProvider PatternsProvider { get; }
		private readonly int _limit;
		private readonly Dictionary<string, double> _wordFrequencies;

		public EntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<string, double> wordFrequencies, int limit, string identifier)
			: base(firstGuessProvider, constraintManager, [.. wordFrequencies.Keys], identifier)
		{
			PatternsProvider = patternsProvider;
			_wordFrequencies = wordFrequencies;
			_limit = limit;
		}

		public override void AddResponse(WordleResponse response)
		{
			_lastPattern = string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
			base.AddResponse(response);
		}

		public override string GetNextGuess()
		{
			var possibleWords = GetPossibleWords();

			if (possibleWords.Count < _limit) return possibleWords[0];

			if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;

			var entropies = GetEntropies(possibleWords);

			var guess = entropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;

			if (GuessedWords.Count == 1) CachedBestSecond.Add(_lastPattern!, guess);

			GuessedWords.Add(guess);
			return guess;
		}

		protected virtual List<string> GetPossibleWords()
		{
			var possibleWords = new List<string>();

			foreach (var word in Words)
			{
				if (GuessedWords.Contains(word)) continue;

				if (FitsConstraints(word))
				{
					possibleWords.Add(word);
				}
			}
			return possibleWords;
		}

		protected virtual bool TryGetCachedGuess(out string cachedGuess)
		{
			if (GuessedWords.Count == 1 && CachedBestSecond.TryGetValue(_lastPattern!, out var value))
			{
				GuessedWords.Add(value);
				cachedGuess = value;
				return true;
			}
			cachedGuess = string.Empty;
			return false;
		}

		protected virtual ConcurrentDictionary<string, double> GetEntropies(List<string> possibleWords)
		{
			var normalizedFrequencies = GetNormalizedFrequencies(possibleWords);

			ConcurrentDictionary<string, double> entropies = [];

			Parallel.For(0, Words.Length, i =>
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

				var entropy = patternGroups.Sum(pattern => pattern.Value * Math.Log2(1 / pattern.Value));

				entropies.TryAdd(word, entropy);
			});

			return entropies;
		}

		public override string GetFirstGuess()
		{
			GuessedWords.Add(FirstGuess);
			return base.GetFirstGuess();
		}

		public override void Reset()
		{
			_lastPattern = null;
			GuessedWords = [];
			base.Reset();
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

		private static readonly Dictionary<Correctness, char> CorrectnessMappings = new()
		{
			{ Correctness.Absent, 'A' },
			{ Correctness.Present, 'P' },
			{ Correctness.Correct, 'C' },
			{ Correctness.OverCount, 'O' },
		};
	}
}
