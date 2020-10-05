using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class MineSweeper : Form
    {
        System.Windows.Forms.Button flag;
        int btnSize;
        int turns;
        bool flagon;
        int bombs;//num of bombs
        int size;//size of the board
        int newSize;
        int flags;//amount of flags left
        Button[][] board;
        int[][] numBoard;
        Label flagCount = new Label();  //shows how many flags you have left
        Button addBombs = new Button(); //increase amount of bombs
        Button decBombs = new Button(); //decrease amount of bombs
        Label bombCount = new Label(); //show amount of new settings bombs
        Button newGame = new Button(); // start a new game with the new settings on click
        Button addSize = new Button(); //increase amount of size
        Button decSize = new Button(); //decrease amount of size
        Label sizeCount = new Label(); //show amount of new settings size

        public MineSweeper()
        {
            InitializeComponent();

            SetStyle(ControlStyles.Selectable, false);
            turns = 0;
            bombs = 10;
            flags = bombs;
            flagon = false;
            size = 10;
            newSize = size;
            board = new Button[size][];
            numBoard = new int[size][];
            btnSize = 30;
            CreateButtons();

            ResetBoard(-1, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }



        private void Place(object sender, EventArgs e)
        {

            flagCount.Text = "Flags left:" + flags;

            Button btn = (Button)sender;
            int x = 0;
            int y = 0;
            //finding pressed button location
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (btn.Location.X == board[i][j].Location.X && btn.Location.Y == board[i][j].Location.Y)
                    {
                        x = i;
                        y = j;
                        i = -1;
                        break;
                    }
                }
                if (i == -1) break;
            }
            //set board afterfirst click
            if (turns == 0)
            {
                ResetBoard(x, y);
                turns++;
            }
            //if pressed on bomb while clearing
            if (numBoard[x][y] == 9 && !flagon && board[x][y].BackColor != Color.FromName("Red"))
            {
                ExposeBombs();
                MessageBox.Show("You lost!");
                //ResetBoard(-1, -1);
                RestartGame(sender, e);
                return;

            }
            //flood an already pressed button
            if (board[x][y].Text != "")
            {
                if (ExposedBombCountAround(x, y) == numBoard[x][y])
                    FloodZero(x, y);
                flag.Focus();
                CheckWin();
                return;
            }
            //place flag

            if (flagon && board[x][y].Text == "")
            {




                if (board[x][y].BackColor == Color.FromName("Red"))
                {
                    board[x][y].BackColor = Color.FromName("Control");
                    board[x][y].Image = null;
                    flags++;
                    flagCount.Text = "Flags left:" + flags;
                    flag.Focus();
                    CheckWin();
                    return;
                }
                if (flags != 0)
                {
                    board[x][y].BackColor = Color.FromName("Red");
                    board[x][y].Image = Desktop_Minigames.Properties.Resources.filled_flag_128;
                    flags--;
                    flagCount.Text = "Flags left:" + flags;
                    flag.Focus();
                    CheckWin();
                    return;
                }
            }
            //place number
            if (!flagon && board[x][y].BackColor != Color.FromName("Gray") && board[x][y].BackColor != Color.FromName("Red"))
            {
                Press(x, y);
                CheckWin();
            }
            flag.Focus();

        }
        public void CheckWin()
        {
            if (flags == 0)
            {
                int counter = 0;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (numBoard[i][j] == 9 && board[i][j].BackColor == Color.FromName("Red"))
                        {
                            counter++;
                        }
                    }
                }
                if (counter == bombs)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            Press(i, j);
                        }
                    }
                    MessageBox.Show("You won!");

                }
            }
        }
        public void ExposeBombs()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (numBoard[i][j] == 9 && board[i][j].BackColor == Color.FromName("Red"))
                    {
                        board[i][j].BackColor = Color.FromName("Green");
                    }

                    if (numBoard[i][j] == 9 && board[i][j].BackColor != Color.FromName("Green"))
                    {
                        board[i][j].Image = Desktop_Minigames.Properties.Resources.bomb;
                    }
                }
            }
        }
        private void Flag_Click(object sender, EventArgs e)
        {
            flagon = !(flagon);
            if (flagon) this.flag.Text = "Sweeping Mines";
            if (!flagon) this.flag.Text = "Marking clear blocks";
        }
        //reset game board
        public void ResetBoard(int px, int py)
        {
            Controls.Clear();
            flags = bombs;
            flagCount.Text = "Flags left:" + flags;
            bombCount.Text = "" + bombs;
            flagon = false;
            size = newSize;
            CreateButtons();

            //bombs setup
            for (int i = 0; i < bombs; i++)
            {
                Random rnd = new Random();
                int x = rnd.Next(0, size);
                int y = rnd.Next(0, size);

                //  if (numBoard[x][y] == 9) i--;
                if (numBoard[x][y] != 9)
                {
                    if (px != x && py != y && turns == 0)
                    {
                        numBoard[x][y] = 9;
                    }
                    if (px != -1)
                    {
                         numBoard[x][y] = 9;
                    }

                }
                else
                {
                    i--;
                }



            }
            //numbers setup
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (numBoard[i][j] == 9)
                    {
                        if (j != 0)
                        {
                            if (numBoard[i][j - 1] != 9) numBoard[i][j - 1]++;
                        }
                        if (j != size - 1)
                        {
                            if (numBoard[i][j + 1] != 9)
                                numBoard[i][j + 1]++;
                        }
                        if (i != 0)
                        {
                            if (numBoard[i - 1][j] != 9)
                                numBoard[i - 1][j]++;
                        }
                        if (i != size - 1)
                        {
                            if (numBoard[i + 1][j] != 9)
                                numBoard[i + 1][j]++;
                        }
                        if (i != 0 && j != 0)
                        {
                            if (numBoard[i - 1][j - 1] != 9)
                                numBoard[i - 1][j - 1]++;
                        }
                        if (i != size - 1 && j != size - 1)
                        {
                            if (numBoard[i + 1][j + 1] != 9)
                                numBoard[i + 1][j + 1]++;

                        }
                        if (i != 0 && j != size - 1) { if (numBoard[i - 1][j + 1] != 9) numBoard[i - 1][j + 1]++; }
                        if (i != size - 1 && j != 0) { if (numBoard[i + 1][j - 1] != 9) numBoard[i + 1][j - 1]++; }
                    }
                }
            }

        }
        public int ExposedBombCountAround(int i, int j)
        {
            int bombsA = 0;
            if (j != 0)
            {

                if (numBoard[i][j - 1] == 9 && board[i][j - 1].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }
            }
            if (j != size - 1)
            {

                if (numBoard[i][j + 1] == 9 && board[i][j + 1].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }
            }
            if (i != 0)
            {

                if (numBoard[i - 1][j] == 9 && board[i - 1][j].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }
            }
            if (i != size - 1)
            {

                if (numBoard[i + 1][j] == 9 && board[i + 1][j].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }
            }

            if (i != 0 && j != 0)
            {

                if (numBoard[i - 1][j - 1] == 9 && board[i - 1][j - 1].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }
            }
            if (i != size - 1 && j != size - 1)
            {

                if (numBoard[i + 1][j + 1] == 9 && board[i + 1][j + 1].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }

            }
            if (i != 0 && j != size - 1)
            {

                if (numBoard[i - 1][j + 1] == 9 && board[i - 1][j + 1].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }
            }
            if (i != size - 1 && j != 0)
            {
                if (numBoard[i + 1][j - 1] == 9 && board[i + 1][j - 1].BackColor == Color.FromName("Red"))
                {
                    bombsA++;
                }
            }
            return bombsA;
        }
        public void FloodZero(int i, int j)
        {
            if (j != 0)
            {
                if (numBoard[i][j - 1] == 0 && board[i][j - 1].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i][j - 1]);
                    FloodZero(i, j - 1);

                }
                if (numBoard[i][j - 1] != 0 && numBoard[i][j - 1] != 9)
                {
                    Press(i, j - 1);
                }
            }
            if (j != size - 1)
            {
                if (numBoard[i][j + 1] == 0 && board[i][j + 1].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i][j + 1]);
                    FloodZero(i, j + 1);
                }
                if (numBoard[i][j + 1] != 0 && numBoard[i][j + 1] != 9)
                {
                    Press(i, j + 1);
                }
            }
            if (i != 0)
            {
                if (numBoard[i - 1][j] == 0 && board[i - 1][j].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i - 1][j]);
                    FloodZero(i - 1, j);
                }
                if (numBoard[i - 1][j] != 0 && numBoard[i - 1][j] != 9)
                {
                    Press(i - 1, j);
                }
            }
            if (i != size - 1)
            {
                if (numBoard[i + 1][j] == 0 && board[i + 1][j].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i + 1][j]);
                    FloodZero(i + 1, j);
                }
                if (numBoard[i + 1][j] != 0 && numBoard[i + 1][j] != 9)
                {
                    Press(i + 1, j);
                }
            }
            //////checking four corners of each zero block is unncessary for the game
            if (i != 0 && j != 0)
            {
                if (numBoard[i - 1][j - 1] == 0 && board[i - 1][j - 1].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i - 1][j - 1]);
                    FloodZero(i - 1, j - 1);
                }
                if (numBoard[i - 1][j - 1] != 0 && numBoard[i - 1][j - 1] != 9)
                {
                    Press(i - 1, j - 1);
                }
            }
            if (i != size - 1 && j != size - 1)
            {
                if (numBoard[i + 1][j + 1] == 0 && board[i + 1][j + 1].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i + 1][j + 1]);
                    FloodZero(i + 1, j + 1);
                }
                if (numBoard[i + 1][j + 1] != 0 && numBoard[i + 1][j + 1] != 9)
                {
                    Press(i + 1, j + 1);
                }

            }
            if (i != 0 && j != size - 1)
            {
                if (numBoard[i - 1][j + 1] == 0 && board[i - 1][j + 1].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i - 1][j + 1]);
                    FloodZero(i - 1, j + 1);
                }
                if (numBoard[i - 1][j + 1] != 0 && numBoard[i - 1][j + 1] != 9)
                {
                    Press(i - 1, j + 1);
                }
            }
            if (i != size - 1 && j != 0)
            {
                if (numBoard[i + 1][j - 1] == 0 && board[i + 1][j - 1].BackColor != Color.FromName("Gray"))
                {
                    Press(board[i + 1][j - 1]);
                    FloodZero(i + 1, j - 1);
                }
                if (numBoard[i + 1][j - 1] != 0 && numBoard[i + 1][j - 1] != 9)
                {
                    Press(i + 1, j - 1);
                }
            }


        }

        public void Press(Button prs)
        {
            if (prs.BackColor != Color.FromName("Red"))
            {
                prs.BackColor = Color.FromName("Gray");
            }
        }
        public void Press(int x, int y)
        {
            if (numBoard[x][y] == 0)
            {
                Press(board[x][y]);
                FloodZero(x, y);
                flag.Focus();
                return;
            }
            if (board[x][y].BackColor != Color.FromName("Red"))
            {
                board[x][y].Text = "" + numBoard[x][y];
                board[x][y].BackColor = Color.FromName("Gray");

                //color the number according to the bombs count around it
                switch (numBoard[x][y])
                {
                    case 1:
                        {
                            board[x][y].ForeColor = Color.FromName("Blue");
                            break;
                        }
                    case 2:
                        {
                            board[x][y].ForeColor = Color.FromName("Green");
                            break;
                        }
                    case 3:
                        {
                            board[x][y].ForeColor = Color.FromName("Red");
                            break;
                        }
                    case 4:
                        {
                            board[x][y].ForeColor = Color.FromName("Purple");
                            break;
                        }
                }
            }
            flag.Focus();
        }
        private void Add_Bombs(object sender, EventArgs e)
        {
            if (bombs + 1 != newSize * newSize)
            {
                bombs++;
                bombCount.Text = "" + bombs;
            }
        }
        private void Dec_Bombs(object sender, EventArgs e)
        {

            if (bombs - 1 != 0)
            {
                bombs--;
                bombCount.Text = "" + bombs;
            }
        }
        private void Add_Size(object sender, EventArgs e)
        {
            if (newSize + 1 != 21)
            {
                newSize++;
                sizeCount.Text = "" + newSize;
            }
        }
        private void Dec_Size(object sender, EventArgs e)
        {

            if ((newSize - 1) * (newSize - 1) > bombs)
            {
                newSize--;
                sizeCount.Text = "" + newSize;
            }
        }
        private void RestartGame(object sender, EventArgs e)
        {
            turns = 0;
            ResetBoard(-1, 0);
        }
        public void CreateButtons()
        {
            
                flagCount = new Label();
                addBombs = new Button();
                decBombs = new Button();
                bombCount = new Label();
                newGame = new Button();
                addSize = new Button();
                decSize = new Button();
                sizeCount = new Label();
            
            //Creating the game buttons
            board = new Button[size][];
            numBoard = new int[size][];
            Button[] temp = new Button[size];
            int[] numTemp = new int[size];
            for (int i = 0; i < size; i++)
            {
                temp = new Button[size];
                numTemp = new int[size];
                for (int j = 0; j < size; j++)
                {
                    numTemp[j] = 0;
                    temp[j] = new System.Windows.Forms.Button();
                    temp[j].Size = new System.Drawing.Size(30, 30);
                    temp[j].Location = new System.Drawing.Point(j * 30, i * 30);
                    temp[j].UseVisualStyleBackColor = true;
                    temp[j].BackColor = Color.FromName("Control");
                    Controls.Add(temp[j]);
                    temp[j].Click += new System.EventHandler(this.Place);
                }
                numBoard[i] = numTemp;
                board[i] = temp;
            }
            
                //decrease amount of bombs button
                decBombs.Location = new System.Drawing.Point(size * btnSize + 60, 70);
                decBombs.Text = "Remove Bombs";
                decBombs.Size = new System.Drawing.Size(90, 20);
                Controls.Add(decBombs);
                decBombs.Click += new System.EventHandler(this.Dec_Bombs);
                //show how many bombs will be in the new game
                bombCount.Location = new System.Drawing.Point(size * btnSize + 150, 73);
                bombCount.Text = "" + bombs;
                bombCount.Size = new System.Drawing.Size(20, 20);
                Controls.Add(bombCount);
                //increase amount of bombs button
                addBombs.Location = new System.Drawing.Point(size * btnSize + 170, 70);
                addBombs.Text = "Add Bombs";
                addBombs.Size = new System.Drawing.Size(90, 20);
                Controls.Add(addBombs);
                addBombs.Click += new System.EventHandler(this.Add_Bombs);
                ////////
                ////////   
                /////decrease amount of bombs button
                decSize.Location = new System.Drawing.Point(size * btnSize + 60, 100);
                decSize.Text = "Decrease size";
                decSize.Size = new System.Drawing.Size(90, 20);
                Controls.Add(decSize);
                decSize.Click += new System.EventHandler(this.Dec_Size);
                //show how many bombs will be in the new game
                sizeCount.Location = new System.Drawing.Point(size * btnSize + 150, 103);
                sizeCount.Text = "" + size;
                sizeCount.Size = new System.Drawing.Size(20, 20);
                Controls.Add(sizeCount);
                //increase amount of bombs button
                addSize.Location = new System.Drawing.Point(size * btnSize + 170, 100);
                addSize.Text = "Increase size";
                addSize.Size = new System.Drawing.Size(90, 20);
                Controls.Add(addSize);
                addSize.Click += new System.EventHandler(this.Add_Size);
                //new game button
                newGame.Location = new System.Drawing.Point(size * btnSize + 125, 150);
                newGame.Text = "New Game";
                newGame.Size = new System.Drawing.Size(70, 30);
                Controls.Add(newGame);
                newGame.Click += new System.EventHandler(this.RestartGame);

                //the sweeping toggle button
                flag = new System.Windows.Forms.Button();
                flag.Location = new System.Drawing.Point(size * btnSize, 0);
                flag.Size = new System.Drawing.Size(75, 50);
                flag.Text = "Marking clear blocks";
                flag.UseVisualStyleBackColor = true;
                Controls.Add(flag);
                flag.Click += new System.EventHandler(this.Flag_Click);
            
            //show amount of flags you can place
            flagCount.Location = new System.Drawing.Point(size * btnSize, 50);
            flagCount.Text = "Flags left:" + bombs;
            Controls.Add(flagCount);
            //

        }




    }
}
