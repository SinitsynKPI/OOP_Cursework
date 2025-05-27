using OOP_Cursework.SudokuApp.Core.Enums;
using OOP_Cursework.SudokuApp.Core.Events;
using OOP_Cursework.SudokuApp.Core.Models;
using OOP_Cursework.SudokuApp.Core.Services.Contracts;
using OOP_Cursework.SudokuApp.UI.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OOP_Cursework.SudokuApp.UI.Forms
{
    public class SudokuForm : Form
    {
        private readonly IGameService _gameService;
        private readonly GameEvents _events;

        private SudokuGrid _grid;
        private NumbersPanel _numbersPanel;
        private Button _generateButton;
        private Button _solveButton;
        private Button _stepButton;
        private Button _hintButton;
        private Button _clearButton;
        private Button _undoButton;
        private ComboBox _difficultyCombo;
        private TextBox _explanationTextBox;
        private System.ComponentModel.IContainer components;
        private StatusStrip _statusStrip;
        private Label _timerLabel;
        private Timer _timer;
        private ToolStripStatusLabel _statusLabel;

        public SudokuForm(IGameService gameService, GameEvents events)
        {
            InitializeComponent();
            _gameService = gameService;
            _events = events;
            SubscribeToEvents();
            _gameService.GenerateNewPuzzle(Difficulty.Easy);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._generateButton = new System.Windows.Forms.Button();
            this._solveButton = new System.Windows.Forms.Button();
            this._stepButton = new System.Windows.Forms.Button();
            this._hintButton = new System.Windows.Forms.Button();
            this._clearButton = new System.Windows.Forms.Button();
            this._undoButton = new System.Windows.Forms.Button();
            this._difficultyCombo = new System.Windows.Forms.ComboBox();
            this._explanationTextBox = new System.Windows.Forms.TextBox();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._timerLabel = new System.Windows.Forms.Label();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._numbersPanel = new OOP_Cursework.SudokuApp.UI.Controls.NumbersPanel();
            this._grid = new OOP_Cursework.SudokuApp.UI.Controls.SudokuGrid();
            this._statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _generateButton
            // 
            this._generateButton.BackColor = System.Drawing.Color.DodgerBlue;
            this._generateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this._generateButton.FlatAppearance.BorderSize = 0;
            this._generateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._generateButton.ForeColor = System.Drawing.Color.White;
            this._generateButton.Location = new System.Drawing.Point(289, 574);
            this._generateButton.Name = "_generateButton";
            this._generateButton.Size = new System.Drawing.Size(100, 35);
            this._generateButton.TabIndex = 2;
            this._generateButton.Text = "Generate";
            this._generateButton.UseVisualStyleBackColor = false;
            this._generateButton.Click += new System.EventHandler(this._generateButton_Click);
            // 
            // _solveButton
            // 
            this._solveButton.BackColor = System.Drawing.Color.SeaGreen;
            this._solveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this._solveButton.FlatAppearance.BorderSize = 0;
            this._solveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._solveButton.ForeColor = System.Drawing.Color.White;
            this._solveButton.Location = new System.Drawing.Point(395, 574);
            this._solveButton.Name = "_solveButton";
            this._solveButton.Size = new System.Drawing.Size(100, 35);
            this._solveButton.TabIndex = 3;
            this._solveButton.Text = "Solve";
            this._solveButton.UseVisualStyleBackColor = false;
            this._solveButton.Click += new System.EventHandler(this._solveButton_Click);
            // 
            // _stepButton
            // 
            this._stepButton.BackColor = System.Drawing.Color.Teal;
            this._stepButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this._stepButton.FlatAppearance.BorderSize = 0;
            this._stepButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._stepButton.ForeColor = System.Drawing.Color.White;
            this._stepButton.Location = new System.Drawing.Point(501, 574);
            this._stepButton.Name = "_stepButton";
            this._stepButton.Size = new System.Drawing.Size(100, 35);
            this._stepButton.TabIndex = 4;
            this._stepButton.Text = "Step";
            this._stepButton.UseVisualStyleBackColor = false;
            this._stepButton.Click += new System.EventHandler(this._stepButton_Click);
            // 
            // _hintButton
            // 
            this._hintButton.BackColor = System.Drawing.Color.SteelBlue;
            this._hintButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this._hintButton.FlatAppearance.BorderSize = 0;
            this._hintButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._hintButton.ForeColor = System.Drawing.Color.White;
            this._hintButton.Location = new System.Drawing.Point(183, 615);
            this._hintButton.Name = "_hintButton";
            this._hintButton.Size = new System.Drawing.Size(100, 35);
            this._hintButton.TabIndex = 6;
            this._hintButton.Text = "Hint";
            this._hintButton.UseVisualStyleBackColor = false;
            this._hintButton.Click += new System.EventHandler(this._hintButton_Click);
            // 
            // _clearButton
            // 
            this._clearButton.BackColor = System.Drawing.Color.DarkOrange;
            this._clearButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this._clearButton.FlatAppearance.BorderSize = 0;
            this._clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._clearButton.ForeColor = System.Drawing.Color.White;
            this._clearButton.Location = new System.Drawing.Point(289, 616);
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(100, 35);
            this._clearButton.TabIndex = 7;
            this._clearButton.Text = "Clear";
            this._clearButton.UseVisualStyleBackColor = false;
            this._clearButton.Click += new System.EventHandler(this._clearButton_Click);
            // 
            // _undoButton
            // 
            this._undoButton.BackColor = System.Drawing.Color.IndianRed;
            this._undoButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this._undoButton.FlatAppearance.BorderSize = 0;
            this._undoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._undoButton.ForeColor = System.Drawing.Color.White;
            this._undoButton.Location = new System.Drawing.Point(395, 616);
            this._undoButton.Name = "_undoButton";
            this._undoButton.Size = new System.Drawing.Size(100, 35);
            this._undoButton.TabIndex = 8;
            this._undoButton.Text = "Undo";
            this._undoButton.UseVisualStyleBackColor = false;
            this._undoButton.Click += new System.EventHandler(this._undoButton_Click);
            // 
            // _difficultyCombo
            // 
            this._difficultyCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._difficultyCombo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._difficultyCombo.FormattingEnabled = true;
            this._difficultyCombo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._difficultyCombo.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard"});
            _difficultyCombo.SelectedIndex = 0;
            this._difficultyCombo.Location = new System.Drawing.Point(162, 575);
            this._difficultyCombo.Name = "_difficultyCombo";
            this._difficultyCombo.Size = new System.Drawing.Size(120, 24);
            this._difficultyCombo.TabIndex = 9;
            // 
            // _explanationTextBox
            // 
            this._explanationTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this._explanationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._explanationTextBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._explanationTextBox.Location = new System.Drawing.Point(183, 681);
            this._explanationTextBox.Multiline = true;
            this._explanationTextBox.Name = "_explanationTextBox";
            this._explanationTextBox.ReadOnly = true;
            this._explanationTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._explanationTextBox.Size = new System.Drawing.Size(100, 23); 
            this._explanationTextBox.TabIndex = 10;
            // 
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel});
            this._statusStrip.Location = new System.Drawing.Point(0, 739);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(904, 22);
            this._statusStrip.TabIndex = 11;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _statusLabel
            // 
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(118, 17);
            this._statusLabel.Text = "toolStripStatusLabel1";
            // 
            // _timerLabel
            // 
            this._timerLabel.AutoSize = true;
            this._timerLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._timerLabel.ForeColor = System.Drawing.Color.DarkSlateGray;
            this._timerLabel.Location = new System.Drawing.Point(20, 20);
            this._timerLabel.Name = "_timerLabel";
            this._timerLabel.Size = new System.Drawing.Size(79, 17);
            this._timerLabel.TabIndex = 12;
            this._timerLabel.Text = "Time: 00:00";
            // 
            // _timer
            // 
            this._timer.Interval = 1000;
            this._timer.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // _numbersPanel
            // 
            this._numbersPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._numbersPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._numbersPanel.Location = new System.Drawing.Point(656, 96);
            this._numbersPanel.Name = "_numbersPanel";
            this._numbersPanel.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this._numbersPanel.Size = new System.Drawing.Size(160, 370);
            this._numbersPanel.TabIndex = 1;
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.White;
            this._grid.Location = new System.Drawing.Point(162, 96);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(450, 450);
            this._grid.TabIndex = 0;
            this._grid.CellValueChanged += new System.EventHandler<OOP_Cursework.SudokuApp.UI.Controls.CellValueChangedEventArgs>(this.OnGridCellValueChanged);
            this._grid.CellSelected += new System.EventHandler<OOP_Cursework.SudokuApp.UI.Controls.CellSelectedEventArgs>(this.OnGridCellSelected);
            // 
            // SudokuForm
            // 
            this.ClientSize = new System.Drawing.Size(904, 761);
            this.Controls.Add(this._timerLabel);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._explanationTextBox);
            this.Controls.Add(this._difficultyCombo);
            this.Controls.Add(this._undoButton);
            this.Controls.Add(this._clearButton);
            this.Controls.Add(this._hintButton);
            this.Controls.Add(this._stepButton);
            this.Controls.Add(this._solveButton);
            this.Controls.Add(this._generateButton);
            this.Controls.Add(this._numbersPanel);
            this.Controls.Add(this._grid);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.MinimumSize = new System.Drawing.Size(900, 760);
            this.Name = "SudokuForm";
            this.Text = "Classic Sudoku";
            this.Load += new System.EventHandler(this.SudokuForm_Load);
            this.Resize += new System.EventHandler(this.UpdateLayout);
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void UpdateLayout(object sender, EventArgs e)
        {
            int margin = 20;
            int controlSpacing = 10;
            int gridTop = _timerLabel.Bottom + margin;
            int availableHeight = this.ClientSize.Height - gridTop - margin - 120;
            int gridSize = Math.Min(9 * 50, availableHeight);
            _grid.Size = new Size(gridSize, gridSize);
            _grid.Location = new Point(
                (this.ClientSize.Width - _grid.Width - _numbersPanel.Width - margin) / 2,
                gridTop);
            _numbersPanel.Location = new Point(
                _grid.Right + margin,
                _grid.Top + (_grid.Height - _numbersPanel.Height) / 2);
            _timerLabel.Location = new Point(margin, margin);
            int controlsTop = _grid.Bottom + margin;
            int controlsLeft = _grid.Left;
            _difficultyCombo.Location = new Point(controlsLeft, controlsTop);   
            controlsLeft += _difficultyCombo.Width + controlSpacing;
            _generateButton.Location = new Point(controlsLeft, controlsTop);
            controlsLeft += _generateButton.Width + controlSpacing;
            _solveButton.Location = new Point(controlsLeft, controlsTop);
            controlsLeft += _solveButton.Width + controlSpacing;
            _stepButton.Location = new Point(controlsLeft, controlsTop);
            controlsTop += _generateButton.Height + controlSpacing;
            controlsLeft = _grid.Left;
            _hintButton.Location = new Point(controlsLeft, controlsTop);
            controlsLeft += _hintButton.Width + controlSpacing;
            _clearButton.Location = new Point(controlsLeft, controlsTop);
            controlsLeft += _clearButton.Width + controlSpacing;
            _undoButton.Location = new Point(controlsLeft, controlsTop);
            _explanationTextBox.Location = new Point(
                _grid.Left,
                controlsTop + _hintButton.Height + controlSpacing);
            _explanationTextBox.Size = new Size(
                _grid.Width + _numbersPanel.Width + margin,
                this.ClientSize.Height - _explanationTextBox.Top - _statusStrip.Height - margin);
            _statusStrip.Location = new Point(0, this.ClientSize.Height - _statusStrip.Height);
            _statusStrip.Size = new Size(this.ClientSize.Width, _statusStrip.Height);
        }
        private void SubscribeToEvents()
        {
            _events.StateChanged += OnGameStateChanged;
            _events.MessagePublished += OnMessagePublished;
        }
        private void OnGameStateChanged(GameState state)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<GameState>(OnGameStateChanged), state);
                return;
            }
            _grid.SuppressFocusHighlighting(true);
            _grid.SetConflicts(state.Conflicts);
            bool[,] fixedCells = new bool[9, 9];
            for (int r = 0; r < 9; r++)
                for (int c = 0; c < 9; c++)
                    fixedCells[r, c] = state.OriginalGrid[r, c] != 0;

            _grid.SetFixedCells(fixedCells);
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    string value = state.Grid[row, col] == 0 ? "" : state.Grid[row, col].ToString();
                    bool isFixed = state.OriginalGrid[row, col] != 0;
                    bool isConflict = state.Conflicts.Contains((row, col));

                    _grid.UpdateCell(row, col, value, isFixed, isConflict);
                }
            }
            _grid.SuppressFocusHighlighting(false);
            int[] counts = new int[9];
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                    if (state.Grid[row, col] > 0)
                        counts[state.Grid[row, col] - 1]++;
            _numbersPanel.UpdateCounts(counts);
            _timerLabel.Text = $"Time: {state.ElapsedTime:mm\\:ss}";
            if (state.IsComplete)
            {
                _timer.Stop();
                _explanationTextBox.Text = "Congratulations! Puzzle solved!\r\n";
            }
            else if (!_timer.Enabled)
            {
                _timer.Start();
            }
            _solveButton.Enabled = !state.IsComplete;
            _stepButton.Enabled = !state.IsComplete;
            _hintButton.Enabled = !state.IsComplete;
            _undoButton.Enabled = !state.IsComplete;
            _clearButton.Enabled = !state.IsComplete;
        }
        private void OnMessagePublished(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(OnMessagePublished), message);
                return;
            }
            if (message.IndexOf("error", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("failed", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.IndexOf("could not", StringComparison.OrdinalIgnoreCase) >= 0||
                message.IndexOf("clear", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                _statusLabel.Text = message; 
            }
            else
            {
                _statusLabel.Text = string.Empty;
            }

            _explanationTextBox.AppendText(message + "\r\n");
        }
        private void OnGridCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (int.TryParse(e.Value, out int num))
            {
                var state = _gameService.GetCurrentState();
                if (state.OriginalGrid[e.Row, e.Column] == 0 && state.Grid[e.Row, e.Column] != num)
                {
                    _gameService.MakeMove(e.Row, e.Column, num);
                }
            }
            else
            {
                _gameService.MakeMove(e.Row, e.Column, 0);
            }
        }
        private void OnGridCellSelected(object sender, CellSelectedEventArgs e)
        {
            var state = _gameService.GetCurrentState();
            int value = state.Grid[e.Row, e.Column];
            string cellInfo = value > 0
                ? $"Selected Cell: Row {e.Row + 1}, Col {e.Column + 1} = {value}"
                : $"Selected Cell: Row {e.Row + 1}, Col {e.Column + 1} (empty)";

            _statusLabel.Text = cellInfo;
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            var state = _gameService.GetCurrentState();
            _timerLabel.Text = $"Time: {state.ElapsedTime:mm\\:ss}";
        }
        private void SudokuForm_Load(object sender, EventArgs e)
        {
            UpdateLayout(sender, e);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                ClearInitialFocus();
            }));
        }
        private void ClearInitialFocus()
        {
            var dummy = new Label
            {
                TabStop = true,
                Size = new Size(0, 0),
                Location = new Point(-100, -100)
            };
            this.Controls.Add(dummy);
            dummy.Focus();
            this.ActiveControl = dummy;
            this.Controls.Remove(dummy);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _events.StateChanged -= OnGameStateChanged;
                _events.MessagePublished -= OnMessagePublished;
                _timer?.Dispose();
            }
            base.Dispose(disposing);
        }
        private void _generateButton_Click(object sender, EventArgs e)
        {
            var difficulty = (Difficulty)_difficultyCombo.SelectedIndex;
            _explanationTextBox.Clear();
            _gameService.GenerateNewPuzzle(difficulty);
        }
        private void _solveButton_Click(object sender, EventArgs e)
        {
            _gameService.SolvePuzzle();
        }
        private void _stepButton_Click(object sender, EventArgs e)
        {
            var step = _gameService.GetNextStep();
            if (step != null)
            {
                _explanationTextBox.AppendText($"{step.Explanation}\r\n");
                _explanationTextBox.ScrollToCaret();
            }
        }
        private void _hintButton_Click(object sender, EventArgs e)
        {
            var hint = _gameService.GetHint();
            if (hint != null)
            {
                _grid.HighlightCell(hint.Row, hint.Col, Color.LightBlue);
                _explanationTextBox.ScrollToCaret();
            }
        }
        private void _clearButton_Click(object sender, EventArgs e)
        {
            _gameService.ClearUserInput();
            _explanationTextBox.Clear();
        }
        private void _undoButton_Click(object sender, EventArgs e)
        {
            _gameService.UndoLastMove();
        }
    }
}