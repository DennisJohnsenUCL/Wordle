using System.Collections.Concurrent;
using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropySolver : FilteredSortedSolver
    {
        public override string Identifier { get; } = "EntropySolver, uses information theory to calculate the guess with most information";
        private string? _lastGuess;
        private string? _lastPattern;
        private List<string> _guessedWords = [];
        private readonly Dictionary<string, string> CachedBestSecond = [];
        private readonly IPatternsProvider _patternsProvider;
        private readonly int _limit = 20;

        public EntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider)
            : base(firstGuessProvider, constraintManager)
        {
            _patternsProvider = patternsProvider;
        }

        public override void AddResponse(WordleResponse response)
        {
            _lastPattern = string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
            base.AddResponse(response);
        }

        public override string GetNextGuess()
        {
            var possibleWords = GetPossibleWords();

            if (possibleWords.Count < _limit) return possibleWords[0];

            if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;

            var entropies = GetEntropies(possibleWords);

            var guess = entropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;

            if (_lastGuess == "SALET") CachedBestSecond.Add(_lastPattern!, guess);

            _guessedWords.Add(guess);
            _lastGuess = guess;
            return guess;
        }

        protected virtual List<string> GetPossibleWords()
        {
            var possibleWords = new List<string>();

            foreach (var word in Words)
            {
                if (_guessedWords.Contains(word)) continue;

                if (FitsConstraints(word))
                {
                    possibleWords.Add(word);
                }
            }
            return possibleWords;
        }

        protected virtual bool TryGetCachedGuess(out string cachedGuess)
        {
            if (_lastGuess == FirstGuess)
            {
                if (CachedBestSecond.TryGetValue(_lastPattern!, out var value))
                {
                    _guessedWords.Add(value);
                    _lastGuess = value;
                    cachedGuess = value;
                    return true;
                }
            }
            cachedGuess = string.Empty;
            return false;
        }

        protected virtual ConcurrentDictionary<string, double> GetEntropies(List<string> possibleWords)
        {
            ConcurrentDictionary<string, double> entropies = [];

            Parallel.For(0, Words.Count, i =>
            {
                var word = Words[i];

                if (_guessedWords.Contains(word)) return;

                Dictionary<string, List<string>> patternGroups = [];

                for (int j = 0; j < possibleWords.Count; j++)
                {
                    var possibleWord = possibleWords[j];

                    var pattern = _patternsProvider.GetPattern(i, possibleWord);

                    if (patternGroups.TryGetValue(pattern, out var value)) value.Add(possibleWord);
                    else patternGroups.Add(pattern, [possibleWord]);
                }

                var probabilities = patternGroups.Select(pattern => (double)pattern.Value.Count / possibleWords.Count);

                var entropy = probabilities.Sum(probability => probability * Math.Log2(1 / probability));

                entropies.TryAdd(word, entropy);
            });

            return entropies;
        }

        public override string GetFirstGuess()
        {
            _guessedWords.Add(FirstGuess);
            _lastGuess = FirstGuess;
            return base.GetFirstGuess();
        }

        public override void Reset()
        {
            _lastGuess = null;
            _lastPattern = null;
            _guessedWords = [];
            base.Reset();
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
