using WordleSolver.Enums;
using WordleSolver.Services;

namespace WordleSolver.Solvers
{
	internal class EntropyFrequencyThresholdSolver : EntropySolver
	{
		private readonly double _threshold;

		public EntropyFrequencyThresholdSolver(SolverContext context, Frequencies frequencies, double threshold, string identifier)
			: base(context, frequencies, 0, identifier)
		{
			_threshold = threshold;
		}

		protected override bool TryGetThresholdGuess(Dictionary<string, double> normalizedFrequencies, out string guess)
		{
			var maxPair = normalizedFrequencies.Aggregate((a, b) => a.Value > b.Value ? a : b);
			if (maxPair.Value > _threshold)
			{
				guess = maxPair.Key;
				GameKey += guess;
				return true;
			}
			guess = "";
			return false;
		}
	}
}
