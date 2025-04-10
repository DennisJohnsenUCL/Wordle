namespace Wordle_Console.Models
{
    internal class InputValidationRules
    {
        public Func<char, string, bool> AcceptChar { get; set; } = (c, s) => true;
        public Predicate<string> CanSubmit { get; set; } = _ => true;
        public bool AllowEmpty { get; set; } = false;
    }
}
