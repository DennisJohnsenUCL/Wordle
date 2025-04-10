namespace Wordle_Console.Models
{
    internal record WordleOptions
    {
        public string? Wordle { get; }
        public int? Guesses { get; }

        public WordleOptions(string? wordle = null, int? guesses = null)
        {
            Wordle = wordle;
            Guesses = guesses;
        }
    }
}
