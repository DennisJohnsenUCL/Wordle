using WordleSolver.Core;
using WordleSolver.Interfaces;
using WordleSolver.Solvers;

namespace WordleSolver.Services
{
    internal class SolverFactory
    {
        private readonly string[] _words;
        private readonly Dictionary<string, long> _sortedWordOccurrences;
        private readonly IEnumerable<string> _activeSolvers;
        private readonly IFirstGuessProvider _firstGuessProvider;
        private readonly IPatternsProvider _patternsProvider;

        public SolverFactory(string[] words, Dictionary<string, long> sortedWordOccurrences, IEnumerable<string> activeSolvers, IFirstGuessProvider firstGuessProvider, IPatternsProvider patternsProvider)
        {
            _words = words;
            _sortedWordOccurrences = sortedWordOccurrences;
            _activeSolvers = activeSolvers;
            _firstGuessProvider = firstGuessProvider;
            _patternsProvider = patternsProvider;
        }

        public List<ISolver> GetSolvers()
        {
            var solvers = new List<ISolver>();

            foreach (var activeSolver in _activeSolvers)
            {
                switch (activeSolver)
                {
                    case "random":
                        var random = new LazyRandomSolver(_words);
                        solvers.Add(random);
                        break;
                    case "sorted":
                        var sorted = GetLazySortedSolver();
                        solvers.Add(sorted);
                        break;
                    case "filtered":
                        var filtered = GetFilteredSortedSolver();
                        solvers.Add(filtered);
                        break;
                    case "entropy":
                        var entropy = GetEntropySolver();
                        solvers.Add(entropy);
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
    }
}
