using WordleCore.Models;
using WordleCore.Utils;
using WordleSolver.Interfaces;

namespace WordleSolver.Solvers
{
    internal class WordleSolver1 : IWordleSolver
    {
        protected virtual List<string> Words { get; }
        private int _index = 0;

        public WordleSolver1()
        {
            Words = LoadWords();
        }

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
            var guess = Words[_index];
            _index++;
            return guess;
        }

        public virtual void Reset()
        {
            _index = 0;
        }
    }
}
