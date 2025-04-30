using System.Diagnostics;
using WordleCore;
using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Controllers
{
    internal class SolverController
    {
        private readonly ISolver _solver;
        private readonly IEnumerable<WordleGame> _games;
        private int _guessesMade = 0;
        private const int GuessesAllowed = int.MaxValue;

        public SolverController(ISolver solver, IEnumerable<WordleGame> games)
        {
            _solver = solver;
            _games = games;
        }

        public SolverResult Run()
        {
            var timer = new Stopwatch();

            timer.Start();
            foreach (var game in _games)
            {
                ExecuteGame(game);
            }
            timer.Stop();

            double guessesPerWordle = (double)_guessesMade / _games.Count();
            return new SolverResult(_solver.SolverIdentifier, guessesPerWordle, timer.ElapsedMilliseconds);
        }

        private void ExecuteGame(WordleGame game)
        {
            game.Start();
            var response = MakeFirstGuess(game);

            while (!IsGameOver(game))
            {
                if (_solver is IReactiveSolver solver) solver.AddResponse(response);
                response = MakeNextGuess(game);
            }
            _solver.Reset();
        }

        private WordleResponse MakeFirstGuess(WordleGame game)
        {
            var guess = _solver.GetFirstGuess();
            _guessesMade++;
            return game.GuessWordle(guess);
        }

        private WordleResponse MakeNextGuess(WordleGame game)
        {
            var guess = _solver.GetNextGuess();
            _guessesMade++;
            return game.GuessWordle(guess);
        }

        private static bool IsGameOver(WordleGame game)
        {
            return (game.GameState == GameState.Completed || game.GameState == GameState.Failed);
        }
    }
}
