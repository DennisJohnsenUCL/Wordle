using WordleCore.Models;

namespace WordleSolver.Interfaces
{
    internal interface IWordleSolver
    {
        string SolverIdentifier { get; }
        string GetFirstGuess();
        void AddResponse(WordleResponse response);
        string GetNextGuess();
        void Reset();
    }
}
