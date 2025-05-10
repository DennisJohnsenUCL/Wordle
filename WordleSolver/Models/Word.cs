using System.Collections;
using WordleSolver.Interfaces;

namespace WordleSolver.Models
{
	internal readonly struct Word : IEquatable<Word>, IIndexable<char>, ISliceable<char>, IEnumerable<char>
	{
		private readonly char c0, c1, c2, c3, c4;
		private readonly int _cachedHash;

		public Word(char[] chars)
		{
			if (chars.Length != 5) throw new ArgumentException("Word must be 5 characters long.", nameof(chars));

			c0 = chars[0];
			c1 = chars[1];
			c2 = chars[2];
			c3 = chars[3];
			c4 = chars[4];
			_cachedHash = HashCode.Combine(c0, c1, c2, c3, c4);
		}

		public Word(ReadOnlySpan<char> span)
		{
			if (span.Length != 5) throw new ArgumentException("Word must be 5 characters long.", nameof(span));

			c0 = span[0];
			c1 = span[1];
			c2 = span[2];
			c3 = span[3];
			c4 = span[4];
			_cachedHash = HashCode.Combine(c0, c1, c2, c3, c4);
		}

		public Word(string word)
		{
			if (word.Length != 5) throw new ArgumentException("Word must be 5 characters long.", nameof(word));

			c0 = word[0];
			c1 = word[1];
			c2 = word[2];
			c3 = word[3];
			c4 = word[4];
			_cachedHash = HashCode.Combine(c0, c1, c2, c3, c4);
		}

		public Word(IEnumerable<char> chars)
		{
			ArgumentNullException.ThrowIfNull(chars);

			using var enumerator = chars.GetEnumerator();

			if (!enumerator.MoveNext()) throw new ArgumentException("Not enough characters", nameof(chars));
			c0 = enumerator.Current;

			if (!enumerator.MoveNext()) throw new ArgumentException("Not enough characters", nameof(chars));
			c1 = enumerator.Current;

			if (!enumerator.MoveNext()) throw new ArgumentException("Not enough characters", nameof(chars));
			c2 = enumerator.Current;

			if (!enumerator.MoveNext()) throw new ArgumentException("Not enough characters", nameof(chars));
			c3 = enumerator.Current;

			if (!enumerator.MoveNext()) throw new ArgumentException("Not enough characters", nameof(chars));
			c4 = enumerator.Current;

			if (enumerator.MoveNext()) throw new ArgumentException("Too many characters", nameof(chars));
		}


		public static implicit operator Word(char[] chars) => new(chars);

		public static implicit operator Word(Span<char> span) => new(span);

		public static implicit operator Word(string word) => new(word);

		public char this[int index] => index switch
		{
			0 => c0,
			1 => c1,
			2 => c2,
			3 => c3,
			4 => c4,
			_ => throw new IndexOutOfRangeException()
		};

		public char[] this[Range range]
		{
			get
			{
				int start = range.Start.GetOffset(Length);
				int end = range.End.GetOffset(Length);
				if (start < 0 || end > Length || start > end) throw new ArgumentOutOfRangeException();

				char[] buffer = new char[end - start];
				for (int i = start; i < end; i++) buffer[i - start] = this[i];
				return buffer;
			}
		}

		public int Length => 5;

		public static Word Empty => new("     ");

		public bool Equals(Word other)
		{
			return c0 == other.c0 && c1 == other.c1 && c2 == other.c2 && c3 == other.c3 && c4 == other.c4;
		}

		public override bool Equals(object? obj)
		{
			return obj is Word word && Equals(word);
		}

		public override int GetHashCode() => _cachedHash;

		public override string ToString() => $"{c0}{c1}{c2}{c3}{c4}";

		public char[] ToCharArray() => [c0, c1, c2, c3, c4];

		public static bool operator ==(Word left, Word right) => left.Equals(right);

		public static bool operator !=(Word left, Word right) => !left.Equals(right);

		public char[] Remove(int startIndex, int count)
		{
			if (startIndex + count > Length) throw new ArgumentOutOfRangeException();

			int offset = startIndex + count;
			int j = 0;
			char[] buffer = new char[Length - count];
			for (int i = 0; i < Length; i++)
			{
				if (i < startIndex || i >= offset) buffer[j++] = this[i];
			}
			return buffer;
		}

		public Enumerator GetEnumerator() => new(this);

		IEnumerator<char> IEnumerable<char>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public struct Enumerator : IEnumerator<char>
		{
			private readonly Word _word;
			private int _index;

			public Enumerator(Word word)
			{
				_word = word;
				_index = -1;
			}

			public readonly char Current => _index switch
			{
				0 => _word.c0,
				1 => _word.c1,
				2 => _word.c2,
				3 => _word.c3,
				4 => _word.c4,
				_ => throw new InvalidOperationException()
			};

			readonly object IEnumerator.Current => Current;

			public bool MoveNext() => ++_index < 5;
			public void Reset() => _index = -1;
			public readonly void Dispose() { }
		}
	}
}
