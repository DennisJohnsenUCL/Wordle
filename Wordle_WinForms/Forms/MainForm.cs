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

            menuView.Dock = DockStyle.Fill;

            Controls.Add(menuView);

            menuView.BringToFront();
        }
    }
}
