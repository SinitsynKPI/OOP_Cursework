using OOP_Cursework.SudokuApp.Core.Models;
using OOP_Cursework.SudokuApp.Core.Services.Contracts;
using OOP_Cursework.SudokuApp.Core.Utilities;
using System.Collections.Generic;

namespace OOP_Cursework.SudokuApp.Core.Services
{
    public class SudokuSolver : ISudokuSolver
    {
        public bool Solve(int[,] grid, List<StepResult> steps = null)
        {
            return SolveRecursive(grid, steps);
        }
        private bool SolveRecursive(int[,] grid, List<StepResult> steps, int depth = 0)
        {
            var (row, col) = FindMostConstrainedCell(grid);
            if (row == -1) return true;
            for (int num = 1; num <= 9; num++)
            {
                if (IsValid(grid, row, col, num))
                {
                    steps?.Add(new StepResult
                    {
                        Row = row,
                        Column = col,
                        Number = num,
                        Explanation = $"Step {steps?.Count + 1}: Place {num} at ({row + 1},{col + 1})"
                    });

                    grid[row, col] = num;

                    if (SolveRecursive(grid, steps, depth + 1))
                        return true;
                    grid[row, col] = 0;
                    steps?.Add(new StepResult
                    {
                        Row = row,
                        Column = col,
                        Number = 0,
                        Explanation = $"Step {steps?.Count + 1}: Backtrack at ({row + 1},{col + 1})"
                    });
                }
            }
            return false;
        }
        private (int row, int col) FindMostConstrainedCell(int[,] grid)
        {
            int minOptions = 10;
            var result = (-1, -1);
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        int options = CountPossibleNumbers(grid, row, col);
                        if (options < minOptions)
                        {
                            minOptions = options;
                            result = (row, col);
                            if (minOptions == 1) break;
                        }
                    }
                }
                if (minOptions == 1) break;
            }
            return result;
        }
        private int CountPossibleNumbers(int[,] grid, int row, int col)
        {
            int count = 0;
            for (int num = 1; num <= 9; num++)
                if (IsValid(grid, row, col, num)) count++;
            return count;
        }
        private bool IsValid(int[,] grid, int row, int col, int num)
        {
            return Validatior.IsValid(grid, row, col, num);
        }
    }
}
