using WordleCore.Models;

namespace Wordle_WinForms.Events
{
    public class StartGameEventArgs : EventArgs
    {
        public WordleOptions Options { get; }

        public StartGameEventArgs(WordleOptions options)
        {
            Options = options;
        }
    }
}
