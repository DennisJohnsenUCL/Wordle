using WordleCore;
using WordleCore.Enums;

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

            char k = Console.ReadKey(true).KeyChar;

            if (k == '1')
            {
                Console.Clear();
                WordleGame wordleGame = new();
                wordleGame.Start();
                Console.WriteLine($"Wordle game started, you have {wordleGame.GuessesLeft} guesses to guess {wordleGame.Wordle}\n");

                Console.WriteLine("Enter your guess");

                while (wordleGame.GuessesLeft > 0)
                {
                    //>> Create a method using ReadKey to take inputs and block enter while not proper Wordle guess
                    string guess = (Console.ReadLine() ?? "").ToUpper();
                    var wordleResponse = wordleGame.GuessWordle(guess);
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    for (int i = 0; i < 5; i++)
                    {
                        Console.ForegroundColor = CorrectnessColors[wordleResponse.Correctness[i]];
                        Console.Write(wordleResponse.Chars[i]);
                    }
                    Console.WriteLine();
                    Console.ResetColor();

                    if (wordleGame.GameState == GameState.Completed)
                    {
                        Console.WriteLine("You guessed the right word!\n");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey(true);
                        break;
                    }

                    if (wordleGame.GameState == GameState.Failed)
                    {
                        Console.WriteLine("You did not guess the right word!\n");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey(true);
                        break;
                    }
                }
            }
            else if (k == '2') { }
            else if (k == '3') { Environment.Exit(0); }
            else { }
        }

        internal static Dictionary<Correctness, ConsoleColor> CorrectnessColors = new()
        {
            { Correctness.Correct, ConsoleColor.Green },
            { Correctness.Absent, ConsoleColor.Red },
            { Correctness.Present, ConsoleColor.Yellow }
        };
    }
}
