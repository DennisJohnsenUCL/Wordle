using WordleCore;

namespace Wordle_Console
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Press a key to pick an option:");
            Console.WriteLine("1: Start a new game of Wordle");
            Console.WriteLine("2: Start a game of Wordle with custom options");
            Console.WriteLine("3: Exit");

            char k = Console.ReadKey().KeyChar;

            if (k == 1)
            {
                WordleGame wordleGame = new();
                wordleGame.Start();
                Console.WriteLine("Enter your guess");
                string guess = Console.ReadLine() ?? "";
                var wordleResponse = wordleGame.GuessWordle(guess);
                Console.WriteLine(wordleResponse);

            }
            else if (k == 2) { }
            else if (k == 3) { Environment.Exit(0); }
            else { }
        }
    }
}
