namespace Wordle_Console.Models
{
    internal class WordleOptions
    {
        public string? Wordle { get; set; }
        public int? Guesses { get; set; }

        public WordleOptions(string? wordle, int? guesses)
        {
            Wordle = wordle;
            Guesses = guesses;
        }

        public WordleOptions() { }
    }
}
