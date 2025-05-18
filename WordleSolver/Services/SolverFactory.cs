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
			var limits = _context.AnswerPools == AnswerPools.AllWords ? _limitsAllWords : _limitsOnlyWordles;

			var solvers = new List<ISolver>();

			foreach (var solverType in solversToGet)
			{
				ISolver solver = solverType switch
				{
					SolverTypes.Lazy => new LazySolver(_context.Words, "LazySolver"),
					SolverTypes.Filtered => new FilteredSolver(_context, "FilteredSolver"),
					SolverTypes.EntropyFlat => new EntropySolver(_context, Frequencies.Flat, limits[SolverTypes.EntropyFlat], "EntropyFlatSolver"),
					SolverTypes.EntropyWeighted => new EntropySolver(_context, Frequencies.Weighted, limits[SolverTypes.EntropyWeighted], "EntropyWeightedSolver"),
					SolverTypes.EntropySigmoid => new EntropySolver(_context, Frequencies.Sigmoid, limits[SolverTypes.EntropySigmoid], "EntropySigmoidSolver"),
					SolverTypes.EntropyLog => new EntropySolver(_context, Frequencies.Log, limits[SolverTypes.EntropyLog], "EntropyLogSolver"),
					SolverTypes.PositionalFlat => new PositionalEntropySolver(_context, Frequencies.Flat, limits[SolverTypes.PositionalFlat], "PositionalEntropyFlatSolver"),
					SolverTypes.PositionalWeighted => new PositionalEntropySolver(_context, Frequencies.Weighted, limits[SolverTypes.PositionalWeighted], "PositionalEntropyWeightedSolver"),
					SolverTypes.PositionalSigmoid => new PositionalEntropySolver(_context, Frequencies.Sigmoid, limits[SolverTypes.PositionalSigmoid], "PositionalEntropySigmoidSolver"),
					SolverTypes.PositionalLog => new PositionalEntropySolver(_context, Frequencies.Log, limits[SolverTypes.PositionalLog], "PositionalEntropyLogSolver"),
					SolverTypes.FrequencyThreshold => new EntropyFrequencyThresholdSolver(_context, Frequencies.Weighted, 0.5, "EntropyFrequencyThreshold"),
					SolverTypes.MiniMax => new MiniMaxSolver(_context, limits[SolverTypes.MiniMax], "MiniMax"),
					_ => throw new Exception()
				};

				solvers.Add(solver);
			}
			return solvers;
		}

		private readonly Dictionary<SolverTypes, int> _limitsAllWords = new()
		{
			{ SolverTypes.EntropyFlat, 15 },
			{ SolverTypes.EntropyWeighted, 13 },
			{ SolverTypes.EntropySigmoid, 13 },
			{ SolverTypes.EntropyLog, 14 },
			{ SolverTypes.PositionalFlat, 14 },
			{ SolverTypes.PositionalWeighted, 14 },
			{ SolverTypes.PositionalSigmoid, 6 },
			{ SolverTypes.PositionalLog, 5 },
			{ SolverTypes.MiniMax, 16 }
		};

		private readonly Dictionary<SolverTypes, int> _limitsOnlyWordles = new()
		{
			{ SolverTypes.EntropyFlat, 15 },
			{ SolverTypes.EntropyWeighted, 13 },
			{ SolverTypes.EntropySigmoid, 13 },
			{ SolverTypes.EntropyLog, 14 },
			{ SolverTypes.PositionalFlat, 14 },
			{ SolverTypes.PositionalWeighted, 14 },
			{ SolverTypes.PositionalSigmoid, 6 },
			{ SolverTypes.PositionalLog, 5 },
			{ SolverTypes.MiniMax, 16 }
		};
	}
}
