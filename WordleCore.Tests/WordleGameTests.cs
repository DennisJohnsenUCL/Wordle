using WordleCore.Enums;
using WordleCore.Exceptions;
using WordleCore.Utils;

namespace WordleCore.Tests
{
    [TestClass]
    public sealed class WordleGameTests
    {
        [TestMethod]
        public void Constructor_AllInputs_SetsInputs()
        {
            WordleGame wordleGame = new("CIGAR", 6);

            Assert.AreEqual("CRANE", wordleGame.Wordle);
            Assert.AreEqual(6, wordleGame.Guesses);
        }

        [TestMethod]
        public void Constructor_NoInputs_SetsDefaults()
        {
            WordleGame wordleGame = new();

            Assert.IsTrue(WordleGameUtils.previousWordles!.Contains(wordleGame.Wordle));
            Assert.AreEqual(6, wordleGame.Guesses);
        }

        [TestMethod]
        public void Constructor_WordleInput_SetsDefaultGuesses()
        {
            WordleGame wordleGame = new("CIGAR");

            Assert.AreEqual(6, wordleGame.Guesses);
        }

        [TestMethod]
        public void Constructor_GuessesInput_GetsRandomWordle()
        {
            WordleGame wordleGame = new(6);

            Assert.IsTrue(WordleGameUtils.previousWordles!.Contains(wordleGame.Wordle));
        }

        [TestMethod]
        public void Constructor_Instantiation_IsNotStarted()
        {
            WordleGame wordleGame = new("CIGAR", 6);

            Assert.IsTrue(wordleGame.GameState == GameState.NotStarted);
        }

        [TestMethod]
        public void Constructor_Instantiation_HasNoGuesses()
        {
            WordleGame wordleGame = new("CIGAR", 6);

            Assert.IsTrue(wordleGame.GuessesLeft == 0);
        }

        [TestMethod]
        public void Constructor_WordleTooLong_ThrowsException()
        {
            var action = () => { WordleGame wordleGame = new("CIGARS", 6); };

            Assert.ThrowsException<WordleWrongLengthException>(action);
        }

        [TestMethod]
        public void Constructor_WordleTooShort_ThrowsException()
        {
            var action = () => { WordleGame wordleGame = new("CIGA", 6); };

            Assert.ThrowsException<WordleWrongLengthException>(action);
        }

        [TestMethod]
        public void Constructor_NoGuesses_ThrowsException()
        {
            var action = () => { WordleGame wordleGame = new("CIGAR", 0); };

            Assert.ThrowsException<NoGuessesException>(action);
        }

        [TestMethod]
        public void Constructor_NotAllowedWord_ThrowsException()
        {
            var action = () => { WordleGame wordleGame = new("AAAAA", 6); };

            Assert.ThrowsException<WordleNotAllowedWordException>(action);
        }

        [TestMethod]
        public void Start_GameNotStarted_StartsGame()
        {
            WordleGame wordleGame = new("CIGAR", 6);
            wordleGame.Start();

            Assert.IsTrue(wordleGame.GameState == GameState.Ongoing);
        }

        [TestMethod]
        public void Start_GameNotStarted_SetsGuessesLeft()
        {
            WordleGame wordleGame = new("CIGAR", 6);
            wordleGame.Start();

            Assert.IsTrue(wordleGame.GuessesLeft == 6);
        }

        [TestMethod]
        public void Start_GameAlreadyStarted_ThrowsException()
        {
            WordleGame wordleGame = new("CIGAR", 6);
            wordleGame.Start();
            var action = () => wordleGame.Start();

            Assert.ThrowsException<WordleGameAlreadyStartedException>(action);
        }
    }
}
