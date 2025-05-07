using WordleCore.Utils;
using WordleSolver.Controllers;
using WordleSolver.Interfaces;
using WordleSolver.Services;

namespace WordleSolver
{
    public class Program
    {
        public static void Main()
        {
            var staticFirstGuessProvider = new StaticFirstGuessProvider("SALET");

            string[] words = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words.txt");

            var sortedWordOccurrences = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words_sorted_frequencies.txt")
                .Select(line => line.Split('\t'))
                .ToDictionary(parts => parts[0], parts => long.Parse(parts[1]));

            string[] wordles = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt");

            var guesses = int.MaxValue;
            var gameFactory = new WordleGameFactory();
            var patternsProvider = new PatternsProvider([.. sortedWordOccurrences.Keys]);

            SolverTypes[] activeSolvers =
                [SolverTypes.Random,
                SolverTypes.Sorted,
                SolverTypes.Filtered,
                SolverTypes.Entropy,
                SolverTypes.Frequency,
                SolverTypes.Sigmoid,
                SolverTypes.Log];

            var solverFactory = new SolverFactory(
                words,
                sortedWordOccurrences,
                activeSolvers,
                staticFirstGuessProvider,
                patternsProvider
                );

            var solvers = solverFactory.GetSolvers();

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
