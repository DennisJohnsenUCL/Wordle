namespace WordleSolver.Interfaces
{
    internal interface ISolver
    {
        string Identifier { get; }
        string GetFirstGuess();
        string GetNextGuess();
        void Reset();
    }
}
