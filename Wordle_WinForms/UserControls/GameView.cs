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
        private int? _guesses = null;

        public GameView(INavigationController<Views> navigation)
        {
            _navigation = navigation;

            InitializeComponent();
        }

        public void StartGame(WordleGame game)
        {
            _game = game;
            _game.Start();
            _guesses = _game.Guesses;
            wordleLabel.Text = _game.Wordle;
            StartNewRow();
        }

        private void Reset()
        {
            _game = null;
            wordlePanel.Reset();
            alphabetPanel.Reset();
            guessesLabel.Text = "";
        }

        private void GameView_KeyDown(object sender, KeyEventArgs e)
        {
            var k = e.KeyData;
            var c = e.KeyCode;

            if (c >= Keys.A && c <= Keys.Z) wordlePanel.AddLetter((char)k);
            else if (k == Keys.Enter) HandleEnterPress();
            else if (k == Keys.Back) wordlePanel.RemoveLetter();
        }

        private void HandleEnterPress()
        {
            if (_game != null)
            {
                HandleWordleGuess();
            }
            else
            {
                HandleNewGameClick();
            }
        }

        private void HandleWordleGuess()
        {
            if (_game == null) return;

            string guess = wordlePanel.GetActiveWord();
            if (guess.Length == 5 && WordleGameUtils.IsAllowedWord(guess))
            {
                var response = _game.GuessWordle(guess);

                wordlePanel.PrintCorrectness(response);
                alphabetPanel.ColorizeAlphabet(_game.LetterHints);

                if (IsGameOver()) HandleGameOver();
                else StartNewRow();
            }
        }

        private bool IsGameOver()
        {
            return _game?.GameState == GameState.Completed || _game?.GameState == GameState.Failed;
        }

        private void HandleGameOver()
        {
            string message = "";

            if (_game?.GameState == GameState.Completed)
            {
                message = $"You guessed the Wordle\nWith {_game!.GuessesLeft} guesses to spare";
            }
            else if (_game?.GameState == GameState.Failed)
            {
                message = $"Better luck next time!\nThe wordle was {_game!.Wordle}";
            }

            wordlePanel.PrintMessage(message);
            guessesLabel.Text = "";
            _game = null;
        }

        private void StartNewRow()
        {
            wordlePanel.AddRow();
            guessesLabel.Text = $"You have {_game!.GuessesLeft} guesses left";
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Reset();
            _navigation.NavigateTo(Views.menuView);
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            HandleNewGameClick();
        }

        private void HandleNewGameClick()
        {
            Reset();
            StartGame(new WordleGame((int)_guesses!));
            ActiveControl = null;
        }
    }
}
