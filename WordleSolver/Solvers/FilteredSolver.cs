using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class FilteredSolver : LazySolver, IReactiveSolver
	{
		protected IPatternsProvider PatternsProvider { get; }
		protected string FirstGuess { get; protected private set; }
		protected string GameKey { get; protected private set; } = string.Empty;
		private List<string> _possibleWords;

		public FilteredSolver(IFirstGuessProvider firstGuessProvider, IPatternsProvider patternsProvider, string[] words, string identifier) : base(words, identifier)
		{
			FirstGuess = firstGuessProvider.Value;
			PatternsProvider = patternsProvider;
			_possibleWords = [.. Words];
		}

		public virtual void AddResponse(WordleResponse response)
		{
			GameKey += string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
		}

		public override string GetNextGuess()
		{
			if (GameKey == "")
			{
				GameKey += FirstGuess;
				return FirstGuess;
			}

			SetPossibleWords();

			var guess = _possibleWords[0];
			GameKey += guess;
			return guess;
		}

		private void SetPossibleWords()
		{
			var possibleWords = new List<string>();
			var guess = GameKey[^10..^5];
			var guessIndex = PatternsProvider.GetWordIndex(guess);
			var pattern = GameKey[^5..^0];
			var patternIndex = PatternsProvider.GetPatternIndex(pattern);

			foreach (var word in _possibleWords)
			{
				var wordIndex = PatternsProvider.GetWordIndex(word);

				if (PatternsProvider.FitsPattern(guessIndex, wordIndex, patternIndex))
				{
					possibleWords.Add(word);
				}
			}
			_possibleWords = possibleWords;
		}

		public override void Reset()
		{
			GameKey = string.Empty;
			_possibleWords = [.. Words];
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
