using Wordle_WinForms.UserControls;
using WordleCore;
using WordleCore.Models;

namespace Wordle_WinForms
{
    public partial class MainForm : Form
    {
        private readonly MenuView menuView = new();
        private readonly GameView gameView = new();
        private readonly OptionsView optionsView = new();

        public MainForm()
        {
            InitializeComponent();
            InitializeViews();
            InitializeEvents();
        }

        private void InitializeViews()
        {
            menuView.Dock = DockStyle.Fill;
            gameView.Dock = DockStyle.Fill;
            optionsView.Dock = DockStyle.Fill;

            Controls.Add(menuView);
            Controls.Add(gameView);
            Controls.Add(optionsView);

            menuView.BringToFront();
        }

        private void InitializeEvents()
        {
            menuView.StartGame += (s, e) =>
            {
                var options = e.Options;
                var game = GetWordleGameFromOptions(options);
                gameView.Focus();
                gameView.StartGame(game);
                gameView.BringToFront();
            };

            menuView.GoToOptions += (s, e) =>
            {
                optionsView.Focus();
                optionsView.BringToFront();
            };

            gameView.GoBack += (s, e) =>
            {
                menuView.Focus();
                menuView.BringToFront();
            };
        }

        private static WordleGame GetWordleGameFromOptions(WordleOptions options)
        {
            var (wordle, guesses) = options;

            if (wordle != null && guesses != null) return new WordleGame(wordle, (int)guesses);
            else if (wordle != null) return new WordleGame(wordle);
            else if (guesses != null) return new WordleGame((int)guesses);
            else return new WordleGame();
        }
    }
}
