using WordleCore.Models;

namespace Wordle_Console.Interfaces
{
    internal interface IRenderer
    {
        void PrintAlphabet(LetterHints hints);
        void PrintWordleGuessCorrectness(WordleResponse response);
        void PrintGameStart(int guessesLeft, string wordle);
        void PrintGameCompleted();
        void PrintGameOver();
    }
}
