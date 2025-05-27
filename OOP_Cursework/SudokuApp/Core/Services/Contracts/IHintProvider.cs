using OOP_Cursework.SudokuApp.Core.Models;

namespace OOP_Cursework.SudokuApp.Core.Services.Contracts
{
    public interface IHintProvider
    {
        HintResult GetHint(int[,] grid, int[,] originalGrid);
    }
}
