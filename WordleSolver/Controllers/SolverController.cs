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
        private readonly IWordleSolver _solver;
        private readonly IEnumerable<string> _wordles;
        private int _guessesMade = 0;
        private const int GuessesAllowed = int.MaxValue;

        public SolverController(IWordleSolver solver, IEnumerable<string> wordles)
        {
            _solver = solver;
            _wordles = wordles;
        }

        public SolverResult Run()
        {
            var timer = new Stopwatch();

            timer.Start();
            foreach (string wordle in _wordles)
            {
                ExecuteGame(wordle);
            }
            timer.Stop();

            double guessesPerWordle = _guessesMade / _wordles.Count();
            return new SolverResult(_solver.SolverIdentifier, guessesPerWordle, timer.ElapsedMilliseconds);
        }

        private void ExecuteGame(string wordle)
        {
            var game = InitializeGame(wordle);
            var response = MakeFirstGuess(game);

            while (!IsGameOver(game))
            {
                _solver.AddResponse(response);
                response = MakeNextGuess(game);
            }
            _solver.Reset();
        }

        private static WordleGame InitializeGame(string wordle)
        {
            var game = new WordleGame(wordle, GuessesAllowed);
            game.Start();
            return game;
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
