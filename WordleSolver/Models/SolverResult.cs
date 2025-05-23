namespace WordleSolver.Models
{
	internal record SolverResult
	{
		public string SolverIdentifier { get; }
		public double AverageGuesses { get; }
		public int FailedGames { get; }
		public int WorstCase { get; }
		public int TotalGuesses { get; }
		public int GamesPlayed { get; }
		public long ElapsedMilliseconds { get; }

		public SolverResult(string solverIdentifier, double averageGuesses, int failedGames, int worstCase, int totalGuesses, int gamesPlayed, long elapsedMilliseconds)
		{
			SolverIdentifier = solverIdentifier;
			AverageGuesses = averageGuesses;
			FailedGames = failedGames;
			TotalGuesses = totalGuesses;
			WorstCase = worstCase;
			GamesPlayed = gamesPlayed;
			ElapsedMilliseconds = elapsedMilliseconds;
		}

		public override string ToString()
		{
			return $"Solver: {SolverIdentifier}\n" +
				$"Avg: {AverageGuesses:0.0000}, Time: {ElapsedMilliseconds}, Total: {TotalGuesses}\n" +
				$"Games: {GamesPlayed}, Failed: {FailedGames}, Worst case: {WorstCase}";
		}
	}
}
