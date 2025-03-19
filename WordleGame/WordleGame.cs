namespace WordleGame
{
	public class WordleGame
	{
		public string Wordle { get; }
		public int Guesses { get; }
		public int GuessesLeft { get; private set; }
		public GameState GameState { get; private set; }
	}
	public enum GameState
	{
		NotStarted,
		Ongoing,
		Completed,
		Failed
	}
}
