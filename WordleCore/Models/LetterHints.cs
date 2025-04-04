using WordleCore.Enums;

namespace WordleCore.Models
{
	public class LetterHints
	{
		private readonly HashSet<char> _absent = [];
		private readonly HashSet<char> _present = [];
		private readonly HashSet<char> _correct = [];

		public IReadOnlySet<char> Absent { get { return _absent; } }
		public IReadOnlySet<char> Present { get { return _present; } }
		public IReadOnlySet<char> Correct { get { return _correct; } }

		public LetterHints() { }

		private void AddToAbsent(char c)
		{
			_absent.Add(c);
		}
		private void AddToPresent(char c)
		{
			if (!_correct.Contains(c)) _present.Add(c);
		}
		private void AddToCorrect(char c)
		{
			_present.Remove(c);
			_correct.Add(c);
		}

		internal void AddHintsFromResponse(WordleResponse wordleResponse)
		{
			for (int i = 0; i < wordleResponse.Chars.Length; i++)
			{
				if (wordleResponse.Correctness[i] == Correctness.Absent)
				{
					AddToAbsent(wordleResponse.Chars[i]);
				}

				else if (wordleResponse.Correctness[i] == Correctness.Present)
				{
					AddToPresent(wordleResponse.Chars[i]);
				}

				else if (wordleResponse.Correctness[i] == Correctness.Correct)
				{
					AddToCorrect(wordleResponse.Chars[i]);
				}
			}
		}
	}
}
