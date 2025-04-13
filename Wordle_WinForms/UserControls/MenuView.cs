namespace Wordle_WinForms.UserControls
{
    public partial class MenuView : UserControl
    {
        public event EventHandler StartGame = delegate { };
        public event EventHandler GoToOptions = delegate { };

        public MenuView()
        {
            InitializeComponent();
        }

        private void DefaultGameButton_Click(object sender, EventArgs e)
        {
            StartGame.Invoke(this, EventArgs.Empty);
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
