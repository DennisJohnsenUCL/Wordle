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

		internal void AddHintsFromResponse(WordleResponse response)
		{
			for (int i = 0; i < response.Chars.Length; i++)
			{
				if (response.Correctness[i] == Correctness.Absent)
				{
					AddToAbsent(response.Chars[i]);
				}

				else if (response.Correctness[i] == Correctness.Present)
				{
					AddToPresent(response.Chars[i]);
				}

				else if (response.Correctness[i] == Correctness.Correct)
				{
					AddToCorrect(response.Chars[i]);
				}
			}
		}
	}
}
