namespace Sudoku_solver
{
    public partial class Form : System.Windows.Forms.Form
    {
        private const int ButtonSize = 90;
        private const int GridSize = 9;
        private readonly Cell[,] _grid = new Cell[GridSize, GridSize];
        private readonly IntPtr _baseAddress;
        private readonly MyMemory _memory;
        private int _empty;

        public Form()
        {
            InitializeComponent();
            InitBoard();
            _memory = new MyMemory("SudokuFree10");
            if (!_memory.IsOk())
            {
                MessageBox.Show("Mở game lên trước");
                Close();
            }

            _baseAddress = _memory.GetBaseAddress();
        }

        private void InitBoard()
        {
            Button oldButton = new() { Location = new Point { X = 0, Y = 10 }, Width = 0, Height = 0 };
            // pnButton.Controls.Add(oldButton);
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Button button = new()
                    {
                        Location = oldButton.Location with
                        {
                            X = oldButton.Location.X + oldButton.Width + (j % 3 == 0 ? 10 : 0)
                        },
                        Width = ButtonSize,
                        Height = ButtonSize,
                        // Enabled = false,
                        Font = new Font(Font.FontFamily, 15),
                        BackColor = Color.White
                    };
                    _grid[i, j] = new Cell(button);
                    pnButton.Controls.Add(_grid[i, j].Button);
                    oldButton = button;
                }

                oldButton = new Button()
                {
                    Location = new Point
                        { X = 0, Y = oldButton.Location.Y + oldButton.Height + ((i + 1) % 3 == 0 ? 10 : 0) },
                    Width = 0,
                    Height = 0
                };
            }
        }

        private void ImportBoard()
        {
            _empty = 0;
            long[] pointer = { (0x011B7038) + _baseAddress.ToInt64(), 0xE8, 0x58, 0x8, 0x10, 0x1c };
            long firstAddress = _memory.GetAddressFromPointer(pointer);
            long current = firstAddress;
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    int value = _memory.ReadInt(current);
                    _grid[i, j].Value = value;
                    _grid[i, j].CanEdit = value == 0;
                    
                    if (value == 0)
                    {
                        _empty++;
                    }

                    current += 0x118;
                }

                current = firstAddress + (i + 1) * 0x9d8;
            }
        }


        private bool IsValid(int row, int col, int value)
        {
            // Kiểm tra trong cùng dòng và cùng cột
            for (int i = 0; i < GridSize; i++)
            {
                if (_grid[row, i].Value == value && i != col)
                {
                    return false;
                }

                if (_grid[i, col].Value == value && i != row)
                {
                    return false;
                }
            }

            // Kiểm tra trong ô 3x3
            int startRow = row / 3 * 3;
            int startCol = col / 3 * 3;
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (_grid[i, j].Value == value && (i != row || j != col))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int[]? FindEmptySpot()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (_grid[i, j].Value == 0) return new[] { i, j };
                }
            }

            return null;
        }

        private bool Solve()
        {
            int[]? spot = FindEmptySpot();
            if (spot == null) return true;
            int x = spot[0];
            int y = spot[1];
            for (int value = 1; value <= GridSize; value++)
            {
                if (!IsValid(x, y, value)) continue;
                _grid[x, y].Value = value;
                if (Solve())
                {
                    return true;
                }

                _grid[x, y].Value = 0;
            }

            return false;
        }

        private void UpdateDisplay()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    int value = _grid[i, j].Value;
                    bool canEdit = _grid[i, j].CanEdit;
                    _grid[i, j].Button.Text = value.ToString();
                    _grid[i, j].Button.ForeColor = canEdit? Color.Red: Color.Black;
                    _grid[i, j].Button.Font = new Font(_grid[i, j].Button.Font.FontFamily, 15,
                        canEdit == false ? FontStyle.Bold : FontStyle.Regular);
                    _grid[i, j].Button.Enabled = canEdit;
                }
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            ImportBoard();
            Solve();
            UpdateDisplay();
        }

        private void ClearBoard()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    _grid[i, j].Value = 0;
                    _grid[i, j].Button.Text = "";
                    _grid[i, j].Button.Enabled = true;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearBoard();
        }

        private void FillBoard()
        {
            if (FindEmptySpot() != null)
            {
                MessageBox.Show("Solve it first");
                return;
            }

            long[] pointer = { (0x011B7038) + _baseAddress.ToInt64(), 0xE8, 0x58, 0x8, 0x10, 0x1c };
            long firstAddress = _memory.GetAddressFromPointer(pointer);
            long current = firstAddress;
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (_empty <= 1)
                    {
                        MessageBox.Show("Ok");
                        return;
                    }

                    //int value = _memory.ReadInt(current);
                    if (_grid[i, j].CanEdit) // can edit mean the original value is 0
                    {
                        _memory.WriteNumber(current, _grid[i, j].Value, 1);
                        _empty--;
                    }

                    current += 0x118;
                }

                current = firstAddress + (i + 1) * 0x9d8;
            }
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            btnSolve_Click(sender, e);
            FillBoard();
        }
    }
}