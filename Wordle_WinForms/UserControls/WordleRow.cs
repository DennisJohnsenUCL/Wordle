using System.Text;
using Wordle_WinForms.CustomControls;

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

        public string GetWord()
        {
            var sb = new StringBuilder();
            foreach (LetterLabel control in Controls)
            {
                sb.Append(control.Text);
            }
            return sb.ToString();
        }

        public bool AddLetter(char c)
        {
            if (Controls[^1].Text != "") return false;
            foreach (LetterLabel control in Controls)
            {
                if (control.Text == "")
                {
                    control.Text = c.ToString();
                    return true;
                }
            }
            return false;
        }

        public bool RemoveLetter()
        {
            if (Controls[0].Text == "") return false;
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                if (Controls[i].Text != "")
                {
                    Controls[i].Text = "";
                    return true;
                }
            }
            return false;
        }
    }
}
