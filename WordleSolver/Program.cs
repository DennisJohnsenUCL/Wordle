using WordleCore.Utils;
using WordleSolver.Controllers;
using WordleSolver.Interfaces;
using WordleSolver.Solvers;

namespace WordleSolver
{
    public class Program
    {
        public static void Main()
        {
            List<string> wordles = [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.previous_wordles.txt")];
            var solvers = new List<IWordleSolver>()
            {
                new WordleSolver1(),
                new WordleSolver2()
            };

            var appController = new AppController(wordles, solvers);
            appController.Run();
        }
    }
}
