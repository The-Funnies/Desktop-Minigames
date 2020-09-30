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
    public partial class UltimateTicTacToe : Form
    {
        private bool turn;
        private int turns;
        private int nextTurn;
        private Button[][] brdBoard;
        private String markingColor="Gray";
       
        public UltimateTicTacToe()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
    
            //setup
            turn = true;
            turns = 0;
            nextTurn = 10;
           brdBoard = new Button[9][];
            Button[] btnBoard = new Button[9];
            int floor = 0;
            int margin = 0;
            for (int i = 0; i < 9; i++)
            {
                if (i == 1 || i == 4 || i == 7) margin = 190;
                if (i == 0 || i == 3 || i == 6) margin = 0;
                if (i == 2 || i == 5 || i == 8) margin = 380;


                if (i < 3) floor = 0;
                if (i < 6 && i >= 3) floor = 190;
                if (i < 9 && i >= 6) floor = 380;
                btnBoard = new Button[9];
                for (int j = 0; j < 9; j++)
                {
                    btnBoard[j] = new System.Windows.Forms.Button();
                  
                    btnBoard[j].Size = new System.Drawing.Size(60, 60);
                    if (j < 3) btnBoard[j].Location = new System.Drawing.Point(j * 60 + margin, floor);
                    if (j < 6 && j >= 3) { btnBoard[j].Location = new System.Drawing.Point((j - 3) * 60 + margin, 60 + floor); }
                    if (j < 9 && j >= 6) btnBoard[j].Location = new System.Drawing.Point((j - 6) * 60 + margin, 120 + floor);
                    btnBoard[j].UseVisualStyleBackColor = true;
                    btnBoard[j].BackColor = Color.FromName("Control");
                    Controls.Add(btnBoard[j]);
                    btnBoard[j].Click += new System.EventHandler(this.Sorting);

                }
                brdBoard[i] = btnBoard;
            }
            
            //setup end
            //menu bar
            this.Menu = new MainMenu();
            MenuItem item = new MenuItem("Options");
            this.Menu.MenuItems.Add(item);
             item.MenuItems.Add("Reset game", new EventHandler(ResetHandler));
            // item.MenuItems.Add("Open", new EventHandler(Open_Click));
           
            //menu bar end
        }
        private void Sorting(object sender, EventArgs e)
        {
           if(!turn) this.label1.Text = "Blue\'s Turn";
           if(turn) this.label1.Text = "Red\'s Turn";
            UnMarkNextTurn();
            Button block = (Button)sender;
            if (block.BackColor == Color.FromName("Red") || block.BackColor == Color.FromName("Blue"))
            {
                MarkNextTurn();
                return;
            }
            

            int x = block.Location.X;
            int y = block.Location.Y;
            int place = 0;
            //finding the Board location and presenting it in int place
            //line 1
            //0
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if(brdBoard[i][j].Location.X==x&& brdBoard[i][j].Location.Y == y)
                    {
                        place = i;
                        i = 10;break;
                    }
                }
            }
            //checking if a player has a free turn(can place everywhere)
            int g = 0;
            if (nextTurn == 10) {g++; }
            //making sure that the player is putting a block in the right board
            if (place != nextTurn && g == 0)
            {
                MessageBox.Show("This is not the right board, you can only place in board number " + (nextTurn+1) + " right now!");
                MarkNextTurn();
                return;
            }
            //check if the player placed in a filled board
            if (Check(brdBoard[place]) != 0)
            {
                MessageBox.Show("You can't place in a filled board, try another board!");
                MarkNextTurn();
                return;
            }
                //inserting a block
                for (int i = 0; i < 1; i++)
                { 
                    if (block.BackColor == Color.FromName("Control"))
                    {
                        if (turn)
                        {
                        block.BackColor = Color.FromName("Blue");
                        turn = false;
                        turns++;
                        break;
                        }
                        if (!(turn))
                        {
                        block.BackColor = Color.FromName("Red");
                        turn = true;
                        turns++;
                        break;
                        }
                    }
                }
            //find location of the pressed button in the current board(brdBoard[place])
            for (int i = 0; i < 9; i++)
            {
                if (brdBoard[place][i].Location.X == x && brdBoard[place][i].Location.Y == y)
                {
                    if (Check(brdBoard[i]) == 0) { nextTurn = i; break; }
                    //give free turn if the board is closed

                    nextTurn = 10 ;
                }
            }
            
            //Check for a win in the current Board(brdBoard[place])
            if (Check(brdBoard[place]) == 1)
            {
               Win("Blue", brdBoard[place]);
                MessageBox.Show("Blue just acquired a Board");
             

            }
            if (Check(brdBoard[place])==2)
            {
                Win("Red", brdBoard[place]);
                MessageBox.Show("Red just acquired a Board");
                
            }
            //Check for a full game win
            if (FullBoardCheck() == 1)
            {
                MessageBox.Show("Blue won the game");
                FullWin("Blue");
            }
            if (FullBoardCheck() == 2)
            {
                MessageBox.Show("Red won the game");
                FullWin("Red");
            }
            //make a tie option
            //highlight the placing options
            MarkNextTurn();
        }
        //Check for a win in a single Board
        public int Check(Button[] btnBoard)
        {
            //collumn check 012 345 678
            for (int i = 0; i < 3; i++)
            {
                if (btnBoard[i * 3].BackColor == btnBoard[i * 3 + 1].BackColor && btnBoard[i * 3 + 1].BackColor == btnBoard[i * 3 + 2].BackColor)
                {
                    if (btnBoard[i * 3].BackColor == Color.FromName("Red")) return 2;
                    if (btnBoard[i * 3].BackColor == Color.FromName("Blue")) return 1;
                }
            }
            //rows check 036 147 258
            for (int i = 0; i < 3; i++)
            {
                if (btnBoard[i].BackColor == btnBoard[i + 3].BackColor && btnBoard[i + 3].BackColor == btnBoard[i + 6].BackColor)
                {
                    if (btnBoard[i].BackColor == Color.FromName("Red")) return 2;
                    if (btnBoard[i].BackColor == Color.FromName("Blue")) return 1;
                }
            }
            //diagonal 0 4 8 (upper left)
            if (btnBoard[0].BackColor == btnBoard[4].BackColor && btnBoard[8].BackColor == btnBoard[4].BackColor)
            {
                if (btnBoard[0].BackColor == Color.FromName("Red")) return 2;
                if (btnBoard[0].BackColor == Color.FromName("Blue")) return 1;
            }
            //diagonal 6 4 2 (upper right)
            if (btnBoard[6].BackColor == btnBoard[4].BackColor && btnBoard[2].BackColor == btnBoard[4].BackColor)
            {
                if (btnBoard[4].BackColor == Color.FromName("Red")) return 2;
                if (btnBoard[4].BackColor == Color.FromName("Blue")) return 1;
            }
           
            return 0;
        }
        //Check for a win in the game
        private int FullBoardCheck()
        {
            //collumn check 012 345 678
            for (int i = 0; i < 3; i++)
            {
                if (Check(brdBoard[i * 3]) == Check(brdBoard[i * 3 + 1]) && Check(brdBoard[i * 3 + 1]) == Check(brdBoard[i * 3 + 2]))
                {
                    if (Check(brdBoard[i * 3]) == 2) return 2;
                    if (Check(brdBoard[i * 3]) == 1) return 1;
                }
            }
            //rows check 036 147 258
            for (int i = 0; i < 3; i++)
            {
                if (Check(brdBoard[i]) == Check(brdBoard[i + 3]) && Check(brdBoard[i + 3]) == Check(brdBoard[i + 6]))
                {
                    if (Check(brdBoard[i]) == 2) return 2;
                    if (Check(brdBoard[i]) == 1) return 1;
                }
            }
            //diagonal 0 4 8 (upper left)
            if (Check(brdBoard[0]) == Check(brdBoard[4]) && Check(brdBoard[4]) == Check(brdBoard[8]))
            {
                if (Check(brdBoard[0]) == 2) return 2;
                if (Check(brdBoard[0]) == 1) return 1;
            }
            //diagonal 6 4 2 (upper right)
            if (Check(brdBoard[6]) == Check(brdBoard[4]) && Check(brdBoard[4]) == Check(brdBoard[2]))
            {
                if (Check(brdBoard[4]) == 2) return 2;
                if (Check(brdBoard[4]) == 1) return 1;
            }

            return 0;
        }
        //finish a single Board(fill with color)
        public void Win(String color,Button[] btnBoard)
        {
            for (int i = 0; i < 9; i++)
            {
                btnBoard[i].BackColor = Color.FromName(color);
            }
        }
        //finish game(fill with color)
        public void FullWin(String color)
        {
            for (int i = 0; i < 9; i++)
            {
                Win(color, brdBoard[i]);
            }
        }
        //event handler for reset menu bar option
        private void ResetHandler(object sender, EventArgs e)
        {
            FullReset();
        }
        //reset single Board
        public void Reset(Button[] btnBoard)
        { 
                for (int i = 0; i < 9; i++)
                {
                    btnBoard[i].BackColor = Color.FromName("Control");
                }
        }
        //reset full game
        public void FullReset()
        {
            turn = true;
            turns = 0;
            nextTurn = 10;
            for (int i = 0; i < 9; i++)
            {
                Reset(brdBoard[i]);
            }
        }
        //mark by next turn block
        public void MarkNextTurn()
        {
            if (nextTurn == 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (brdBoard[i][j].BackColor == Color.FromName("Control"))
                        {
                            brdBoard[i][j].BackColor = Color.FromName("Gray");
                        }
                    }
                }
            }
            if (nextTurn != 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (brdBoard[nextTurn][i].BackColor == Color.FromName("Control"))
                    {
                        brdBoard[nextTurn][i].BackColor = Color.FromName("Gray");
                    }
                }
            }
        }
        //unmark previous marked blocks
        public void UnMarkNextTurn()
        {
            if (nextTurn == 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (brdBoard[i][j].BackColor == Color.FromName("Gray"))
                        {
                            brdBoard[i][j].BackColor = Color.FromName("Control");
                        }
                    }
                }
            }
            if (nextTurn != 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (brdBoard[nextTurn][i].BackColor == Color.FromName("Gray"))
                    {
                        brdBoard[nextTurn][i].BackColor = Color.FromName("Control");
                    }
                }
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
