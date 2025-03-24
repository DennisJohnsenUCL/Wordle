using WordleCore.Enums;
using WordleCore.Utils;

namespace WordleCore.Tests
{
    [TestClass]
    public sealed class WordleGameUtilsTests
    {
        [TestMethod]
        public void LoadAllowedWords_ReturnsPreviousWordles()
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
        public void GetRandomWordle_GetsPreviousWordle()
        {
            var randomWordle = WordleGameUtils.GetRandomWordle();

            Assert.IsTrue(WordleGameUtils.previousWordles!.Contains(randomWordle));
        }

        [TestMethod]
        public void GetCorrectnesses_AllCorrect_ReturnsAllCorrect()
        {
            var correctnesses = WordleGameUtils.GetCorrectnesses("CIGAR", "CIGAR");
            var expected = Enumerable.Repeat(Correctness.Correct, 5);

            Assert.AreEqual(expected, correctnesses);
        }

        [TestMethod]
        public void GetCorrectnesses_AllAbsent_ReturnsAllAbsent()
        {
            var correctnesses = WordleGameUtils.GetCorrectnesses("CIGAR", "BOOKS");
            var expected = Enumerable.Repeat(Correctness.Absent, 5);

            Assert.AreEqual(expected, correctnesses);
        }
    }
}
