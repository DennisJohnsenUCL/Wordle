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
					SolverTypes.EntropyFlat => new EntropySolver(_context, Frequencies.Flat, 15, "EntropyFlatSolver"),
					SolverTypes.EntropyWeighted => new EntropySolver(_context, Frequencies.Weighted, 13, "EntropyWeightedSolver"),
					SolverTypes.EntropySigmoid => new EntropySolver(_context, Frequencies.Sigmoid, 13, "EntropySigmoidSolver"),
					SolverTypes.EntropyLog => new EntropySolver(_context, Frequencies.Log, 14, "EntropyLogSolver"),
					SolverTypes.PositionalFlat => new PositionalEntropySolver(_context, Frequencies.Flat, 14, "PositionalEntropyFlatSolver"),
					SolverTypes.PositionalWeighted => new PositionalEntropySolver(_context, Frequencies.Weighted, 14, "PositionalEntropyWeightedSolver"),
					SolverTypes.PositionalSigmoid => new PositionalEntropySolver(_context, Frequencies.Sigmoid, 6, "PositionalEntropySigmoidSolver"),
					SolverTypes.PositionalLog => new PositionalEntropySolver(_context, Frequencies.Log, 5, "PositionalEntropyLogSolver"),
					SolverTypes.FrequencyThreshold => new EntropyFrequencyThresholdSolver(_context, Frequencies.Weighted, 0.5, "EntropyFrequencyThreshold"),
					SolverTypes.MiniMax => new MiniMaxSolver(_context, 16, "MiniMax"),
					_ => throw new Exception()
				};

				solvers.Add(solver);
			}
			return solvers;
		}
	}
}
