using OOP_Cursework.SudokuApp.Core.Models;
using OOP_Cursework.SudokuApp.Core.Services.Contracts;

namespace OOP_Cursework.SudokuApp.Core.Services
{
    public class HintProvider : IHintProvider
    {
        private readonly ISudokuSolver _solver;

        public HintProvider(ISudokuSolver solver)
        {
            _solver = solver;
        }
        public HintResult GetHint(int[,] grid, int[,] originalGrid)
        {
            int[,] solution = (int[,])grid.Clone();
            if (!_solver.Solve(solution)) return null;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0 && originalGrid[row, col] == 0 && solution[row, col] > 0)
                    {
                        return new HintResult
                        {
                            Row = row,
                            Col = col,
                            Number = solution[row, col],
                            Explanation = $"Place {solution[row, col]} at ({row + 1}, {col + 1})"
                        };
                    }
                }
            }
            return null;
        }
    }
}
