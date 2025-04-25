using WordleCore.Utils;
using WordleSolver.Controllers;
using WordleSolver.Solvers;

namespace WordleSolver
{
    public class Program
    {
        public static void Main()
        {
            List<string> wordles = [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt")];
            var wordleSolver = new WordleSolver1();
            var solverController = new SolverController(wordleSolver, wordles);

            var result = solverController.Run();
            Console.WriteLine(result);
        }
    }
}
