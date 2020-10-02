using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class SingleplayerGame : Form
    {
        private Board board;
        private int difficultyDepth = 3;
        public SingleplayerGame()
        {
            InitializeComponent();
            board = new Board(this);
            this.Controls.Add(board);
        }

        private void SingleplayerGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private int GetBestPlay(int[] boardValues, int depth, bool isWhiteTurn)
        {
            boardValues = this.board.GetValues();
            int isThrereAWinner = Board.IsThrereAWinner(boardValues);
            Cell[] availablePlaces;
            if (isThrereAWinner == 2)
            {
                if (isWhiteTurn)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (depth == 0 || isThrereAWinner == 1)
            {
                return 0;
            }
            else
            {
                availablePlaces = this.board.GetAllAvailableCells();

                if (isWhiteTurn)
                {
                    int bestResult = -100;
                    foreach (Cell cell in availablePlaces)
                    {
                        cell.SetValue(2);
                        int result = GetBestPlay(boardValues, depth - 1, !isWhiteTurn);
                        cell.SetValue(0);
                        bestResult = Math.Max(result, bestResult);
                        if (bestResult > 0) break;                       
                    }
                    return bestResult;
                }

                else
                {
                    int bestResult = 100;
                    foreach (Cell cell in availablePlaces)
                    {
                        cell.SetValue(1);
                        int result = GetBestPlay(boardValues, depth - 1, !isWhiteTurn);
                        cell.SetValue(0);
                        bestResult = Math.Min(result, bestResult);
                        if (bestResult < 0) break;
                    }
                    return bestResult;
                }
            }
        }

        private Cell GetBestCellToPlay()
        {
            Random rnd = new Random();
            Cell[] availablePlaces = this.board.GetAllAvailableCells();
            Cell[] shuffled = availablePlaces.OrderBy(a => Guid.NewGuid()).ToArray();

            int bestValue = 100;
            int bestIndex = -1;
            int moveVal;
            foreach (Cell cell in shuffled)
            {
                cell.SetValue(1);
                if (Board.IsThrereAWinner(this.board.GetValues()) == 2)
                {
                    moveVal = -1;
                }
                else if(Board.IsThrereAWinner(this.board.GetValues()) == 1)
                {
                    moveVal = 0;
                }
                else
                {
                    moveVal = GetBestPlay(null, this.difficultyDepth, true);
                }
                cell.SetValue(0);
                if (moveVal < bestValue)
                {
                    if (moveVal == -1)
                    {
                        bestValue = moveVal;
                        bestIndex = cell.GetIndex();
                        break;
                    }
                    bestValue = moveVal;
                    bestIndex = cell.GetIndex();
                }
            }
            return board.GetCellByIndex(bestIndex);
        }

        public void Button_Clicked(object sender, EventArgs e)
        {
            Cell clickedCell = sender as Cell;
            if (clickedCell.GetValue() != 0) return;
            clickedCell.SetTextByValue(2);
            int isThrereAWinner = Board.IsThrereAWinner(board.GetValues());
            if (isThrereAWinner == 2)
            {
                MessageBox.Show("You won!");
                board.Reset();
                return;
            }
            else if (isThrereAWinner == 1)
            {
                MessageBox.Show("Tie");
                board.Reset();
                return;
            }

            GetBestCellToPlay().SetTextByValue(1);
            isThrereAWinner = Board.IsThrereAWinner(board.GetValues());
            if (isThrereAWinner == 2)
            {
                MessageBox.Show("You lost!");
                board.Reset();
            }
            else if (isThrereAWinner == 1)
            {
                MessageBox.Show("Tie");
                board.Reset();
            }
        }



        private void SingleplayerGame_Load(object sender, EventArgs e)
        {

        }

        private void Easy_Click(object sender, EventArgs e)
        {
            this.difficultyDepth = 1;
        }

        private void Medium_Click(object sender, EventArgs e)
        {
            this.difficultyDepth = 3;
        }

        private void Impossible_Click(object sender, EventArgs e)
        {
            this.difficultyDepth = 9;
        }
    }
}
