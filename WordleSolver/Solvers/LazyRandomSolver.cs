using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class LazyRandomSolver : ISolver
    {
        protected virtual string[] Words { get; }
        protected int Index { get; set; } = 0;
        public virtual string Identifier { get; } = "LazyRandomSolver, guesses all words (no mask) in non-specific order";

        public LazyRandomSolver(string[] words)
        {
            Words = words;
        }

        public virtual string GetFirstGuess()
        {
            return GetNextGuess();
        }

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
