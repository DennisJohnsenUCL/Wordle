using System.Diagnostics;
using WordleCore.Enums;
using WordleCore.Models;
using WordleCore.Utils;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class EntropySolver : FilteredSortedSolver
    {
        public override string Identifier { get; } = "EntropySolver, uses information theory to calculate the guess with most information";
        private string? _lastGuess;
        private string? _lastPattern;
        private List<string> _guessedWords = [];

        public EntropySolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager)
            : base(firstGuessProvider, constraintManager)
        {
            ComputePatterns();
            MakeReverseLookup();
        }

        private void ComputePatterns()
        {
            var timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < Words.Count; i++)
            {
                for (int j = 0; j < Words.Count; j++)
                {
                    var results = WordleGameUtils.GetCorrectnesses(Words[i], Words[j]);
                    var pattern = string.Concat(results.Select(result => CorrectnessMappings[result.Correctness]));

                    if (!AllPatterns.Contains(pattern)) AllPatterns.Add(pattern);
                    var index = AllPatterns.IndexOf(pattern);
                    PatternsIndices[i, j] = (ushort)index;
                }
            }
            timer.Stop();
            Console.WriteLine("Time to compute patterns: " + timer.ElapsedMilliseconds + "\n");
        }

        private void MakeReverseLookup()
        {
            ReverseLookup = Words.Select((word, i) => (word, i)).ToDictionary(x => x.word, x => x.i);
        }

        public override void AddResponse(WordleResponse response)
        {
            _lastPattern = string.Concat(response.LetterResults.Select(result => CorrectnessMappings[result.Correctness]));
            base.AddResponse(response);
        }

        public override string GetNextGuess()
        {
            List<string> possibleWords = [];
            Dictionary<string, double> entropies = [];

            // Can achieve this by checking constraint count instead of state tracking last guess and pattern
            if (_lastGuess == "SALET" && _lastPattern != null)
            {
                if (CachedBestSecond.TryGetValue(_lastPattern, out var value))
                {
                    _guessedWords.Add(value);
                    _lastGuess = value;
                    return value;
                }
            }

            foreach (var word in Words)
            {
                if (FitsConstraints(word))
                {
                    possibleWords.Add(word);
                }
            }

            // Maybe don't do this? Check followup video, 3b1b does not do it in first video? Maybe just 1
            if (possibleWords.Count < 25)
            {
                foreach (var word in possibleWords)
                {
                    if (!_guessedWords.Contains(word))
                    {
                        Console.WriteLine("Guessing: " + possibleWords[0]);
                        return possibleWords[0];
                    }
                }
            }

            for (int i = 0; i < Words.Count; i++)
            {
                var word = Words[i];

                // Find a better name for this one
                Dictionary<string, List<string>> patterns = [];

                // Parallelize?
                // If state is stored between runs (_lastguess == salet for precomputed entropy usage)
                // Maybe parallelize in the solvercontroller? It does not reset state between runs,
                // But it uses the same solver instance so that state would still concur
                // Maybe instead find a way to get rid of that state
                // Just keeps a growing list (no specific order, so no issue)
                // But yes issue because solver resets constraintmanager
                // So only way to parallelize would be to do one solver instance per game and have solver control the game in run
                for (int j = 0; j < possibleWords.Count; j++)
                {
                    var possibleWord = possibleWords[j];
                    // Have to do this since index in possiblewords is not corresponding to index in words
                    // But i need index in words for matrix lookup
                    // Easily fixed by iterating over Words and doing constraint checking in this loop
                    // Since I am doing it on all words anyways
                    // Just have to build the list that I need after the loop

                    // Pre-compute patterns? First make sure it is faster

                    var index = PatternsIndices[i, ReverseLookup[possibleWord]];
                    var pattern = AllPatterns[index];

                    if (patterns.TryGetValue(pattern, out List<string>? value)) value.Add(possibleWord);
                    else patterns.Add(pattern, [possibleWord]);
                }

                // Use pattern frequencies instead of amounts (for next algo?)
                var probabilities = patterns.Select(pattern => (double)pattern.Value.Count / possibleWords.Count);

                var entropy = probabilities.Sum(probability => probability * Math.Log2(1 / probability));

                entropies.Add(word, entropy);
            }

            foreach (var entropy in entropies)
            {
                if (_guessedWords.Contains(entropy.Key)) entropies.Remove(entropy.Key);
            }

            var guess = entropies.Aggregate((acc, current) => acc.Value > current.Value ? acc : current).Key;

            // Highest entropies (best second guesses) can be precomputed knowing only the pattern
            // As the only thing you need to calculate them is the list of possible words given the constraints
            // Or pattern, in the case of only one guess being made is here since that's a small pool
            // Make constraintmanager factory to pass separate to solvers and to entropy precomputer
            // Use precomputed patterns

            // ABOVE IS WRONG
            // It depends on opener because the constraints make the possible words,
            // And those are based on the pattern gained from guessing salet
            // But only 243 patterns because no overcount is posssible in salet since no repeated letters
            if (_lastPattern != null && _lastGuess == "SALET")
            {
                CachedBestSecond.Add(_lastPattern, guess);
            }

            _guessedWords.Add(guess);
            _lastGuess = guess;
            Console.WriteLine("Guessing: " + guess);
            return guess;
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

            Console.WriteLine("Cached best second guesses: " + CachedBestSecond.Count);

            base.Reset();
        }

        private static readonly ushort[,] PatternsIndices = new ushort[15000, 15000];

        private static readonly List<string> AllPatterns = [];

        private static Dictionary<string, int> ReverseLookup = [];

        private static readonly Dictionary<string, string> CachedBestSecond = [];

        private static readonly Dictionary<Correctness, char> CorrectnessMappings = new()
        {
            { Correctness.Absent, 'A' },
            { Correctness.Present, 'P' },
            { Correctness.Correct, 'C' },
            { Correctness.OverCount, 'O' },
        };
    }
}
