using WordleCore.Enums;
using WordleCore.Utils;

namespace WordleCore.Tests
{
    [TestClass]
    public sealed class WordleGameUtilsTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            WordleGameUtils.previousWordles = null;
        }

        [TestMethod]
        public void LoadAllowedWords_ReturnsAllowedWords()
        {
            var allowedWords = WordleGameUtils.LoadAllowedWords();

            Assert.IsNotNull(allowedWords);
        }

        [TestMethod]
        public void AllowedWords_IsLoaded_ContainsAllowedWords()
        {
            Assert.IsTrue(WordleGameUtils.allowedWords.Contains("ROSSA"));
        }

        [TestMethod]
        public void AllowedWords_IsLoaded_ContainsOnlyAllowedWords()
        {
            Assert.IsFalse(WordleGameUtils.allowedWords.Contains("TRUCKS"));
        }

        [TestMethod]
        public void AllowedWords_IsLoaded_ContainsOnly5LetterWords()
        {
            var allowedWords = WordleGameUtils.LoadAllowedWords();

            Assert.IsTrue(allowedWords.All(x => x.Length == 5));
        }

        [TestMethod]
        public void PreviousWordles_IsNotLoaded_IsNull()
        {
            Assert.IsNull(WordleGameUtils.previousWordles);
        }

        [TestMethod]
        public void PreviousWordles_IsLoaded_IsNotNull()
        {
            WordleGameUtils.LoadPreviousWordles();

            Assert.IsNotNull(WordleGameUtils.previousWordles);
        }

        [TestMethod]
        public void PreviousWordles_IsLoaded_ContainsPreviousWordles()
        {
            WordleGameUtils.LoadPreviousWordles();

            Assert.IsTrue(WordleGameUtils.previousWordles!.Contains("CIGAR"));
        }

        [TestMethod]
        public void PreviousWordles_IsLoaded_ContainsOnlyPreviousWordles()
        {
            WordleGameUtils.LoadPreviousWordles();

            Assert.IsFalse(WordleGameUtils.previousWordles!.Contains("CIGARS"));
        }

        [TestMethod]
        public void PreviousWordles_IsLoaded_ContainsOnly5LetterWords()
        {
            WordleGameUtils.LoadPreviousWordles();

            Assert.IsTrue(WordleGameUtils.previousWordles!.All(x => x.Length == 5));
        }

        [TestMethod]
        public void PreviousWordles_AreAllAllowed()
        {
            WordleGameUtils.LoadPreviousWordles();

            Assert.IsTrue(WordleGameUtils.previousWordles!.All(WordleGameUtils.allowedWords.Contains));
        }

        [TestMethod]
        public void GetRandomWordle_GetsPreviousWordle()
        {
            var randomWordle = WordleGameUtils.GetRandomWordle();

            Assert.IsTrue(WordleGameUtils.previousWordles!.Contains(randomWordle));
        }

        [TestMethod]
        public void GetRandomWordle_Gets5LetterWord()
        {
            var randomWordle = WordleGameUtils.GetRandomWordle();

            Assert.IsTrue(randomWordle.Length == 5);
        }

        [TestMethod]
        public void GetRandomWordle_GetsAllowedWords()
        {
            var randomWordle = WordleGameUtils.GetRandomWordle();

            Assert.IsTrue(WordleGameUtils.allowedWords.Contains(randomWordle));
        }

        [TestMethod]
        public void GetCorrectnesses_AllCorrect_ReturnsAllCorrect()
        {
            var actual = WordleGameUtils.GetCorrectnesses("CIGAR", "CIGAR");
            Correctness[] expected = [.. Enumerable.Repeat(Correctness.Correct, 5)];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetCorrectnesses_AllAbsent_ReturnsAllAbsent()
        {
            var actual = WordleGameUtils.GetCorrectnesses("CIGAR", "BOOKS");
            Correctness[] expected = [.. Enumerable.Repeat(Correctness.Absent, 5)];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetCorrectnesses_Books_Kazoo_ReturnsPAAPP()
        {
            var actual = WordleGameUtils.GetCorrectnesses("BOOKS", "KAZOO");
            Correctness[] expected = [Correctness.Present, Correctness.Absent, Correctness.Absent, Correctness.Present, Correctness.Present];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetCorrectnesses_Total_Tools_ReturnsCCAPA()
        {
            var actual = WordleGameUtils.GetCorrectnesses("TOTAL", "TOOLS");
            Correctness[] expected = [Correctness.Correct, Correctness.Correct, Correctness.Absent, Correctness.Present, Correctness.Absent];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetCorrectnesses_Total_Tarot_ReturnsCPAPP()
        {
            var actual = WordleGameUtils.GetCorrectnesses("TOTAL", "TAROT");
            Correctness[] expected = [Correctness.Correct, Correctness.Present, Correctness.Absent, Correctness.Present, Correctness.Present];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetCorrectnesses_Level_Leave_ReturnsCCAPP()
        {
            var actual = WordleGameUtils.GetCorrectnesses("LEVEL", "LEAVE");
            Correctness[] expected = [Correctness.Correct, Correctness.Correct, Correctness.Absent, Correctness.Present, Correctness.Present];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetCorrectnesses_Leave_Level_ReturnsCCPPA()
        {
            var actual = WordleGameUtils.GetCorrectnesses("LEAVE", "LEVEL");
            Correctness[] expected = [Correctness.Correct, Correctness.Correct, Correctness.Present, Correctness.Present, Correctness.Absent];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetCorrectnesses_Total_Potoo_ReturnsACCAA()
        {
            var actual = WordleGameUtils.GetCorrectnesses("TOTAL", "POTOO");
            Correctness[] expected = [Correctness.Absent, Correctness.Correct, Correctness.Correct, Correctness.Absent, Correctness.Absent];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        //>> This test fails because the last O in pinto is counted to label the first o in potoo correct
        //>> This is wrong since the correct O eats the O in pinto, leaving none left to mark present
        [TestMethod]
        public void GetCorrectnesses_Pinto_Potoo_ReturnsCAPAC()
        {
            var actual = WordleGameUtils.GetCorrectnesses("PINTO", "POTOO");
            Correctness[] expected = [Correctness.Correct, Correctness.Absent, Correctness.Present, Correctness.Absent, Correctness.Correct];

            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
