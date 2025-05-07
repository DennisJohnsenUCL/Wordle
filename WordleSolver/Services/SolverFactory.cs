using WordleSolver.Interfaces;
using WordleSolver.Solvers;

namespace WordleSolver.Services
{
    internal class SolverFactory
    {
        private string[] _words;
        private Dictionary<string, long> _sortedWordOccurrences;
        private IEnumerable<string> _activeSolvers;

        public SolverFactory(string[] words, Dictionary<string, long> sortedWordOccurrences, IEnumerable<string> activeSolvers)
        {
            _words = words;
            _sortedWordOccurrences = sortedWordOccurrences;
            _activeSolvers = activeSolvers;
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
                    default:
                        break;
                }
            }

            return solvers;
        }
    }
}
