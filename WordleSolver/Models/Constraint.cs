using WordleCore.Enums;

namespace WordleSolver.Models
{
    public record Constraint
    {
        public char Letter { get; }
        public Correctness Correctness { get; }
        public int Position { get; }

        public Constraint(char letter, Correctness correctness, int position)
        {
            Letter = letter;
            Correctness = correctness;
            Position = position;
        }

        public void Deconstruct(out char letter, out Correctness correctness, out int position)
        {
            letter = Letter;
            correctness = Correctness;
            position = Position;
        }
    }
}
