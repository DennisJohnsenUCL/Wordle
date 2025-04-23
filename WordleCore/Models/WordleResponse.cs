namespace WordleCore.Models
{
	public record WordleResponse
	{
		private readonly LetterResult[] _letterResults = [];
		public IReadOnlyList<LetterResult> LetterResults { get { return _letterResults; } }

		public WordleResponse(LetterResult[] letterResults)
		{
			_letterResults = letterResults;
		}
	}
}
