namespace WordleSolver.Models
{
    internal record SolverResult
    {
        public string Identifier { get; }
        public double GuessesPerWordle { get; }
        public int FailedGames { get; }
        public int GamesPlayed { get; }
        public long ElapsedMilliseconds { get; }

        public SolverResult(string solverIdentifier, double guessesPerWordle, int failedGames, int gamesPlayed, long elapsedMilliseconds)
        {
            Identifier = solverIdentifier;
            GuessesPerWordle = guessesPerWordle;
            FailedGames = failedGames;
            GamesPlayed = gamesPlayed;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public override string ToString()
        {
            return $"Solver: {Identifier}\n" +
                $"Average Guesses per Wordle = {GuessesPerWordle:0.00}, Games failed (> 6 guesses): {FailedGames}\n" +
                $"Games played: {GamesPlayed}, time taken (in ms): {ElapsedMilliseconds}";
        }
    }
}
