namespace WordleGame.Exceptions
{
	public class WordleWrongLengthException : Exception
	{
		public WordleWrongLengthException() { }
		public WordleWrongLengthException(string message) : base(message) { }
		public WordleWrongLengthException(string message, Exception inner) : base(message, inner) { }
	}
}
