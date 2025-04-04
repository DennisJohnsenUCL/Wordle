using System.Text;
using Wordle_Console.Models;
using WordleCore;
using WordleCore.Enums;
using WordleCore.Models;
using WordleCore.Utils;

namespace Wordle_Console
{
    class Program
    {
        static void Main()
        {
            //>> pass to game controller class
            //>> Frame game content

            while (true)
            {
                Console.Clear();

                var wordleOptions = GetWordleOptions();

                var wordleGame = GetWordleGameFromOptions(wordleOptions);

                wordleGame.Start();
                Console.Clear();
                Console.WriteLine($"Wordle game started, you have {wordleGame.GuessesLeft} guesses to guess {wordleGame.Wordle}\n");

                Console.WriteLine("Enter your guess");

                while (wordleGame.GuessesLeft > 0)
                {
                    string guess = GetWordleGuessInput();

                    var wordleResponse = wordleGame.GuessWordle(guess);

                    PrintWordleGuessCorrectness(wordleResponse);
                    PrintAlphabet(wordleGame);

                    if (IsGameEnded(wordleGame.GameState)) break;
                }
            }
        }

        private static void ClearAlphabet()
        {
            (int left, int top) = Console.GetCursorPosition();

            for (int i = 2; i <= 5; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(left, top);
        }

        private static void PrintAlphabet(WordleGame wordleGame)
        {
            (int left, int top) = Console.GetCursorPosition();

            ClearAlphabet();

            Console.SetCursorPosition(left, top + 3);

            foreach (char c in "QWERTYUIOP\n ASDFGHJKL\n ZXCVBNM")
            {
                if (wordleGame.LetterHints.Absent.Contains(c)) Console.ForegroundColor = CorrectnessColors[Correctness.Absent];
                else if (wordleGame.LetterHints.Present.Contains(c)) Console.ForegroundColor = CorrectnessColors[Correctness.Present];
                else if (wordleGame.LetterHints.Correct.Contains(c)) Console.ForegroundColor = CorrectnessColors[Correctness.Correct];
                else Console.ForegroundColor = ConsoleColor.White;
                Console.Write(c);
            }
            Console.ResetColor();
            Console.SetCursorPosition(left, top);
        }

        private static void PrintWordleGuessCorrectness(WordleResponse wordleResponse)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            for (int i = 0; i < 5; i++)
            {
                Console.ForegroundColor = CorrectnessColors[wordleResponse.Correctness[i]];
                Console.Write(wordleResponse.Chars[i]);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        private static bool IsGameEnded(GameState gameState)
        {
            if (gameState == GameState.Completed)
            {
                ClearAlphabet();
                Console.WriteLine("\nYou guessed the right word!\n");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return true;
            }

            if (gameState == GameState.Failed)
            {
                ClearAlphabet();
                Console.WriteLine("\nYou did not guess the right word!\n");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return true;
            }
            return false;
        }

        private static WordleOptions GetWordleOptions()
        {
            Console.Clear();
            Console.WriteLine("Press a key to pick an option:");
            Console.WriteLine("1: Start a new game of Wordle");
            Console.WriteLine("2: Start a game of Wordle with custom options");
            Console.WriteLine("3: Exit");

            WordleOptions? wordleOptions = null;

            while (wordleOptions == null)
            {
                char k = Console.ReadKey(true).KeyChar;
                if (k == '1')
                {
                    wordleOptions = new WordleOptions();
                }
                else if (k == '2')
                {
                    string? wordle = GetWordleOptionInput();
                    int? guesses = GetGuessesInput();

                    wordleOptions = new WordleOptions(wordle, guesses);
                }
                else if (k == '3') { Environment.Exit(0); }
                else
                {
                    PrintInvalidInput(k);
                }
            }
            return wordleOptions;
        }

        //>> Print warnings about invalid inputs and not allowed words and wrong lengths
        private static string? GetWordleOptionInput()
        {
            Console.Clear();
            Console.WriteLine("Enter a valid Wordle for your game:");

            var sb = new StringBuilder();

            while (true)
            {
                var k = Console.ReadKey(true);
                if (char.IsLetter(k.KeyChar) && sb.Length < 5)
                {
                    sb.Append(char.ToUpper(k.KeyChar));
                    Console.Write(char.ToUpper(k.KeyChar));
                }
                else if (k.Key == ConsoleKey.Enter && IsValidWordle()) return sb.ToString();
                else if (k.Key == ConsoleKey.Enter && sb.Length == 0) return null;
                else if (k.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }

            bool IsValidWordle()
            {
                return sb.Length == 5 && WordleGameUtils.IsAllowedWord(sb.ToString());
            }
        }

        //>> Print warnings about invalid inputs and not allowed words and wrong lengths
        private static string GetWordleGuessInput()
        {
            var sb = new StringBuilder();

            while (true)
            {
                var k = Console.ReadKey(true);
                if (char.IsLetter(k.KeyChar) && sb.Length < 5)
                {
                    sb.Append(char.ToUpper(k.KeyChar));
                    Console.Write(char.ToUpper(k.KeyChar));
                }
                else if (k.Key == ConsoleKey.Enter && IsValidWordle()) { Console.WriteLine(); return sb.ToString(); }
                else if (k.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }

            bool IsValidWordle()
            {
                return sb.Length == 5 && WordleGameUtils.IsAllowedWord(sb.ToString());
            }
        }

        //>> Print warnings
        private static int? GetGuessesInput()
        {
            Console.Clear();
            Console.WriteLine("Enter a valid amount of guesses for your game:");

            var sb = new StringBuilder();

            while (true)
            {
                var k = Console.ReadKey(true);
                if (char.IsDigit(k.KeyChar))
                {
                    sb.Append(k.KeyChar);
                    Console.Write(k.KeyChar);
                }
                else if (k.Key == ConsoleKey.Enter && int.TryParse(sb.ToString(), out int guesses)) return guesses;
                else if (k.Key == ConsoleKey.Enter && sb.Length == 0) return null;
                else if (k.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
        }

        private static void PrintInvalidInput(char k)
        {
            var (left, top) = Console.GetCursorPosition();

            Console.SetCursorPosition(left, top + 3);
            Console.WriteLine("Invalid input " + k);
            Console.SetCursorPosition(left, top);
        }

        private static WordleGame GetWordleGameFromOptions(WordleOptions wordleOptions)
        {
            if (wordleOptions.Wordle != null && wordleOptions.Guesses != null) return new WordleGame(wordleOptions.Wordle, (int)wordleOptions.Guesses);
            else if (wordleOptions.Wordle != null) return new WordleGame(wordleOptions.Wordle);
            else if (wordleOptions.Guesses != null) return new WordleGame((int)wordleOptions.Guesses);
            else return new WordleGame();
        }

        private static Dictionary<Correctness, ConsoleColor> CorrectnessColors = new()
        {
            { Correctness.Correct, ConsoleColor.Green },
            { Correctness.Absent, ConsoleColor.Red },
            { Correctness.OverCount, ConsoleColor.Red },
            { Correctness.Present, ConsoleColor.DarkYellow }
        };
    }
}
