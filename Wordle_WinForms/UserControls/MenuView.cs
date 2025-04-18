using Wordle_WinForms.Enums;
using Wordle_WinForms.Events;
using Wordle_WinForms.Interfaces;
using WordleCore.Models;

namespace Wordle_WinForms.UserControls
{
    public partial class MenuView : UserControl
    {
        private readonly INavigationController<Views> _navigation;
        public event EventHandler<StartGameEventArgs>? StartGame;

        public MenuView(INavigationController<Views> navigation)
        {
            _navigation = navigation;

            InitializeComponent();
        }

        private void DefaultGameButton_Click(object sender, EventArgs e)
        {
            var options = new WordleOptions();
            StartGame?.Invoke(this, new StartGameEventArgs(options));
        }

        private void CustomGameButton_Click(object sender, EventArgs e)
        {
            _navigation.NavigateTo(Views.optionsView);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
