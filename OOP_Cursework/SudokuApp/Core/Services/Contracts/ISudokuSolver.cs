using OOP_Cursework.SudokuApp.Core.Models;
using System.Collections.Generic;

namespace OOP_Cursework.SudokuApp.Core.Services.Contracts
{
    public interface ISudokuSolver
    {
        bool Solve(int[,] grid, List<StepResult> steps = null);
    }
}
