using WordleCore.Enums;

namespace Wordle_Console.Utils
{
    internal static class ColorProvider
    {
        public static readonly Dictionary<Correctness, ConsoleColor> Colors = new()
        {
            { Correctness.Correct, ConsoleColor.Green },
            { Correctness.Absent, ConsoleColor.Red },
            { Correctness.OverCount, ConsoleColor.Red },
            { Correctness.Present, ConsoleColor.DarkYellow }
        };
    }
}
