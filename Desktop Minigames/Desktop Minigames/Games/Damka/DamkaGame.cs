using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Damka
{
    class Board : TableLayoutPanel
    {
        private DamkaBoard damkaBoard = new DamkaBoard();
        private Cell[,] board = new Cell[8, 8];

        public Board(Damka form)
        {
            this.Location = new Point(0, 24);
            this.Width = 800;
            this.Height = 800;
            this.ColumnCount = 8;
            this.RowCount = 8;
            for (int i = 0; i < 8; i++)
            {
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
                this.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));

                for (int j = 0; j < 8; j++)
                {
                    Cell tmpCell = new Cell(i, j);

                    tmpCell.TabStop = false;
                    tmpCell.Font = new Font(tmpCell.Font.FontFamily, 70);
                    tmpCell.Size = new Size(this.Width / 8, this.Height / 8);
                    tmpCell.BackgroundImageLayout = ImageLayout.Stretch;
                    tmpCell.Click += new EventHandler(form.Button_Click);
                    tmpCell.FlatStyle = FlatStyle.Flat;

                    if (j % 2 == 0)
                    {
                        if (i % 2 == 0)
                        {
                            tmpCell.SetOriginalColor(Color.LightGray);
                        }
                        else
                        {
                            if (i > 4)
                            {
                                tmpCell.SetPieceWithText(Piece.RedPiece);
                            }
                            else if(i < 3)
                            {
                                tmpCell.SetPieceWithText(Piece.BlackPiece);
                            }
                            tmpCell.SetOriginalColor(Color.DarkGray);
                        }
                    }
                    else
                    {
                        if (i % 2 != 0)
                        {
                            tmpCell.SetOriginalColor(Color.LightGray);
                        }
                        else
                        {
                            if (i > 4)
                            {
                                tmpCell.SetPieceWithText(Piece.RedPiece);
                            }
                            else if (i < 3)
                            {
                                tmpCell.SetPieceWithText(Piece.BlackPiece);
                            }
                            tmpCell.SetOriginalColor(Color.DarkGray);
                        }
                    }
                    this.board[i, j] = tmpCell;
                    this.damkaBoard.SetValueByIndex(i, j, tmpCell.GetPieceValue());
                    this.Controls.Add(tmpCell);
                }
            }
        }

        private void SetCell(Cell cell)
        {
            int i = cell.colume;
            int j = cell.row;
            cell.SetPieceWithText(Piece.Nothing);
            if (j % 2 == 0)
            {
                if (i % 2 != 0)
                {
                    if (i > 4)
                    {
                        cell.SetPieceWithText(Piece.RedPiece);
                    }
                    else if (i < 3)
                    {
                        cell.SetPieceWithText(Piece.BlackPiece);
                    }
                }
            }
            else
            {
                if (i % 2 == 0)
                {
                    if (i > 4)
                    {
                        cell.SetPieceWithText(Piece.RedPiece);
                    }
                    else if (i < 3)
                    {
                        cell.SetPieceWithText(Piece.BlackPiece);
                    }
                }
            }
            cell.SetToOriginalColor();
            this.damkaBoard.SetValueByIndex(i, j, cell.GetPieceValue());
        }

        public void Reset()
        {
            this.damkaBoard = new DamkaBoard();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SetCell(this.board[i, j]);
                }
            }
        }

        public void AppendFromDamkaBoard(DamkaBoard board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.board[i, j].SetPieceWithText(board.GetPieceByIndex(i, j));
                }
            }
            this.damkaBoard = board.Clone();
        }
        
        public DamkaBoard GetDamkaBoard()
        {
            return this.damkaBoard;
        }

        private void GetMovedToCell(DamkaBoard otherBoard)
        {
            Piece[,] thisPices = this.damkaBoard.GetBoard();
            Piece[,] otherPices = otherBoard.GetBoard();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (thisPices[i,j] == Piece.Nothing && otherPices[i, j] != Piece.Nothing)
                    {
                        Cell cell = this.board[i, j];
                        cell.SetBoard(otherBoard);
                        cell.BackColor = Color.Green;
                    }
                }
            }
        }

        public void ClearMoves()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    
                    Cell cell = this.board[i, j];
                    cell.SetBoard(null);
                    cell.SetToOriginalColor();
                    
                }
            }
        }

        private void GetMovedToCells(DamkaBoard[] damkaBoards)
        {
            foreach (DamkaBoard board in damkaBoards)
            {
                GetMovedToCell(board);
            }
        }


        public void ShowMoves(Cell clickedCell, bool isBlack)
        {
            int y = clickedCell.colume;
            int x = clickedCell.row;
            Piece currentPiece = this.damkaBoard.GetPieceByIndex(y, x);
            if (currentPiece != Piece.RedPiece && currentPiece != Piece.RedQueen && !isBlack)
            {
                return;
            }

            if (currentPiece != Piece.BlackPiece && currentPiece != Piece.BlackQueen && isBlack)
            {
                return;
            }

            DamkaBoard[] moves = this.damkaBoard.GetAllMovesWithIndex(y, x);

            if (moves.Length > 0)
            {
                GetMovedToCells(moves);
            }
        }
    }

    class Cell : Button
    {
        public int colume { get; }
        public int row { get; }

        private DamkaBoard tmpBoard;       
        private Piece piece;
        private Color originalColor;

        public Cell(int colume, int row)
        {
            this.row = row;
            this.colume = colume;
            this.piece = Piece.Nothing;
        }

        public void SetToOriginalColor()
        {
            this.BackColor = this.originalColor;
        }

        public void SetOriginalColor(Color color)
        {
            this.BackColor = color;
            this.originalColor = color;
        }

        public void SetBoard(DamkaBoard board)
        {
            this.tmpBoard = board;
        }

        public DamkaBoard GetBoard()
        {
            return this.tmpBoard;
        }
        public int GetPieceValue()
        {
            return (int)this.piece;
        }

        public Cell(int row, int colume, Piece piece)
        {
            this.row = row;
            this.colume = colume;
            this.piece = piece;
        }

        public void SetPiece(Piece piece)
        {
            this.piece = piece;
        }

        public void SetPieceWithText(Piece piece)
        {
            SetPiece(piece);
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    SetPieceWithImage();
                });
            }
            else
            {
                SetPieceWithImage();
            }
        }
        private void SetPieceWithImage()
        {
            if (piece == Piece.Nothing)
            {
                this.BackgroundImage = null;
            }
            else if (piece == Piece.RedPiece)
            {
                this.BackgroundImage = Desktop_Minigames.Properties.Resources.RedPiece_remove;
            }
            else if (piece == Piece.BlackPiece)
            {
                this.BackgroundImage = Desktop_Minigames.Properties.Resources.BlackPiece_remove;
            }
            else if (piece == Piece.RedQueen)
            {
                this.BackgroundImage = Desktop_Minigames.Properties.Resources.RedQueen_remove;
            }
            else
            {
                this.BackgroundImage = Desktop_Minigames.Properties.Resources.BlackQueen_remove;
            }
        }
    }

    class DamkaBoard
    {
        private byte[,] board = new byte[8, 2];
        private int numberOfMovesWithoutSkips = 0;

        public DamkaBoard()
        {

        }
        public DamkaBoard(byte[] arr)
        {
            this.board = DamkaBoard.ConvertTo2DArray(arr.Take(arr.Length - 1).ToArray());
            this.numberOfMovesWithoutSkips = arr[arr.Length - 1];
        }

        public void SetValueByIndex(int i, int j, int value)
        {
            if (IsNonePlace(i, j)) return;
            this.board[i, j / 4] = Utilities.SetByteValue(this.board[i, j / 4], value, IsFirstHalf(i, j));
        }

        private static bool IsFirstHalf(int y, int x)
        {
            return y % 2 == 0 ? x == 1 || x == 5 : x == 0 || x == 4;
        }
        private static bool IsNonePlace(int y, int x)
        {
            return y % 2 == 0 ? x % 2 == 0 : x % 2 != 0;
        }
        public Piece GetPieceByIndex(int i, int j)
        {
            if (IsNonePlace(i, j))
            {
                return Piece.Nothing;
            }
            return (Piece)Utilities.GetByteValue(this.board[i, j/4], IsFirstHalf(i, j));
        }
        public Piece GetPieceByIndex(int i, int j, DamkaBoard board)
        {
            return board.GetPieceByIndex(i, j);
        }

        public int GetNumberOfMovesWithoutSkips()
        {
            return this.numberOfMovesWithoutSkips;
        }

        public Winner WhoWins()
        {
            int[] pices = GetPices();
            if (pices[0] == 0 && pices[1] == 0)
            {
                return Winner.Red;
            }
            else if(pices[2] == 0 && pices[3] == 0)
            {
                return Winner.Black;
            }
            else if (GetAllMoves(true).Length == 0)
            {
                return Winner.Black;
            }
            else if (GetAllMoves(false).Length == 0)
            {
                return Winner.Red;
            }

            if (this.numberOfMovesWithoutSkips >= 40)
            {
                return Winner.Draw;
            }
            return Winner.NoOne;
        } 
        
        public int[] GetPices()
        {
            int[] pices = new int[4] {0,0,0,0};
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (GetPieceByIndex(i,j))
                    {
                        case Piece.BlackPiece:
                            pices[0]++;
                            break;
                        case Piece.BlackQueen:
                            pices[1]++;
                            break;
                        case Piece.RedPiece:
                            pices[2]++;
                            break;
                        case Piece.RedQueen:
                            pices[3]++;
                            break;
                    }
                }
            }
            return pices;
        }

        public int Evaluate(bool isRed)
        {
            switch (WhoWins())
            {
                case Winner.Red:
                    return int.MinValue+1;
                case Winner.Black:
                    return int.MaxValue-1;
                case Winner.Draw:
                    return 0;
            }
            return Evaluation.Get(this, isRed);
        }

        public DamkaBoard Clone()
        {
            DamkaBoard newBoard = new DamkaBoard();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    newBoard.board[i, j] = this.board[i, j];
                }
            }
            newBoard.numberOfMovesWithoutSkips = this.numberOfMovesWithoutSkips;
            return newBoard;
        }

        private bool IsSkipRequired(int y, int x)
        {
            Piece piece = GetPieceByIndex(y, x);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece tmpPiece = GetPieceByIndex(j, i);
                    if (piece == Piece.RedPiece || piece == Piece.RedQueen ? tmpPiece == Piece.RedPiece || tmpPiece == Piece.RedQueen : tmpPiece == Piece.BlackPiece || tmpPiece == Piece.BlackQueen)
                    {
                        if (GetAvailableSkipMoves(j, i).Length > 0)
                        {
                            return true;
                        }
                    }                    
                }
            }
            return false;
        }

        public bool IsSkipRequired(bool isRed)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece tmpPiece = GetPieceByIndex(j, i);
                    if (isRed ? tmpPiece == Piece.RedPiece || tmpPiece == Piece.RedQueen : tmpPiece == Piece.BlackPiece || tmpPiece == Piece.BlackQueen)
                    {
                        if (CanSkip(j, i))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public DamkaBoard MakeNullMove()
        {
            DamkaBoard tmp = this.Clone();
            return tmp;
        }

        public DamkaBoard[] GetAllMovesWithIndex(int y, int x)
        {
            if (IsSkipRequired(y, x))
            {
                DamkaBoard[] skipMoves = GetAvailableSkipMoves(y, x);
                if (skipMoves.Length > 0)
                {
                    return skipMoves;
                }
                else
                {
                    return new DamkaBoard[0];
                }
            }
            else
            {
                return GetAvailableNormalMoves(y, x);
            }
        }

        private DamkaBoard[] GetAllMovesWithIndex(int y, int x, bool isSkipRequired)
        {
            if (isSkipRequired)
            {
                DamkaBoard[] skipMoves = GetAvailableSkipMoves(y, x);
                if (skipMoves.Length > 0)
                {
                    return skipMoves;
                }
                else
                {
                    return new DamkaBoard[0];
                }
            }
            else
            {
                return GetAvailableNormalMoves(y, x);
            }
        }

        private int[][] GetAllIndexesFromColor(bool isRed)
        {
            List<int[]> indexes = new List<int[]>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece thisPiece = GetPieceByIndex(i, j);
                    if (thisPiece != Piece.Nothing)
                    {
                        if (isRed? thisPiece == Piece.RedPiece || thisPiece == Piece.RedQueen : thisPiece == Piece.BlackPiece || thisPiece == Piece.BlackQueen)
                        {
                            indexes.Add(new int[2] { i, j });
                        }
                    }
                }
            }
            return indexes.ToArray();
        }

        public DamkaBoard[] GetAllMoves(bool isRed)
        {
            List<DamkaBoard> moves = new List<DamkaBoard>();
            bool isSkipRequired = IsSkipRequired(isRed);
            foreach (int[] index in GetAllIndexesFromColor(isRed))
            {
                moves.AddRange(GetAllMovesWithIndex(index[0], index[1], isSkipRequired));
            }
            return moves.ToArray();
        }

        public void Move(int fromY, int fromX, int toY, int toX)
        {
            this.numberOfMovesWithoutSkips++;
            Piece piece = GetPieceByIndex(fromY, fromX);
            if (piece == Piece.RedPiece && toY == 0)
            {
                this.board[toY, toX / 4] = Utilities.SetByteValue(this.board[toY, toX / 4], (int)Piece.RedQueen, IsFirstHalf(toY, toX));
            }
            else if (piece == Piece.BlackPiece && toY == 7)
            {
                this.board[toY, toX / 4] = Utilities.SetByteValue(this.board[toY, toX / 4], (int)Piece.BlackQueen, IsFirstHalf(toY, toX));
            }
            else
            {
                this.board[toY, toX / 4] = Utilities.SetByteValue(this.board[toY, toX / 4], Utilities.GetByteValue(this.board[fromY, fromX / 4], IsFirstHalf(fromY, fromX)), IsFirstHalf(toY, toX));
            }
            this.board[fromY, fromX / 4] = Utilities.SetByteValue(this.board[fromY, fromX / 4], (int)Piece.Nothing, IsFirstHalf(fromY, fromX));
        }

        public void Skip(int fromY, int fromX, int toY, int toX)
        {
            int avrY = (fromY + toY) / 2;
            int avrX = (fromX + toX) / 2;
            Move(fromY, fromX, toY, toX);
            this.board[avrY, avrX/4] = Utilities.SetByteValue(this.board[avrY, avrX / 4], (int)Piece.Nothing, IsFirstHalf(avrY, avrX));
            this.numberOfMovesWithoutSkips = 0;
        }

        public DamkaBoard CloneSkip(int fromY, int fromX, int toY, int toX)
        {
            DamkaBoard newBoard = this.Clone();
            newBoard.Skip(fromY, fromX, toY, toX);
            return newBoard;
        }

        public DamkaBoard CloneSkip(int fromY, int fromX, int toY, int toX, DamkaBoard board)
        {
            DamkaBoard newBoard = board.Clone();
            newBoard.Skip(fromY, fromX, toY, toX);
            return newBoard;
        }

        public DamkaBoard CloneMove(int fromY, int fromX, int toY, int toX)
        {
            DamkaBoard newBoard = this.Clone();
            newBoard.Move(fromY, fromX, toY, toX);
            return newBoard;
        }

        public Piece[,] GetBoard()
        {
            Piece[,] pieceBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pieceBoard[i, j] = GetPieceByIndex(i, j);
                }
            }
            return pieceBoard;
        }

        public bool CanSkip(int y, int x)
        {
            Piece piece = GetPieceByIndex(y, x);

            int uY;
            int uuY;
            int dY;
            int ddY;
            int lX;
            int llX;
            int rX;
            int rrX;

            if (piece == Piece.BlackPiece)
            {
                dY = y + 1;
                ddY = dY + 1;
                rX = x + 1;
                rrX = rX + 1;
                lX = x - 1;
                llX = lX - 1;

                if (dY < 8)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, lX);
                        if (nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen)
                        {
                            if (ddY < 8 && llX >= 0)
                            {
                                if (GetPieceByIndex(ddY, llX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, rX);
                        if (nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen)
                        {
                            if (ddY < 8 && rrX < 8)
                            {
                                if (GetPieceByIndex(ddY, rrX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            else if (piece == Piece.RedPiece)
            {
                uY = y - 1;
                uuY = uY - 1;
                rX = x + 1;
                rrX = rX + 1;
                lX = x - 1;
                llX = lX - 1;

                if (uY >= 0)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, lX);
                        if (nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && llX >= 0)
                            {
                                if (GetPieceByIndex(uuY, llX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, rX);
                        if (nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && rrX < 8)
                            {
                                if (GetPieceByIndex(uuY, rrX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            else if (piece == Piece.BlackQueen || piece == Piece.RedQueen)
            {
                uY = y - 1;
                uuY = uY - 1;
                dY = y + 1;
                ddY = dY + 1;
                rX = x + 1;
                rrX = rX + 1;
                lX = x - 1;
                llX = lX - 1;

                if (uY >= 0)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, lX);
                        if (piece == Piece.BlackQueen ? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && llX >= 0)
                            {
                                if (GetPieceByIndex(uuY, llX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, rX);
                        if (piece == Piece.BlackQueen ? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && rrX < 8)
                            {
                                if (GetPieceByIndex(uuY, rrX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                if (dY < 8)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, lX);
                        if (piece == Piece.BlackQueen ? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (ddY < 8 && llX >= 0)
                            {
                                if (GetPieceByIndex(ddY, llX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, rX);
                        if (piece == Piece.BlackQueen ? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (ddY < 8 && rrX < 8)
                            {
                                if (GetPieceByIndex(ddY, rrX) == Piece.Nothing)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public DamkaBoard[] GetAvailableSkipMoves(int y, int x, DamkaBoard board = null, List<DamkaBoard> moves = null)
        {
            if (moves == null)
            {
                moves = new List<DamkaBoard>();
                board = this;
            }
            Piece piece = board.GetPieceByIndex(y, x);

            int uY;
            int uuY;
            int dY;
            int ddY;
            int lX;
            int llX;
            int rX;
            int rrX;

            if (piece == Piece.BlackPiece)
            {
                dY = y + 1;
                ddY = dY + 1;
                rX = x + 1;
                rrX = rX + 1;
                lX = x - 1;
                llX = lX - 1;

                if (dY < 8)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, lX, board);
                        if (nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen)
                        {
                            if (ddY < 8 && llX >= 0)
                            {
                                if (GetPieceByIndex(ddY, llX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, ddY, llX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(ddY, llX, tmpBoard, moves);
                                }
                            }
                        }
                    }

                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, rX, board);
                        if (nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen)
                        {
                            if (ddY < 8 && rrX < 8)
                            {
                                if (GetPieceByIndex(ddY, rrX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, ddY, rrX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(ddY, rrX, tmpBoard, moves);
                                }
                            }
                        }
                    }
                }
            }

            else if (piece == Piece.RedPiece)
            {
                uY = y - 1;
                uuY = uY - 1;
                rX = x + 1;
                rrX = rX + 1;
                lX = x - 1;
                llX = lX - 1;

                if (uY >= 0)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, lX, board);
                        if (nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && llX >= 0)
                            {
                                if (GetPieceByIndex(uuY, llX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, uuY, llX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(uuY, llX, tmpBoard, moves);
                                }
                            }
                        }
                    }

                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, rX, board);
                        if (nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && rrX < 8)
                            {
                                if (GetPieceByIndex(uuY, rrX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, uuY, rrX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(uuY, rrX, tmpBoard, moves);
                                }
                            }
                        }
                    }
                }
            }

            else if (piece == Piece.BlackQueen || piece == Piece.RedQueen)
            {
                uY = y - 1;
                uuY = uY - 1;
                dY = y + 1;
                ddY = dY + 1;
                rX = x + 1;
                rrX = rX + 1;
                lX = x - 1;
                llX = lX - 1;

                if (uY >= 0)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, lX, board);
                        if (piece == Piece.BlackQueen? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && llX >= 0)
                            {
                                if (GetPieceByIndex(uuY, llX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, uuY, llX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(uuY, llX, tmpBoard, moves);
                                }
                            }
                        }
                    }
                    
                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(uY, rX, board);
                        if (piece == Piece.BlackQueen ? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (uuY >= 0 && rrX < 8)
                            {
                                if (GetPieceByIndex(uuY, rrX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, uuY, rrX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(uuY, rrX, tmpBoard, moves);
                                }
                            }
                        }
                    }
                }
                if (dY < 8)
                {
                    if (lX >= 0)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, lX, board);
                        if (piece == Piece.BlackQueen ? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (ddY < 8 && llX >= 0)
                            {
                                if (GetPieceByIndex(ddY, llX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, ddY, llX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(ddY, llX, tmpBoard, moves);
                                }
                            }
                        }
                    }

                    if (rX < 8)
                    {
                        Piece nextPiece = GetPieceByIndex(dY, rX, board);
                        if (piece == Piece.BlackQueen ? nextPiece == Piece.RedPiece || nextPiece == Piece.RedQueen : nextPiece == Piece.BlackPiece || nextPiece == Piece.BlackQueen)
                        {
                            if (ddY < 8 && rrX < 8)
                            {
                                if (GetPieceByIndex(ddY, rrX, board) == Piece.Nothing)
                                {
                                    DamkaBoard tmpBoard = CloneSkip(y, x, ddY, rrX, board);
                                    moves.Add(tmpBoard);
                                    GetAvailableSkipMoves(ddY, rrX, tmpBoard, moves);
                                }
                            }
                        }
                    }
                }
            }

            return moves.ToArray();
        }

        public DamkaBoard[] GetAvailableNormalMoves(int y, int x)
        {
            Piece piece = GetPieceByIndex(y, x);
            List<DamkaBoard> moves = new List<DamkaBoard>();
            int nextY;
            int uY;
            int dY;
            int lX;
            int rX;

            if (piece == Piece.BlackPiece)
            {
                nextY = y + 1;
                rX = x + 1;
                lX = x - 1;
                if (nextY < 8)
                {
                    if (lX >= 0)
                    {
                        if (GetPieceByIndex(nextY, lX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, nextY, lX));
                        }
                    }

                    if (rX < 8)
                    {
                        if (GetPieceByIndex(nextY, rX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, nextY, rX));
                        }
                    }
                }
            }

            else if (piece == Piece.RedPiece)
            {
                nextY = y - 1;
                rX = x + 1;
                lX = x - 1;
                if (nextY >= 0)
                {
                    if (lX >= 0)
                    {
                        if (GetPieceByIndex(nextY, lX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, nextY, lX));
                        }
                    }

                    if (rX < 8)
                    {
                        if (GetPieceByIndex(nextY, rX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, nextY, rX));
                        }
                    }
                }
            }

            else if (piece == Piece.BlackQueen || piece == Piece.RedQueen)
            {
                uY = y - 1;
                dY = y + 1;
                rX = x + 1;
                lX = x - 1;

                if (uY >= 0)
                {
                    if (lX >= 0)
                    {
                        if (GetPieceByIndex(uY, lX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, uY, lX));
                        }
                    }

                    if (rX < 8)
                    {
                        if (GetPieceByIndex(uY, rX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, uY, rX));
                        }
                    }
                }
                if (dY < 8)
                {
                    if (lX >= 0)
                    {
                        if (GetPieceByIndex(dY, lX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, dY, lX));
                        }
                    }

                    if (rX < 8)
                    {
                        if (GetPieceByIndex(dY, rX) == Piece.Nothing)
                        {
                            moves.Add(CloneMove(y, x, dY, rX));
                        }
                    }
                }
            }
            
            return moves.ToArray();
        }

        public byte[] Hash()
        {           
            byte[] hash;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                hash = sha1.ComputeHash(ConvertTo1DArray());
            }
            return hash;
        }

        public byte[] ConvertTo1DArray()
        {
            byte[] oneDBoard = new byte[this.board.Length + 1];
            Buffer.BlockCopy(this.board, 0, oneDBoard, 0, this.board.Length);
            oneDBoard[this.board.Length] = (byte)this.numberOfMovesWithoutSkips;
            return oneDBoard;
        }

        public byte[] GetByteArray(bool turn)
        {
            byte[] arr = new byte[this.board.Length+1];
            Buffer.BlockCopy(this.board, 0, arr, 0, this.board.Length);
            arr[this.board.Length] = Convert.ToByte(turn);
            return arr;
        }
            

        public static byte[,] ConvertTo2DArray(byte[] array)
        {
            byte[,] output = new byte[8, 2];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    output[i, j] = array[i * 2 + j];
                }
            }
            return output;
            //TODO: check
        }
    }
    
    enum Winner : byte
    {
        Red,
        Black,
        Draw,
        NoOne
    }

    enum Piece : byte
    {
        Nothing,
        BlackPiece,
        BlackQueen,
        RedPiece,
        RedQueen
    }
}
