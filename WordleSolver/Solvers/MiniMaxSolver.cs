using System.Collections.Concurrent;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class MiniMaxSolver : EntropySolver
	{
		public MiniMaxSolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<string, double> wordFrequencies, int limit, string identifier)
			: base(firstGuessProvider, constraintManager, patternsProvider, wordFrequencies, limit, identifier) { }

		public override string GetNextGuess()
		{
			var possibleWords = GetPossibleWords();
			if (possibleWords.Count < Limit) return possibleWords[0];
			if (TryGetCachedGuess(out var cachedGuess)) return cachedGuess;
			var maxPatterns = GetMaxPatterns(possibleWords);
			var minWord = maxPatterns.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
			if (GuessedWords.Count == 1) CachedBestSecond.Add(LastPattern!, minWord);
			GuessedWords.Add(minWord);
			return minWord;
		}

		protected new List<string> GetPossibleWords()
		{
			List<string> possibleWords = [];
			foreach (var word in Words)
			{
				if (FitsConstraints(word)) possibleWords.Add(word);
			}
			return possibleWords;
		}

		protected virtual ConcurrentDictionary<string, int> GetMaxPatterns(List<string> possibleWords)
		{
			ConcurrentDictionary<string, int> maxPatterns = [];
			Parallel.ForEach(Words, word =>
			{
				if (GuessedWords.Contains(word)) return;
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
