using WordleSolver.Interfaces;

namespace WordleSolver.Services
{
	internal class SolverContext
	{
		public IFirstGuessProvider FirstGuessProvider { get; }
		public IPatternsProvider PatternsProvider { get; }
		public string[] Words { get; }
		public Dictionary<string, long> WordOccurrences { get; }
		public string[] Wordles { get; }
		public Dictionary<string, double> WordFrequenciesFlat { get; }
		public Dictionary<string, double> WordFrequenciesWeighted { get; }
		public Dictionary<string, double> WordFrequenciesSigmoid { get; }
		public Dictionary<string, double> WordFrequenciesLog { get; }

		public SolverContext(IFirstGuessProvider firstGuessProvider, IPatternsProvider patternsProvider, string[] words, Dictionary<string, long> wordOccurrences, string[] wordles)
		{
			FirstGuessProvider = firstGuessProvider;
			PatternsProvider = patternsProvider;
			Words = words;
			WordOccurrences = wordOccurrences;
			Wordles = wordles;
			WordFrequenciesFlat = GetFlatFrequencies();
			WordFrequenciesWeighted = GetWeightedFrequencies();
			WordFrequenciesSigmoid = GetSigmoidFrequencies();
			WordFrequenciesLog = GetLogFrequencies();
		}

		private Dictionary<string, double> GetLogFrequencies()
		{
			var logFrequency = new Dictionary<string, double>();

			foreach (var key in WordOccurrences.Keys)
			{
				var value = WordOccurrences[key];
				var newValue = Math.Log(1 + value);

				logFrequency[key] = newValue;
			}

			double total = logFrequency.Values.Sum();

			var normalizedLogFrequency = logFrequency.ToDictionary(x => x.Key, x => x.Value / total);

			return normalizedLogFrequency;
		}

		private Dictionary<string, double> GetSigmoidFrequencies()
		{
			int m = 5000000;
			int s = 1000000;
			var sigmoidFrequency = new Dictionary<string, double>();

			foreach (var key in WordOccurrences.Keys)
			{
				var value = WordOccurrences[key];

				var exponent = -(double)(value - m) / s;
				var newValue = 1.0 / (1.0 + Math.Exp(exponent));

				sigmoidFrequency[key] = newValue;
			}

			double total = sigmoidFrequency.Values.Sum();

			var normalizedSigmoidFrequency = sigmoidFrequency.ToDictionary(x => x.Key, x => x.Value / total);

			return normalizedSigmoidFrequency;
		}

		private Dictionary<string, double> GetWeightedFrequencies()
		{
			var total = WordOccurrences.Values.Sum();
			var normalizedFrequencies = WordOccurrences.ToDictionary(x => x.Key, x => (double)x.Value / total);

			return normalizedFrequencies;
		}

		private Dictionary<string, double> GetFlatFrequencies()
		{
			var total = WordOccurrences.Count;
			var flatFrequencies = WordOccurrences.ToDictionary(x => x.Key, x => 1d / total);

			return flatFrequencies;
		}
	}
}
