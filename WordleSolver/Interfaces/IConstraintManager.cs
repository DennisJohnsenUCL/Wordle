using WordleCore.Models;
using WordleSolver.Models;

namespace WordleSolver.Interfaces
{
	internal interface IConstraintManager
	{
		public void AddConstraints(IReadOnlyList<LetterResult> results);
		public bool FitsConstraints(Word word);
		public void Clear();
	}
}
