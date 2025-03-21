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
	}
}
