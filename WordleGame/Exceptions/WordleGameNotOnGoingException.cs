namespace WordleGame.Exceptions
{
	public class WordleGameNotOnGoingException : Exception
	{
		public WordleGameNotOnGoingException() { }
		public WordleGameNotOnGoingException(string message) : base(message) { }
		public WordleGameNotOnGoingException(string message, Exception inner) : base(message, inner) { }
	}
}
