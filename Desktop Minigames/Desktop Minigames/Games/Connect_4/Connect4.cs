using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public partial class Connect4 : Form
    {

        Button[][] bboard;
        int[][] board;
        bool turns;
        int winr;
        int winb;
        int plays;
        bool mode2;

        public Connect4()
        {

            //setup start
            mode2 = false;
            winr = 0;
            winb = 0;
            plays = 0;
            turns = true;
            board = new int[8][];
            bboard = InitializeComponent();
            Reset();
            //setup end
            //menu bar
            this.Menu = new MainMenu();
            MenuItem item = new MenuItem("Options");
            this.Menu.MenuItems.Add(item);
            item.MenuItems.Add("Reset game", new EventHandler(Reset));
            // item.MenuItems.Add("Open", new EventHandler(Open_Click));
            item = new MenuItem("Mode");
            this.Menu.MenuItems.Add(item);
            item.MenuItems.Add("2 Players", new EventHandler(Players2));
            item.MenuItems.Add("1 Player", new EventHandler(Player));
            //menu bar end



        }

        private void Player(object sender, EventArgs e)
        {
            if (!(mode2))
            {
                MessageBox.Show("Could not change mode because you are currently in 1 Player mode");
                return;
            }
            mode2 = false;
            Reset();
        }

        private void Players2(object sender, EventArgs e)
        {
            if (mode2)
            {
                MessageBox.Show("Could not change mode because you are currently in 2 Players mode");
                return;
            }
            mode2 = true;
            Reset();
        }

        private void Reset()
        {
            board = new int[8][];
            for (int i = 0; i < 8; i++)
            {
                board[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int j = 0; j < 8; j++)
                {
                    bboard[i][j].BackColor = Color.FromName("Yellow");
                }

            }
            turns = true;
            plays = 0;

        }
        private void Reset(object sender, EventArgs e)
        {
            Reset();
        }
        private bool IsFull(int[] arr)
        {
            for (int i = 0; i < 8; i++)
            {
                if (arr[i] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        private int Check()
        {
            //rows
            for (int i = 0; i < 8; i++)
            {
                for (int j = 7, count1 = 0, count2 = 0; j >= 0; j--)
                {
                    if (board[i][j] == 0)
                    {
                        count1 = 0;
                        count2 = 0;
                    }
                    if (board[i][j] == 1)
                    {
                        count1++;
                        count2 = 0;
                    }
                    if (board[i][j] == 2)
                    {
                        count2++;
                        count1 = 0;
                    }
                    if (count1 == 4)
                    {
                        return 1;
                    }
                    if (count2 == 4)
                    {
                        return 2;
                    }
                }
            }
            //collumns
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (board[i][j] == 1 && board[i + 1][j] == 1 && board[i + 2][j] == 1 && board[i + 3][j] == 1)
                    {
                        return 1;
                    }
                    if (board[i][j] == 2 && board[i + 1][j] == 2 && board[i + 2][j] == 2 && board[i + 3][j] == 2)
                    {
                        return 2;
                    }

                }
            }
            //diagonal-upper side is right
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i][j + 3] == 1 && board[i + 1][j + 2] == 1 && board[i + 2][j + 1] == 1 && board[i + 3][j] == 1)
                    {
                        return 1;
                    }
                    if (board[i][j + 3] == 2 && board[i + 1][j + 2] == 2 && board[i + 2][j + 1] == 2 && board[i + 3][j] == 2)
                    {
                        return 2;
                    }



                }
            }
            //diagonal-upper side is the left side
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i][j] == 1 && board[i + 1][j + 1] == 1 && board[i + 2][j + 2] == 1 && board[i + 3][j + 3] == 1)
                    {
                        return 1;
                    }
                    if (board[i][j] == 2 && board[i + 1][j + 1] == 2 && board[i + 2][j + 2] == 2 && board[i + 3][j + 3] == 2)
                    {
                        return 2;

                    }



                }
            }

            return 0;
        }

        private void PcTurn()
        {
            //winning section
            //rows
            for (int i = 0; i < 8; i++)
            {
                for (int j = 7, count2 = 0; j > 0; j--)
                {
                    if (board[i][j] == 0)
                    {
                        count2 = 0;
                    }
                    if (board[i][j] == 2)
                    {
                        count2++;

                    }
                    if (board[i][j] == 1)
                    {
                        count2 = 0;
                    }
                    if (count2 == 3)
                    {
                        if (board[i][j - 1] == 0)
                        {
                            board[i][j - 1] = 2;
                            bboard[i][j - 1].BackColor = Color.FromName("Red");
                            return;
                        }
                    }

                }
            }
            //collumns
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //right edge missing
                    if (board[i][j] == 2 && board[i + 1][j] == 2 && board[i + 2][j] == 2)
                    {
                        if (board[i + 3][j] == 0)
                        {
                            put(i + 3);
                            return;
                        }
                    }
                    // 2 from right edge missing
                    if (board[i][j] == 2 && board[i + 2][j] == 2 && board[i + 3][j] == 2)
                    {
                        if (board[i + 1][j] == 0)
                        {
                            put(i + 1);
                            return;
                        }
                    }
                    // 1 from right edge missing
                    if (board[i][j] == 2 && board[i + 1][j] == 2 && board[i + 3][j] == 2)
                    {
                        if (board[i + 2][j] == 0)
                        {
                            put(i + 2);
                            return;
                        }
                    }
                    //left edge missing
                    if (board[i + 2][j] == 2 && board[i + 1][j] == 2 && board[i + 3][j] == 2)
                    {
                        if (board[i][j] == 0)
                        {
                            put(i);
                            return;
                        }
                    }


                }
            }
            //diagonal-upper side is right
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i + 1][j + 2] == 2 && board[i + 2][j + 1] == 2 && board[i + 3][j] == 2)
                    {
                        if (board[i][j + 3] == 0)
                        {
                            put(i);
                            return;
                        }
                    }
                    if (board[i][j + 3] == 2 && board[i + 2][j + 1] == 2 && board[i + 3][j] == 2)
                    {
                        if (board[i + 1][j + 2] == 0)
                        {
                            put(i + 1);
                            return;
                        }
                    }
                    if (board[i][j + 3] == 2 && board[i + 1][j + 2] == 2 && board[i + 3][j] == 2)
                    {
                        if (board[i + 2][j + 1] == 0)
                        {
                            put(i + 2);
                            return;
                        }
                    }
                    if (board[i][j + 3] == 2 && board[i + 1][j + 2] == 2 && board[i + 2][j + 1] == 2)
                    {
                        if (board[i + 3][j] == 0)
                        {
                            put(i + 3);
                            return;
                        }
                    }



                }
            }
            //diagonal-upper side left
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //first from left side is missing
                    if (board[i + 1][j + 1] == 2 && board[i + 2][j + 2] == 2 && board[i + 3][j + 3] == 2)
                    {
                        if (board[i][j] == 0)
                        {
                            put(i);
                            return;
                        }
                    }
                    //second from left side is missing
                    if (board[i][j] == 2 && board[i + 2][j + 2] == 2 && board[i + 3][j + 3] == 2)
                    {
                        if (board[i + 1][j + 1] == 0)
                        {
                            put(i + 1);
                            return;
                        }
                    }
                    //third from left side is missing
                    if (board[i][j] == 2 && board[i + 1][j + 1] == 2 && board[i + 3][j + 3] == 2)
                    {
                        if (board[i + 2][j + 2] == 0)
                        {
                            put(i + 2);
                            return;
                        }
                    }
                    //right edge is missing
                    if (board[i][j] == 2 && board[i + 1][j + 1] == 2 && board[i + 2][j + 2] == 2)
                    {
                        if (board[i + 3][j + 3] == 0)
                        {
                            put(i + 3);
                            return;
                        }
                    }

                }
            }
            //blocking section
            //rows
            for (int i = 0; i < 8; i++)
            {
                for (int j = 7, count1 = 0; j > 0; j--)
                {
                    if (board[i][j] == 0)
                    {
                        count1 = 0;
                    }
                    if (board[i][j] == 1)
                    {
                        count1++;

                    }
                    if (board[i][j] == 2)
                    {
                        count1 = 0;
                    }
                    if (count1 == 3)
                    {
                        if (board[i][j - 1] == 0)
                        {
                            board[i][j - 1] = 2;
                            bboard[i][j - 1].BackColor = Color.FromName("Red");
                            return;
                        }
                    }

                }
            }
            //collumns
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //right edge missing
                    if (board[i][j] == 1 && board[i + 1][j] == 1 && board[i + 2][j] == 1)
                    {
                        if (board[i + 3][j] == 0)
                        {
                            put(i + 3);
                            return;
                        }
                    }
                    // 2 from right edge missing
                    if (board[i][j] == 1 && board[i + 2][j] == 1 && board[i + 3][j] == 1)
                    {
                        if (board[i + 1][j] == 0)
                        {
                            put(i + 1);
                            return;
                        }
                    }
                    // 1 from right edge missing
                    if (board[i][j] == 1 && board[i + 1][j] == 1 && board[i + 3][j] == 1)
                    {
                        if (board[i + 2][j] == 0)
                        {
                            put(i + 2);
                            return;
                        }
                    }
                    //left edge missing
                    if (board[i + 2][j] == 1 && board[i + 1][j] == 1 && board[i + 3][j] == 1)
                    {
                        if (board[i][j] == 0)
                        {
                            put(i);
                            return;
                        }
                    }


                }
            }
            //diagonal-upper side is right
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i + 1][j + 2] == 1 && board[i + 2][j + 1] == 1 && board[i + 3][j] == 1)
                    {
                        if (board[i][j + 3] == 0)
                        {
                            put(i);
                            return;
                        }
                    }
                    if (board[i][j + 3] == 1 && board[i + 2][j + 1] == 1 && board[i + 3][j] == 1)
                    {
                        if (board[i + 1][j + 2] == 0)
                        {
                            put(i + 1);
                            return;
                        }
                    }
                    if (board[i][j + 3] == 1 && board[i + 1][j + 2] == 1 && board[i + 3][j] == 1)
                    {
                        if (board[i + 2][j + 1] == 0)
                        {
                            put(i + 2);
                            return;
                        }
                    }
                    if (board[i][j + 3] == 1 && board[i + 1][j + 2] == 1 && board[i + 2][j + 1] == 1)
                    {
                        if (board[i + 3][j] == 0)
                        {
                            put(i + 3);
                            return;
                        }
                    }



                }
            }
            //diagonal-upper side left
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //first from left side is missing
                    if (board[i + 1][j + 1] == 1 && board[i + 2][j + 2] == 1 && board[i + 3][j + 3] == 1)
                    {
                        if (board[i][j] == 0)
                        {
                            put(i);
                            return;
                        }
                    }
                    //second from left side is missing
                    if (board[i][j] == 1 && board[i + 2][j + 2] == 1 && board[i + 3][j + 3] == 1)
                    {
                        if (board[i + 1][j + 1] == 0)
                        {
                            put(i + 1);
                            return;
                        }
                    }
                    //third from left side is missing
                    if (board[i][j] == 1 && board[i + 1][j + 1] == 1 && board[i + 3][j + 3] == 1)
                    {
                        if (board[i + 2][j + 2] == 0)
                        {
                            put(i + 2);
                            return;
                        }
                    }
                    //right edge is missing
                    if (board[i][j] == 1 && board[i + 1][j + 1] == 1 && board[i + 2][j + 2] == 1)
                    {
                        if (board[i + 3][j + 3] == 0)
                        {
                            put(i + 3);
                            return;
                        }
                    }

                }
            }
            put(new Random().Next(0, 8));

        }
        private void put(int row)
        {
            for (int i = 7; i >= 0; i--)
            {
                if (board[row][i] == 0)
                {
                    board[row][i] = 2;
                    bboard[row][i].BackColor = Color.FromName("Red");
                    return;
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void row(int curr)
        {
            for (int i = 0; i < 8; i++)
            {
                if (board[curr][7 - i] == 0)
                {
                    if (turns)
                    {

                        board[curr][7 - i] = 1;
                        bboard[curr][7 - i].BackColor = Color.FromName("Blue");
                        plays++;
                        if (mode2)
                        {
                            turns = false;
                        }
                        if (!(mode2))
                        {
                            PcTurn();
                            plays++;
                        }
                        break;
                    }
                    if (!(turns))
                    {

                        board[curr][7 - i] = 2;
                        bboard[curr][7 - i].BackColor = Color.FromName("Red");
                        turns = true;
                        plays++;
                        break;
                    }
                }

            }
            if (plays == 64)
            {
                MessageBox.Show("It's a tie!");
                Reset();
            }

            if (Check() == 1)
            {
                if (mode2) MessageBox.Show("Blue player won");
                if (!(mode2)) MessageBox.Show("You won");
                winb++;
                Reset();
            }
            if ((Check() == 2))
            {
                if (mode2) MessageBox.Show("Red player won");
                if (!(mode2)) MessageBox.Show("PC won");
                winr++;
                Reset();
            }


        }
        private void Row1(object sender, EventArgs e)
        {
            short temporarynum = 0;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }


        private void Row2(object sender, EventArgs e)
        {
            short temporarynum = 1;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }

        private void Row3(object sender, EventArgs e)
        {
            short temporarynum = 2;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }

        private void Row4(object sender, EventArgs e)
        {
            short temporarynum = 3;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }

        private void Row5(object sender, EventArgs e)
        {
            short temporarynum = 4;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }

        private void Row6(object sender, EventArgs e)
        {
            short temporarynum = 5;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }

        private void Row7(object sender, EventArgs e)
        {
            short temporarynum = 6;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }

        private void Row8(object sender, EventArgs e)
        {
            short temporarynum = 7;
            if (!(IsFull(board[temporarynum])))
            {
                row(temporarynum);
            }
            return;
        }
    }
}
