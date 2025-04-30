using WordleCore.Utils;
using WordleSolver.Controllers;
using WordleSolver.Core;
using WordleSolver.Interfaces;
using WordleSolver.Services;
using WordleSolver.Solvers;

namespace WordleSolver
{
    public class Program
    {
        public static void Main()
        {
            var staticFirstGuessProvider = new StaticFirstGuessProvider("SALET");
            var constraintManager = new ConstraintManager();
            List<string> wordles = [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt")];
            var guesses = int.MaxValue;
            var gameFactory = new WordleGameFactory();

            var solvers = new List<ISolver>()
            {
                //new LazyRandomSolver(),
                //new LazySortedSolver(),
                new FilteredSortedSolver(staticFirstGuessProvider, constraintManager),
            };

            var controllers = new List<SolverController>();

            foreach (ISolver solver in solvers)
            {
                var games = gameFactory.CreateGames(wordles, guesses);
                var controller = new SolverController(solver, games);

                controllers.Add(controller);
            }

            var appController = new AppController(controllers);
            appController.Run();
        }
    }
}
