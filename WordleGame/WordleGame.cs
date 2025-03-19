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
	}
	public enum GameState
	{
		NotStarted,
		Ongoing,
		Completed,
		Failed
	}
}
