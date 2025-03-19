namespace WordleGame
{
	public class WordleGame
	{
		public string Wordle { get; }
		public int Guesses { get; }
		public int GuessesLeft { get; private set; }
		public GameState GameState { get; private set; }
		public WordleGame(string wordle, int guesses)
		{
			Wordle = wordle;
			Guesses = guesses;
			GuessesLeft = guesses;
			GameState = GameState.NotStarted;
		}
	}
	public enum GameState
	{
		NotStarted,
		Ongoing,
		Completed,
		Failed
	}
}
