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
    public partial class UltimateUltimateTicTacToe : Form
    {
        private bool turn;
        private int turns;
        private int nextTurn;
        private int gameTurn;

   
        private Button[][][] gameBoard;
        private String markingColor="Gray";
       
        public UltimateUltimateTicTacToe()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
           
            //setup
            turn = true;
            turns = 0;
            gameTurn = 10;
           nextTurn = 10;
           Button[][] brdBoard = new Button[9][];
            Button[] btnBoard = new Button[9];
            gameBoard = new Button[9][][];
            int floor = 0;
            int margin = 0;
            int fullMargin = 0;
            int fullFloor = 0;
            for (int n = 0; n < 9; n++)
            {

                brdBoard = new Button[9][];
                if (n == 1 || n == 4 || n == 7)fullMargin = 330;
                if (n == 0 || n == 3 || n == 6)fullMargin = 0;
                if (n == 2 || n == 5 || n == 8)fullMargin = 660;
                if(n < 3) fullFloor = 0;
                if (n < 6 && n >= 3) fullFloor = 330;
                if (n < 9 && n >= 6) fullFloor = 660;

                for (int i = 0; i < 9; i++)
                {
                    if (i == 1 || i == 4 || i == 7) margin = 100;
                    if (i == 0 || i == 3 || i == 6) margin = 0;
                    if (i == 2 || i == 5 || i == 8) margin = 200;


                    if (i < 3) floor = 0;
                    if (i < 6 && i >= 3) floor = 100;
                    if (i < 9 && i >= 6) floor = 200;
                    btnBoard = new Button[9];
                    for (int j = 0; j < 9; j++)
                    {
                        btnBoard[j] = new System.Windows.Forms.Button();

                        btnBoard[j].Size = new System.Drawing.Size(30, 30);
                        if (j < 3) btnBoard[j].Location = new System.Drawing.Point(j * 30 + margin + fullMargin, floor+fullFloor);
                        if (j < 6 && j >= 3) { btnBoard[j].Location = new System.Drawing.Point((j - 3) * 30 + margin+fullMargin, 30 + floor + fullFloor); }
                        if (j < 9 && j >= 6) btnBoard[j].Location = new System.Drawing.Point((j - 6) * 30 + margin + fullMargin, 60 + floor + fullFloor);
                        btnBoard[j].UseVisualStyleBackColor = true;
                        btnBoard[j].BackColor = Color.FromName("Control");
                        Controls.Add(btnBoard[j]);
                        btnBoard[j].Click += new System.EventHandler(this.Sorting);

                    }
                    brdBoard[i] = btnBoard;
                }
                gameBoard[n] = brdBoard;
            }
            //setup end
            //menu bar
            this.Menu = new MainMenu();
            MenuItem item = new MenuItem("Options");
            this.Menu.MenuItems.Add(item);
             item.MenuItems.Add("Reset game", new EventHandler(ResetHandler));
            // item.MenuItems.Add("Open", new EventHandler(Open_Click));
            MenuItem color = new MenuItem("Color");
            this.Menu.MenuItems.Add(color);
            color.MenuItems.Add("Gray", new EventHandler(ChangeGray));
            color.MenuItems.Add("Yellow", new EventHandler(ChangeYellow));
            color.MenuItems.Add("Green", new EventHandler(ChangeGreen));

            //menu bar end
        }
        private void Sorting(object sender, EventArgs e)
        {
            if (!turn)
            {
                this.label1.Text = "Blue\'s Turn";
            }
            if (turn)
            {
                this.label1.Text = "Red\'s Turn";
            }
            FullUnMarkNextTurn();
            Button block = (Button)sender;
            if (block.BackColor == Color.FromName("Red") || block.BackColor == Color.FromName("Blue"))
            {
                FullMarkNextTurn();
                return;
            }
            

            int x = block.Location.X;
            int y = block.Location.Y;

            int gamePlace = 0;
            int place = 0;
            //finding the pressed button location
            int currentplace = 0;
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                       if( gameBoard[i][j][k].Location.X == x && gameBoard[i][j][k].Location.Y == y)
                        {
                            currentplace = k;
                            place = j;
                            gamePlace = i;
                            i = 10;j = 10;
                            break;
                        }
                    }
                }
            }
            //making sure that the player is coloring a button in the right Ultimate Ultimate
            int fullFreeTurn = 0;if (gameTurn == 10) fullFreeTurn++;
            if (gamePlace != gameTurn && fullFreeTurn == 0)
            {
                MessageBox.Show("This is not the size 2 right game board, you can only place in board number " + (gameTurn + 1) + " right now!");
                FullMarkNextTurn();
                return;
            }
            if(fullFreeTurn==1)gameTurn = gamePlace;
            int freeturn = 0;if (nextTurn == 10) {freeturn++; }
            //making sure that the player is coloring a button in the right Ultimate
            if (place != nextTurn &&freeturn == 0)
            {
                MessageBox.Show("This is not the right board, you can only place in board number " + (nextTurn+1) + " right now!");
                FullMarkNextTurn();
                return;
            }
            //check if the player placed in a filled board
            if (Check(gameBoard[gamePlace][place]) != 0)
            {
                MessageBox.Show("You can't place in a filled board, try another board!");
                FullMarkNextTurn();
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
                if (gameBoard[gamePlace][place][i].Location.X == x && gameBoard[gamePlace][place][i].Location.Y == y)
                {
                    if (Check(gameBoard[gamePlace][place]) == 0) { nextTurn = i; break; }
                    //give free turn if the board is closed

                    nextTurn = 10 ;
                }
            }
            if (Check(gameBoard[gamePlace][currentplace]) != 0)
            {
                nextTurn = 10; 
            }
            //Check for a win in the current Board(brdBoard[place])
            if (Check(gameBoard[gamePlace][place]) == 1)
            {
                FullUnMarkNextTurn();
                gameTurn = place;
                nextTurn = currentplace;
                FullMarkNextTurn();
                Win("Blue", gameBoard[gamePlace][place]);
                MessageBox.Show("Blue just acquired a Board");
            }
            if (Check(gameBoard[gamePlace][place])==2)
            {
                FullUnMarkNextTurn();
                gameTurn = place;
                nextTurn = currentplace;
                FullMarkNextTurn();
                Win("Red", gameBoard[gamePlace][place]);
                MessageBox.Show("Red just acquired a Board");    
            }
            //Check for a full win(Ultimate win)
            if (FullBoardCheck(gameBoard[gamePlace]) == 1)
            {
                MessageBox.Show("Blue just acquired a size 2 board");
                FullWin("Blue", gameBoard[gamePlace]);
            }
            if (FullBoardCheck(gameBoard[gamePlace]) == 2)
            {
                MessageBox.Show("Red just acquired a size 2 board");
                FullWin("Red", gameBoard[gamePlace]);
            }
            if (CompleteCheck() == 1)
            {
                MessageBox.Show("Blue has won the game, finally!");
                CompleteWin("Blue");
            }
            if (CompleteCheck() == 2)
            {
                MessageBox.Show("Red has won the game, finally!");
                CompleteWin("Red");
            }
            if (nextTurn != 10)
            {
                if (Check(gameBoard[gameTurn][nextTurn]) != 0)
                {
                    nextTurn = 10;
                }
            }
            if (gameTurn != 10)
            {
                if (FullBoardCheck(gameBoard[gameTurn]) != 0)
                {
                    gameTurn = 10;
                }
            }
            //make a tie option
            for (int i = 0; i < 9; i++)
            {
                if (FullBoardCheck(gameBoard[i])==0)
                {
                    break;
                }
                if (i == 8)
                {
                    MessageBox.Show("The game is still developing so theres no tie feature!Keep playing");
                    CompleteReset();

                }
            }
            //highlight the placing options
            FullMarkNextTurn();

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
        private int FullBoardCheck(Button[][] brdBoard)
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
        public int CompleteCheck()
        {
            //collumn check 012 345 678
            for (int i = 0; i < 3; i++)
            {
                if (FullBoardCheck(gameBoard[i * 3]) == FullBoardCheck(gameBoard[i * 3 + 1]) && FullBoardCheck(gameBoard[i * 3 + 1]) == FullBoardCheck(gameBoard[i * 3 + 2]))
                {
                    if (FullBoardCheck(gameBoard[i * 3]) == 2) return 2;
                    if (FullBoardCheck(gameBoard[i * 3]) == 1) return 1;
                }
            }
            //rows check 036 147 258
            for (int i = 0; i < 3; i++)
            {
                if (FullBoardCheck(gameBoard[i]) == FullBoardCheck(gameBoard[i + 3]) && FullBoardCheck(gameBoard[i + 3]) == FullBoardCheck(gameBoard[i + 6]))
                {
                    if (FullBoardCheck(gameBoard[i]) == 2) return 2;
                    if (FullBoardCheck(gameBoard[i]) == 1) return 1;
                }
            }
            //diagonal 0 4 8 (upper left)
            if (FullBoardCheck(gameBoard[0]) == FullBoardCheck(gameBoard[4]) && FullBoardCheck(gameBoard[4]) == FullBoardCheck(gameBoard[8]))
            {
                if (FullBoardCheck(gameBoard[0]) == 2) return 2;
                if (FullBoardCheck(gameBoard[0]) == 1) return 1;
            }
            //diagonal 6 4 2 (upper right)
            if (FullBoardCheck(gameBoard[6]) == FullBoardCheck(gameBoard[4]) && FullBoardCheck(gameBoard[4]) == FullBoardCheck(gameBoard[2]))
            {
                if (FullBoardCheck(gameBoard[4]) == 2) return 2;
                if (FullBoardCheck(gameBoard[4]) == 1) return 1;
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
        public void FullWin(String color, Button[][] brdBoard)
        {
            for (int i = 0; i < 9; i++)
            {
                Win(color, brdBoard[i]);
            }
        }
        public void CompleteWin(String color)
        {
            for (int i = 0; i < 9; i++)
            {
                FullWin(color,gameBoard[i]);
            }
        }
        //event handler for reset menu bar option
        private void ResetHandler(object sender, EventArgs e)
        {
            CompleteReset();
        }
        //change color to gray
        private void ChangeGray(object sender, EventArgs e)
        {
            FullUnMarkNextTurn();
            markingColor = "Gray";
            FullMarkNextTurn();
        }
        //change color to yellow
        private void ChangeYellow(object sender, EventArgs e)
        {
            FullUnMarkNextTurn();
            markingColor = "Yellow";
            FullMarkNextTurn();
        }
        //change color to green
        private void ChangeGreen(object sender, EventArgs e)
        {
            FullUnMarkNextTurn();
            markingColor = "Green";
            FullMarkNextTurn();
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
        public void FullReset(Button[][] brdBoard)
        {
            turn = true;
            turns = 0;
            nextTurn = 10;
            for (int i = 0; i < 9; i++)
            {
                Reset(brdBoard[i]);
            }
        }
        public void CompleteReset()
        {
            turn = true;
            turns = 0;
            gameTurn = 10;
            nextTurn = 10;
          
            for (int i = 0; i < 9; i++)
            {
                FullReset(gameBoard[i]);
            }
        }
        //mark by next turn block
        public void MarkNextTurn(Button[][] brdBoard)
        {
            if (nextTurn == 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (brdBoard[i][j].BackColor == Color.FromName("Control"))
                        {
                            brdBoard[i][j].BackColor = Color.FromName(markingColor);
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
                        brdBoard[nextTurn][i].BackColor = Color.FromName(markingColor);
                    }
                }
            }
        }
        public void FullMarkNextTurn()
        {
            if (gameTurn == 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (gameBoard[i][k][j].BackColor == Color.FromName("Control"))
                            {
                                gameBoard[i][k][j].BackColor = Color.FromName(markingColor);
                            }
                        }
                    }
                }
                return;
            }
            MarkNextTurn(gameBoard[gameTurn]);
            
        }
        //unmark previous marked blocks
        public void UnMarkNextTurn(Button[][] brdBoard)
        {
            if (nextTurn == 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (brdBoard[i][j].BackColor == Color.FromName(markingColor))
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
                    if (brdBoard[nextTurn][i].BackColor == Color.FromName(markingColor))
                    {
                        brdBoard[nextTurn][i].BackColor = Color.FromName("Control");
                    }
                }
            }
        }
        public void FullUnMarkNextTurn()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int k = 0; k < 9; k++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (gameBoard[i][k][j].BackColor == Color.FromName(markingColor))
                        {
                            gameBoard[i][k][j].BackColor = Color.FromName("Control");
                        }
                    }
                }
            }
          
        }
        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
