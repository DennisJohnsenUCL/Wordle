using Wordle_WinForms.Enums;
using Wordle_WinForms.Interfaces;
using Wordle_WinForms.UserControls;
using WordleCore;
using WordleCore.Models;

namespace Wordle_WinForms
{
    public partial class MainForm : Form
    {
        private readonly INavigationController<Views> _navigation;
        private readonly MenuView menuView = new();
        private readonly GameView gameView = new();
        private readonly OptionsView optionsView = new();

        public MainForm(INavigationController<Views> navigation)
        {
            _navigation = navigation;
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

            _navigation.Register(Views.menuView, menuView);
            _navigation.Register(Views.optionsView, optionsView);
            _navigation.Register(Views.gameView, gameView);

            _navigation.NavigateTo(Views.menuView);
        }

        private void InitializeEvents()
        {
            menuView.StartGame += (s, e) =>
            {
                var options = e.Options;
                var game = GetWordleGameFromOptions(options);
                gameView.StartGame(game);
                _navigation.NavigateTo(Views.gameView);
            };

            menuView.GoToOptions += (s, e) =>
            {
                _navigation.NavigateTo(Views.optionsView);
            };

            gameView.GoBack += (s, e) =>
            {
                _navigation.NavigateTo(Views.menuView);
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
