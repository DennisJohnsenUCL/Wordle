using Wordle_WinForms.Enums;
using Wordle_WinForms.Interfaces;
using Wordle_WinForms.UserControls;
using WordleCore.Models;
using WordleCore.Utils;

namespace Wordle_WinForms
{
    public partial class MainForm : Form
    {
        private readonly INavigationController<Views> _navigation;
        private readonly MenuView _menuView;
        private readonly GameView _gameView;
        private readonly OptionsView _optionsView;

        public MainForm(INavigationController<Views> navigation)
        {
            _navigation = navigation;
            _menuView = new(_navigation);
            _optionsView = new(_navigation);
            _gameView = new(_navigation);

            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

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
            _menuView.StartGame += (s, e) => StartGame(e.Options);
            _optionsView.StartGame += (s, e) => StartGame(e.Options);
        }

        private void StartGame(WordleOptions options)
        {
            var game = WordleGameUtils.GetWordleGameFromOptions(options);
            _gameView.StartGame(game);
            _navigation.NavigateTo(Views.gameView);
        }
    }
}
