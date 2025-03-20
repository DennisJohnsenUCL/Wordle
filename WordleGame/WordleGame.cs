namespace WordleGame
{
	public class WordleGame
	{
		public string Wordle { get; }
		public int Guesses { get; }
		public int GuessesLeft { get; private set; }
		public GameState GameState { get; private set; }
		private static readonly HashSet<string> allowedWords = LoadAllowedWords();

		//>> Do constructor chaining instead
		//>> Pick random Wordle from previous Wordles list if not given
		public WordleGame(string wordle, int guesses = 6)
		{
			if (wordle.Length != 5) throw new WordleWrongLengthException("The Wordle must be 5 letters");
			if (guesses < 1) throw new NoGuessesException("The amount of guesses must be greater than 0");

			if (!allowedWords.Contains(wordle)) throw new WordleNotAllowedWordException("The Wordle is not in the list of allowed words");

			Wordle = wordle;
			Guesses = guesses;
			GuessesLeft = 0;
			GameState = GameState.NotStarted;
		}
		private static HashSet<string> LoadAllowedWords() => [.. File.ReadAllLines(@"Data\allowed_words.txt")];

		public void Start()
		{
			if (GameState == GameState.NotStarted)
			{
				GameState = GameState.Ongoing;
				GuessesLeft = Guesses;
			}
			else throw new WordleGameAlreadyStartedException();
		}

		public WordleResponse GuessWordle(string wordleGuess)
		{
			if (wordleGuess.Length != 5) throw new WordleGuessWrongLengthException("Wordle guesses must be 5 letters");
			if (!allowedWords.Contains(wordleGuess)) throw new WordleNotAllowedWordException("The guessed word is not in the list of allowed words");

			char[] chars = wordleGuess.ToCharArray();
			Correctness[] correctness = new Correctness[5];

			for (int i = 0; i < wordleGuess.Length; i++)
			{
				if (wordleGuess[i] == Wordle[i]) correctness[i] = Correctness.Correct;
				else if (!Wordle.Contains(wordleGuess[i])) correctness[i] = Correctness.Absent;
				//>> I don't think this will work if there are also Correct of the same letter
				else if (Wordle.Count(x => x == wordleGuess[i]) >= wordleGuess.Substring(0, i + 1).Count(x => x == wordleGuess[i])) correctness[i] = Correctness.Present;
			}

			return new WordleResponse(chars, correctness);
		}

		public override string ToString()
		{
			return $"This Wordle game is {GameState}, the Wordle is {Wordle}, " +
				$"and there are {GuessesLeft} guesses left out of {Guesses} guesses";
		}
	}
	public class WordleResponse
	{
		public char[] Chars { get; }
		public Correctness[] Correctness { get; }
		public WordleResponse(char[] chars, Correctness[] correctness)
		{
			Chars = chars;
			Correctness = correctness;
		}
	}
	public enum GameState
	{
		NotStarted,
		Ongoing,
		Completed,
		Failed
	}
	public enum Correctness
	{
		Correct,
		Present,
		Absent
	}
	public class WordleWrongLengthException : Exception
	{
		public WordleWrongLengthException() { }
		public WordleWrongLengthException(string message) : base(message) { }
		public WordleWrongLengthException(string message, Exception inner) : base(message, inner) { }
	}
	public class WordleGuessWrongLengthException : Exception
	{
		public WordleGuessWrongLengthException() { }
		public WordleGuessWrongLengthException(string message) : base(message) { }
		public WordleGuessWrongLengthException(string message, Exception inner) : base(message, inner) { }
	}
	public class WordleNotAllowedWordException : Exception
	{
		public WordleNotAllowedWordException() { }
		public WordleNotAllowedWordException(string message) : base(message) { }
		public WordleNotAllowedWordException(string message, Exception inner) : base(message, inner) { }
	}
	public class NoGuessesException : Exception
	{
		public NoGuessesException() { }
		public NoGuessesException(string message) : base(message) { }
		public NoGuessesException(string message, Exception inner) : base(message, inner) { }
	}
	public class WordleGameAlreadyStartedException : Exception
	{
		public WordleGameAlreadyStartedException() { }
		public WordleGameAlreadyStartedException(string message) : base(message) { }
		public WordleGameAlreadyStartedException(string message, Exception inner) : base(message, inner) { }
	}
}
