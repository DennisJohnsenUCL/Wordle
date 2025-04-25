namespace WordleSolver.Models
{
    internal record SolverResult
    {
        public string SolverIdentifier { get; }
        public double GuessesPerWordle { get; }
        public long ElapsedMilliseconds { get; }

        public SolverResult(string solverIdentifier, double guessesPerWordle, long elapsedMilliseconds)
        {
            SolverIdentifier = solverIdentifier;
            GuessesPerWordle = guessesPerWordle;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public override string ToString()
        {
            return $"Solver: {SolverIdentifier}\n" +
                $"Guesses per Wordle = {GuessesPerWordle:0.00}, time taken (in ms): {ElapsedMilliseconds}";
        }

        public void Deconstruct(out string solverIdentifier, out double guessesPerWordle, out long elapsedMilliseconds)
        {
            solverIdentifier = SolverIdentifier;
            guessesPerWordle = GuessesPerWordle;
            elapsedMilliseconds = ElapsedMilliseconds;
        }
    }
}
