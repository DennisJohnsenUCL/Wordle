﻿using WordleCore.Models;

namespace Wordle_Console.Interfaces
{
    internal interface IInputHandler
    {
        WordleOptions GetWordleOptions();
        string GetWordleGuessInput();
    }
}
