using WordleCore.Enums;

namespace WordleCore.Utils
{
	internal static class WordleGameUtils
	{
		internal static readonly HashSet<string> allowedWords = LoadAllowedWords();
		internal static string[]? previousWordles;
		private static Random? rng;

		internal static HashSet<string> LoadAllowedWords() =>
			[.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words.txt")];

		internal static void LoadPreviousWordles()
		{
			previousWordles = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt");
		}

		internal static string GetRandomWordle()
		{
			if (null == previousWordles) LoadPreviousWordles();
			if (null == rng) rng = new();
			return previousWordles![rng.Next(0, previousWordles.Length)];
		}

		//>> Returns wrong Correctness if Present before Correct guess
		//>> To mark present should only count Wordle letters that are not marked correct later on in the sequence
		//>> Mark all correct and totally absent first, then separate loop for present?
		//>> That way can look ahead, or only calculate present based on remaining letters
		internal static Correctness[] GetCorrectnesses(string wordle, string wordleGuess)
		{
			Correctness[] correctness = new Correctness[5];

			for (int i = 0; i < wordleGuess.Length; i++)
			{
				if (wordleGuess[i] == wordle[i]) correctness[i] = Correctness.Correct;
				else if (!wordle.Contains(wordleGuess[i])) correctness[i] = Correctness.Absent;
				else if (wordle.Count(x => x == wordleGuess[i]) >= wordleGuess[..(i + 1)].Count(x => x == wordleGuess[i])) correctness[i] = Correctness.Present;
				else correctness[i] = Correctness.Absent;
			}

			return correctness;
		}
	}
}
