using WordleCore.Utils;

namespace WordleCore.Tests
{
    [TestClass]
    public sealed class WordleCoreUtilsTests
    {
        [TestMethod]
        public void LoadEmbeddedResource_AllowedWords_ReturnsAllowedWords()
        {
            var allowedWords = WordleCoreUtils.LoadEmbeddedResource("WordleCore.Data.allowed_words.txt");

            Assert.IsNotNull(allowedWords);
        }

        [TestMethod]
        public void LoadEmbeddedResource_PreviousWordles_ReturnsPreviousWordles()
        {
            var previousWordles = WordleCoreUtils.LoadEmbeddedResource("WordleCore.Data.previous_wordles.txt");

            Assert.IsNotNull(previousWordles);
        }

        [TestMethod]
        public void LoadEmbeddedResource_BadResource_ThrowsError()
        {
            var path = "WordleCore.Data.previous_woordles.txt";
            var action = () => WordleCoreUtils.LoadEmbeddedResource(path);

            Assert.ThrowsException<InvalidOperationException>(action);
        }
    }
}
