using WordleSolver.Interfaces;

namespace WordleSolver.Services
{
    internal class StaticFirstGuessProvider : IFirstGuessProvider
    {
        public string Value { get; }

        public StaticFirstGuessProvider(string firstGuess)
        {
            Value = firstGuess;
        }
    }
}
