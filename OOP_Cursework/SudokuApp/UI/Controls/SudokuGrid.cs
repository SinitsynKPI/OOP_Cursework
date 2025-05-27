using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OOP_Cursework.SudokuApp.UI.Controls
{
    public class SudokuGrid : Panel
    {
        bool[,] _isFixed = new bool[9, 9];
        private bool _suppressFocus = false;

        private const int CellSize = 50;
        private TextBox[,] _cells = new TextBox[9, 9];
        private (int row, int col)? _selectedCell = null;
        private HashSet<(int row, int col)> _conflictCells = new();

        public event EventHandler<CellValueChangedEventArgs> CellValueChanged;
        public event EventHandler<CellSelectedEventArgs> CellSelected;

        public SudokuGrid()
        {
            InitializeGrid();
            this.DoubleBuffered = true;
        }

        private void InitializeGrid()
        {
            this.BackColor = Color.White;
            this.Size = new Size(9 * CellSize, 9 * CellSize);
            this.Paint += DrawGridLines;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    var cell = CreateCell(row, col);
                    _cells[row, col] = cell;
                    this.Controls.Add(cell);
                }
            }
        }

        private TextBox CreateCell(int row, int col)
        {
            var cell = new TextBox
            {
                Size = new Size(CellSize - 2, CellSize - 2),
                Location = new Point(col * CellSize + 1, row * CellSize + 1),
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                MaxLength = 1,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Tag = (row, col),
                TabStop = false
            };

            cell.TextChanged += OnCellTextChanged;
            cell.KeyPress += OnCellKeyPress;
            cell.Enter += OnCellEnter;
            cell.Leave += OnCellLeave;
            cell.Click += OnCellClick;
            cell.KeyDown += OnCellKeyDown;

            return cell;
        }

        private void DrawGridLines(object sender, PaintEventArgs e)
        {
            using var thinPen = new Pen(Color.LightSlateGray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
            using var thickPen = new Pen(Color.Black, 2);
            for (int i = 0; i <= 9; i++)
            {
                int x = i * CellSize;
                Pen pen = (i % 3 == 0) ? thickPen : thinPen;
                e.Graphics.DrawLine(pen, x, 0, x, this.Height);
            }
            for (int i = 0; i <= 9; i++)
            {
                int y = i * CellSize;
                Pen pen = (i % 3 == 0) ? thickPen : thinPen;
                e.Graphics.DrawLine(pen, 0, y, this.Width, y);
            }
        }

        private void OnCellTextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox cell && cell.Tag is ValueTuple<int, int> location)
            {
                int row = location.Item1;
                int col = location.Item2;

                if (_isFixed[row, col]) return;

                CellValueChanged?.Invoke(this, new CellValueChangedEventArgs
                {
                    Row = row,
                    Column = col,
                    Value = cell.Text
                });
            }
        }


        private void OnCellKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is TextBox cell && cell.Tag is ValueTuple<int, int> location)
            {
                int row = location.Item1;
                int col = location.Item2;

                if (_isFixed[row, col])
                {
                    e.Handled = true; 
                    return;
                }

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar) || e.KeyChar < '1' || e.KeyChar > '9'))
                {
                    e.Handled = true;
                }
            }
        }


        private void OnCellEnter(object sender, EventArgs e)
        {
            if (_suppressFocus) return; 

            if (sender is TextBox cell && cell.Tag is ValueTuple<int, int> location)
            {
                _selectedCell = location;
                HighlightRelatedCells(location.Item1, location.Item2);
                CellSelected?.Invoke(this, new CellSelectedEventArgs
                {
                    Row = location.Item1,
                    Column = location.Item2
                });
            }
        }

        public void SuppressFocusHighlighting(bool suppress)
        {
            _suppressFocus = suppress;
        }


        private void OnCellLeave(object sender, EventArgs e)
        {
            ResetCellHighlights();
            _selectedCell = null;
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            if (sender is TextBox cell && cell.Tag is ValueTuple<int, int> location)
            {
                if (!cell.Focused)
                {
                    BeginInvoke((MethodInvoker)(() => cell.Select(0, cell.Text.Length)));
                }
            }
        }

        private void OnCellKeyDown(object sender, KeyEventArgs e)
        {
            if (_selectedCell == null) return;

            var (row, col) = _selectedCell.Value;

            if (_isFixed[row, col])
            {
                e.SuppressKeyPress = true; 
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (row > 0) _cells[row - 1, col].Focus();
                    e.Handled = true;
                    break;
                case Keys.Down:
                    if (row < 8) _cells[row + 1, col].Focus();
                    e.Handled = true;
                    break;
                case Keys.Left:
                    if (col > 0) _cells[row, col - 1].Focus();
                    e.Handled = true;
                    break;
                case Keys.Right:
                    if (col < 8) _cells[row, col + 1].Focus();
                    e.Handled = true;
                    break;
                case Keys.Delete:
                case Keys.Back:
                    _cells[row, col].Text = ""; 
                    e.Handled = true;
                    break;
            }
        }


        public void SetFixedCells(bool[,] fixedCells)
        {
            _isFixed = fixedCells;
        }

        private void HighlightRelatedCells(int row, int col)
        {
            string currentValue = _cells[row, col].Text;
            bool hasValue = !string.IsNullOrEmpty(currentValue);

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    var cell = _cells[r, c];
                    bool isFixed = _isFixed[r, c];
                    bool isConflict = _conflictCells.Contains((r, c));
                    bool isSameCell = (r == row && c == col);
                    bool isSameValue = hasValue && cell.Text == currentValue;
                    bool isRelated = (r == row) || (c == col) ||
                                     (r / 3 == row / 3 && c / 3 == col / 3);
                    if (isConflict)
                        cell.BackColor = Color.LightCoral;
                    else if (isSameCell)
                        cell.BackColor = Color.Blue;
                    else if (isSameValue)
                        cell.BackColor = Color.Yellow;
                    else if (isRelated && hasValue)
                        cell.BackColor = Color.LightBlue;
                    else
                        cell.BackColor = isFixed ? Color.FromArgb(240, 240, 240) : Color.White;
                    cell.ForeColor = isConflict ? Color.Red : Color.Black;
                    cell.ReadOnly = isFixed;
                    cell.Cursor = isFixed ? Cursors.Default : Cursors.IBeam;
                }
            }
        }
        public void SetConflicts(HashSet<(int, int)> conflicts)
        {
            _conflictCells = conflicts;
        }
        private void ResetCellHighlights()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    var cell = _cells[row, col];
                    if (_conflictCells.Contains((row, col)))
                    {
                        cell.BackColor = Color.FromArgb(255, 200, 200);
                        cell.ForeColor = Color.Red;
                    }
                    else
                    {
                        cell.BackColor = Color.White;
                        cell.ForeColor = Color.Black;
                    }
                }
            }
        }

        public void UpdateCell(int row, int col, string value, bool isFixed, bool isConflict)
        {
            var cell = _cells[row, col];
            if (!cell.Focused)
                cell.Text = value;
            cell.BackColor = isFixed ? Color.FromArgb(240, 240, 240) :
                             isConflict ? Color.FromArgb(255, 200, 200) : Color.White;
            cell.ForeColor = isConflict ? Color.Red : Color.Black;
            cell.ReadOnly = isFixed;
            cell.Cursor = isFixed ? Cursors.Default : Cursors.IBeam;
            _isFixed[row, col] = isFixed;
        }


        public void HighlightCell(int row, int col, Color color, int durationMs = 1000)
        {
            var cell = _cells[row, col];
            var originalColor = cell.BackColor;

            cell.BackColor = color;

            if (durationMs > 0)
            {
                var timer = new Timer { Interval = durationMs };
                timer.Tick += (s, e) =>
                {
                    cell.BackColor = originalColor;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
        }
    }
    public class CellValueChangedEventArgs : EventArgs
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Value { get; set; }
    }
    public class CellSelectedEventArgs : EventArgs
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
