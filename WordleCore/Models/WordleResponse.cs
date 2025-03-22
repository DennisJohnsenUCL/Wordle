using WordleCore.Enums;

namespace WordleCore.Models
{
	public class WordleResponse
	{
		public char[] Chars { get; }
		public Correctness[] Correctness { get; }
		public WordleResponse(char[] chars, Correctness[] correctness)
		{
			Chars = chars;
			Correctness = correctness;
		}
	}
}
