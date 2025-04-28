using WordleCore.Models;
using WordleCore.Utils;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class LazyRandomSolver : IWordleSolver
    {
        protected virtual List<string> Words { get; } = LoadWords();
        protected int Index { get; set; } = 0;
        public virtual string SolverIdentifier { get; } = "Solver1, guesses all words (no mask) in non-specific order";

        private static List<string> LoadWords()
        {
            return [.. WordleCoreUtils.LoadEmbeddedTxt("WordleCore.Data.allowed_words.txt")];
        }

        public virtual string GetFirstGuess()
        {
            return GetNextGuess();
        }

        public virtual void AddResponse(WordleResponse _) { }

        public virtual string GetNextGuess()
        {
            var guess = Words[Index];
            Index++;
            return guess;
        }

        public virtual void Reset()
        {
            Index = 0;
        }
    }
}
