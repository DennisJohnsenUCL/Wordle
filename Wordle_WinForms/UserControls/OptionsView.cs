﻿using Wordle_WinForms.Enums;
using Wordle_WinForms.Events;
using Wordle_WinForms.Interfaces;
using WordleCore.Models;
using WordleCore.Utils;

namespace Wordle_WinForms.UserControls
{
    public partial class OptionsView : UserControl
    {
        private readonly INavigationController<Views> _navigation;
        public event EventHandler<StartGameEventArgs>? StartGame;

        public OptionsView(INavigationController<Views> navigation)
        {
            _navigation = navigation;
            InitializeComponent();
        }

        private bool ValidateWordle()
        {
            var wordle = wordleTextBox.Text;

            if (!(wordle.Length == 0 || wordle.Length == 5))
            {
                InvalidWordleLabel.Text = "Wordle must be 5 letters";
                return false;
            }
            else if (wordle != "" && !WordleGameUtils.IsAllowedWord(wordle))
            {
                InvalidWordleLabel.Text = "Wordle must be on the list of allowed words";
                return false;
            }
            else return true;
        }

        private void Reset()
        {
            wordleTextBox.Text = "";
            guessesUpDown.Value = 6;
            InvalidWordleLabel.Text = "";
        }

        private void Submit()
        {
            var validInput = ValidateWordle();

            if (validInput)
            {
                var wordle = wordleTextBox.TextLength == 0 ? null : wordleTextBox.Text;
                var options = new WordleOptions(wordle, (int)guessesUpDown.Value);
                StartGame?.Invoke(this, new StartGameEventArgs(options));
                Reset();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Reset();
            _navigation.NavigateTo(Views.menuView);
        }

        private void WordleTextBox_TextChanged(object sender, EventArgs e)
        {
            InvalidWordleLabel.Text = "";
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            Submit();
        }

        private void WordleTextBox_Leave(object sender, EventArgs e)
        {
            ValidateWordle();
        }

        private void WordleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ActiveControl = guessesUpDown;
            }
        }

        private void GuessesUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Submit();
            }
        }

        private void OptionsView_Click(object sender, EventArgs e)
        {
            ActiveControl = InvalidWordleLabel;
        }
    }
}
