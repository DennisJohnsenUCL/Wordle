using WordleCore;

namespace WordleSolver.Services
{
    internal class WordleGameFactory
    {
        public IEnumerable<WordleGame> CreateGames(IEnumerable<string> wordles, int guesses)
        {
            var games = new List<WordleGame>();

            foreach (var wordle in wordles)
            {
                games.Add(new WordleGame(wordle, guesses));
            }

            return games;
        }
    }
}
