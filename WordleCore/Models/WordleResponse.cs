using System.Collections.Immutable;
using WordleCore.Enums;

namespace WordleCore.Models
{
	public record WordleResponse
	{
		public ImmutableArray<char> Chars { get; }
		public ImmutableArray<Correctness> Correctness { get; }

		public WordleResponse(char[] chars, Correctness[] correctness)
		{
			Chars = [.. chars];
			Correctness = [.. correctness];
		}
	}
}
