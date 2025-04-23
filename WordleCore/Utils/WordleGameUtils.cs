using System.Collections.Immutable;
using WordleCore.Enums;
using WordleCore.Models;

namespace WordleCore.Utils
{
	public static class WordleGameUtils
	{
		internal static readonly IReadOnlySet<string> _allowedWords = LoadAllowedWords();
		internal static readonly Lazy<ImmutableArray<string>> _previousWordles = new(() => [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt")]);
		private static readonly Lazy<Random> _rng = new(() => new Random());

		internal static HashSet<string> LoadAllowedWords() =>
			[.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words.txt")];

		public static bool IsAllowedWord(string word) => _allowedWords.Contains(word);

		internal static string GetRandomWordle() => _previousWordles.Value[_rng.Value.Next(0, _previousWordles.Value.Length)];

		internal static LetterResult[] GetCorrectnesses(string wordle, string guess)
		{
			LetterResult[] letterResults = new LetterResult[5];

			for (int i = 0; i < 5; i++)
			{
				Correctness correctness;

				if (guess[i] == wordle[i]) correctness = Correctness.Correct;
				else if (!wordle.Contains(guess[i])) correctness = Correctness.Absent;
				else
				{
					int total = wordle.Count(x => x == guess[i]);
					int correctAfter = wordle.Where((x, j) => j > i && x == guess[i] && x == guess[j]).Count();
					int countUpTo = guess[..(i + 1)].Count(x => x == guess[i]);

					if (total - correctAfter >= countUpTo) correctness = Correctness.Present;
					else correctness = Correctness.OverCount;
				}

				letterResults[i] = new LetterResult(guess[i], correctness);
			}

			return letterResults;
		}

		public static WordleGame GetWordleGameFromOptions(WordleOptions options)
		{
			var (wordle, guesses) = options;

			if (wordle != null && guesses != null) return new WordleGame(wordle, (int)guesses);
			else if (wordle != null) return new WordleGame(wordle);
			else if (guesses != null) return new WordleGame((int)guesses);
			else return new WordleGame();
		}
	}
}
