namespace Wordle_Console.Models
{
    internal class InputValidationRules
    {
        public Predicate<char> AcceptChar { get; set; } = _ => true;
        public Predicate<string> CanSubmit { get; set; } = _ => true;
        public bool AllowEmpty { get; set; } = false;
    }
}
