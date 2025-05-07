using WordleSolver.Core;
using WordleSolver.Interfaces;
using WordleSolver.Solvers;

namespace WordleSolver.Services
{
    internal class SolverFactory
    {
        private readonly string[] _words;
        private readonly Dictionary<string, long> _sortedWordOccurrences;
        private readonly IFirstGuessProvider _firstGuessProvider;
        private readonly IPatternsProvider _patternsProvider;

        public SolverFactory(string[] words, Dictionary<string, long> sortedWordOccurrences, IFirstGuessProvider firstGuessProvider, IPatternsProvider patternsProvider)
        {
            _words = words;
            _sortedWordOccurrences = sortedWordOccurrences;
            _firstGuessProvider = firstGuessProvider;
            _patternsProvider = patternsProvider;
        }

        public List<ISolver> GetSolvers(IEnumerable<SolverTypes> solversToGet)
        {
            var solvers = new List<ISolver>();

            foreach (var solver in solversToGet)
            {
                switch (solver)
                {
                    case SolverTypes.Random:
                        var random = new LazyRandomSolver(_words);
                        solvers.Add(random);
                        break;
                    case SolverTypes.Sorted:
                        var sorted = GetLazySortedSolver();
                        solvers.Add(sorted);
                        break;
                    case SolverTypes.Filtered:
                        var filtered = GetFilteredSortedSolver();
                        solvers.Add(filtered);
                        break;
                    case SolverTypes.Entropy:
                        var entropy = GetEntropySolver();
                        solvers.Add(entropy);
                        break;
                    case SolverTypes.Frequency:
                        var frequency = GetEntropyFrequencySolver();
                        solvers.Add(frequency);
                        break;
                    case SolverTypes.Sigmoid:
                        var sigmoid = GetEntropyFrequencySigmoidSolver();
                        solvers.Add(sigmoid);
                        break;
                    case SolverTypes.Log:
                        var log = GetEntropyFrequencyLogSolver();
                        solvers.Add(log);
                        break;
                    default:
                        break;
                }
            }
            return solvers;
        }

        private LazySortedSolver GetLazySortedSolver()
        {
            var words = _sortedWordOccurrences.Keys.ToArray();
            var solver = new LazySortedSolver(words);
            return solver;
        }

        private FilteredSortedSolver GetFilteredSortedSolver()
        {
            var words = _sortedWordOccurrences.Keys.ToArray();
            var constraintManager = new ConstraintManager();
            var solver = new FilteredSortedSolver(_firstGuessProvider, constraintManager, words);
            return solver;
        }

        private EntropySolver GetEntropySolver()
        {
            var words = _sortedWordOccurrences.Keys.ToArray();
            var constraintManager = new ConstraintManager();
            var solver = new EntropySolver(_firstGuessProvider, constraintManager, _patternsProvider, words);
            return solver;
        }

        private EntropyFrequencySolver GetEntropyFrequencySolver()
        {
            var words = _sortedWordOccurrences.Keys.ToArray();
            var total = _sortedWordOccurrences.Values.Sum();
            var wordFrequencies = _sortedWordOccurrences.ToDictionary(x => x.Key, x => (double)x.Value / total);
            var constraintManager = new ConstraintManager();
            var solver = new EntropyFrequencySolver(_firstGuessProvider, constraintManager, _patternsProvider, wordFrequencies, words);
            return solver;
        }

        private EntropyFrequencySigmoidSolver GetEntropyFrequencySigmoidSolver()
        {
            var words = _sortedWordOccurrences.Keys.ToArray();
            int m = 5000000;
            int s = 1000000;
            var wordSigmoidFrequency = new Dictionary<string, double>();

            foreach (var key in _sortedWordOccurrences.Keys)
            {
                var value = _sortedWordOccurrences[key];

                var exponent = -(double)(value - m) / s;
                var newValue = 1.0 / (1.0 + Math.Exp(exponent));

                wordSigmoidFrequency[key] = newValue;
            }

            double total = wordSigmoidFrequency.Values.Sum();

            var normalizedWordSigmoidFrequency = wordSigmoidFrequency.ToDictionary(x => x.Key, x => x.Value / total);

            var constraintManager = new ConstraintManager();
            var solver = new EntropyFrequencySigmoidSolver(_firstGuessProvider, constraintManager, _patternsProvider, normalizedWordSigmoidFrequency, words);
            return solver;
        }

        private EntropyFrequencyLogSolver GetEntropyFrequencyLogSolver()
        {
            var words = _sortedWordOccurrences.Keys.ToArray();
            var wordLogFrequency = new Dictionary<string, double>();

            foreach (var key in _sortedWordOccurrences.Keys)
            {
                var value = _sortedWordOccurrences[key];
                var newValue = Math.Log(1 + value);

                wordLogFrequency[key] = newValue;
            }

            double total = wordLogFrequency.Values.Sum();

            var normalizedWordLogFrequency = wordLogFrequency.ToDictionary(x => x.Key, x => x.Value / total);

            var constraintManager = new ConstraintManager();
            var solver = new EntropyFrequencyLogSolver(_firstGuessProvider, constraintManager, _patternsProvider, normalizedWordLogFrequency, words);
            return solver;
        }
    }
}
