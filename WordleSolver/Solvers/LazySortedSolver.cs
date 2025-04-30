using WordleCore.Utils;

namespace WordleSolver.Solvers
{
    internal class LazySortedSolver : LazyRandomSolver
    {
        protected override List<string> Words { get; } = LoadWords();
        public override string Identifier { get; } = "Solver2, guesses all words (no mask) ordered by usage in literature";

        private static List<string> LoadWords()
        {
            return [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words_sorted.txt")];
        }
    }
}
