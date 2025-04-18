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
        private readonly MenuView _menuView;
        private readonly GameView _gameView;
        private readonly OptionsView _optionsView = new();

        public MainForm(INavigationController<Views> navigation)
        {
            _navigation = navigation;
            _menuView = new(_navigation);
            _gameView = new(_navigation);

            InitializeComponent();
            InitializeViews();
            InitializeEvents();
        }

        private void InitializeViews()
        {
            _menuView.Dock = DockStyle.Fill;
            _gameView.Dock = DockStyle.Fill;
            _optionsView.Dock = DockStyle.Fill;

            Controls.Add(_menuView);
            Controls.Add(_gameView);
            Controls.Add(_optionsView);

            _navigation.Register(Views.menuView, _menuView);
            _navigation.Register(Views.optionsView, _optionsView);
            _navigation.Register(Views.gameView, _gameView);

            _navigation.NavigateTo(Views.menuView);
        }

        private void InitializeEvents()
        {
            _menuView.StartGame += (s, e) =>
            {
                var options = e.Options;
                var game = GetWordleGameFromOptions(options);
                _gameView.StartGame(game);
                _navigation.NavigateTo(Views.gameView);
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
