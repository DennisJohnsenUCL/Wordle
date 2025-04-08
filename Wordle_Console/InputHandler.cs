using System.Text;
using Wordle_Console.Models;
using WordleCore.Utils;

namespace Wordle_Console
{
    internal class InputHandler
    {
        internal WordleOptions GetWordleOptions()
        {
            Console.Clear();
            Console.WriteLine("Press a key to pick an option:");
            Console.WriteLine("1: Start a new game of Wordle");
            Console.WriteLine("2: Start a game of Wordle with custom options");
            Console.WriteLine("3: Exit");

            WordleOptions? options = null;

            while (options == null)
            {
                char k = Console.ReadKey(true).KeyChar;
                if (k == '1')
                {
                    options = new WordleOptions();
                }
                else if (k == '2')
                {
                    string? wordle = GetWordleOptionInput();
                    int? guesses = GetGuessesInput();

                    options = new WordleOptions(wordle, guesses);
                }
                else if (k == '3') { Environment.Exit(0); }
                else
                {
                    PrintInvalidInput(k);
                }
            }
            return options;
        }

        //>> Print warnings about invalid inputs and not allowed words and wrong lengths
        internal static string? GetWordleOptionInput()
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
        internal string GetWordleGuessInput()
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
        internal static int? GetGuessesInput()
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

        internal static void PrintInvalidInput(char k)
        {
            var (left, top) = Console.GetCursorPosition();

            Console.SetCursorPosition(left, top + 3);
            Console.WriteLine("Invalid input " + k);
            Console.SetCursorPosition(left, top);
        }
    }
}
