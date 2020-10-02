namespace TicTacToe
{
    partial class SingleplayerGame
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleplayerGame));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.difficulty = new System.Windows.Forms.ToolStripMenuItem();
            this.Easy = new System.Windows.Forms.ToolStripMenuItem();
            this.Medium = new System.Windows.Forms.ToolStripMenuItem();
            this.Impossible = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.difficulty});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(499, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // difficulty
            // 
            this.difficulty.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Easy,
            this.Medium,
            this.Impossible});
            this.difficulty.Name = "difficulty";
            this.difficulty.Size = new System.Drawing.Size(67, 20);
            this.difficulty.Text = "Difficulty";
            // 
            // Easy
            // 
            this.Easy.Name = "Easy";
            this.Easy.Size = new System.Drawing.Size(131, 22);
            this.Easy.Text = "Easy";
            this.Easy.Click += new System.EventHandler(this.Easy_Click);
            // 
            // Medium
            // 
            this.Medium.Name = "Medium";
            this.Medium.Size = new System.Drawing.Size(131, 22);
            this.Medium.Text = "Medium";
            this.Medium.Click += new System.EventHandler(this.Medium_Click);
            // 
            // Impossible
            // 
            this.Impossible.Name = "Impossible";
            this.Impossible.Size = new System.Drawing.Size(131, 22);
            this.Impossible.Text = "Impossible";
            this.Impossible.Click += new System.EventHandler(this.Impossible_Click);
            // 
            // SingleplayerGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 549);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SingleplayerGame";
            this.Text = "Tic Tac Toe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SingleplayerGame_FormClosing);
            this.Load += new System.EventHandler(this.SingleplayerGame_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem difficulty;
        private System.Windows.Forms.ToolStripMenuItem Easy;
        private System.Windows.Forms.ToolStripMenuItem Medium;
        private System.Windows.Forms.ToolStripMenuItem Impossible;
    }
}