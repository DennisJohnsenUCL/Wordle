namespace WordleSolver.Models
{
	internal record SolverResult
	{
		public string SolverIdentifier { get; }
		public double AverageGuesses { get; }
		public int FailedGames { get; }
		public int WorstCase { get; }
		public int GamesPlayed { get; }
		public long ElapsedMilliseconds { get; }

		public SolverResult(string solverIdentifier, double averageGuesses, int failedGames, int worstCase, int gamesPlayed, long elapsedMilliseconds)
		{
			SolverIdentifier = solverIdentifier;
			AverageGuesses = averageGuesses;
			FailedGames = failedGames;
			WorstCase = worstCase;
			GamesPlayed = gamesPlayed;
			ElapsedMilliseconds = elapsedMilliseconds;
		}

		public override string ToString()
		{
			return $"Solver: {SolverIdentifier}\n" +
				$"Average guesses per Wordle = {AverageGuesses:0.00}, time taken (in ms): {ElapsedMilliseconds}\n" +
				$"Games played: {GamesPlayed}, failed games: {FailedGames}, Worst case: {WorstCase}";
		}
	}
}
