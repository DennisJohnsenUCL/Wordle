using Wordle_Console.Interfaces;
using Wordle_Console.Models;
using WordleCore;
using WordleCore.Enums;

namespace Wordle_Console
{
    internal class GameController
    {
        private readonly IInputHandler _inputHandler;
        private readonly IRenderer _renderer;

        internal GameController(IInputHandler inputHandler, IRenderer renderer)
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

                game.Start();

                _renderer.PrintGameStart(game.GuessesLeft, game.Wordle);

                while (game.GuessesLeft > 0)
                {
                    string guess = _inputHandler.GetWordleGuessInput();

                    var response = game.GuessWordle(guess);

                    _renderer.PrintWordleGuessCorrectness(response);
                    _renderer.PrintAlphabet(game.LetterHints);

                    if (IsGameEnded(game.GameState)) break;
                }
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
