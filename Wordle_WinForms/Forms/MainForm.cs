using Wordle_WinForms.UserControls;

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
                gameView.BringToFront();
            };

            menuView.GoToOptions += (s, e) =>
            {
                optionsView.BringToFront();
            };
        }
    }
}
