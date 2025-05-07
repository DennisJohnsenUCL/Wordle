using System.Collections.Concurrent;
using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropySolver : FilteredSortedSolver
    {
        public override string Identifier { get; } = "EntropySolver, uses information theory to calculate the guess with most information";
        private string? _lastPattern;
        protected HashSet<string> GuessedWords { get; protected private set; } = [];
        private readonly Dictionary<string, string> CachedBestSecond = [];
        protected IPatternsProvider PatternsProvider { get; }
        protected virtual int Limit { get; protected private set; } = 20;

        public EntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, string[] words)
            : base(firstGuessProvider, constraintManager, words)
        {
            PatternsProvider = patternsProvider;
        }

        public override void AddResponse(WordleResponse response)
        {
            _lastPattern = string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
            base.AddResponse(response);
        }

        public override string GetNextGuess()
        {
            var possibleWords = GetPossibleWords();

            if (possibleWords.Count < Limit) return possibleWords[0];

            if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;

            var entropies = GetEntropies(possibleWords);

            var guess = entropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;

            if (GuessedWords.Count == 1) CachedBestSecond.Add(_lastPattern!, guess);

            GuessedWords.Add(guess);
            return guess;
        }

        protected virtual List<string> GetPossibleWords()
        {
            var possibleWords = new List<string>();

            foreach (var word in Words)
            {
                if (GuessedWords.Contains(word)) continue;

                if (FitsConstraints(word))
                {
                    possibleWords.Add(word);
                }
            }
            return possibleWords;
        }

        protected virtual bool TryGetCachedGuess(out string cachedGuess)
        {
            if (GuessedWords.Count == 1 && CachedBestSecond.TryGetValue(_lastPattern!, out var value))
            {
                GuessedWords.Add(value);
                cachedGuess = value;
                return true;
            }
            cachedGuess = string.Empty;
            return false;
        }

        protected virtual ConcurrentDictionary<string, double> GetEntropies(List<string> possibleWords)
        {
            ConcurrentDictionary<string, double> entropies = [];

            Parallel.For(0, Words.Length, i =>
            {
                var word = Words[i];

                if (GuessedWords.Contains(word)) return;

                Dictionary<string, int> patternGroups = [];

                for (int j = 0; j < possibleWords.Count; j++)
                {
                    var possibleWord = possibleWords[j];

                    var pattern = PatternsProvider.GetPattern(i, possibleWord);

                    if (!patternGroups.TryAdd(pattern, 1)) patternGroups[pattern] += 1;
                }

                var probabilities = patternGroups.Select(pattern => (double)pattern.Value / patternGroups.Values.Sum());

                var entropy = probabilities.Sum(probability => probability * Math.Log2(1 / probability));

                entropies.TryAdd(word, entropy);
            });

            return entropies;
        }

        public override string GetFirstGuess()
        {
            GuessedWords.Add(FirstGuess);
            return base.GetFirstGuess();
        }

        public override void Reset()
        {
            _lastPattern = null;
            GuessedWords = [];
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
