namespace WordleSolver.Interfaces
{
    internal interface IPatternsProvider
    {
        string GetPattern(int guessIndex, string wordle);
    }
}
