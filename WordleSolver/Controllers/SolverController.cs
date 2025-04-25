using System.Diagnostics;
using WordleCore;
using WordleCore.Enums;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Controllers
{
    internal class SolverController
    {
        private readonly IWordleSolver _solver;
        private readonly List<string> _wordles;
        private int _guessesMade = 0;
        private const int _guessesAllowed = int.MaxValue;

        public SolverController(IWordleSolver solver, List<string> wordles)
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
                var game = new WordleGame(wordle, _guessesAllowed);
                game.Start();
                var guess = _solver.GetFirstGuess();
                _guessesMade++;
                var response = game.GuessWordle(guess);
                if (IsGameOver(game)) { _solver.Reset(); continue; }
                while (true)
                {
                    _solver.AddResponse(response);
                    guess = _solver.GetNextGuess();
                    _guessesMade++;
                    response = game.GuessWordle(guess);
                    if (IsGameOver(game)) { _solver.Reset(); break; }
                }
            }
            timer.Stop();
            var guessesPerWordle = _guessesMade / _wordles.Count;
            return new SolverResult(_solver.SolverIdentifier, guessesPerWordle, timer.ElapsedMilliseconds);
        }

        private static bool IsGameOver(WordleGame game)
        {
            return (game.GameState == GameState.Completed || game.GameState == GameState.Failed);
        }
    }
}
