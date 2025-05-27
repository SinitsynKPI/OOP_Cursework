using System.Drawing;
using System.Windows.Forms;

namespace OOP_Cursework.SudokuApp.UI.Controls
{
    public class NumbersPanel : Panel
    {
        private Label[] _numberLabels = new Label[9];
        private Label[] _countLabels = new Label[9];
        private Label _headerLabel;

        public NumbersPanel()
        {
            SetupPanel();
        }

        private void SetupPanel()
        {
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(160, 370);
            this.Padding = new Padding(12, 10, 12, 10);
            _headerLabel = new Label
            {
                Text = "NUMBERS REMAINING",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.DarkSlateGray,
                AutoSize = true,
                Location = new Point(10, 12)
            };
            this.Controls.Add(_headerLabel);
            var numberHeader = new Label
            {
                Text = "NUMBER",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(70, 26),
                Location = new Point(12, 42)
            };
            this.Controls.Add(numberHeader);
            var countHeader = new Label
            {
                Text = "LEFT",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(60, 26),
                Location = new Point(85, 42)
            };
            this.Controls.Add(countHeader);
            var separator = new Panel
            {
                BackColor = Color.FromArgb(220, 220, 220),
                Size = new Size(150, 1),
                Location = new Point(12, 70)
            };
            this.Controls.Add(separator);
            var verticalSeparator = new Panel
            {
                BackColor = Color.FromArgb(220, 220, 220),
                Size = new Size(1, 270),
                Location = new Point(82, 70)
            };
            this.Controls.Add(verticalSeparator);
            for (int num = 1; num <= 9; num++)
            {
                int yPos = 75 + (num - 1) * 30;
                _numberLabels[num - 1] = new Label
                {
                    Text = num.ToString(),
                    Font = new Font("Segoe UI Semibold", 10.5f),
                    ForeColor = Color.DarkSlateBlue,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(70, 26),
                    Location = new Point(12, yPos)
                };
                this.Controls.Add(_numberLabels[num - 1]);
                _countLabels[num - 1] = new Label
                {
                    Text = "9",
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(60, 26),
                    Location = new Point(85, yPos)
                };
                this.Controls.Add(_countLabels[num - 1]);
                if (num < 9)
                {
                    var rowSeparator = new Panel
                    {
                        BackColor = Color.FromArgb(230, 230, 230),
                        Size = new Size(150, 1),
                        Location = new Point(12, yPos + 26)
                    };
                    this.Controls.Add(rowSeparator);
                }
            }
        }
        public void UpdateCounts(int[] counts)
        {
            for (int num = 1; num <= 9; num++)
            {
                int remaining = 9 - counts[num - 1];
                _countLabels[num - 1].Text = remaining.ToString();

                if (remaining == 0)
                {
                    _countLabels[num - 1].ForeColor = Color.ForestGreen;
                    _numberLabels[num - 1].ForeColor = Color.Gray;
                }
                else if (remaining <= 2)
                {
                    _countLabels[num - 1].ForeColor = Color.Orange;
                    _numberLabels[num - 1].ForeColor = Color.DarkSlateBlue;
                }
                else
                {
                    _countLabels[num - 1].ForeColor = Color.DimGray;
                    _numberLabels[num - 1].ForeColor = Color.DarkSlateBlue;
                }
            }
        }
    }
}
