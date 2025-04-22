using WordleCore.Enums;

namespace Wordle_WinForms.Utils
{
    internal static class ColorProvider
    {
        public static readonly Dictionary<Correctness, Color> Colors = new()
        {
            { Correctness.Correct, Color.Green },
            { Correctness.Absent, Color.Red },
            { Correctness.OverCount, Color.Red },
            { Correctness.Present, Color.Yellow }
        };
    }
}
