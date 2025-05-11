using WordleCore.Enums;
using WordleCore.Models;
using WordleSolver.Interfaces;
using WordleSolver.Models;

namespace WordleSolver.Core
{
	internal class ConstraintManager : IConstraintManager
	{
		private HashSet<Constraint> _constraints = [];
		private Dictionary<char, int> _counts = [];

		public void AddConstraints(IReadOnlyList<LetterResult> results)
		{
			for (int i = 0; i < results.Count; i++)
			{
				var (letter, correctness) = results[i];

				if (correctness == Correctness.OverCount)
				{
					var count = results.Count(r => r.Letter == letter &&
					(r.Correctness == Correctness.Present || r.Correctness == Correctness.Correct));
					_counts.TryAdd(letter, count);
				}
				var constraint = new Constraint(letter, correctness, i);
				_constraints.Add(constraint);
			}
		}

		public bool FitsConstraints(string word)
		{
			foreach (var constraint in _constraints)
			{
				var (letter, correctness, position) = constraint;

				if (correctness == Correctness.Absent && word.Contains(letter)) return false;
				if (correctness == Correctness.Correct && word[position] != letter) return false;
				if (correctness == Correctness.Present && !word.Remove(position, 1).Contains(letter)) return false;
				if (correctness == Correctness.OverCount && !word.Remove(position, 1).Contains(letter)) return false;
			}

			foreach (var count in _counts)
			{
				if (word.Count(c => c == count.Key) != count.Value) return false;
			}
			return true;
		}

		public void Clear()
		{
			_constraints = [];
			_counts = [];
		}
	}
}
