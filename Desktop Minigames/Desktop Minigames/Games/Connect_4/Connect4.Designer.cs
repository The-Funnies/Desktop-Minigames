using System.Windows.Forms;

namespace Desktop_Minigames
{
    partial class Connect4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private Button[][] InitializeComponent()
        {

            Size = new System.Drawing.Size(500, 550);
            Button[][] btnBoard = new Button[8][];
            Text = "Connect Four Game";
            for (int i = 0; i < 8; i++)
            {
                btnBoard[i] = new Button[8];
                for (int j = 0; j < 8; j++)
                {
                    btnBoard[i][j] = new System.Windows.Forms.Button();
                    btnBoard[i][j].Location = new System.Drawing.Point(i * 60, j * 60);

                    btnBoard[i][j].Size = new System.Drawing.Size(60, 60);
                    btnBoard[i][j].UseVisualStyleBackColor = true;
                    Controls.Add(btnBoard[i][j]);

                    if (i == 0)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row1);
                    if (i == 1)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row2);
                    if (i == 2)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row3);
                    if (i == 3)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row4);
                    if (i == 4)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row5);
                    if (i == 5)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row6);
                    if (i == 6)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row7);
                    if (i == 7)
                        btnBoard[i][j].Click += new System.EventHandler(this.Row8);

                }
            }

            return btnBoard;
        }

    }

}
#endregion
