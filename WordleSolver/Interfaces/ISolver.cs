namespace WordleSolver.Interfaces
{
    internal interface ISolver
    {
        string SolverIdentifier { get; }
        string GetFirstGuess();
        string GetNextGuess();
        void Reset();
    }
}
