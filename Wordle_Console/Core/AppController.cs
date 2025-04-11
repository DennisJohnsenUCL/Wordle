using Wordle_Console.Interfaces;
using Wordle_Console.Models;
using WordleCore;

namespace Wordle_Console.Core
{
    internal class AppController
    {
        private readonly IInputHandler _inputHandler;
        private readonly IRenderer _renderer;

        internal AppController(IInputHandler inputHandler, IRenderer renderer)
        {
            _inputHandler = inputHandler;
            _renderer = renderer;
        }

        internal void Run()
        {
            while (true)
            {
                var options = _inputHandler.GetWordleOptions();

                var game = GetWordleGameFromOptions(options);

                var gameController = new GameController(game, _inputHandler, _renderer);

                gameController.Run();
            }
        }

        private static WordleGame GetWordleGameFromOptions(WordleOptions options)
        {
            var (wordle, guesses) = options;

            if (wordle != null && guesses != null) return new WordleGame(wordle, (int)guesses);
            else if (wordle != null) return new WordleGame(wordle);
            else if (guesses != null) return new WordleGame((int)guesses);
            else return new WordleGame();
        }
    }
}
