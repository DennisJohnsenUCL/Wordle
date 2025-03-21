using WordleGame.Enums;
using WordleGame.Exceptions;
using WordleGame.Models;
using WordleGame.Utils;

namespace WordleGame
{
	public class WordleGame
	{
		public string Wordle { get; }
		public int Guesses { get; }
		public int GuessesLeft { get; private set; }
		public GameState GameState { get; private set; }

		public WordleGame() : this(WordleGameUtils.GetRandomWordle(), 6) { }
		public WordleGame(string wordle) : this(wordle, 6) { }
		public WordleGame(int guesses) : this(WordleGameUtils.GetRandomWordle(), guesses) { }
		public WordleGame(string wordle, int guesses)
		{
			if (wordle.Length != 5) throw new WordleWrongLengthException("The Wordle must be 5 letters");
			if (guesses < 1) throw new NoGuessesException("The amount of guesses must be greater than 0");
			if (!WordleGameUtils.allowedWords.Contains(wordle)) throw new WordleNotAllowedWordException("The Wordle is not in the list of allowed words");

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
			else throw new WordleGameAlreadyStartedException("This game has already been started");
		}

		public WordleResponse GuessWordle(string wordleGuess)
		{
			if (GameState != GameState.Ongoing) throw new WordleGameNotOnGoingException("This game is not currently ongoing");
			if (wordleGuess.Length != 5) throw new WordleGuessWrongLengthException("Wordle guesses must be 5 letters");
			if (!WordleGameUtils.allowedWords.Contains(wordleGuess)) throw new WordleNotAllowedWordException("The guessed word is not in the list of allowed words");

			char[] chars = wordleGuess.ToCharArray();

			if (wordleGuess == Wordle)
			{
				GameState = GameState.Completed;
				return new WordleResponse(chars, [.. Enumerable.Repeat(Correctness.Correct, 5)]);
			}

			Correctness[] correctness = new Correctness[5];

			for (int i = 0; i < wordleGuess.Length; i++)
			{
				if (wordleGuess[i] == Wordle[i]) correctness[i] = Correctness.Correct;
				else if (!Wordle.Contains(wordleGuess[i])) correctness[i] = Correctness.Absent;
				//>> I don't think this will work if there are also Correct of the same letter?
				else if (Wordle.Count(x => x == wordleGuess[i]) >= wordleGuess.Substring(0, i + 1).Count(x => x == wordleGuess[i])) correctness[i] = Correctness.Present;
			}

			GuessesLeft--;
			if (GuessesLeft == 0) GameState = GameState.Failed;
			return new WordleResponse(chars, correctness);
		}

		public override string ToString()
		{
			return $"This Wordle game is {GameState}, the Wordle is {Wordle}, " +
				$"and there are {GuessesLeft} guesses left out of {Guesses} guesses";
		}
	}
}
