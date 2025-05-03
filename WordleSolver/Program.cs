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

            List<string> words = [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words.txt")];
            List<string> sortedWords = [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words_sorted.txt")];
            List<string> wordles = [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt")];

            var guesses = int.MaxValue;
            var gameFactory = new WordleGameFactory();

            var patternsProvider = new PatternsProvider(sortedWords);

            var solvers = new List<ISolver>()
            {
                //new LazyRandomSolver(),
                //new LazySortedSolver(),
                //new FilteredSortedSolver(staticFirstGuessProvider, new ConstraintManager()),
                new EntropySolver(staticFirstGuessProvider, new ConstraintManager(), patternsProvider),
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
