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
        private readonly IPatternsProvider _patternsProvider;

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
            List<string> possibleWords = [];
            Dictionary<string, double> entropies = [];

            foreach (var word in Words)
            {
                if (FitsConstraints(word))
                {
                    possibleWords.Add(word);
                }
            }

            if (possibleWords.Count < 20)
            {
                foreach (var word in possibleWords)
                {
                    if (!_guessedWords.Contains(word))
                    {
                        return possibleWords[0];
                    }
                }
            }

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

            for (int i = 0; i < Words.Count; i++)
            {
                var word = Words[i];

                // Find a better name for this one
                Dictionary<string, List<string>> patterns = [];

                for (int j = 0; j < possibleWords.Count; j++)
                {
                    var possibleWord = possibleWords[j];

                    var pattern = _patternsProvider.GetPattern(i, possibleWord);

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

            if (_lastPattern != null && _lastGuess == "SALET")
            {
                CachedBestSecond.Add(_lastPattern, guess);
            }

            _guessedWords.Add(guess);
            _lastGuess = guess;
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

            base.Reset();
        }

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
