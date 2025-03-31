using WordleCore;
using WordleCore.Enums;

namespace Wordle_Console
{
    class Program
    {
        static void Main()
        {
            //>> Method for displaying menu and taking input
            //>> class model for options
            //>> pass to game controller class
            //>> Store list of absent word in game? Or in game controller? Prob game bc reuse for gui
            //>> ^ Necessitates differentiation total absent and absent over count
            //>> Increase text size
            //>> Frame game content
            //>> Rewrite invalid input when pressing !1-3
            //>> Print alphabet at bottom of console. use Get and SetCursorPosition to jump down and back

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

        //>> Add a key for over count
        //>> Change color for absent & over count to grey?
        internal static Dictionary<Correctness, ConsoleColor> CorrectnessColors = new()
        {
            { Correctness.Correct, ConsoleColor.Green },
            { Correctness.Absent, ConsoleColor.Red },
            { Correctness.Present, ConsoleColor.Yellow }
        };
    }
}
