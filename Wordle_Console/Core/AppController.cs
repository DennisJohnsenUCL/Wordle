using Wordle_Console.Interfaces;
using WordleCore.Utils;

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

                var game = WordleGameUtils.GetWordleGameFromOptions(options);

                var gameController = new GameController(game, _inputHandler, _renderer);

                gameController.Run();
            }
        }
    }
}
