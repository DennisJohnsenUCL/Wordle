using WordleCore;
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

            var solvers = new List<ISolver>()
            {
                //new LazyRandomSolver(),
                //new LazySortedSolver(),
                new FilteredSortedSolver(staticFirstGuessProvider, constraintManager),
            };

            var controllers = new List<SolverController>();

            foreach (ISolver solver in solvers)
            {
                List<WordleGame> games = [];

                foreach (string wordle in wordles)
                {
                    games.Add(new WordleGame(wordle, guesses));
                }

                controllers.Add(new SolverController(solver, games));
            }

            var appController = new AppController(controllers);
            appController.Run();
        }
    }
}
