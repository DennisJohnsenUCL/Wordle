namespace WordleSolver.Models
{
    internal record SolverResult
    {
        public string SolverIdentifier { get; }
        public int GuessesPerWordle { get; }
        public long ElapsedMilliseconds { get; }

        public SolverResult(string solverIdentifier, int guessesPerWordle, long elapsedMilliseconds)
        {
            SolverIdentifier = solverIdentifier;
            GuessesPerWordle = guessesPerWordle;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public override string ToString()
        {
            return $"Solver: {SolverIdentifier}\n" +
                $"Guesses per Wordle = {GuessesPerWordle}, time taken (in ms): {ElapsedMilliseconds}";
        }

        public void Deconstruct(out string solverIdentifier, out int guessesPerWordle, out long elapsedMilliseconds)
        {
            solverIdentifier = SolverIdentifier;
            guessesPerWordle = GuessesPerWordle;
            elapsedMilliseconds = ElapsedMilliseconds;
        }
    }
}
