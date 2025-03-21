namespace WordleGame.Exceptions
{
	public class NoGuessesException : Exception
	{
		public NoGuessesException() { }
		public NoGuessesException(string message) : base(message) { }
		public NoGuessesException(string message, Exception inner) : base(message, inner) { }
	}
}
