using Wordle_WinForms.UserControls;

namespace Wordle_WinForms.CustomControls
{
    public class WordleFlowPanel : FlowLayoutPanel
    {
        private WordleRow? _activeRow;

        public WordleFlowPanel()
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
            AutoScroll = true;
            FlowDirection = FlowDirection.TopDown;
            Size = new Size(244, 400);
        }

        public string GetActiveWord() => _activeRow?.GetWord() ?? "";

        public bool AddLetter(char c) => _activeRow?.AddLetter(c) ?? false;

        public bool RemoveLetter() => _activeRow?.RemoveLetter() ?? false;

        public void AddRow()
        {
            var row = new WordleRow();
            Controls.Add(row);
            _activeRow = row;
        }

        public bool PrintCorrectness(char[] chars, Color[] colors)
        {
            if (_activeRow == null) return false;

            for (int i = 0; i < chars.Length; i++)
            {
                _activeRow.Controls[i].Text = chars[i].ToString();
                _activeRow.Controls[i].BackColor = colors[i];
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
            Controls.Add(label);
        }

        public void Reset()
        {
            _activeRow = null;
            Controls.Clear();
        }
    }
}
