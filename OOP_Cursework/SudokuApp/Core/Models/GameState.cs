using System;
using System.Collections.Generic;

namespace OOP_Cursework.SudokuApp.Core.Models
{
    public class GameState
    {
        public int[,] Grid { get; set; }
        public int[,] OriginalGrid { get; set; }
        public HashSet<(int, int)> Conflicts { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public bool IsComplete { get; set; }
        public (int row, int col)? LastModifiedCell { get; set; }
    }
}
