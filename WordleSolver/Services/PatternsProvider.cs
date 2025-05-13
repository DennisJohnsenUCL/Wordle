using System.Diagnostics;
using WordleSolver.Interfaces;

namespace WordleSolver.Services
{
	internal class PatternsProvider : IPatternsProvider
	{
		private readonly IReadOnlyList<string> _words;
		private readonly ushort[,] _PatternsIndexMatrix;
		private readonly string[] _patterns;
		private readonly Dictionary<string, int> _wordsReverseLookup;
		private readonly Dictionary<string, ushort> _patternsReverseLookup;

		public PatternsProvider(IReadOnlyList<string> words)
		{
			_words = words;

			_PatternsIndexMatrix = new ushort[_words.Count, _words.Count];

			_patterns = GeneratePatterns();
			_wordsReverseLookup = GenerateWordsReverseLookup();
			_patternsReverseLookup = GeneratePatternsReverseLookup();

			var timer = new Stopwatch();
			timer.Start();
			Compute();
			timer.Stop();
			Console.WriteLine("Time to pre-compute patterns: " + timer.ElapsedMilliseconds + "\n");
		}

		public string GetPattern(string guess, string wordle)
		{
			var guessIndex = _wordsReverseLookup[guess];
			var wordleIndex = _wordsReverseLookup[wordle];
			var patternIndex = _PatternsIndexMatrix[guessIndex, wordleIndex];
			var pattern = _patterns[patternIndex];
			return pattern;
		}

		public int GetWordIndex(string word) => _wordsReverseLookup[word];

		public int GetPatternIndex(string pattern) => _patternsReverseLookup[pattern];

		public bool FitsPattern(int guessIndex, int wordleIndex, int patternIndex)
		{
			return _PatternsIndexMatrix[guessIndex, wordleIndex] == patternIndex;
		}

		private static string[] GeneratePatterns()
		{
			char[] letters = ['A', 'P', 'C', 'O'];

			var patterns = letters
				.SelectMany(c1 => letters
				.SelectMany(c2 => letters
				.SelectMany(c3 => letters
				.SelectMany(c4 => letters
				.Select(c5 => $"{c1}{c2}{c3}{c4}{c5}"))))).ToArray();

			return patterns;
		}

		private Dictionary<string, int> GenerateWordsReverseLookup()
		{
			var reverseLookup = _words
				.Select((word, i) => (word, i))
				.ToDictionary(x => x.word, x => x.i);

			return reverseLookup;
		}

		private Dictionary<string, ushort> GeneratePatternsReverseLookup()
		{
			var reverseLookup = _patterns
				.Select((pattern, i) => (pattern, i))
				.ToDictionary(x => x.pattern, x => (ushort)x.i);

			return reverseLookup;
		}

		private void Compute()
		{
			Parallel.For(0, _words.Count, i =>
			{
				for (int j = 0; j < _words.Count; j++)
				{
					var pattern = CalculatePattern(_words[i], _words[j]);

					var patternIndex = _patternsReverseLookup[pattern];

					_PatternsIndexMatrix[i, j] = patternIndex;
				}
			});
		}

		private static string CalculatePattern(string guess, string wordle)
		{
			Span<char> buffer = stackalloc char[5];
			var frequency = new int[26];

			for (int i = 0; i < 5; i++) frequency[wordle[i] - 'A']++;

			for (int i = 0; i < 5; i++)
			{
				int letterIndex = guess[i] - 'A';
				if (frequency[letterIndex] == 0)
				{
					buffer[i] = 'A';
				}
			}

			for (int i = 0; i < 5; i++)
			{
				if (guess[i] == wordle[i])
				{
					buffer[i] = 'C';
					frequency[guess[i] - 'A']--;
				}
			}

			for (int i = 0; i < 5; i++)
			{
				if (buffer[i] == 'C' || buffer[i] == 'A') continue;

				int letterIndex = guess[i] - 'A';
				if (frequency[letterIndex] > 0)
				{
					buffer[i] = 'P';
					frequency[letterIndex]--;
				}
				else
				{
					buffer[i] = 'O';
				}
			}
			return buffer.ToString();
		}
	}
}
