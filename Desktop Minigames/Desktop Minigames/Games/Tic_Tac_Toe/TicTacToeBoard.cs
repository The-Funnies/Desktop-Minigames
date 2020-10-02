using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    class Board : System.Windows.Forms.TableLayoutPanel
    {
        private int width = 500;
        private int height = 500;

        public Board(SingleplayerGame form)
        {
            this.ColumnCount = 3;
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));

            this.RowCount = 3;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));

            this.Location = new System.Drawing.Point(0, 45);
            this.Name = "Board";
            this.Size = new System.Drawing.Size(this.width, this.height);

            for (int i = 0; i < 9; i++)
            {
                Cell tmpCell = new Cell(i);
                tmpCell.TabStop = false;
                tmpCell.Font = new Font(tmpCell.Font.FontFamily, 70);
                tmpCell.FlatStyle = FlatStyle.System;
                tmpCell.Click += form.Button_Clicked;
                tmpCell.AutoSize = false;
                tmpCell.Size = new System.Drawing.Size(this.width / 3, this.height / 3);
                this.Controls.Add(tmpCell);
            }

        }

        public Board(MultiplayerGame form)
        {
            this.ColumnCount = 3;
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));

            this.RowCount = 3;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));

            this.Location = new System.Drawing.Point(0, 45);
            this.Name = "Board";
            this.Size = new System.Drawing.Size(this.width, this.height);

            for (int i = 0; i < 9; i++)
            {
                Cell tmpCell = new Cell(i);
                tmpCell.TabStop = false;
                tmpCell.Font = new Font(tmpCell.Font.FontFamily, 70);
                tmpCell.FlatStyle = FlatStyle.System;
                tmpCell.Click += form.Button_Clicked;
                tmpCell.AutoSize = false;
                tmpCell.Size = new System.Drawing.Size(this.width / 3, this.height / 3);
                this.Controls.Add(tmpCell);
            }

        }

        public Cell GetCellByIndex(int index)
        {
            return (Cell)this.Controls[index];
        }

        public void SetCellText(int index, int value)
        {
            this.GetCellByIndex(index).SetTextByValue(value);
        }

        public Cell[] GetAllCells()
        {
            Cell[] arr = new Cell[9];
            for (int i = 0; i < 9; i++)
            {
                arr[i] = (Cell)this.Controls[i];
            }
            return arr;
        }

        public void DisableButtonsWithValue()
        {
            foreach (Cell cell in this.Controls)
            {
                if (cell.GetValue() != 0)
                {
                    cell.Enabled = false;
                }
            }
        }

        public Cell[] GetAllAvailableCells()
        {
            List<Cell> list = new List<Cell>();
            foreach (Cell cell in this.Controls)
            {
                if (cell.GetValue() == 0)
                {
                    list.Add(cell);
                }
            }
            return list.ToArray();
        }

        public void SetValuesByArray(int[] values)
        {
            int index = 0;
            foreach (Cell cell in this.Controls)
            {
                cell.SetTextByValue(values[index]);
                index++;
            }
        }

        public int[] GetValues()
        {
            int[] arr = new int[9];
            int index = 0;
            foreach (Cell cell in this.Controls)
            {
                arr[index] = cell.GetValue();
                index++;
            }
            return arr;
        }

        public void Reset()
        {
            foreach (Cell cell in this.Controls)
            {
                cell.Text = "";
                cell.SetValue(0);
                cell.Enabled = true;
            }
        }

        public void EnableAll()
        {
            foreach (Cell cell in this.Controls)
            {
                if (cell.InvokeRequired)
                {
                    cell.Invoke((MethodInvoker)delegate ()
                   {
                       cell.Enabled = true;
                   });
                }
                else
                {
                    cell.Enabled = true;
                }
            }
        }

        public void DisableAll()
        {
            try
            {
                foreach (Cell cell in this.Controls)
                {
                    if (cell.InvokeRequired)
                    {
                        cell.Invoke((MethodInvoker)delegate ()
                        {
                            cell.Enabled = false;
                        });
                    }
                    else
                    {
                        cell.Enabled = false;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public static int IsThrereAWinner(int[] board)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i] == board[i + 3] && board[i] == board[i + 6] && board[i] != 0)
                {
                    return 2;
                }
                if (board[i * 3] == board[i * 3 + 1] && board[i * 3] == board[i * 3 + 2] && board[i * 3] != 0)
                {
                    return 2;
                }
            }
            if ((board[0] == board[4] && board[0] == board[8] && board[4] != 0) || (board[2] == board[4] && board[6] == board[2] && board[4] != 0))
            {
                return 2;
            }


            bool isTie = true;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    isTie = false;
                }
            }
            if (isTie) return 1;

            return 0;
        }
    }

    class Cell : System.Windows.Forms.Button
    {
        private int index;
        private int value;

        public Cell(int index)
        {
            this.index = index;
            this.value = 0;
        }

        public int GetIndex()
        {
            return this.index;
        }

        public int GetValue()
        {
            return this.value;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }

        public void SetTextByValue(int value)
        {
            this.value = value;
            if (value == 0)
            {
                this.Text = "";
            }
            else if (value == 1)
            {
                this.Text = "X";
            }
            else if (value == 2)
            {
                this.Text = "O";
            }
        }
    }
}
