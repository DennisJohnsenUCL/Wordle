using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Solvers
{
	internal class EntropyFrequencyThresholdSolver : EntropySolver
	{
		private readonly double _threshold;

		public EntropyFrequencyThresholdSolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<Word, double> wordFrequencies, double threshold, string identifier)
			: base(firstGuessProvider, constraintManager, patternsProvider, wordFrequencies, 0, identifier)
		{
			_threshold = threshold;
		}

		protected override bool TryGetThresholdGuess(Dictionary<Word, double> normalizedFrequencies, out Word guess)
		{
			var maxPair = normalizedFrequencies.Aggregate((a, b) => a.Value > b.Value ? a : b);
			if (maxPair.Value > _threshold)
			{
				guess = maxPair.Key;
				return true;
			}
			guess = Word.Empty;
			return false;
		}
	}
}
