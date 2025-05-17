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

			foreach (var solverType in solversToGet)
			{
				ISolver solver = solverType switch
				{
					SolverTypes.Lazy => new LazySolver(_context.Words, "LazySolver"),
					SolverTypes.Filtered => new FilteredSolver(_context, "FilteredSolver"),
					SolverTypes.EntropyFlat => new EntropySolver(_context, Frequencies.Flat, 20, "EntropySolver"),
					SolverTypes.EntropyWeighted => new EntropySolver(_context, Frequencies.Weighted, 21, "EntropyFrequencySolver"),
					SolverTypes.EntropySigmoid => new EntropySolver(_context, Frequencies.Sigmoid, 20, "EntropyFrequencySigmoidSolver"),
					SolverTypes.EntropyLog => new EntropySolver(_context, Frequencies.Log, 20, "EntropyFrequencyLogSolver"),
					SolverTypes.PositionalWeighted => new PositionalEntropySolver(_context, Frequencies.Weighted, 20, "PositionalEntropy"),
					SolverTypes.FrequencyThreshold => new EntropyFrequencyThresholdSolver(_context, Frequencies.Weighted, 0.5, "EntropyFrequencyThreshold"),
					SolverTypes.MiniMax => new MiniMaxSolver(_context, 25, "MiniMax"),
					_ => throw new Exception()
				};

				solvers.Add(solver);

			}
			return solvers;
		}
	}
}
