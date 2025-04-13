using Wordle_WinForms.UserControls;

namespace Wordle_WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeViews();
        }

        private void InitializeViews()
        {
            var menuView = new MenuView();
            var gameView = new GameView();

            menuView.Dock = DockStyle.Fill;
            gameView.Dock = DockStyle.Fill;

            Controls.Add(menuView);
            Controls.Add(gameView);

            menuView.BringToFront();
        }
    }
}
