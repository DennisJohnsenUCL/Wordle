using WordleCore.Models;

namespace WordleSolver.Interfaces
{
    internal interface IConstraintManager
    {
        public void AddConstraints(IReadOnlyList<LetterResult> results);
        public bool FitsConstraints(string word);
        public void Clear();
    }
}
