using WordleCore.Utils;
using WordleSolver.Controllers;
using WordleSolver.Enums;
using WordleSolver.Interfaces;
using WordleSolver.Services;

namespace WordleSolver
{
	public class Program
	{
		public static void Main()
		{
			var staticFirstGuessProvider = new StaticFirstGuessProvider("SALET");

			string[] wordles = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt");

			var sortedWordOccurrences = WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words_sorted_occurrences.txt")
				.Select(line => line.Split('\t'))
				.ToDictionary(parts => parts[0], parts => long.Parse(parts[1]));

			var patternsProvider = new PatternsProvider([.. sortedWordOccurrences.Keys], Patterns.Simple);

			var context = new SolverContext(staticFirstGuessProvider, patternsProvider, sortedWordOccurrences, wordles, AnswerPools.OnlyWordles);

			var guesses = int.MaxValue;
			var gameFactory = new WordleGameFactory();

			SolverTypes[] solversToGet =
				[
				SolverTypes.Lazy,
				SolverTypes.Filtered,
				SolverTypes.EntropyFlat,
				SolverTypes.EntropyWeighted,
				SolverTypes.EntropySigmoid,
				SolverTypes.EntropyLog,
				SolverTypes.PositionalFlat,
				SolverTypes.PositionalWeighted,
				SolverTypes.PositionalSigmoid,
				SolverTypes.PositionalLog,
				SolverTypes.FrequencyThreshold,
				SolverTypes.MiniMax,
				];

			var solverFactory = new SolverFactory(context);

			var solvers = solverFactory.GetSolvers(solversToGet);

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
