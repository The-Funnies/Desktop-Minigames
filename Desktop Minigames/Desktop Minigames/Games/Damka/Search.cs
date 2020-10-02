using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Damka
{

    class Search
    {
        private TranspositionTable tTable;
        private List<Tuple<DamkaBoard, int>> movesWithEvaluations;
        public Search(TranspositionTable tTable)
        {
            this.tTable = tTable;
        }

        private int MinMaxWithoutTT(DamkaBoard board, int depth, bool isRed, int alpha = int.MinValue, int beta = int.MaxValue, bool isFirstMove = true, bool doNull = true)
        {

            if (depth <= 0 || board.WhoWins() != Winner.NoOne)
            {
                if (!board.IsSkipRequired(isRed))
                {
                    return board.Evaluate(isRed);
                }
            }

            if (depth >= 4 && !isFirstMove && doNull)
            {
                int eval = MinMaxWithoutTT(board.MakeNullMove(), depth - 4, !isRed,  alpha, beta-1, isFirstMove, false);
                if (eval >= beta)
                {
                    return beta;
                }
            }

            if (isRed)
            {
                int minEval = int.MaxValue;
                foreach (DamkaBoard tmpBoard in board.GetAllMoves(isRed))
                {                   
                    int eval = MinMaxWithoutTT(tmpBoard, depth - 1, !isRed, alpha, beta, false, doNull);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }

            else
            {
                int maxEval = int.MinValue;
                foreach (DamkaBoard tmpBoard in board.GetAllMoves(isRed))
                {                   
                    int eval = MinMaxWithoutTT(tmpBoard, depth - 1, !isRed, alpha, beta, false, doNull);                      
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return maxEval;
            }
        }

        private int MinMax(DamkaBoard board, int depth, bool isRed, int alpha = int.MinValue, int beta = int.MaxValue, bool isFirstMove = true, bool doNull = true)
        {

            if (depth <= 0 || board.WhoWins() != Winner.NoOne)
            {
                if (!board.IsSkipRequired(isRed))
                {
                    return board.Evaluate(isRed);
                }
            }

            if (isRed)
            {
                int minEval = int.MaxValue;
                foreach (DamkaBoard tmpBoard in board.GetAllMoves(isRed))
                {
                    int[] results = tTable.Check(tmpBoard, isRed, depth);
                    int eval;
                    if (results[0] != -15 && results[0] != -20)
                    {
                        eval = results[1];
                    }
                    else
                    {
                        eval = MinMax(tmpBoard, depth - 1, !isRed, alpha, beta, false, doNull);
                        if (depth > 6)
                        {
                            if (results[0] == -20)
                            {
                                tTable.Update(tmpBoard, isRed, depth, eval);
                            }
                            else
                            {
                                tTable.Add(tmpBoard, isRed, depth, eval);
                            }
                        }
                    }
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }

            else
            {
                int maxEval = int.MinValue;
                foreach (DamkaBoard tmpBoard in board.GetAllMoves(isRed))
                {
                    int[] results = tTable.Check(tmpBoard, isRed, depth);
                    int eval;
                    if (results[0] != -15 && results[0] != -20)
                    {
                        eval = results[1];
                    }
                    else
                    {
                        eval = MinMax(tmpBoard, depth - 1, !isRed, alpha, beta, false, doNull);
                        if (depth > 6)
                        {
                            if (results[0] == -20)
                            {
                                tTable.Update(tmpBoard, isRed, depth, eval);
                            }
                            else
                            {
                                tTable.Add(tmpBoard, isRed, depth, eval);
                            }
                        }
                    }
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return maxEval;
            }
        }
        private DamkaBoard[][] Split(DamkaBoard[] moves, int numberOfThreads)
        {
            DamkaBoard[][] arr = new DamkaBoard[numberOfThreads][];
            int length = moves.Length / numberOfThreads;
            for (int i = 0; i < numberOfThreads; i++)
            {
                if (i == numberOfThreads - 1)
                {
                    arr[i] = moves.Skip(length * i).ToArray();
                }
                else
                {
                    arr[i] = moves.Skip(length * i).Take(length).ToArray();

                }
            }
            return arr;
        }
        public DamkaBoard GetBestMove(int depth, bool isRed, DamkaBoard board)
        {
            DamkaBoard[] moves = board.GetAllMoves(isRed);
            if (moves.Length == 1)
            {
                return moves[0];
            }
            this.movesWithEvaluations = new List<Tuple<DamkaBoard, int>>(moves.Length);
            DamkaBoard[] shuffledMoves = moves.OrderBy(a => Guid.NewGuid()).ToArray();
            foreach (DamkaBoard[] splittedMoves in Split(shuffledMoves, Environment.ProcessorCount/2))
            {
                GetEvalNewThread(splittedMoves, depth, !isRed);
            }
            while (true)
            {
                if (movesWithEvaluations.Count == moves.Length)
                {
                    if (movesWithEvaluations.Count == 0)
                    {
                        return null;
                    }
                    if (isRed)
                    {
                        movesWithEvaluations.Sort((x, y) => x.Item2.CompareTo(y.Item2));
                    }
                    else
                    {
                        movesWithEvaluations.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                    }
                    return movesWithEvaluations.First().Item1;
                }
            }
        }

        private void GetEvalNewThread(DamkaBoard[] moves, int depth, bool isRed)
        {
            Thread Evaluate = new Thread(() =>
            {
                foreach (DamkaBoard move in moves)
                {
                    int moveEval = MinMax(move, depth, isRed);
                    this.movesWithEvaluations.Add(new Tuple<DamkaBoard, int>(move, moveEval));
                }              
            });
            Evaluate.Start();
        }     

        public int GetEvaluationMultiThread(int depth, bool isRed, DamkaBoard board)
        {
            DamkaBoard[] moves = board.GetAllMoves(isRed);
            this.movesWithEvaluations = new List<Tuple<DamkaBoard, int>>(moves.Length);
            DamkaBoard[] shuffledMoves = moves.OrderBy(a => Guid.NewGuid()).ToArray();
            foreach (DamkaBoard[] splittedMoves in Split(shuffledMoves, Environment.ProcessorCount))
            {
                GetEvalNewThread(splittedMoves, depth, !isRed);
            }
            while (true)
            {
                if (movesWithEvaluations.Count == moves.Length)
                {
                    if (movesWithEvaluations.Count == 0)
                    {
                        if (isRed)
                        {
                            return int.MaxValue - 1;
                        }
                        else
                        {
                            return int.MinValue + 1;
                        }
                    }
                    if (isRed)
                    {
                        movesWithEvaluations.Sort((x, y) => x.Item2.CompareTo(y.Item2));
                    }
                    else
                    {
                        movesWithEvaluations.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                    }
                    return movesWithEvaluations.First().Item2;
                }
            }
        }

        public int GetEvaluation(int depth, bool isRed, DamkaBoard board)
        {
            DamkaBoard[] moves = board.GetAllMoves(isRed);          //TODO
            this.movesWithEvaluations = new List<Tuple<DamkaBoard, int>>(moves.Length);
            DamkaBoard[] shuffledMoves = moves.OrderBy(a => Guid.NewGuid()).ToArray();
            int bestEval;
            if (isRed)
            {
                bestEval = int.MaxValue;
                foreach (DamkaBoard move in shuffledMoves)
                {
                    int moveEval = MinMax(move, depth, !isRed);
                    if (moveEval < bestEval)
                    {
                        bestEval = moveEval;
                    }
                }
            }
            else
            {
                bestEval = int.MinValue;
                foreach (DamkaBoard move in shuffledMoves)
                {
                    int moveEval = MinMax(move, depth, !isRed);
                    if (moveEval > bestEval)
                    {
                        bestEval = moveEval;
                    }
                }
            }
            return bestEval;
        }
    }
}
