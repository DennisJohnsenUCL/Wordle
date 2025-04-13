using Wordle_WinForms.Events;
using WordleCore.Models;

namespace Wordle_WinForms.UserControls
{
    public partial class MenuView : UserControl
    {
        public event EventHandler<StartGameEventArgs> StartGame = delegate { };
        public event EventHandler GoToOptions = delegate { };

        public MenuView()
        {
            InitializeComponent();
        }

        private void DefaultGameButton_Click(object sender, EventArgs e)
        {
            var options = new WordleOptions();
            StartGame.Invoke(this, new StartGameEventArgs(options));
        }

        private void CustomGameButton_Click(object sender, EventArgs e)
        {
            GoToOptions.Invoke(this, EventArgs.Empty);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
