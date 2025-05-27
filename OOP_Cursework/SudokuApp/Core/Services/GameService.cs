using OOP_Cursework.SudokuApp.Core.Enums;
using OOP_Cursework.SudokuApp.Core.Events;
using OOP_Cursework.SudokuApp.Core.Models;
using OOP_Cursework.SudokuApp.Core.Services.Contracts;
using System;
using System.Collections.Generic;

namespace OOP_Cursework.SudokuApp.Core.Services
{
    public class GameService : IGameService
    {
        private readonly SudokuGame _game;
        private readonly ISudokuGenerator _generator;
        private readonly ISudokuSolver _solver;
        private readonly IHintProvider _hintProvider;
        private readonly GameEvents _events;

        private DateTime _startTime;
        private List<StepResult> _steps = new List<StepResult>();
        private int _stepIndex = -1;
        private (int row, int col)? _lastModifiedCell = null;

        public GameService(ISudokuGenerator generator, ISudokuSolver solver,
                         IHintProvider hintProvider, GameEvents events)
        {
            _generator = generator;
            _solver = solver;
            _hintProvider = hintProvider;
            _events = events;
            _game = new SudokuGame();
        }
        public void GenerateNewPuzzle(Difficulty difficulty)
        {
            var (puzzle, _) = _generator.GeneratePuzzle(difficulty);
            _game.Reset(puzzle);
            _steps.Clear();
            _stepIndex = -1;
            _startTime = DateTime.Now;
            RaiseStateChanged();
            _events.PublishMessage($"New {difficulty} puzzle generated. Good luck!");
        }
        public GameState GetCurrentState()
        {
            return new GameState
            {
                Grid = _game.Grid,
                OriginalGrid = _game.OriginalGrid,
                Conflicts = GetConflictingCells(),
                ElapsedTime = DateTime.Now - _startTime,
                IsComplete = _game.IsComplete(),
                LastModifiedCell = _lastModifiedCell
            };
        }
        public MoveResult MakeMove(int row, int col, int number)
        {
            if (_game.OriginalGrid[row, col] != 0)
                return new MoveResult { IsValid = false, Message = "Cannot modify original cell" };

            int previousValue = _game.Grid[row, col];
            _game.MakeMove(row, col, number);
            _lastModifiedCell = (row, col);

            var conflicts = GetConflictingCells();
            bool hasConflict = conflicts.Contains((row, col));

            RaiseStateChanged();

            if (hasConflict)
            {
                _events.PublishMessage($"Conflict: {number} at ({row + 1}, {col + 1}) violates Sudoku rules.");
            }
            else if (previousValue != number)
            {
                _events.PublishMessage($"Placed {number} at ({row + 1}, {col + 1})");
            }

            return new MoveResult
            {
                IsValid = true,
                HasConflict = hasConflict
            };
        }
        public void UndoLastMove()
        {
            if (_game.MoveHistory.Count > 0)
            {
                _game.UndoLastMove();
                _lastModifiedCell = null;
                RaiseStateChanged();
                _events.PublishMessage("Undid last move");
            }
            else
            {
                _events.PublishMessage("Nothing to undo");
            }
        }
        public HintResult GetHint()
        {
            var hint = _hintProvider.GetHint(_game.Grid, _game.OriginalGrid);
            if (hint != null)
            {
                _events.PublishMessage($"Hint: Place {hint.Number} at ({hint.Row + 1}, {hint.Col + 1})");
                return hint;
            }

            _events.PublishMessage("No hints available. The puzzle might be complete or unsolvable.");
            return null;
        }
        public bool SolvePuzzle()
        {
            _steps.Clear();
            int[,] gridToSolve = (int[,])_game.OriginalGrid.Clone();

            if (_solver.Solve(gridToSolve, _steps))
            {
                _game.Reset(gridToSolve);
                _lastModifiedCell = null;
                RaiseStateChanged();
                return true;
            }

            _events.PublishMessage("Failed to solve the puzzle. It may be invalid.");
            return false;
        }
        public StepResult GetNextStep()
        {
            if (_steps.Count == 0 || _stepIndex == _steps.Count - 1)
            {
                _steps.Clear();
                _stepIndex = -1;
                int[,] solveGrid = (int[,])_game.OriginalGrid.Clone();

                if (!_solver.Solve(solveGrid, _steps) || _steps.Count == 0)
                {
                    _events.PublishMessage("Could not generate solving steps for this puzzle.");
                    return null;
                }
            }
            if (_stepIndex < _steps.Count - 1)
            {
                _stepIndex++;
                var step = _steps[_stepIndex];
                if (step.Number != 0)
                {
                    _game.MakeMove(step.Row, step.Column, step.Number);
                    _lastModifiedCell = (step.Row, step.Column);
                }
                else
                {
                    _game.MakeMove(step.Row, step.Column, 0);
                    _lastModifiedCell = null;
                }
                RaiseStateChanged();
                return step;
            }
            return null;
        }
        public void ClearUserInput()
        {
            bool changesMade = false;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (_game.OriginalGrid[row, col] == 0 && _game.Grid[row, col] != 0)
                    {
                        _game.MakeMove(row, col, 0);
                        changesMade = true;
                    }
                }
            }
            _steps.Clear();
            _stepIndex = -1;
            _game.MoveHistory.Clear();
            if (changesMade)
            {
                _lastModifiedCell = null;
                RaiseStateChanged();
                _events.PublishMessage("Cleared all user input");
            }
            else
            {
                _events.PublishMessage("No user input to clear");
            }
        }
        private HashSet<(int, int)> GetConflictingCells()
        {
            var conflicts = new HashSet<(int, int)>();
            var grid = _game.Grid;
            var originalGrid = _game.OriginalGrid;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int value = grid[row, col];
                    if (value == 0 || originalGrid[row, col] != 0) continue;
                    grid[row, col] = 0;
                    bool isValid = _game.IsValid(row, col, value);
                    grid[row, col] = value;
                    if (!isValid) conflicts.Add((row, col));
                }
            }
            return conflicts;
        }
        private void RaiseStateChanged()
        {
            _events.RaiseStateChanged(GetCurrentState());
        }
    }
    internal class SudokuGame
    {
        public int[,] Grid { get; private set; } = new int[9, 9];
        public int[,] OriginalGrid { get; private set; } = new int[9, 9];
        public Stack<(int row, int col, int previousValue)> MoveHistory { get; } = new Stack<(int, int, int)>();
        public void Reset(int[,] puzzle)
        {
            Grid = (int[,])puzzle.Clone();
            OriginalGrid = (int[,])puzzle.Clone();
            MoveHistory.Clear();
        }
        public bool IsValid(int row, int col, int num)
        {
            for (int c = 0; c < 9; c++)
                if (c != col && Grid[row, c] == num) return false;
            for (int r = 0; r < 9; r++)
                if (r != row && Grid[r, col] == num) return false;
            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (r != row && c != col && Grid[r, c] == num)
                        return false;
            return true;
        }
        public bool IsComplete()
        {
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                    if (Grid[row, col] == 0 || !IsValid(row, col, Grid[row, col]))
                        return false;
            return true;
        }
        public void MakeMove(int row, int col, int num)
        {
            if (Grid[row, col] != num)
            {
                MoveHistory.Push((row, col, Grid[row, col]));
                Grid[row, col] = num;
            }
        }
        public void UndoLastMove()
        {
            if (MoveHistory.Count > 0)
            {
                var (row, col, value) = MoveHistory.Pop();
                Grid[row, col] = value;
            }
        }
    }
}
