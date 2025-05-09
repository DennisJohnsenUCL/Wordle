using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
	internal class EntropyFrequencyThresholdSolver : EntropySolver
	{
		private readonly double _threshold;

		public EntropyFrequencyThresholdSolver(IFirstGuessProvider firstGuessProvider, IConstraintManager constraintManager, IPatternsProvider patternsProvider, Dictionary<string, double> wordFrequencies, double threshold, string identifier)
			: base(firstGuessProvider, constraintManager, patternsProvider, wordFrequencies, 0, identifier)
		{
			_threshold = threshold;
		}

		protected override bool TryGetThresholdGuess(Dictionary<string, double> normalizedFrequencies, out string guess)
		{
			var maxPair = normalizedFrequencies.Aggregate((a, b) => a.Value > b.Value ? a : b);
			if (maxPair.Value > _threshold)
			{
				guess = maxPair.Key;
				return true;
			}
			guess = "";
			return false;
		}
	}
}
