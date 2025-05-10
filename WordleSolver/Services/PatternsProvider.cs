using System.Diagnostics;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Services
{
	internal class PatternsProvider : IPatternsProvider
	{
		private readonly Word[] _words;
		private readonly ushort[,] _PatternsIndexMatrix;
		private readonly Word[] _patterns;
		private readonly Dictionary<Word, int> _wordsReverseLookup;
		private readonly Dictionary<Word, ushort> _patternsReverseLookup;

		public PatternsProvider(Word[] words)
		{
			_words = words;

			_PatternsIndexMatrix = new ushort[_words.Length, _words.Length];

			_patterns = GeneratePatterns();
			_wordsReverseLookup = GenerateWordsReverseLookup();
			_patternsReverseLookup = GeneratePatternsReverseLookup();

			var timer = new Stopwatch();
			timer.Start();
			Compute();
			timer.Stop();
			Console.WriteLine("Time to pre-compute patterns: " + timer.ElapsedMilliseconds + "\n");
		}

		public Word GetPattern(Word guess, Word wordle)
		{
			var guessIndex = _wordsReverseLookup[guess];
			var wordleIndex = _wordsReverseLookup[wordle];
			var patternIndex = _PatternsIndexMatrix[guessIndex, wordleIndex];
			var pattern = _patterns[patternIndex];
			return pattern;
		}

		private static Word[] GeneratePatterns()
		{
			char[] letters = ['A', 'P', 'C', 'O'];

			var patterns = letters
				.SelectMany(c1 => letters
				.SelectMany(c2 => letters
				.SelectMany(c3 => letters
				.SelectMany(c4 => letters
				.Select(c5 => (Word)$"{c1}{c2}{c3}{c4}{c5}"))))).ToArray();

			return patterns;
		}

		private Dictionary<Word, int> GenerateWordsReverseLookup()
		{
			var reverseLookup = new Dictionary<Word, int>();

			for (int i = 0; i < _words.Length; i++)
			{
				reverseLookup.Add(_words[i], i);
			}
			return reverseLookup;
		}

		private Dictionary<Word, ushort> GeneratePatternsReverseLookup()
		{
			var reverseLookup = new Dictionary<Word, ushort>();

			for (int i = 0; i < _patterns.Length; i++)
			{
				reverseLookup.Add(_patterns[i], (ushort)i);
			}
			return reverseLookup;
		}

		private void Compute()
		{
			Parallel.For(0, _words.Length, i =>
			{
				for (int j = 0; j < _words.Length; j++)
				{
					var pattern = CalculatePattern(_words[i], _words[j]);

					var patternIndex = _patternsReverseLookup[pattern];

					_PatternsIndexMatrix[i, j] = patternIndex;
				}
			});
		}

		private static Word CalculatePattern(Word guess, Word wordle)
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
			return buffer;
		}
	}
}
