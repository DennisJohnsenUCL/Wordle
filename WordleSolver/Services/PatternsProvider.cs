using System.Diagnostics;
using WordleCore.Enums;
using WordleCore.Utils;
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

        public string GetPattern(int guessIndex, string wordle)
        {
            var jindex = _wordsReverseLookup[wordle];
            var index = _PatternsIndexMatrix[guessIndex, jindex];
            var pattern = _patterns[index];
            return pattern;
        }

        private static string[] GeneratePatterns()
        {
            var letters = CorrectnessMappings.Values.ToArray();

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
                var word1 = _words[i];

                for (int j = 0; j < _words.Count; j++)
                {
                    var word2 = _words[j];

                    var results = WordleGameUtils.GetCorrectnesses(word2, word1);
                    var pattern = string.Concat(results.Select(result => CorrectnessMappings[result.Correctness]));

                    var patternIndex = _patternsReverseLookup[pattern];

                    _PatternsIndexMatrix[i, j] = patternIndex;
                }
            });
        }

        private static readonly Dictionary<Correctness, char> CorrectnessMappings = new()
        {
            { Correctness.Absent, 'A' },
            { Correctness.Present, 'P' },
            { Correctness.Correct, 'C' },
            { Correctness.OverCount, 'O' },
        };
    }
}
