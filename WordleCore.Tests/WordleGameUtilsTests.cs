﻿using WordleCore.Enums;
using WordleCore.Utils;

namespace WordleCore.Tests
{
    [TestClass]
    public sealed class WordleGameUtilsTests
    {
        [TestMethod]
        public void LoadAllowedWords_ReturnsAllowedWords()
        {
            var allowedWords = WordleGameUtils.LoadAllowedWords();

            Assert.IsNotNull(allowedWords);
        }

        [TestMethod]
        public void AllowedWords_IsLoaded_ContainsAllowedWords()
        {
            Assert.IsTrue(WordleGameUtils._allowedWords.Contains("ROSSA"));
        }

        [TestMethod]
        public void AllowedWords_IsLoaded_ContainsOnlyAllowedWords()
        {
            Assert.IsFalse(WordleGameUtils._allowedWords.Contains("TRUCKS"));
        }

        [TestMethod]
        public void AllowedWords_IsLoaded_ContainsOnly5LetterWords()
        {
            var allowedWords = WordleGameUtils.LoadAllowedWords();

            Assert.IsTrue(allowedWords.All(x => x.Length == 5));
        }

        [TestMethod]
        public void IsAllowedWord_AllowedWord_ReturnsTrue()
        {
            Assert.IsTrue(WordleGameUtils.IsAllowedWord("CIGAR"));
        }

        [TestMethod]
        public void IsAllowedWord_NotAllowedWord_ReturnsFalse()
        {
            Assert.IsFalse(WordleGameUtils.IsAllowedWord("AAAAA"));
        }

        [TestMethod]
        public void PreviousWordles_IsLoaded_ContainsPreviousWordles()
        {
            Assert.IsTrue(WordleGameUtils._previousWordles.Value.Contains("CIGAR"));
        }

        [TestMethod]
        public void PreviousWordles_IsLoaded_ContainsOnlyPreviousWordles()
        {
            Assert.IsFalse(WordleGameUtils._previousWordles.Value.Contains("CIGARS"));
        }

        [TestMethod]
        public void PreviousWordles_IsLoaded_ContainsOnly5LetterWords()
        {
            Assert.IsTrue(WordleGameUtils._previousWordles.Value.All(x => x.Length == 5));
        }

        [TestMethod]
        public void PreviousWordles_AreAllAllowed()
        {
            Assert.IsTrue(WordleGameUtils._previousWordles.Value.All(WordleGameUtils._allowedWords.Contains));
        }

        [TestMethod]
        public void GetRandomWordle_GetsPreviousWordle()
        {
            var randomWordle = WordleGameUtils.GetRandomWordle();

            Assert.IsTrue(WordleGameUtils._previousWordles.Value.Contains(randomWordle));
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

            Assert.IsTrue(WordleGameUtils._allowedWords.Contains(randomWordle));
        }

        [TestMethod]
        public void GetCorrectnesses_AllCorrect_ReturnsAllCorrect()
        {
            var actual = WordleGameUtils.GetCorrectnesses("CIGAR", "CIGAR");
            Correctness[] expected = [.. Enumerable.Repeat(Correctness.Correct, 5)];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_AllAbsent_ReturnsAllAbsent()
        {
            var actual = WordleGameUtils.GetCorrectnesses("CIGAR", "BOOKS");
            Correctness[] expected = [.. Enumerable.Repeat(Correctness.Absent, 5)];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_Books_Kazoo_ReturnsPAAPP()
        {
            var actual = WordleGameUtils.GetCorrectnesses("BOOKS", "KAZOO");
            Correctness[] expected = [Correctness.Present, Correctness.Absent, Correctness.Absent, Correctness.Present, Correctness.Present];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_Total_Tools_ReturnsCCOPA()
        {
            var actual = WordleGameUtils.GetCorrectnesses("TOTAL", "TOOLS");
            Correctness[] expected = [Correctness.Correct, Correctness.Correct, Correctness.OverCount, Correctness.Present, Correctness.Absent];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_Total_Tarot_ReturnsCPAPP()
        {
            var actual = WordleGameUtils.GetCorrectnesses("TOTAL", "TAROT");
            Correctness[] expected = [Correctness.Correct, Correctness.Present, Correctness.Absent, Correctness.Present, Correctness.Present];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_Level_Leave_ReturnsCCAPP()
        {
            var actual = WordleGameUtils.GetCorrectnesses("LEVEL", "LEAVE");
            Correctness[] expected = [Correctness.Correct, Correctness.Correct, Correctness.Absent, Correctness.Present, Correctness.Present];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_Leave_Level_ReturnsCCPPO()
        {
            var actual = WordleGameUtils.GetCorrectnesses("LEAVE", "LEVEL");
            Correctness[] expected = [Correctness.Correct, Correctness.Correct, Correctness.Present, Correctness.Present, Correctness.OverCount];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_Total_Potoo_ReturnsACCOO()
        {
            var actual = WordleGameUtils.GetCorrectnesses("TOTAL", "POTOO");
            Correctness[] expected = [Correctness.Absent, Correctness.Correct, Correctness.Correct, Correctness.OverCount, Correctness.OverCount];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }

        [TestMethod]
        public void GetCorrectnesses_Pinto_Potoo_ReturnsCOPOC()
        {
            var actual = WordleGameUtils.GetCorrectnesses("PINTO", "POTOO");
            Correctness[] expected = [Correctness.Correct, Correctness.OverCount, Correctness.Present, Correctness.OverCount, Correctness.Correct];

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expected[i], actual[i].Correctness);
            }
        }
    }
}
