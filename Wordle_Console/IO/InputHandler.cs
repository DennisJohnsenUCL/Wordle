using System.Text;
using Wordle_Console.Interfaces;
using Wordle_Console.Models;
using WordleCore.Models;
using WordleCore.Utils;

namespace Wordle_Console.IO
{
    internal class InputHandler : IInputHandler
    {
        public WordleOptions GetWordleOptions()
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
                    string? wordle = GetWordleInput();
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
        private static string? GetWordleInput()
        {
            Console.Clear();
            Console.WriteLine("Enter a valid Wordle for your game:");

            var rules = new InputValidationRules()
            {
                AcceptChar = (c, s) => char.IsLetter(c) && s.Length < 5,
                CanSubmit = s => s.Length == 5 && WordleGameUtils.IsAllowedWord(s),
                AllowEmpty = true
            };

            var s = GetControlledInput(rules);

            if (s == "") return null;
            else return s;
        }

        //>> Print warnings about invalid inputs and not allowed words and wrong lengths
        public string GetWordleGuessInput()
        {
            var rules = new InputValidationRules()
            {
                AcceptChar = (c, s) => char.IsLetter(c) && s.Length < 5,
                CanSubmit = s => s.Length == 5 && WordleGameUtils.IsAllowedWord(s),
            };

            return GetControlledInput(rules);
        }

        //>> Print warnings
        private static int? GetGuessesInput()
        {
            Console.Clear();
            Console.WriteLine("Enter a valid amount of guesses for your game:");

            var rules = new InputValidationRules()
            {
                AcceptChar = (c, _) => char.IsDigit(c),
                CanSubmit = s => int.TryParse(s, out _),
                AllowEmpty = true
            };

            var s = GetControlledInput(rules);

            if (s == "") return null;
            else return Convert.ToInt32(s);
        }

        private static string GetControlledInput(InputValidationRules rules)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var k = Console.ReadKey(true);
                var s = sb.ToString();

                if (rules.AcceptChar(k.KeyChar, s))
                {
                    sb.Append(char.ToUpper(k.KeyChar));
                    Console.Write(char.ToUpper(k.KeyChar));
                }
                else if (k.Key == ConsoleKey.Enter && s == "" && rules.AllowEmpty) return "";
                else if (k.Key == ConsoleKey.Enter && rules.CanSubmit(s))
                {
                    Console.WriteLine();
                    return s;
                }
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
    }
}
