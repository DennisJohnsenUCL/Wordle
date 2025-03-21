namespace WordleGame.Exceptions
{
	public class WordleGameAlreadyStartedException : Exception
	{
		public WordleGameAlreadyStartedException() { }
		public WordleGameAlreadyStartedException(string message) : base(message) { }
		public WordleGameAlreadyStartedException(string message, Exception inner) : base(message, inner) { }
	}
}
