using WordleCore.Enums;

namespace WordleCore.Utils
{
	public static class WordleGameUtils
	{
		internal static readonly HashSet<string> _allowedWords = LoadAllowedWords();
		private static readonly Lazy<string[]> _previousWordles = new(() => WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt"));
		private static readonly Random _rng = new();

		internal static HashSet<string> LoadAllowedWords() =>
			[.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words.txt")];

		public static bool IsAllowedWord(string word) => _allowedWords.Contains(word);

		internal static string GetRandomWordle() => _previousWordles.Value[_rng.Next(0, _previousWordles.Value.Length)];

		internal static Correctness[] GetCorrectnesses(string wordle, string wordleGuess)
		{
			Correctness[] correctness = new Correctness[5];

			for (int i = 0; i < wordleGuess.Length; i++)
			{
				if (wordleGuess[i] == wordle[i]) { correctness[i] = Correctness.Correct; continue; }
				else if (!wordle.Contains(wordleGuess[i])) { correctness[i] = Correctness.Absent; continue; }

				int total = wordle.Count(x => x == wordleGuess[i]);
				int correctAfter = wordle.Where((x, j) => j > i && x == wordleGuess[i] && x == wordleGuess[j]).Count();
				int countUpTo = wordleGuess[..(i + 1)].Count(x => x == wordleGuess[i]);

				if (total - correctAfter >= countUpTo) correctness[i] = Correctness.Present;
				else correctness[i] = Correctness.OverCount;
			}

			return correctness;
		}
	}
}
