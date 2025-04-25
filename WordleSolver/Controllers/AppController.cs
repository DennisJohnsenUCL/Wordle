using WordleSolver.Interfaces;

namespace WordleSolver.Controllers
{
    internal class AppController
    {
        private IEnumerable<string> _wordles;
        private IEnumerable<IWordleSolver> _solvers;

        public AppController(IEnumerable<string> wordles, IEnumerable<IWordleSolver> solvers)
        {
            _wordles = wordles;
            _solvers = solvers;
        }

        public void Run()
        {
            foreach (var solver in _solvers)
            {
                var solverController = new SolverController(solver, _wordles);
                var result = solverController.Run();
                Console.WriteLine(result);
            }
        }
    }
}
