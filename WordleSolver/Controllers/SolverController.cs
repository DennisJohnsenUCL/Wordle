using System.Diagnostics;
using WordleCore;
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
                var gameController = new GameController(_solver, game);
                int guesses = gameController.Run();
                _guessesList.Add(guesses);
                _solver.Reset();
            }
            timer.Stop();

            double avgGuesses = (double)_guessesList.Sum() / _guessesList.Count;
            int failedGames = _guessesList.Where(guesses => guesses > 6).Count();

            var result = new SolverResult(
                _solver.Identifier,
                avgGuesses,
                failedGames,
                _games.Count(),
                timer.ElapsedMilliseconds);

            return result;
        }
    }
}
