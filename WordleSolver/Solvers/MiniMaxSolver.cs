using System.Collections.Concurrent;
using WordleSolver.Enums;
using WordleSolver.Services;

namespace WordleSolver.Solvers
{
	internal class MiniMaxSolver : EntropySolver
	{
		public MiniMaxSolver(SolverContext context, int limit, string identifier)
			: base(context, Frequencies.Flat, limit, identifier) { }

		protected override string GetStrategyGuess(Dictionary<string, double> possibleWords)
		{
			return GetMiniMaxGuess([.. possibleWords.Keys]);
		}

		protected virtual string GetMiniMaxGuess(string[] possibleWords)
		{
			var maxPatterns = GetMaxPatterns(possibleWords);
			var minWord = maxPatterns.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
			return minWord;
		}

		protected virtual ConcurrentDictionary<string, int> GetMaxPatterns(string[] possibleWords)
		{
			ConcurrentDictionary<string, int> maxPatterns = [];
			Parallel.ForEach(Words, word =>
			{
				Dictionary<string, int> patternGroups = [];

				foreach (var possibleWord in possibleWords)
				{
					var pattern = PatternsProvider.GetPattern(word, possibleWord);
					if (!patternGroups.TryAdd(pattern, 1)) patternGroups[pattern] += 1;
				}
				var maxPartition = patternGroups.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
				maxPatterns.TryAdd(word, maxPartition);
			});
			return maxPatterns;
		}
	}
}
