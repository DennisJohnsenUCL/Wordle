using System.Diagnostics;
using WordleCore;
using WordleCore.Enums;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Controllers
{
    internal class SolverController
    {
        private readonly ISolver _solver;
        private readonly IEnumerable<WordleGame> _games;
        private readonly List<int> _guessesList = [];

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
                int guesses = ExecuteGame(game);
                _guessesList.Add(guesses);
                _solver.Reset();
            }
            timer.Stop();

            double averageGuesses = (double)_guessesList.Sum() / _guessesList.Count;
            int failedGames = _guessesList.Count(guesses => guesses > 6);

            var result = new SolverResult(_solver.Identifier,
                averageGuesses,
                failedGames,
                _games.Count(),
                timer.ElapsedMilliseconds);

            return result;
        }

        private int ExecuteGame(WordleGame game)
        {
            game.Start();

            var guess = _solver.GetFirstGuess();
            var response = game.GuessWordle(guess);

            while (!IsGameOver(game))
            {
                if (_solver is IReactiveSolver solver) solver.AddResponse(response);

                guess = _solver.GetNextGuess();
                response = game.GuessWordle(guess);
            }

            int guesses = game.Guesses - game.GuessesLeft;

            return guesses;
        }

        private static bool IsGameOver(WordleGame game)
        {
            return (game.GameState == GameState.Completed || game.GameState == GameState.Failed);
        }
    }
}
