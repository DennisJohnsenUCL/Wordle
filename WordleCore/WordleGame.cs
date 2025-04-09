using WordleCore.Enums;
using WordleCore.Exceptions;
using WordleCore.Models;
using WordleCore.Utils;

namespace WordleCore
{
	public class WordleGame
	{
		public string Wordle { get; }
		public int Guesses { get; }
		public int GuessesLeft { get; private set; } = 0;
		public GameState GameState { get; private set; } = GameState.NotStarted;
		public LetterHints LetterHints { get; } = new();

		public WordleGame() : this(WordleGameUtils.GetRandomWordle(), 6) { }
		public WordleGame(string wordle) : this(wordle, 6) { }
		public WordleGame(int guesses) : this(WordleGameUtils.GetRandomWordle(), guesses) { }
		public WordleGame(string wordle, int guesses)
		{
			if (wordle.Length != 5) throw new WordleWrongLengthException("The Wordle must be 5 letters");
			if (guesses < 1) throw new NoGuessesException("The amount of guesses must be greater than 0");
			if (!WordleGameUtils.IsAllowedWord(wordle)) throw new WordleNotAllowedWordException("The Wordle is not in the list of allowed words");

			Wordle = wordle;
			Guesses = guesses;
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

		public WordleResponse GuessWordle(string guess)
		{
			if (GameState != GameState.Ongoing) throw new WordleGameNotOnGoingException("This game is not currently ongoing");
			if (guess.Length != 5) throw new WordleGuessWrongLengthException("Wordle guesses must be 5 letters");
			if (!WordleGameUtils.IsAllowedWord(guess)) throw new WordleNotAllowedWordException("The guessed word is not in the list of allowed words");

			char[] chars = guess.ToCharArray();

			Correctness[] correctness = WordleGameUtils.GetCorrectnesses(Wordle, guess);

			GuessesLeft--;

			SetGameState(guess);
			WordleResponse response = new(chars, correctness);
			AddToLetterHints(response);
			return response;
		}

		private void AddToLetterHints(WordleResponse response)
		{
			LetterHints.AddHintsFromResponse(response);
		}

		internal void SetGameState(string guess)
		{
			if (Wordle == guess) GameState = GameState.Completed;
			else if (GuessesLeft == 0) GameState = GameState.Failed;
		}

		public override string ToString()
		{
			return $"This Wordle game is {GameState}, the Wordle is {Wordle}, " +
				$"and there are {GuessesLeft} guesses left out of {Guesses} guesses";
		}
	}
}
