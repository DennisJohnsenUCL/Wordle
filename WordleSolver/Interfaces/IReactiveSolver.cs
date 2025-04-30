using WordleCore.Models;

namespace WordleSolver.Interfaces
{
    internal interface IReactiveSolver
    {
        void AddResponse(WordleResponse response);
    }
}
