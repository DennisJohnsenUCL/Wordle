using Wordle_WinForms.UserControls;
using Wordle_WinForms.Utils;
using WordleCore.Models;

namespace Wordle_WinForms.CustomControls
{
    public class WordleFlowPanel : FlowLayoutPanel
    {
        private WordleRow? _activeRow;

        public WordleFlowPanel()
        {
            AutoScroll = true;
            FlowDirection = FlowDirection.TopDown;
            Size = new Size(246, 400);
            WrapContents = false;
        }

        public string GetActiveWord() => _activeRow?.GetWord() ?? "";

        public bool AddLetter(char c) => _activeRow?.AddLetter(c) ?? false;

        public bool RemoveLetter() => _activeRow?.RemoveLetter() ?? false;

        public void AddRow()
        {
            var row = new WordleRow();
            AddAndScroll(row);
            _activeRow = row;
        }

        public bool PrintCorrectness(WordleResponse response)
        {
            if (_activeRow == null) return false;

            for (int i = 0; i < 5; i++)
            {
                var (letter, correctness) = response.LetterResults[i];

                _activeRow.Controls[i].Text = letter.ToString();
                _activeRow.Controls[i].BackColor = ColorProvider.Colors[correctness];
            }
            return true;
        }

        public void PrintMessage(string message)
        {
            _activeRow = null;
            var label = new Label
            {
                AutoSize = true,
                Margin = new Padding(5),
                Font = new Font(DefaultFont.FontFamily, 14),
                Text = message
            };
            AddAndScroll(label);
        }

        private void AddAndScroll(Control control)
        {
            Controls.Add(control);
            VerticalScroll.Value = VerticalScroll.Maximum;
        }

        public void Reset()
        {
            _activeRow = null;
            Controls.Clear();
        }
    }
}
