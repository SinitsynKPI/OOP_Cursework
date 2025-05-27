using OOP_Cursework.SudokuApp.Core.Events;
using OOP_Cursework.SudokuApp.Core.Services;
using OOP_Cursework.SudokuApp.UI.Forms;
using System;
using System.Windows.Forms;

namespace SudokuApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var events = new GameEvents();
            var generator = new SudokuGenerator();
            var solver = new SudokuSolver();
            var hintProvider = new HintProvider(solver);
            var gameService = new GameService(generator, solver, hintProvider, events);

            Application.Run(new SudokuForm(gameService, events));
        }
    }
}