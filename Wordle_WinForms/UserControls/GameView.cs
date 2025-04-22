using Wordle_WinForms.Enums;
using Wordle_WinForms.Interfaces;
using WordleCore;
using WordleCore.Enums;
using WordleCore.Utils;

namespace Wordle_WinForms.UserControls
{
    public partial class GameView : UserControl
    {
        private readonly INavigationController<Views> _navigation;
        private WordleGame? _game = null;

        public GameView(INavigationController<Views> navigation)
        {
            _navigation = navigation;

            InitializeComponent();
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
            WordlePanel.Reset();
            AlphabetPanel1.Visible = false;
            AlphabetPanel1.Reset();
            GuessesLabel.Text = "";
            NewGameButton.Visible = false;
        }

        private void GameView_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.KeyData;
            var c = e.KeyCode;

            if (c >= Keys.A && c <= Keys.Z) WordlePanel.AddLetter((char)k);
            else if (k == Keys.Enter) HandleEnterPress();
            else if (k == Keys.Back) WordlePanel.RemoveLetter();
        }

        private void HandleEnterPress()
        {
            if (_game != null)
            {
                string guess = WordlePanel.GetActiveWord();
                if (guess.Length == 5 && WordleGameUtils.IsAllowedWord(guess))
                {
                    var (chars, correctness) = _game.GuessWordle(guess);

                    WordlePanel.PrintCorrectness(chars, [.. correctness.Select(x => _colors[x])]);
                    AlphabetPanel1.ColorizeAlphabet(_game.LetterHints);

                    if (AlphabetPanel1.Visible == false) AlphabetPanel1.Visible = true;

                    if (!IsGameOver()) StartNewRow();
                    else NewGameButton.Visible = true;
                }
            }
        }

        private bool IsGameOver()
        {
            if (_game?.GameState == GameState.Completed)
            {
                GuessesLabel.Text = "";
                WordlePanel.PrintMessage($"You guessed the Wordle\nWith {_game!.GuessesLeft} guesses to spare");
                return true;
            }
            else if (_game?.GameState == GameState.Failed)
            {
                GuessesLabel.Text = "";
                WordlePanel.PrintMessage($"Better luck next time!\nThe wordle was {_game!.Wordle}");
                return true;
            }
            return false;
        }

        private void StartNewRow()
        {
            WordlePanel.AddRow();
            GuessesLabel.Text = $"You have {_game!.GuessesLeft} guesses left";
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Reset();
            _navigation.NavigateTo(Views.menuView);
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Reset();
            StartGame(new WordleGame());
            ActiveControl = null;
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
