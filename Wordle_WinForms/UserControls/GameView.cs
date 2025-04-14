using System.Text;
using Wordle_WinForms.CustomControls;
using WordleCore;
using WordleCore.Enums;
using WordleCore.Models;
using WordleCore.Utils;

namespace Wordle_WinForms.UserControls
{
    public partial class GameView : UserControl
    {
        private WordleRow? _activeRow = null;
        private WordleGame? _game = null;

        public GameView()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
            WordleRowsFlowPanel.TabStop = false;
        }

        public void StartGame(WordleGame game)
        {
            _game = game;
            _game.Start();
            StartNewRow();
        }

        private void PrintWordleGuessCorrectness(WordleResponse response)
        {
            if (_activeRow != null)
            {
                for (int i = 0; i < response.Chars.Length; i++)
                {
                    _activeRow.Controls[i].Text = response.Chars[i].ToString();
                    _activeRow.Controls[i].BackColor = _colors[response.Correctness[i]];
                }
            }
        }

        private void GameView_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.KeyData;
            var c = e.KeyCode;

            if (c >= Keys.A && c <= Keys.Z) HandleKeyPress((char)k);
            if (k == Keys.Enter) HandleEnterPress();
            else if (k == Keys.Back) HandleBackPress();
        }

        private void HandleKeyPress(char c)
        {
            if (_activeRow != null)
            {
                foreach (LetterLabel control in _activeRow.Controls)
                {
                    if (control.Text == "") { control.Text = c.ToString(); break; }
                }
            }
        }

        private void HandleEnterPress()
        {
            if (_activeRow != null && _game != null)
            {
                string guess = GetRow();
                if (guess.Length == 5 && WordleGameUtils.IsAllowedWord(guess))
                {
                    var response = _game.GuessWordle(guess);
                    PrintWordleGuessCorrectness(response);

                    StartNewRow();
                }
            }
        }

        private void HandleBackPress()
        {
            if (_activeRow != null)
            {
                for (int i = _activeRow.Controls.Count - 1; i >= 0; i--)
                {
                    if (_activeRow.Controls[i].Text != "") { _activeRow.Controls[i].Text = ""; break; }
                }
            }
        }

        private string GetRow()
        {
            var sb = new StringBuilder();
            if (_activeRow != null)
            {
                foreach (LetterLabel control in _activeRow.Controls)
                {
                    sb.Append(control.Text);
                }
            }
            return sb.ToString();
        }

        private void StartNewRow()
        {
            var row = new WordleRow();
            WordleRowsFlowPanel.Controls.Add(row);
            _activeRow = row;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab)
                return true;
            return base.ProcessDialogKey(keyData);
        }

        private static readonly Dictionary<Correctness, Color> _colors = new()
        {
            { Correctness.Correct, Color.Green },
            { Correctness.Absent, Color.Red },
            { Correctness.OverCount, Color.Red },
            { Correctness.Present, Color.Yellow }
        };
    }
}
