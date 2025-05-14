using WordleSolver.Enums;
using WordleSolver.Interfaces;
using WordleSolver.Solvers;

namespace WordleSolver.Services
{
	internal class SolverFactory
	{
		private readonly SolverContext _context;

		public SolverFactory(SolverContext context)
		{
			_context = context;
		}

		public List<ISolver> GetSolvers(IEnumerable<SolverTypes> solversToGet)
		{
			var solvers = new List<ISolver>();

			foreach (var solver in solversToGet)
			{
				switch (solver)
				{
					case SolverTypes.Lazy:
						var sorted = GetLazySolver();
						solvers.Add(sorted);
						break;
					case SolverTypes.Filtered:
						var filteredSorted = GetFilteredSolver();
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
		private LazySolver GetLazySolver()
		{
			var solver = new LazySolver(_context.Words, "LazySortedSolver");
			return solver;
		}

		private FilteredSolver GetFilteredSolver()
		{
			var solver = new FilteredSolver(_context, "FilteredSortedSolver");
			return solver;
		}

		private EntropySolver GetEntropySolver()
		{
			int limit = 20;
			var solver = new EntropySolver(_context, Frequencies.Flat, limit, "EntropySolver");
			return solver;
		}

		private EntropySolver GetEntropyFrequencySolver()
		{
			int limit = 21;
			var solver = new EntropySolver(_context, Frequencies.Weighted, limit, "EntropyFrequencySolver");
			return solver;
		}

		private EntropySolver GetEntropyFrequencySigmoidSolver()
		{
			int limit = 20;
			var solver = new EntropySolver(_context, Frequencies.Sigmoid, limit, "EntropyFrequencySigmoidSolver");
			return solver;
		}

		private EntropySolver GetEntropyFrequencyLogSolver()
		{
			int limit = 20;
			var solver = new EntropySolver(_context, Frequencies.Log, limit, "EntropyFrequencyLogSolver");
			return solver;
		}

		private PositionalEntropySolver GetPositionalEntropySolver()
		{
			int limit = 20;
			var solver = new PositionalEntropySolver(_context, Frequencies.Weighted, limit, "PositionalEntropy");
			return solver;
		}

		private EntropyFrequencyThresholdSolver GetFrequencyThresholdSolver()
		{
			double threshold = 0.5;
			var solver = new EntropyFrequencyThresholdSolver(_context, Frequencies.Weighted, threshold, "EntropyFrequencyThreshold");
			return solver;
		}

		private MiniMaxSolver GetMinimaxSolver()
		{
			int limit = 25;
			var solver = new MiniMaxSolver(_context, limit, "MiniMax");
			return solver;
		}
		#endregion
	}
}
