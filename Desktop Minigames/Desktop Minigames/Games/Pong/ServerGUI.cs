using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class ServerGUI : Form
    {
        private Server server = new Server();
        private string name;
        public ServerOption option = ServerOption.freeplay;

        private int ballTop;
        private int ballLeft;

        private int firstPlayerRacketTop;
        private int firstPlayerRacketLeft;

        private int secondPlayerRacketLeft;
        private int secondPlayerRacketTop;

        private int playgroundBotom;
        private int playgroundTop;
        private int playgroundLeft;
        private int playgroundRight;

        private int ballHeight;
        private int ballWidth;

        private int racketHeight;
        private int racketWidth;

        private int ballXSpeed;
        private int ballYSpeed;

        private int ballBottom;
        private int ballRight;
        private int firstPlayerRacketBottom;
        private int firstPlayerRacketRight;
        private int secondPlayerRacketBottom;
        private int secondPlayerRacketRight;

        private int winner;
       
        public ServerGUI(string name)
        {
            ServerOptions options = new ServerOptions(this);
            options.ShowDialog();
            InitializeComponent();
            try
            {
                ipBox.Text = new WebClient().DownloadString("http://icanhazip.com");
            }
            catch (Exception)
            {
                MessageBox.Show("No internet connection");
                Application.Exit();
            }
            this.Text = $"{name}'s Server";
            this.name = name;            
        }

        private void SetLocations()
        {
            this.ballTop = Constants.BALLTOP;
            this.ballLeft = Constants.BALLLEFT;

            this.firstPlayerRacketTop = Constants.FIRSTPLAYERRACKETTOP;
            this.firstPlayerRacketLeft = Constants.FIRSTPLAYERRACKETLEFT;

            this.secondPlayerRacketLeft = Constants.SECONDPLAYERRACKETLEFT;
            this.secondPlayerRacketTop = Constants.SECONDPLAYERRACKETTOP;

            this.playgroundBotom = Constants.PLAYGROUNDBOTTOM;
            this.playgroundTop = Constants.PLAYGROUNDTOP;
            this.playgroundLeft = Constants.PLAYGROUNDLEFT;
            this.playgroundRight = Constants.PLAYGROUNDRIGHT;

            this.ballHeight = Constants.BALLHEIGHT;
            this.ballWidth = Constants.BALLWIDTH;

            this.racketHeight = Constants.RACKET_HEIGHT;
            this.racketWidth = Constants.RACKET_WIDTH;

            this.firstPlayerRacketRight = Constants.FIRSTPLAYERRACKETRIGHT;
            this.secondPlayerRacketRight = Constants.SECONDPLAYERRACKETRIGHT;

            Random rand = new Random();
            if (rand.Next(0, 2) == 1)
            {
                this.ballYSpeed = rand.Next(1, 6);
            }
            else
            {
                this.ballYSpeed = rand.Next(-5, 0);
            }

            if (rand.Next(0, 2) == 1)
            {
                this.ballXSpeed = 5;
            }
            else
            {
                this.ballXSpeed = -5;
            }
            this.winner = 0;
        }
        private Client HandlePlayer()
        {
            Client player = this.server.WaitForPlayer();
            Tuple<string, int[]> results = player.FirstRecive();
            string name = results.Item1;
            int[] resolution = results.Item2;
            if (!(resolution[0] == 1920 && resolution[1] == 1080))
            {
                AddText(ConsoleTextBox, $"{name} connected but his resolution isn't good so he was kicked by the server");
                player.SendServerFirstAnswer(false, this.name, this.option);
                return HandlePlayer();
            }
            player.name = name;
            player.SendServerFirstAnswer(true, this.name, this.option);
            return player;
        }

        private Client[] WaitForPlayers(int numberOfPlayers)
        {
            this.server.StartLitsening();
            AddText(ConsoleTextBox, $"Waiting for {numberOfPlayers} players...");

            Client[] players = new Client[numberOfPlayers];

            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i] = HandlePlayer();
                Client[] connectedPlayers = GetConnectedPlayers(players);
                for (int k = 0; k < connectedPlayers.Length; k++)
                {
                    players[k] = connectedPlayers[k];
                }
                i = Math.Max(connectedPlayers.Length-1, 0);
                players[i].lineIndex = i;
                AddText(ConsoleTextBox, players[i].name + " connected");
                AddText(Players, players[i].name);
            }
            server.StopLitsening();
            return players;
        }

        private Client[] GetConnectedPlayers(Client[] players)
        {
            List<Client> playersList = players.Where(x => x != null).ToList();
            List<Client> listToEdit = new List<Client>(playersList);

            foreach (Client player in playersList)
            {
                if (!player.IsConnected())
                {
                    AddText(ConsoleTextBox, player.name + " disconnected");
                    listToEdit.RemoveAll(x => x.Equals(player));
                    List<string> lines = this.Players.Lines.ToList();
                    lines.RemoveAt(player.lineIndex);
                    Players.Invoke((MethodInvoker)delegate ()
                    {
                        this.Players.Lines = lines.ToArray();
                    });
                    int counter = 0;
                    foreach (Client tmpPlayer in listToEdit)
                    {
                        tmpPlayer.lineIndex = counter;
                        counter++;
                    }
                }
            }

            return listToEdit.ToArray();
        }


        private void ServerGUI_Load(object sender, EventArgs e)
        {

        }


        private void Server()
        {
            while (true)
            {
                this.ballLeft += this.ballXSpeed;
                this.ballTop += this.ballYSpeed;

                this.ballBottom = this.ballTop + this.ballHeight;
                this.ballRight = this.ballLeft + this.ballWidth;

                this.firstPlayerRacketBottom = this.firstPlayerRacketTop + this.racketHeight;

                this.secondPlayerRacketBottom = this.secondPlayerRacketTop + this.racketHeight;

                if ((this.ballRight >= this.firstPlayerRacketLeft && ballTop <= firstPlayerRacketBottom && ballBottom >= firstPlayerRacketTop)
                    || (this.ballLeft <= this.secondPlayerRacketRight && ballTop <= secondPlayerRacketBottom && ballBottom >= secondPlayerRacketTop))
                {
                    this.ballXSpeed = AddAbsolute(this.ballXSpeed, 1);
                    this.ballYSpeed = AddAbsolute(this.ballYSpeed, 1);
                    this.ballXSpeed = -this.ballXSpeed;
                }
                else if (this.ballLeft < secondPlayerRacketRight - 10)
                {
                    this.winner = 1;
                    break;
                }
                else if (this.ballRight > firstPlayerRacketLeft + 10)
                {
                    this.winner = 2;
                    break;
                }

                if (this.ballTop <= this.playgroundTop)
                {
                    this.ballYSpeed = -this.ballYSpeed;
                }
                if (this.ballBottom >= this.playgroundBotom)
                {
                    ballYSpeed = -this.ballYSpeed;
                }
                
                Thread.Sleep(Constants.SLEEPTIME);
            }
        }

        private int AddAbsolute(int num, int numToAdd)
        {
            if (num < 0)
            {
                return num - numToAdd;
            }
            return num + numToAdd;
        }

        private void ValidateConnection(Client firstPlayer, Client secondPlayer)
        {
            while (true)
            {
                if (!firstPlayer.IsConnected())
                {
                    this.winner = 2;
                    break;
                }
                if (!secondPlayer.IsConnected())
                {
                    this.winner = 1;
                    break;
                }
            }
        }
        private void LitsenToFirstPlayer(Client player)
        {
            while (true)
            {
                try
                {
                    player.SendServer(false, WhoWins.noOne, this.ballLeft, this.ballTop, this.secondPlayerRacketTop);
                    this.firstPlayerRacketTop = player.ReciveRacket();
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private void LitsenToSecondPlayer(Client player)
        {
            while (true)
            {
                try
                {
                    player.SendServer(false, WhoWins.noOne, this.playgroundRight - this.ballLeft - this.ballWidth, this.ballTop, this.firstPlayerRacketTop);
                    this.secondPlayerRacketTop = player.ReciveRacket();
                }
                catch (Exception)
                {
                    break;
                }               
            }
        }

        private Client playGame(Client[] players)
        {
            SetLocations();
            Client firstPlayer = players[0];
            Client secondPlayer = players[1];
            Thread.Sleep(5000);
            firstPlayer.SendName(secondPlayer.name);
            secondPlayer.SendName(firstPlayer.name);
            Thread.Sleep(2000);
            for (int i = 3; i > 0; i--)
            {
                firstPlayer.SendInt(i);
                secondPlayer.SendInt(i);
                Thread.Sleep(1000);
            }
            Thread server = new Thread(() =>
            {
                Server();
            });
            Thread firstPlayerLitsener = new Thread(() =>
            {
                LitsenToFirstPlayer(firstPlayer);
            });
            Thread secondPlayerLitsener = new Thread(() =>
            {
                LitsenToSecondPlayer(secondPlayer);
            });
            Thread validateConnection = new Thread(() =>
            {
                ValidateConnection(firstPlayer, secondPlayer);
            });

            server.Start();
            firstPlayerLitsener.Start();
            secondPlayerLitsener.Start();
            validateConnection.Start();

            while (true)
            {
                if (this.winner != 0)
                {
                    server.Abort();
                    firstPlayerLitsener.Abort();
                    secondPlayerLitsener.Abort();
                    validateConnection.Abort();

                    if (winner == 1)
                    {
                        if (!secondPlayer.IsConnected())
                        {
                            firstPlayer.SendServer(true, WhoWins.you, this.ballLeft, this.ballTop, this.secondPlayerRacketTop);
                        }
                        else
                        {
                            firstPlayer.SendServer(false, WhoWins.you, this.ballLeft, this.ballTop, this.secondPlayerRacketTop);
                            secondPlayer.SendServer(false, WhoWins.enemy, this.playgroundRight - this.ballLeft - this.ballWidth, this.ballTop, this.firstPlayerRacketTop);
                        }
                        return firstPlayer;
                    }

                    else if (winner == 2)
                    {
                        if (!firstPlayer.IsConnected())
                        {
                            secondPlayer.SendServer(true, WhoWins.you, this.playgroundRight - this.ballLeft - this.ballWidth, this.ballTop, this.firstPlayerRacketTop);
                        }
                        else
                        {
                            firstPlayer.SendServer(false, WhoWins.enemy, this.ballLeft, this.ballTop, this.secondPlayerRacketTop);
                            secondPlayer.SendServer(false, WhoWins.you, this.playgroundRight - this.ballLeft - this.ballWidth, this.ballTop, this.firstPlayerRacketTop);
                        }                        
                        return secondPlayer;
                    }
                }
            }
        }

        private void RunGame()
        {
            Thread FreePlay = new Thread(() =>
            {
                Client[] players = WaitForPlayers(2);
                Client gameWinner;
                while (true)
                {
                    gameWinner = playGame(players);
                    if (!players[0].IsConnected())
                    {
                        AddText(ConsoleTextBox, $"{players[0].name} disconnected so {players[1].name} won");
                        break;
                    }
                    else if (!players[1].IsConnected())
                    {
                        AddText(ConsoleTextBox, $"{players[1].name} disconnected so {players[0].name} won");
                        break;
                    }
                    AddText(ConsoleTextBox, $"{gameWinner.name} won");
                }
            });

            Thread TwoPlayers = new Thread(() =>
            {
                Client[] players = WaitForPlayers(2);
            });

            Thread FourPlayers = new Thread(() =>
            {
                Client[] players = WaitForPlayers(4);
            });

            Thread EightPlayers = new Thread(() =>
            {
                Client[] players = WaitForPlayers(8);
            });


            if (this.option == ServerOption.freeplay)
            {
                FreePlay.Start();
            }
            else if (this.option == ServerOption.twoP)
            {
                TwoPlayers.Start();
            }
            else if (this.option == ServerOption.fourP)
            {
                FourPlayers.Start();
            }
            else if (this.option == ServerOption.eightP)
            {
                EightPlayers.Start();
            }
        }

        private void ServerGUI_Shown(object sender, EventArgs e)
        {
            RunGame();
        }

        private void ServerGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void AddText(TextBox box, string text)
        {
            box.Invoke((MethodInvoker)delegate ()
            {
                box.Text += text + Environment.NewLine;
            });
        }

        private void ClearTextBox(TextBox box)
        {
            box.Invoke((MethodInvoker)delegate ()
            {
                box.Text = "";
            });
        }
    }
}
