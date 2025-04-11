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

            Assert.AreEqual("CIGAR", wordleGame.Wordle);
            Assert.AreEqual(6, wordleGame.Guesses);
        }

        [TestMethod]
        public void Constructor_NoInputs_SetsDefaults()
        {
            WordleGame wordleGame = new();

            Assert.IsTrue(WordleGameUtils._previousWordles.Value.Contains(wordleGame.Wordle));
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

            Assert.IsTrue(WordleGameUtils._previousWordles.Value.Contains(wordleGame.Wordle));
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

        [TestMethod]
        public void GuessWordle_GameNotYetStarted_ThrowsException()
        {
            WordleGame wordleGame = new();
            var action = () => wordleGame.GuessWordle("CIGAR");

            Assert.ThrowsException<WordleGameNotOnGoingException>(action);
        }

        [TestMethod]
        public void GuessWordle_GameOver_ThrowsException()
        {
            WordleGame wordleGame = new(1);
            wordleGame.Start();
            wordleGame.GuessWordle("POTOO");

            var action = () => wordleGame.GuessWordle("CIGAR");

            Assert.ThrowsException<WordleGameNotOnGoingException>(action);
        }

        [TestMethod]
        public void GuessWordle_GameCompleted_ThrowsException()
        {
            WordleGame wordleGame = new("CIGAR");
            wordleGame.Start();
            wordleGame.GuessWordle("CIGAR");

            var action = () => wordleGame.GuessWordle("POTOO");

            Assert.ThrowsException<WordleGameNotOnGoingException>(action);
        }

        [TestMethod]
        public void GuessWordle_GameOngoing_ReducesGuessesBy1()
        {
            WordleGame wordleGame = new("CIGAR", 2);
            wordleGame.Start();
            wordleGame.GuessWordle("POTOO");

            Assert.AreEqual(1, wordleGame.GuessesLeft);
        }

        [TestMethod]
        public void GuessWordle_GameOngoing_ReturnsCorrectChars()
        {
            WordleGame wordleGame = new("CIGAR", 2);
            wordleGame.Start();

            char[] expected = ['P', 'O', 'T', 'O', 'O'];
            char[] actual = [.. wordleGame.GuessWordle("POTOO").Chars];

            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        public void GuessWordle_GameOngoing_ReturnsCorrectCorrectnesses()
        {
            WordleGame wordleGame = new("TOTAL", 2);
            wordleGame.Start();

            Correctness[] expected = [Correctness.Absent, Correctness.Correct, Correctness.Correct, Correctness.OverCount, Correctness.OverCount];
            Correctness[] actual = [.. wordleGame.GuessWordle("POTOO").Correctness];

            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        public void AddToLetterHints_WordleGuess_LettersAddedCorrectly()
        {
            WordleGame wordleGame = new("CIGAR", 2);
            wordleGame.Start();

            wordleGame.GuessWordle("CRANE");

            Assert.IsTrue(wordleGame.LetterHints.Absent.Contains('N'));
            Assert.IsTrue(wordleGame.LetterHints.Absent.Contains('E'));
            Assert.IsTrue(wordleGame.LetterHints.Present.Contains('R'));
            Assert.IsTrue(wordleGame.LetterHints.Present.Contains('A'));
            Assert.IsTrue(wordleGame.LetterHints.Correct.Contains('C'));
        }
    }
}
