using Wordle_Console.Interfaces;
using WordleCore;
using WordleCore.Enums;

namespace Wordle_Console
{
    internal class GameController
    {
        private readonly WordleGame _game;
        private readonly IInputHandler _inputHandler;
        private readonly IRenderer _renderer;

        internal GameController(WordleGame game, IInputHandler inputHandler, IRenderer renderer)
        {
            _game = game;
            _inputHandler = inputHandler;
            _renderer = renderer;
        }

        internal void Run()
        {
            _game.Start();

            _renderer.PrintGameStart(_game.GuessesLeft, _game.Wordle);

            while (_game.GuessesLeft > 0)
            {
                string guess = _inputHandler.GetWordleGuessInput();

                var response = _game.GuessWordle(guess);

                _renderer.PrintWordleGuessCorrectness(response);
                _renderer.PrintAlphabet(_game.LetterHints);

                if (IsGameEnded(_game.GameState)) break;
            }
        }

        private bool IsGameEnded(GameState state)
        {
            if (state == GameState.Completed)
            {
                _renderer.PrintGameCompleted();
                return true;
            }

            else if (state == GameState.Failed)
            {
                _renderer.PrintGameOver();
                return true;
            }
            return false;
        }
    }
}
