using OOP_Cursework.SudokuApp.Core.Enums;

namespace OOP_Cursework.SudokuApp.Core.Services.Contracts
{
    public interface ISudokuGenerator
    {
        (int[,] puzzle, int[,] solution) GeneratePuzzle(Difficulty difficulty);
    }
}
