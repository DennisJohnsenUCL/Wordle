using System.Text;
using Wordle_WinForms.CustomControls;
using WordleCore;
using WordleCore.Enums;
using WordleCore.Models;

namespace Wordle_WinForms.UserControls
{
    public partial class GameView : UserControl
    {
        private bool _listening = false;
        private WordleRow? _activeRow = null;
        private WordleGame? _game = null;
        private string? _newGuess = null;

        public GameView()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        public void StartGame(WordleGame game)
        {
            _game = game;
            _game.Start();
            var row = new WordleRow();
            WordleRowsFlowPanel.Controls.Add(row);
            _listening = true;
            _activeRow = row;
            //var guess = GetWordleGuessInput(row);
            //var response = game.GuessWordle(guess);
            //PrintWordleGuessCorrectness(response, row);
        }

        private void PrintWordleGuessCorrectness(WordleResponse response, WordleRow row)
        {
            for (int i = 0; i < response.Chars.Length; i++)
            {
                row.Controls[i].Text = response.Chars[i].ToString();
                row.Controls[i].BackColor = _colors[response.Correctness[i]];
            }
        }

        private string GetWordleGuessInput(WordleRow row)
        {
            var sb = new StringBuilder();
            foreach (LetterLabel control in row.Controls)
            {
                sb.Append(control.Text);
            }

            return sb.ToString();
        }

        private void GameView_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyData.ToString());
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
