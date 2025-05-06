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

            string[] words = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words.txt");
            string[] sortedWords = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words_sorted.txt");

            var sortedWordFrequencies = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words_sorted_frequencies.txt")
                .Select(line => line.Split('\t'))
                .ToDictionary(parts => parts[0], parts => double.Parse(parts[2]));

            var sortedWordOccurrences = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words_sorted_frequencies.txt")
                .Select(line => line.Split('\t'))
                .ToDictionary(parts => parts[0], parts => long.Parse(parts[1]));

            string[] wordles = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt");

            var guesses = int.MaxValue;
            var gameFactory = new WordleGameFactory();

            var patternsProvider = new PatternsProvider(sortedWords);

            var solvers = new List<ISolver>()
            {
                //new LazyRandomSolver(),
                //new LazySortedSolver(),
                //new FilteredSortedSolver(staticFirstGuessProvider, new ConstraintManager()),
                //new EntropySolver(staticFirstGuessProvider, new ConstraintManager(), patternsProvider),
                new EntropyFrequencySolver(staticFirstGuessProvider, new ConstraintManager(), patternsProvider, sortedWordFrequencies),
                //new EntropyFrequencySigmoidSolver(staticFirstGuessProvider, new ConstraintManager(), patternsProvider, sortedWordOccurrences),
                //new EntropyFrequencyLogSolver(staticFirstGuessProvider, new ConstraintManager(), patternsProvider, sortedWordOccurrences),
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
