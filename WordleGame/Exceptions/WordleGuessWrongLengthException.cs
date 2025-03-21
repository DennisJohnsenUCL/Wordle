namespace WordleGame.Exceptions
{
	public class WordleGuessWrongLengthException : Exception
	{
		public WordleGuessWrongLengthException() { }
		public WordleGuessWrongLengthException(string message) : base(message) { }
		public WordleGuessWrongLengthException(string message, Exception inner) : base(message, inner) { }
	}
}
