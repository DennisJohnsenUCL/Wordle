using WordleCore.Utils;

namespace WordleCore.Tests
{
    [TestClass]
    public sealed class WordleGameUtilsTests
    {
        [TestMethod]
        public void AllowedWords_ContainsAllowedWords()
        {
            Assert.IsTrue(WordleGameUtils.allowedWords.Contains("ROSSA"));
        }
    }
}
