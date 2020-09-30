using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Minigames
{
    [Serializable]
    public class SnakeGame
    {
        private int height, width;
        private Block[,] board;
        private LinkedList<Tuple<int, int>> snake;
        private int foodX, foodY;
        private int foodDuration;

        public bool isEnd;

        public SnakeGame(int height, int width)
        {
            this.height = height;
            this.width = width;
            board = new Block[height, width];

            Tuple<int, int> snakeLoc = new Tuple<int, int>(Minigames.random.Next(1, width - 1), Minigames.random.Next(1, height - 1));
            Tuple<int, int> leftSnake = new Tuple<int, int>(snakeLoc.Item1 - 1, snakeLoc.Item2);
            board[snakeLoc.Item2, snakeLoc.Item1] = Block.Snake;
            board[leftSnake.Item2, leftSnake.Item1] = Block.Snake;
            snake = new LinkedList<Tuple<int, int>>();
            snake.AddLast(leftSnake);
            snake.AddLast(snakeLoc);

            GenFood();

            isEnd = false;

            foodDuration = 0;
        }

        public Block[,] GetBoard()
        {
            return board;
        }

        public Tuple<int, int>[] GetDiff()
        {
            Tuple<int, int> tail = snake.First.Value;
            return new Tuple<int, int>[] { snake.Last.Value,
            new Tuple<int, int>(tail.Item1 - 1, tail.Item2),
            new Tuple<int, int>(tail.Item1 + 1, tail.Item2),
            new Tuple<int, int>(tail.Item1, tail.Item2 - 1),
            new Tuple<int, int>(tail.Item1, tail.Item2 + 1),
            new Tuple<int, int>(foodX, foodY)};
        }

        public int GetScore()
        {
            return snake.Count;
        }

        public void Tick(float[] input)
        {
            LinkedListNode<Tuple<int, int>> last = snake.Last;

            int dx = last.Value.Item1 - last.Previous.Value.Item1;
            int dy = last.Value.Item2 - last.Previous.Value.Item2;

            int neuronNo = 0;
            float highest = input[0];
            for (int i = 1; i < input.Length; i++)
                if (input[i] > highest)
                {
                    highest = input[i];
                    neuronNo = i;
                }

            //If the snake didn't turn, don't change dx and dy
            if (neuronNo == 1)
            {
                //If the snake turns left
                int temp = -dx;
                dx = dy;
                dy = temp;
            }
            else if (neuronNo == 2)
            {
                //If the snake turns right
                int temp = -dy;
                dy = dx;
                dx = temp;
            }

            int newX = last.Value.Item1 + dx;
            int newY = last.Value.Item2 + dy;
            if (newX >= width || newY >= height || newX < 0 || newY < 0)
            {
                isEnd = true;
                return;
            }
            Block block = board[newY, newX];
            if (block == Block.Snake)
            {
                isEnd = true;
                return;
            }

            board[newY, newX] = Block.Snake;
            snake.AddLast(new Tuple<int, int>(newX, newY));
            if (block == Block.Food)
            {
                foodDuration += 2;
                GenFood();
            }
            else
            {
                if (foodDuration <= 0)
                {
                    Tuple<int, int> tail = snake.First.Value;
                    board[tail.Item2, tail.Item1] = Block.Blank;
                    snake.RemoveFirst();
                }
                else
                {
                    foodDuration--;
                }
            }
        }

        public void GenFood()
        {
            foodX = Minigames.random.Next(0, width);
            foodY = Minigames.random.Next(0, height);

            if (board[foodY, foodX] != Block.Blank)
            {
                GenFood();
                return;
            }

            board[foodY, foodX] = Block.Food;
        }

        public void Reset()
        {
            board = new Block[height, width];

            Tuple<int, int> snakeLoc = new Tuple<int, int>(Minigames.random.Next(1, width - 1), Minigames.random.Next(1, height - 1));
            Tuple<int, int> leftSnake = new Tuple<int, int>(snakeLoc.Item1 - 1, snakeLoc.Item2);
            board[snakeLoc.Item2, snakeLoc.Item1] = Block.Snake;
            board[leftSnake.Item2, leftSnake.Item1] = Block.Snake;
            snake = new LinkedList<Tuple<int, int>>();
            snake.AddLast(leftSnake);
            snake.AddLast(snakeLoc);

            GenFood();

            isEnd = false;

            foodDuration = 0;
        }

        public void ProcessKey(byte input)
        {
            int dy = snake.Last.Value.Item2 - snake.Last.Previous.Value.Item2;
            int dx = snake.Last.Value.Item1 - snake.Last.Previous.Value.Item1;
            float[] ine = new float[3];

            if (dy == -1)
            {
                switch (input)
                {
                    case 1:
                        ine[1] = 1;
                        break;
                    case 2:
                        ine[2] = 1;
                        break;
                }
            }
            else if (dy == 1)
            {
                switch (input)
                {
                    case 1:
                        ine[2] = 1;
                        break;
                    case 2:
                        ine[1] = 1;
                        break;
                }
            }
            else
            {
                if (dx == 1)
                {
                    switch (input)
                    {
                        case 0:
                            ine[1] = 1;
                            break;
                        case 3:
                            ine[2] = 1;
                            break;
                    }
                }
                else
                {
                    switch (input)
                    {
                        case 0:
                            ine[2] = 1;
                            break;
                        case 3:
                            ine[1] = 1;
                            break;
                    }
                }    
            }

            Tick(ine);
        }
    }

    public enum Block
    {
        Blank, Snake, Food
    }
}
