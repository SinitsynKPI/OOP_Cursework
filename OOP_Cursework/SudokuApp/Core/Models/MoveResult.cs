namespace OOP_Cursework.SudokuApp.Core.Models
{
    public class MoveResult
    {
        public bool IsValid { get; set; }
        public bool HasConflict { get; set; }
        public string Message { get; set; }
    }
}
