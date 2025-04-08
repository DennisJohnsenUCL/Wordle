using Wordle_Console.Models;
using WordleCore;
using WordleCore.Enums;

namespace Wordle_Console
{
    internal class GameController
    {
        private readonly InputHandler _inputHandler = new();
        private readonly Renderer _renderer = new();

        internal void Run()
        {
            while (true)
            {
                Console.Clear();

                var wordleOptions = _inputHandler.GetWordleOptions();

                var wordleGame = GetWordleGameFromOptions(wordleOptions);

                wordleGame.Start();
                Console.Clear();
                Console.WriteLine($"Wordle game started, you have {wordleGame.GuessesLeft} guesses to guess {wordleGame.Wordle}\n");

                Console.WriteLine("Enter your guess");

                while (wordleGame.GuessesLeft > 0)
                {
                    string guess = _inputHandler.GetWordleGuessInput();

                    var wordleResponse = wordleGame.GuessWordle(guess);

                    _renderer.PrintWordleGuessCorrectness(wordleResponse);
                    _renderer.PrintAlphabet(wordleGame.LetterHints);

                    if (IsGameEnded(wordleGame.GameState)) break;
                }
            }
        }

        private bool IsGameEnded(GameState gameState)
        {
            if (gameState == GameState.Completed)
            {
                _renderer.ClearAlphabet();
                Console.WriteLine("\nYou guessed the right word!\n");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return true;
            }

            if (gameState == GameState.Failed)
            {
                _renderer.ClearAlphabet();
                Console.WriteLine("\nYou did not guess the right word!\n");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return true;
            }
            return false;
        }

        private static WordleGame GetWordleGameFromOptions(WordleOptions wordleOptions)
        {
            if (wordleOptions.Wordle != null && wordleOptions.Guesses != null) return new WordleGame(wordleOptions.Wordle, (int)wordleOptions.Guesses);
            else if (wordleOptions.Wordle != null) return new WordleGame(wordleOptions.Wordle);
            else if (wordleOptions.Guesses != null) return new WordleGame((int)wordleOptions.Guesses);
            else return new WordleGame();
        }
    }
}
