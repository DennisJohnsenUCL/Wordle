using Wordle_WinForms.CustomControls;
using Wordle_WinForms.Utils;
using WordleCore.Enums;
using WordleCore.Models;

namespace Wordle_WinForms.UserControls
{
    public partial class AlphabetPanel : UserControl
    {
        public AlphabetPanel()
        {
            InitializeComponent();
            TabStop = false;
        }

        protected override void OnMouseDown(MouseEventArgs e) { }

        public void Reset()
        {
            foreach (AlphabetLabel label in Controls) label.BackColor = Color.White;
        }

        public void ColorizeAlphabet(LetterHints hints)
        {
            foreach (AlphabetLabel label in Controls)
            {
                if (hints.Absent.Contains(label.Text[0])) { label.BackColor = ColorProvider.Colors[Correctness.Absent]; }
                else if (hints.Present.Contains(label.Text[0])) { label.BackColor = ColorProvider.Colors[Correctness.Present]; }
                else if (hints.Correct.Contains(label.Text[0])) { label.BackColor = ColorProvider.Colors[Correctness.Correct]; }
            }
        }
    }
}
