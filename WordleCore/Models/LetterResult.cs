using WordleCore.Enums;

namespace WordleCore.Models
{
	public record LetterResult
	{
		public char Letter { get; }
		public Correctness Correctness { get; }

		public LetterResult(char letter, Correctness correctness)
		{
			Letter = letter;
			Correctness = correctness;
		}

		public void Deconstruct(out char letter, out Correctness correctness)
		{
			letter = Letter;
			correctness = Correctness;
		}
	}
}
