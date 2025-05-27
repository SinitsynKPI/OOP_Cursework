using OOP_Cursework.SudokuApp.Core.Models;
using System;

namespace OOP_Cursework.SudokuApp.Core.Events
{
    public class GameEvents
    {
        public event Action<GameState> StateChanged;
        public event Action<string> MessagePublished;
        public void RaiseStateChanged(GameState state) => StateChanged?.Invoke(state);
        public void PublishMessage(string message) => MessagePublished?.Invoke(message);
    }
}
