using OOP_Cursework.SudokuApp.Core.Enums;
using OOP_Cursework.SudokuApp.Core.Models;

namespace OOP_Cursework.SudokuApp.Core.Services.Contracts
{
    public interface IGameService
    {
        void GenerateNewPuzzle(Difficulty difficulty);
        GameState GetCurrentState();
        MoveResult MakeMove(int row, int col, int number);
        void UndoLastMove();
        HintResult GetHint();
        bool SolvePuzzle();
        StepResult GetNextStep();
        void ClearUserInput();
    }
}
