using WordleCore;
using WordleCore.Enums;
using WordleSolver.Interfaces;

namespace WordleSolver.Controllers
{
    internal class SolverController
    {
        private readonly IWordleSolver _solver;
        private readonly List<string> _wordles;
        private const int _guesses = int.MaxValue;

        public SolverController(IWordleSolver solver, List<string> wordles)
        {
            _solver = solver;
            _wordles = wordles;
        }

        public void Run()
        {
            foreach (string wordle in _wordles)
            {
                var game = new WordleGame(wordle, _guesses);
                game.Start();
                var guess = _solver.GetFirstGuess();
                var response = game.GuessWordle(guess);
                if (IsGameOver(game)) { _solver.Reset(); continue; }
                while (true)
                {
                    _solver.AddResponse(response);
                    guess = _solver.GetNextGuess();
                    response = game.GuessWordle(guess);
                    if (IsGameOver(game)) { _solver.Reset(); break; }
                }
            }
        }

        private static bool IsGameOver(WordleGame game)
        {
            return (game.GameState == GameState.Completed || game.GameState == GameState.Failed);
        }
    }
}
