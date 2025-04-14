namespace Wordle_WinForms.UserControls
{
    public partial class WordleRow : UserControl
    {
        public WordleRow()
        {
            InitializeComponent();
            TabStop = false;
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}
