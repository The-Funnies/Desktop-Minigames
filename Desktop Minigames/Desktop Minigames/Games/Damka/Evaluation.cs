using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damka
{
    class Evaluation
    {
        private static int[,] edge = new int[14, 2] { {7,0}, { 7, 2 }, { 7, 4 }, { 7, 6 }
            ,{ 6, 7 }, { 4, 7 }, { 2, 7 }, { 0, 7 }, { 0, 5 },
            { 0, 3 }, { 0, 1 }, { 1, 0 }, { 3, 0 }, { 5, 0 },};

        private static int[,] center = new int[8, 2] { { 5, 2 }, { 5, 4 }, { 4, 3 }, { 4, 5 },
            { 3, 2 }, { 4, 4 }, { 2, 3 }, { 2, 5 } };

        private static int[,] safeEdge = new int[4, 2] { { 7, 6 }, { 6, 7 }, { 1, 0 }, { 0, 1 } };

        private const int turn = 2;                     //color to move gets +turn
        private const int brv = 3;                      //multiplier for back rank
        private const int qcv = 5;                      //multiplier for queens in center
        private const int pcv = 1;                      //multiplier for piece in center
        private const int pev = 1;                      //multiplier for piece on edge
        private const int qev = 5;                      //multiplier for queens on edge
        private const int cramp = 5;
        private const int opening = -2;                 // multiplier for opening
        private const int midgame = -1;                 // multiplier for midgame
        private const int endgame = 2;                  // multiplier for endgame
        private const int intactdoublecorner = 3;

        public static int Get(DamkaBoard board, bool isRed)
        {
            int evaluation;

            int nbpc = 0;           //number black piece center
            int nbqc = 0;           //number black queen center
            int nrpc = 0;           //number red piece center
            int nrqc = 0;           //number red queen center
            int nbpe = 0;           //number black piece edge
            int nbqe = 0;           //number black queen edge
            int nrpe = 0;           //number red piece edge
            int nrqe = 0;           //number red queen edge

            int code = 0;

            int tempo = 0;

            int backrank;

            int stonesinsystem = 0;

            int[] numberOfPices = board.GetPices();

            int nbp = numberOfPices[0];           //number bleck piece
            int nbq = numberOfPices[1];           //number bleck queen
            int nrp = numberOfPices[2];           //number red piece
            int nrq = numberOfPices[3];           //number red queen

            int blackValue = 100 * nbp + 160 * nbq;
            int redValue = 100 * nrp + 160 * nrq;

            evaluation = blackValue - redValue;

            evaluation += (250 * (blackValue - redValue)) / (blackValue + redValue);       //eat if you have more pieces

            int np = nbp + nrp; //total number of pieces
            int nq = nbq + nrq; //total number of queens

            if (!isRed)
            {
                evaluation += turn;
            }

            else
            {
                evaluation -= turn;
            }


            if (board.GetPieceByIndex(3, 0) == Piece.RedPiece && board.GetPieceByIndex(2, 1) == Piece.BlackPiece)
            {
                evaluation -= cramp;
            }

            if (board.GetPieceByIndex(4, 7) == Piece.BlackPiece && board.GetPieceByIndex(5, 6) == Piece.RedPiece)
            {
                evaluation += cramp;
            }

            code = 0;
            if (board.GetPieceByIndex(7, 0) == Piece.RedPiece)
                code++;
            if (board.GetPieceByIndex(7, 2) == Piece.RedPiece)
                code += 2;
            if (board.GetPieceByIndex(7, 4) == Piece.RedPiece)
                code += 4;
            if (board.GetPieceByIndex(7, 6) == Piece.RedPiece)
                code += 8;

            switch (code)
            {
                case 0:
                    code = 0;
                    break;

                case 1:
                    code = -1;
                    break;

                case 2:
                    code = 1;
                    break;

                case 3:
                    code = 0;
                    break;

                case 4:
                    code = 1;
                    break;

                case 5:
                    code = 1;
                    break;

                case 6:
                    code = 2;
                    break;

                case 7:
                    code = 1;
                    break;

                case 8:
                    code = 1;
                    break;

                case 9:
                    code = 0;
                    break;

                case 10:
                    code = 7;
                    break;

                case 11:
                    code = 4;
                    break;

                case 12:
                    code = 2;
                    break;

                case 13:
                    code = 2;
                    break;

                case 14:
                    code = 9;
                    break;

                case 15:
                    code = 8;
                    break;
            }

            backrank = -code;

            code = 0;
            if (board.GetPieceByIndex(0, 1) == Piece.BlackPiece)
                code += 8;
            if (board.GetPieceByIndex(0, 3) == Piece.BlackPiece)
                code += 4;
            if (board.GetPieceByIndex(0, 5) == Piece.BlackPiece)
                code += 2;
            if (board.GetPieceByIndex(0, 7) == Piece.BlackPiece)
                code++;
            switch (code)
            {
                case 0:
                    code = 0;
                    break;

                case 1:
                    code = -1;
                    break;

                case 2:
                    code = 1;
                    break;

                case 3:
                    code = 0;
                    break;

                case 4:
                    code = 1;
                    break;

                case 5:
                    code = 1;
                    break;

                case 6:
                    code = 2;
                    break;

                case 7:
                    code = 1;
                    break;

                case 8:
                    code = 1;
                    break;

                case 9:
                    code = 0;
                    break;

                case 10:
                    code = 7;
                    break;

                case 11:
                    code = 4;
                    break;

                case 12:
                    code = 2;
                    break;

                case 13:
                    code = 2;
                    break;

                case 14:
                    code = 9;
                    break;

                case 15:
                    code = 8;
                    break;
            }

            backrank += code;

            evaluation += brv * backrank;


            if (board.GetPieceByIndex(7, 6) == Piece.RedPiece)
            {
                if (board.GetPieceByIndex(6, 5) == Piece.RedPiece || board.GetPieceByIndex(6, 7) == Piece.RedPiece)
                {
                    evaluation -= intactdoublecorner;
                }
            }

            if (board.GetPieceByIndex(0, 1) == Piece.BlackPiece)
            {
                if (board.GetPieceByIndex(1, 0) == Piece.BlackPiece || board.GetPieceByIndex(1, 2) == Piece.BlackPiece)
                {
                    evaluation += intactdoublecorner;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                switch (board.GetPieceByIndex(center[i,0], center[i, 1]))
                {
                    case Piece.Nothing:
                        break;
                    case Piece.BlackPiece:
                        nbpc++;
                        break;
                    case Piece.BlackQueen:
                        nbqc++;
                        break;
                    case Piece.RedPiece:
                        nrpc++;
                        break;
                    case Piece.RedQueen:
                        nrqc++;
                        break;
                }
            }

            evaluation += (nbpc - nrpc) * pcv;
            evaluation += (nbqc - nrqc) * qcv;

            for (int i = 0; i < 14; i++)
            {
                switch (board.GetPieceByIndex(edge[i, 0], edge[i, 1]))
                {
                    case Piece.Nothing:
                        break;
                    case Piece.BlackPiece:
                        nbpe++;
                        break;
                    case Piece.BlackQueen:
                        nbqe++;
                        break;
                    case Piece.RedPiece:
                        nrpe++;
                        break;
                    case Piece.RedQueen:
                        nrqe++;
                        break;
                }
            }
            evaluation -= (nbpe - nrpe) * pev;
            evaluation -= (nbqe - nrqe) * qev;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    switch (board.GetPieceByIndex(i, j))
                    {
                        case Piece.Nothing:
                            break;
                        case Piece.BlackPiece:
                            tempo += i;
                            break;
                        case Piece.RedPiece:
                            tempo -= 7 - i;
                            break;
                    }
                }
            }
            if (np >= 16)
            {
                evaluation += opening * tempo;
            }
            if ((np <= 15) && (np >= 12))
            {
                evaluation += midgame * tempo;
            }
            if (np < 9)
            {
                evaluation += endgame * tempo;
            }

            for (int i = 0; i < 4; i++)
            {
                if (nbq + nbp > nrq + nrp && nrq < 3)
                {
                    if (board.GetPieceByIndex(safeEdge[i,0], safeEdge[i, 1]) == Piece.RedQueen)
                    {
                        evaluation -= 15;
                    }
                }

                if (nbq + nbp < nrq + nrp && nbq < 3)
                {
                    if (board.GetPieceByIndex(safeEdge[i, 0], safeEdge[i, 1]) == Piece.BlackQueen)
                    {
                        evaluation += 15;
                    }
                }
            }

            if (nrp + nrq -nbp - nbq == 0)
            {
                if (!isRed)
                {
                    for (int i = 1; i < 8; i+=2)
                    {
                        for (int j = 0; j < 8; j+=2)
                        {
                            if (board.GetPieceByIndex(i, j) != Piece.Nothing)
                            {
                                stonesinsystem++;
                            }
                        }
                    }

                    if (stonesinsystem % 2 == 0)
                    {
                        if (np + nq <= 12)
                        {
                            evaluation++;
                        }
                        if (np + nq <= 10)
                        {
                            evaluation++;
                        }
                        if (np + nq <= 8)
                        {
                            evaluation+=2;
                        }
                        if (np + nq <= 6)
                        {
                            evaluation+=2;
                        }
                    }
                    else
                    {
                        if (np + nq <= 12)
                        {
                            evaluation--;
                        }
                        if (np + nq <= 10)
                        {
                            evaluation--;
                        }
                        if (np + nq <= 8)
                        {
                            evaluation -= 2;
                        }
                        if (np + nq <= 6)
                        {
                            evaluation -= 2;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i += 2)
                    {
                        for (int j = 1; j < 8; j += 2)
                        {
                            if (board.GetPieceByIndex(i, j) != Piece.Nothing)
                            {
                                stonesinsystem++;
                            }
                        }
                    }

                    if (stonesinsystem % 2 == 0)
                    {
                        if (np + nq <= 12)
                        {
                            evaluation++;
                        }
                        if (np + nq <= 10)
                        {
                            evaluation++;
                        }
                        if (np + nq <= 8)
                        {
                            evaluation += 2;
                        }
                        if (np + nq <= 6)
                        {
                            evaluation += 2;
                        }
                    }
                    else
                    {
                        if (np + nq <= 12)
                        {
                            evaluation--;
                        }
                        if (np + nq <= 10)
                        {
                            evaluation--;
                        }
                        if (np + nq <= 8)
                        {
                            evaluation -= 2;
                        }
                        if (np + nq <= 6)
                        {
                            evaluation -= 2;
                        }
                    }
                }
            }

            return evaluation;
        }
    }
}
/*    (white)
   				 37  38  39  40
              32  33  34  35
                28  29  30  31
              23  24  25  26
                19  20  21  22
              14  15  16  17
                10  11  12  13
               5   6   7   8
         (black)   */
