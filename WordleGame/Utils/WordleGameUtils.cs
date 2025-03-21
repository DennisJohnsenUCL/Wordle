using WordleGame.Enums;

namespace WordleGame.Utils
{
	internal static class WordleGameUtils
	{
		public static readonly HashSet<string> allowedWords = LoadAllowedWords();
		private static string[]? previousWordles;
		private static Random? rng;

		public static HashSet<string> LoadAllowedWords() => [.. File.ReadAllLines(@"Data\allowed_words.txt")];

		private static void LoadPreviousWordles()
		{
			previousWordles = File.ReadAllLines(@"Data\previous_wordles.txt");
		}

		public static string GetRandomWordle()
		{
			if (null == previousWordles) LoadPreviousWordles();
			if (null == rng) rng = new();
			return previousWordles![rng.Next(0, previousWordles.Length)];
		}

		public static Correctness[] GetCorrectnesses(string wordle, string wordleGuess)
		{
			Correctness[] correctness = new Correctness[5];

			for (int i = 0; i < wordleGuess.Length; i++)
			{
				if (wordleGuess[i] == wordle[i]) correctness[i] = Correctness.Correct;
				else if (!wordle.Contains(wordleGuess[i])) correctness[i] = Correctness.Absent;
				//>> I don't think this will work if there are also Correct of the same letter?
				else if (wordle.Count(x => x == wordleGuess[i]) >= wordleGuess[..(i + 1)].Count(x => x == wordleGuess[i])) correctness[i] = Correctness.Present;
			}

			return correctness;
		}
	}
}
