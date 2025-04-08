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
                Console.Clear();

                var options = _inputHandler.GetWordleOptions();

                var game = GetWordleGameFromOptions(options);

                game.Start();
                Console.Clear();
                Console.WriteLine($"Wordle game started, you have {game.GuessesLeft} guesses to guess {game.Wordle}\n");

                Console.WriteLine("Enter your guess");

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
                _renderer.ClearAlphabet();
                Console.WriteLine("\nYou guessed the right word!\n");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return true;
            }

            if (state == GameState.Failed)
            {
                _renderer.ClearAlphabet();
                Console.WriteLine("\nYou did not guess the right word!\n");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return true;
            }
            return false;
        }

        private static WordleGame GetWordleGameFromOptions(WordleOptions options)
        {
            if (options.Wordle != null && options.Guesses != null) return new WordleGame(options.Wordle, (int)options.Guesses);
            else if (options.Wordle != null) return new WordleGame(options.Wordle);
            else if (options.Guesses != null) return new WordleGame((int)options.Guesses);
            else return new WordleGame();
        }
    }
}
