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

			for (int i = 0; i < 5; i++)
			{
				if (guess[i] == wordle[i]) buffer[i] = 'C';
				else if (!wordle.Contains(guess[i])) buffer[i] = 'A';
				else
				{
					int total = wordle.Count(x => x == guess[i]);
					int correctAfter = wordle.Where((x, j) => j > i && x == guess[i] && x == guess[j]).Count();
					int countUpTo = guess[..(i + 1)].Count(x => x == guess[i]);

					if (total - correctAfter >= countUpTo) buffer[i] = 'P';
					else buffer[i] = 'O';
				}
			}
			return buffer.ToString();
		}
	}
}
