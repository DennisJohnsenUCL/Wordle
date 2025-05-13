using WordleSolver.Enums;
using WordleSolver.Interfaces;
using WordleSolver.Solvers;

namespace WordleSolver.Services
{
	internal class SolverFactory
	{
		private readonly string[] _words;
		private readonly Dictionary<string, long> _sortedWordOccurrences;
		private readonly IFirstGuessProvider _firstGuessProvider;
		private readonly IPatternsProvider _patternsProvider;

		public SolverFactory(string[] words, Dictionary<string, long> sortedWordOccurrences, IFirstGuessProvider firstGuessProvider, IPatternsProvider patternsProvider)
		{
			_words = words;
			_sortedWordOccurrences = sortedWordOccurrences;
			_firstGuessProvider = firstGuessProvider;
			_patternsProvider = patternsProvider;
		}

		public List<ISolver> GetSolvers(IEnumerable<SolverTypes> solversToGet)
		{
			var solvers = new List<ISolver>();

			foreach (var solver in solversToGet)
			{
				switch (solver)
				{
					case SolverTypes.Random:
						var random = GetLazyRandomSolver();
						solvers.Add(random);
						break;
					case SolverTypes.Sorted:
						var sorted = GetLazySortedSolver();
						solvers.Add(sorted);
						break;
					case SolverTypes.FilteredRandom:
						var filteredRandom = GetFilteredRandomSolver();
						solvers.Add(filteredRandom);
						break;
					case SolverTypes.FilteredSorted:
						var filteredSorted = GetFilteredSortedSolver();
						solvers.Add(filteredSorted);
						break;
					case SolverTypes.Entropy:
						var entropy = GetEntropySolver();
						solvers.Add(entropy);
						break;
					case SolverTypes.Frequency:
						var frequency = GetEntropyFrequencySolver();
						solvers.Add(frequency);
						break;
					case SolverTypes.Sigmoid:
						var sigmoid = GetEntropyFrequencySigmoidSolver();
						solvers.Add(sigmoid);
						break;
					case SolverTypes.Log:
						var log = GetEntropyFrequencyLogSolver();
						solvers.Add(log);
						break;
					case SolverTypes.Positional:
						var positional = GetPositionalEntropySolver();
						solvers.Add(positional);
						break;
					case SolverTypes.FrequencyThreshold:
						var frequencyThreshold = GetFrequencyThresholdSolver();
						solvers.Add(frequencyThreshold);
						break;
					case SolverTypes.MiniMax:
						var minimaxSolver = GetMinimaxSolver();
						solvers.Add(minimaxSolver);
						break;
					default:
						break;
				}
			}
			return solvers;
		}

		#region Solver Get methods
		private LazySolver GetLazyRandomSolver()
		{
			var solver = new LazySolver(_words, "LazyRandomSolver");
			return solver;
		}

		private LazySolver GetLazySortedSolver()
		{
			var words = _sortedWordOccurrences.Keys.ToArray();
			var solver = new LazySolver(words, "LazySortedSolver");
			return solver;
		}

		private FilteredSolver GetFilteredRandomSolver()
		{
			var words = _words;
			var solver = new FilteredSolver(_firstGuessProvider, _patternsProvider, words, "FilteredRandomSolver");
			return solver;
		}

		private FilteredSolver GetFilteredSortedSolver()
		{
			var words = _sortedWordOccurrences.Keys.ToArray();
			var solver = new FilteredSolver(_firstGuessProvider, _patternsProvider, words, "FilteredSortedSolver");
			return solver;
		}

		private EntropySolver GetEntropySolver()
		{
			var flatFrequency = GetFlatFrequencies();
			int limit = 20;
			var solver = new EntropySolver(_firstGuessProvider, _patternsProvider, flatFrequency, limit, "EntropySolver");
			return solver;
		}

		private EntropySolver GetEntropyFrequencySolver()
		{
			var normalizedFrequency = GetNormalizedFrequencies();
			int limit = 21;
			var solver = new EntropySolver(_firstGuessProvider, _patternsProvider, normalizedFrequency, limit, "EntropyFrequencySolver");
			return solver;
		}

		private EntropySolver GetEntropyFrequencySigmoidSolver()
		{
			var normalizedSigmoidFrequency = GetSigmoidFrequencies();
			int limit = 20;
			var solver = new EntropySolver(_firstGuessProvider, _patternsProvider, normalizedSigmoidFrequency, limit, "EntropyFrequencySigmoidSolver");
			return solver;
		}

		private EntropySolver GetEntropyFrequencyLogSolver()
		{
			var normalizedLogFrequency = GetLogFrequencies();
			int limit = 20;
			var solver = new EntropySolver(_firstGuessProvider, _patternsProvider, normalizedLogFrequency, limit, "EntropyFrequencyLogSolver");
			return solver;
		}

		private PositionalEntropySolver GetPositionalEntropySolver()
		{
			var normalizedFrequency = GetNormalizedFrequencies();
			var solver = new PositionalEntropySolver(_firstGuessProvider, _patternsProvider, normalizedFrequency, 20, "PositionalEntropy");
			return solver;
		}

		private EntropyFrequencyThresholdSolver GetFrequencyThresholdSolver()
		{
			var normalizedFrequency = GetNormalizedFrequencies();
			var solver = new EntropyFrequencyThresholdSolver(_firstGuessProvider, _patternsProvider, normalizedFrequency, 0.50, "EntropyFrequencyThreshold");
			return solver;
		}

		private MiniMaxSolver GetMinimaxSolver()
		{
			var flatFrequency = GetFlatFrequencies();
			var solver = new MiniMaxSolver(_firstGuessProvider, _patternsProvider, flatFrequency, 25, "MiniMax");
			return solver;
		}
		#endregion

		#region Transformations
		private Dictionary<string, double> GetFlatFrequencies()
		{
			var total = _words.Length;
			var flatFrequencies = _sortedWordOccurrences.ToDictionary(x => x.Key, x => 1d / total);

			return flatFrequencies;
		}

		private Dictionary<string, double> GetNormalizedFrequencies()
		{
			var total = _sortedWordOccurrences.Values.Sum();
			var normalizedFrequencies = _sortedWordOccurrences.ToDictionary(x => x.Key, x => (double)x.Value / total);

			return normalizedFrequencies;
		}

		private Dictionary<string, double> GetSigmoidFrequencies()
		{
			int m = 5000000;
			int s = 1000000;
			var sigmoidFrequency = new Dictionary<string, double>();

			foreach (var key in _sortedWordOccurrences.Keys)
			{
				var value = _sortedWordOccurrences[key];

				var exponent = -(double)(value - m) / s;
				var newValue = 1.0 / (1.0 + Math.Exp(exponent));

				sigmoidFrequency[key] = newValue;
			}

			double total = sigmoidFrequency.Values.Sum();

			var normalizedSigmoidFrequency = sigmoidFrequency.ToDictionary(x => x.Key, x => x.Value / total);

			return normalizedSigmoidFrequency;
		}

		private Dictionary<string, double> GetLogFrequencies()
		{
			var logFrequency = new Dictionary<string, double>();

			foreach (var key in _sortedWordOccurrences.Keys)
			{
				var value = _sortedWordOccurrences[key];
				var newValue = Math.Log(1 + value);

				logFrequency[key] = newValue;
			}

			double total = logFrequency.Values.Sum();

			var normalizedLogFrequency = logFrequency.ToDictionary(x => x.Key, x => x.Value / total);

			return normalizedLogFrequency;
		}
		#endregion
	}
}
