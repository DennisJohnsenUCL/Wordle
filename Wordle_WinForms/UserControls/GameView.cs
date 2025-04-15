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

        public event EventHandler? GoBack;

        public GameView()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;

            GoBack += (s, e) => Reset();
        }

        public void StartGame(WordleGame game)
        {
            _game = game;
            _game.Start();
            WordleLabel.Text = _game.Wordle;
            StartNewRow();
        }

        private void Reset()
        {
            _game = null;
            _activeRow = null;
            WordleRowsFlowPanel.Controls.Clear();
            GuessesLabel.Text = "";
            _gameOverMessage.Text = "";
            NewGameButton.Visible = false;
        }

        private void PrintWordleGuessCorrectness(WordleResponse response)
        {
            if (_activeRow != null)
            {
                var (chars, correctness) = response;

                for (int i = 0; i < chars.Length; i++)
                {
                    _activeRow.Controls[i].Text = chars[i].ToString();
                    _activeRow.Controls[i].BackColor = _colors[correctness[i]];
                }
            }
        }

        private void GameView_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.KeyData;
            var c = e.KeyCode;

            if (c >= Keys.A && c <= Keys.Z) HandleKeyPress((char)k);
            else if (k == Keys.Enter) HandleEnterPress();
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
            if (_game != null)
            {
                string guess = GetRow();
                if (guess.Length == 5 && WordleGameUtils.IsAllowedWord(guess))
                {
                    var response = _game.GuessWordle(guess);
                    PrintWordleGuessCorrectness(response);

                    if (!IsGameOver()) StartNewRow();
                    else NewGameButton.Visible = true;
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

        private bool IsGameOver()
        {
            if (_game != null)
            {
                if (_game.GameState == GameState.Completed)
                {
                    HandleGameOver($"You guessed the Wordle\nWith {_game!.GuessesLeft} guesses to spare");
                    return true;
                }
                else if (_game.GameState == GameState.Failed)
                {
                    HandleGameOver($"Better luck next time!\nThe wordle was {_game!.Wordle}");
                    return true;
                }
            }
            return false;
        }

        private void HandleGameOver(string message)
        {
            GuessesLabel.Text = "";
            _activeRow = null;
            _gameOverMessage.Text = message;
            WordleRowsFlowPanel.Controls.Add(_gameOverMessage);
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
            GuessesLabel.Text = $"You have {_game!.GuessesLeft} guesses left";
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

        private readonly Label _gameOverMessage = new()
        {
            AutoSize = true,
            Margin = new Padding(5),
            Font = new Font(DefaultFont.FontFamily, 14)
        };

        private void BackButton_Click(object sender, EventArgs e)
        {
            GoBack?.Invoke(this, e);
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Reset();
            StartGame(new WordleGame());
            ActiveControl = null;
        }
    }
}
