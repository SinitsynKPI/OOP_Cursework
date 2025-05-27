using OOP_Cursework.SudokuApp.Core.Enums;
using OOP_Cursework.SudokuApp.Core.Services.Contracts;
using OOP_Cursework.SudokuApp.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_Cursework.SudokuApp.Core.Services
{
    public class SudokuGenerator : ISudokuGenerator
    {
        private readonly Random _random = new Random();
        public (int[,] puzzle, int[,] solution) GeneratePuzzle(Difficulty difficulty)
        {
            int[,] solution = GenerateFullGrid();
            int cellsToRemove = GetCellsToRemove(difficulty);
            int[,] puzzle = RemoveNumbers(solution, cellsToRemove);
            return (puzzle, solution);
        }
        private int[,] GenerateFullGrid()
        {
            int[,] grid = new int[9, 9];
            FillDiagonalBoxes(grid);
            SolveSudoku(grid);
            return grid;
        }
        private void FillDiagonalBoxes(int[,] grid)
        {
            for (int box = 0; box < 9; box += 3)
            {
                List<int> numbers = Enumerable.Range(1, 9).ToList();
                numbers.Shuffle(_random);
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        grid[box + i, box + j] = numbers[i * 3 + j];
            }
        }
        private bool SolveSudoku(int[,] grid)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        List<int> numbers = Enumerable.Range(1, 9).ToList();
                        numbers.Shuffle(_random);

                        foreach (int num in numbers)
                        {
                            if (IsValid(grid, row, col, num))
                            {
                                grid[row, col] = num;
                                if (SolveSudoku(grid)) return true;
                                grid[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }
        private int[,] RemoveNumbers(int[,] grid, int cellsToRemove)
        {
            int[,] puzzle = (int[,])grid.Clone();
            var cells = GetAllCells().Shuffle(_random).ToList();
            int removed = 0;
            foreach (var (row, col) in cells)
            {
                if (removed >= cellsToRemove) break;
                int temp = puzzle[row, col];
                puzzle[row, col] = 0;
                if (CountSolutions((int[,])puzzle.Clone()) != 1)
                    puzzle[row, col] = temp;
                else
                    removed++;
            }
            return puzzle;
        }
        private int CountSolutions(int[,] grid)
        {
            int count = 0;
            CountSolutionsRecursive(grid, ref count);
            return count;
        }
        private bool CountSolutionsRecursive(int[,] grid, ref int count, int maxSolutions = 2)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsValid(grid, row, col, num))
                            {
                                grid[row, col] = num;
                                if (CountSolutionsRecursive(grid, ref count, maxSolutions))
                                {
                                    grid[row, col] = 0;
                                    return true;
                                }
                                grid[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            count++;
            return count >= maxSolutions;
        }
        private int GetCellsToRemove(Difficulty difficulty) => difficulty switch
        {
            Difficulty.Easy => _random.Next(40, 45),
            Difficulty.Medium => _random.Next(50, 55),
            Difficulty.Hard => _random.Next(58, 62),
            _ => 40
        };
        private IEnumerable<(int row, int col)> GetAllCells()
        {
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                    yield return (row, col);
        }
        private bool IsValid(int[,] grid, int row, int col, int num)
        {
            return Validatior.IsValid(grid, row, col, num);
        }
    }
}
