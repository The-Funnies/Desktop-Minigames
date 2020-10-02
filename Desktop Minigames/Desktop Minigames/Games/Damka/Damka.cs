using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Damka
{
    public partial class Damka : Form
    {
        private TranspositionTable tTable;
        private Search search;
        private Board board;
        private bool isThinking = false;
        private Thread doTurn;
        private Thread evaluationThread;
        private int depth;
        private string directoryPath;
        private string boardsDirectoryPath;
        private string transpositionTablePath;
        private bool isBlack = false;
        public Damka()
        {
            InitializeComponent();
            this.board = new Board(this);
            this.Controls.Add(board);
            this.Size = new Size((int)(this.board.Width * 1.02), (int)(this.board.Height * 1.07875));
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.MaximizeBox = false;
            this.medium36SecondsToolStripMenuItem.Checked = true;
            this.depth = 6;
            //file
            string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Damka";
            string transpositionTablePath = directoryPath + "\\TTable";
            string boardsDirectoryPath = directoryPath + "\\Boards";
            if (!Directory.Exists(directoryPath))
            {
                DirectoryInfo directory = Directory.CreateDirectory(directoryPath);
                directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            if (!Directory.Exists(boardsDirectoryPath))
            {
                Directory.CreateDirectory(boardsDirectoryPath);
            }
            if (!File.Exists(transpositionTablePath))
            {
                var file = File.Create(transpositionTablePath);
                file.Close();
            }
            this.transpositionTablePath = transpositionTablePath;
            this.boardsDirectoryPath = boardsDirectoryPath;
            this.directoryPath = directoryPath;

            this.tTable = new TranspositionTable(transpositionTablePath);
            this.search = new Search(tTable);
            //this.board.AppendFromDamkaBoard(search.GetBestMove(7, false, board.GetDamkaBoard()));
            SetEvaluation();
        }

        private void BotGames(int firstDepthm, int secondDepth)
        {
            while (true)
            {
                ShowMove(this.search.GetBestMove(firstDepthm, true, this.board.GetDamkaBoard()));
                switch (this.board.GetDamkaBoard().WhoWins())
                {
                    case Winner.Black:
                        MessageBox.Show("Black won!");
                        this.board.Reset();
                        break;

                    case Winner.Red:
                        MessageBox.Show("Red won!");
                        this.board.Reset();
                        break;

                    case Winner.Draw:
                        MessageBox.Show("Tie");
                        this.board.Reset();
                        break;
                }
                ShowMove(this.search.GetBestMove(secondDepth, false, this.board.GetDamkaBoard()));
                switch (this.board.GetDamkaBoard().WhoWins())
                {
                    case Winner.Black:
                        MessageBox.Show("Black won!");
                        this.board.Reset();
                        break;

                    case Winner.Red:
                        MessageBox.Show("Red won!");
                        this.board.Reset();
                        break;

                    case Winner.Draw:
                        MessageBox.Show("Tie");
                        this.board.Reset();
                        break;
                }
            }
        }
       

        public void Button_Click(object sender, EventArgs e)
        {
            if (this.isThinking)
            {
                return;
            }
            this.doTurn = new Thread(() =>
            {
                this.isThinking = true;
                Cell clickedCell = sender as Cell;
                if (clickedCell.GetBoard() != null)
                {
                    if (this.evaluationThread.IsAlive)
                    {
                        this.evaluationThread.Abort();
                    }
                    this.evaluation.Text = "";
                    DamkaBoard tmpBoard = clickedCell.GetBoard();
                    this.board.ClearMoves();
                    this.board.AppendFromDamkaBoard(tmpBoard);
                    Application.DoEvents();
                    DamkaBoard bestMove = this.search.GetBestMove(this.depth, isBlack, this.board.GetDamkaBoard());
                    if (bestMove == null)
                    {
                        MessageBox.Show("Red won!");
                        this.board.Reset();
                        this.isThinking = false;
                        return;
                    }
                    this.board.AppendFromDamkaBoard(bestMove);
                    switch (bestMove.WhoWins())
                    {
                        case Winner.Black:
                            MessageBox.Show("Black won!");
                            this.board.Reset();
                            break;

                        case Winner.Red:
                            MessageBox.Show("Red won!");
                            this.board.Reset();
                            break;

                        case Winner.Draw:
                            MessageBox.Show("Tie");
                            this.board.Reset();
                            break;
                    }
                    SetEvaluation();
                    this.isThinking = false;
                    return;
                }
                this.board.ClearMoves();
                this.board.ShowMoves(clickedCell, this.isBlack);
                this.isThinking = false;
            });
            this.doTurn.Start();
        }
       
        private void ShowMove(DamkaBoard move)
        {
            this.board.AppendFromDamkaBoard(move);
            Application.DoEvents();
            //Thread.Sleep(30);
            //Application.DoEvents();
        }

        
        private void SetEvaluation()
        {
            evaluationThread = new Thread(() =>
            {
                int depth = 1;
                while (depth <= 20)
                {
                    int eval = this.search.GetEvaluation(depth, !this.isBlack, this.board.GetDamkaBoard());
                    if (eval > 100000)
                    {
                        this.evaluation.Text = "B#";
                        break;
                    }
                    else if (eval < -100000)
                    {
                        this.evaluation.Text = "R#";
                        break;
                    }
                    else
                    {
                        this.evaluation.Text = ((double)eval/-100).ToString() + " {" + depth + "}";
                    }
                    depth++;
                }
            });
            evaluationThread.Start();
        }
      
        private void UncheckAll()
        {
            foreach (ToolStripMenuItem item in levelToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
            }
        }
        private void EasyInstantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckAll();
            ((ToolStripMenuItem)sender).Checked = true;
            this.depth = 2;
        }

        private void Medium36SecondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckAll();
            ((ToolStripMenuItem)sender).Checked = true;
            this.depth = 6;
        }

        private void Hard710SecondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckAll();
            ((ToolStripMenuItem)sender).Checked = true;
            this.depth = 7;
        }

        private void SuperHard2030SecondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckAll();
            ((ToolStripMenuItem)sender).Checked = true;
            this.depth = 8;
        }

        private void ResetTurnThread()
        {
            if (this.doTurn != null)
            {
                if (this.doTurn.IsAlive)
                {
                    this.doTurn.Abort();
                }
                this.isThinking = false;
            }
            if (this.evaluationThread != null)
            {
                if (this.evaluationThread.IsAlive)
                {
                    this.evaluationThread.Abort();
                }
            }         
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetTurnThread();
            this.board.Reset();
            SetEvaluation();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.isThinking)
            {
                MessageBox.Show("Can't save now, wait for your opponent to end his turn...");
                return;
            }
            string boardName = boardName = ShowDialog("Enter board name", "Board Name");

            if (boardName.Equals(""))
            {
                return;
            }
            boardName.Replace("/", "_");

            string filePath = this.boardsDirectoryPath + "\\" + boardName;
            if (File.Exists(filePath))
            {
                MessageBox.Show("You have already saved a board with this name");
                return;
            }
            FileStream myFile = File.Create(filePath);
            myFile.Close();
            File.WriteAllBytes(filePath, this.board.GetDamkaBoard().ConvertTo1DArray());
            File.SetAttributes(filePath, FileAttributes.ReadOnly);
        }

        private void EvaluateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.isThinking)
            {
                MessageBox.Show("Can't evaluate now, wait for your opponent to end his turn...");
                return;
            }
            Thread GetEval = new Thread(() =>
            {
                MessageBox.Show((this.search.GetEvaluation(6, true, this.board.GetDamkaBoard()) * -1).ToString(), "Evaluation");
            });
            GetEval.Start();
        }

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 100, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button confirmation = new Button() { Text = "Ok", Left = 95, Width = 100, Top = 75, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void AddBoardsToToolStrip()
        {
            string[] files = Directory.GetFiles(this.boardsDirectoryPath);
            foreach (string file in files)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = Path.GetFileName(file);
                item.Click += (s, a) =>
                {
                    ResetTurnThread();
                    SetEvaluation();
                    this.board.ClearMoves();
                    DamkaBoard tmpBoard = new DamkaBoard(File.ReadAllBytes(file));
                    this.board.AppendFromDamkaBoard(tmpBoard);
                };
                loadToolStripMenuItem.DropDownItems.Add(item);
            }
        }
        
        private void LoadToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).DropDownItems.Clear();
            AddBoardsToToolStrip();
        }

        private void Damka_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.tTable.Save(this.transpositionTablePath);
            Environment.Exit(Environment.ExitCode);
        }

        private void ChangeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.isThinking)
            {
                MessageBox.Show("Can't change color now, wait for your opponent to end his turn...");
                return;
            }
            this.board.ClearMoves();
            isBlack = !isBlack;
        }
    }
}
