using WordleCore.Enums;
using WordleCore.Models;

namespace Wordle_Console
{
    internal class Renderer
    {
        internal void ClearAlphabet()
        {
            (int left, int top) = Console.GetCursorPosition();

            for (int i = 2; i <= 5; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(left, top);
        }

        internal void PrintAlphabet(LetterHints hints)
        {
            (int left, int top) = Console.GetCursorPosition();

            ClearAlphabet();

            Console.SetCursorPosition(left, top + 3);

            foreach (char c in "QWERTYUIOP\n ASDFGHJKL\n ZXCVBNM")
            {
                if (hints.Absent.Contains(c)) Console.ForegroundColor = _colors[Correctness.Absent];
                else if (hints.Present.Contains(c)) Console.ForegroundColor = _colors[Correctness.Present];
                else if (hints.Correct.Contains(c)) Console.ForegroundColor = _colors[Correctness.Correct];
                else Console.ForegroundColor = ConsoleColor.White;
                Console.Write(c);
            }
            Console.ResetColor();
            Console.SetCursorPosition(left, top);
        }

        internal void PrintWordleGuessCorrectness(WordleResponse response)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            for (int i = 0; i < 5; i++)
            {
                Console.ForegroundColor = _colors[response.Correctness[i]];
                Console.Write(response.Chars[i]);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        internal static readonly Dictionary<Correctness, ConsoleColor> _colors = new()
        {
            { Correctness.Correct, ConsoleColor.Green },
            { Correctness.Absent, ConsoleColor.Red },
            { Correctness.OverCount, ConsoleColor.Red },
            { Correctness.Present, ConsoleColor.DarkYellow }
        };
    }
}
