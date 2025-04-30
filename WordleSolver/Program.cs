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
            List<string> wordles = [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt")];

            var staticFirstGuessProvider = new StaticFirstGuessProvider("SALET");
            var constraintManager = new ConstraintManager();

            var solvers = new List<ISolver>()
            {
                //new LazyRandomSolver(),
                //new LazySortedSolver(),
                new FilteredSortedSolver(staticFirstGuessProvider, constraintManager),
            };

            var appController = new AppController(wordles, solvers);
            appController.Run();
        }
    }
}
