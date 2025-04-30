using WordleCore;
using WordleCore.Enums;
using WordleSolver.Interfaces;

namespace WordleSolver.Controllers
{
    internal class GameController
    {
        private readonly ISolver _solver;
        private readonly WordleGame _game;

        public GameController(ISolver solver, WordleGame game)
        {
            _solver = solver;
            _game = game;
        }

        public int Run()
        {
            _game.Start();

            var guess = _solver.GetFirstGuess();
            var response = _game.GuessWordle(guess);

            while (!IsGameOver(_game))
            {
                if (_solver is IReactiveSolver solver) solver.AddResponse(response);

                guess = _solver.GetNextGuess();
                response = _game.GuessWordle(guess);
            }

            int guesses = _game.Guesses - _game.GuessesLeft;

            return guesses;
        }

        private static bool IsGameOver(WordleGame game)
        {
            return (game.GameState == GameState.Completed || game.GameState == GameState.Failed);
        }
    }
}
