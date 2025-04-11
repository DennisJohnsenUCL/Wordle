using Wordle_Console.Interfaces;

namespace Wordle_Console
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

                var gameController = new GameController(_inputHandler, _renderer);

                gameController.Run(options);
            }
        }
    }
}
