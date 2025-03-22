namespace WordleCore.Exceptions
{
	public class WordleNotAllowedWordException : Exception
	{
		public WordleNotAllowedWordException() { }
		public WordleNotAllowedWordException(string message) : base(message) { }
		public WordleNotAllowedWordException(string message, Exception inner) : base(message, inner) { }
	}
}
