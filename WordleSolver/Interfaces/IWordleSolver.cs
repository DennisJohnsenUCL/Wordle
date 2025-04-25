using WordleCore.Models;

namespace WordleSolver.Interfaces
{
    internal interface IWordleSolver
    {
        public string SolverIdentifier { get; }
        public string GetFirstGuess();
        public void AddResponse(WordleResponse response);
        public string GetNextGuess();
        public void Reset();
    }
}
