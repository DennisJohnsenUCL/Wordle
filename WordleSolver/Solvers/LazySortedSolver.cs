namespace WordleSolver.Solvers
{
    internal class LazySortedSolver : LazyRandomSolver
    {
        public override string Identifier { get; } = "LazySortedSolver, guesses all words (no mask) ordered by usage in literature";

        public LazySortedSolver(string[] words) : base(words) { }
    }
}
