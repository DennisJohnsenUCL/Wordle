using WordleCore.Models;

namespace Wordle_Console.Interfaces
{
    internal interface IRenderer
    {
        void ClearAlphabet();
        void PrintAlphabet(LetterHints hints);
        void PrintWordleGuessCorrectness(WordleResponse response);
        void PrintGameCompleted();
        void PrintGameOver();
    }
}
