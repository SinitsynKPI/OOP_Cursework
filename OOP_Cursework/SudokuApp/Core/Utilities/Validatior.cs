using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Cursework.SudokuApp.Core.Utilities
{
    public static class Validatior
    {
        public static bool IsValid(int[,] grid, int row, int col, int num)
        {
            for (int c = 0; c < 9; c++)
                if (grid[row, c] == num) return false;
            for (int r = 0; r < 9; r++)
                if (grid[r, col] == num) return false;
            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (grid[r, c] == num) return false;

            return true;
        }
    }
}
