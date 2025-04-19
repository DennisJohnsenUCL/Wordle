using Wordle_WinForms.Enums;
using Wordle_WinForms.Events;
using Wordle_WinForms.Interfaces;
using WordleCore.Models;
using WordleCore.Utils;

namespace Wordle_WinForms.UserControls
{
    public partial class OptionsView : UserControl
    {
        private readonly INavigationController<Views> _navigation;
        public event EventHandler<StartGameEventArgs>? StartGame;

        public OptionsView(INavigationController<Views> navigation)
        {
            _navigation = navigation;

            InitializeComponent();
            ActiveControl = WordleTextBox;
        }

        private bool ValidateWordle()
        {
            var wordle = WordleTextBox.Text;

            if (!(wordle.Length == 0 || wordle.Length == 5))
            {
                InvalidWordleLabel.Text = "Wordle must be 5 letters";
                return false;
            }
            else if (wordle != "" && !WordleGameUtils.IsAllowedWord(wordle))
            {
                InvalidWordleLabel.Text = "Wordle must be on the list of allowed words";
                return false;
            }
            else return true;
        }

        private void Reset()
        {
            WordleTextBox.Text = "";
            GuessesUpDown.Value = 6;
            InvalidWordleLabel.Text = "";
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Reset();
            _navigation.NavigateTo(Views.menuView);
        }

        private void WordleTextBox_TextChanged(object sender, EventArgs e)
        {
            InvalidWordleLabel.Text = "";
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            var validInput = ValidateWordle();

            if (validInput)
            {
                var wordle = WordleTextBox.TextLength == 0 ? null : WordleTextBox.Text;
                var options = new WordleOptions(wordle, (int)GuessesUpDown.Value);
                StartGame?.Invoke(this, new StartGameEventArgs(options));
                Reset();
            }
        }

        private void WordleTextBox_Leave(object sender, EventArgs e)
        {
            ValidateWordle();
        }
    }
}
