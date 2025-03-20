namespace WordleGame
{
	public class WordleGame
	{
		public string Wordle { get; }
		public int Guesses { get; }
		public int GuessesLeft { get; private set; }
		public GameState GameState { get; private set; }
		public WordleGame(string wordle, int guesses = 6)
		{
			if (wordle.Length != 5) throw new WordleWrongLengthException();
			if (guesses < 1) throw new NoGuessesException();

			Wordle = wordle;
			Guesses = guesses;
			GuessesLeft = 0;
			GameState = GameState.NotStarted;
		}
		public void Start()
		{
			if (GameState == GameState.NotStarted)
			{
				GameState = GameState.Ongoing;
				GuessesLeft = Guesses;
			}
		}
		public WordleResponse GuessWordle(string wordleGuess)
		{
			if (wordleGuess.Length != 5) throw new WordleGuessWrongLengthException();

			char[] chars = wordleGuess.ToCharArray();
			Correctness[] correctness = new Correctness[5];

			for (int i = 0; i < wordleGuess.Length; i++)
			{
				if (wordleGuess[i] == Wordle[i]) correctness[i] = Correctness.Correct;
				else if (!Wordle.Contains(wordleGuess[i])) correctness[i] = Correctness.Absent;
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
	public class NoGuessesException : Exception
	{
		public NoGuessesException() { }
		public NoGuessesException(string message) : base(message) { }
		public NoGuessesException(string message, Exception inner) : base(message, inner) { }
	}
}
