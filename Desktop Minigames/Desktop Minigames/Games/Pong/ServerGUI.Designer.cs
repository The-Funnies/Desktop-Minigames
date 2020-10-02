namespace Pong
{
    partial class ServerGUI
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
            this.Players = new System.Windows.Forms.TextBox();
            this.PlayersLabel = new System.Windows.Forms.Label();
            this.ConsoleTextBox = new System.Windows.Forms.TextBox();
            this.ConsoleLabel = new System.Windows.Forms.Label();
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Players
            // 
            this.Players.Location = new System.Drawing.Point(448, 38);
            this.Players.Multiline = true;
            this.Players.Name = "Players";
            this.Players.ReadOnly = true;
            this.Players.Size = new System.Drawing.Size(200, 365);
            this.Players.TabIndex = 1;
            this.Players.TabStop = false;
            // 
            // PlayersLabel
            // 
            this.PlayersLabel.AutoSize = true;
            this.PlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayersLabel.Location = new System.Drawing.Point(467, 9);
            this.PlayersLabel.Name = "PlayersLabel";
            this.PlayersLabel.Size = new System.Drawing.Size(168, 24);
            this.PlayersLabel.TabIndex = 1;
            this.PlayersLabel.Text = "Connected players";
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Location = new System.Drawing.Point(12, 38);
            this.ConsoleTextBox.Multiline = true;
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.ReadOnly = true;
            this.ConsoleTextBox.Size = new System.Drawing.Size(418, 365);
            this.ConsoleTextBox.TabIndex = 2;
            this.ConsoleTextBox.TabStop = false;
            // 
            // ConsoleLabel
            // 
            this.ConsoleLabel.AutoSize = true;
            this.ConsoleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsoleLabel.Location = new System.Drawing.Point(174, 9);
            this.ConsoleLabel.Name = "ConsoleLabel";
            this.ConsoleLabel.Size = new System.Drawing.Size(80, 24);
            this.ConsoleLabel.TabIndex = 3;
            this.ConsoleLabel.Text = "Console";
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipLabel.Location = new System.Drawing.Point(28, 416);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(159, 24);
            this.ipLabel.TabIndex = 0;
            this.ipLabel.Text = "Public ip address:";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(193, 420);
            this.ipBox.Name = "ipBox";
            this.ipBox.ReadOnly = true;
            this.ipBox.Size = new System.Drawing.Size(80, 20);
            this.ipBox.TabIndex = 5;
            this.ipBox.TabStop = false;
            this.ipBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ServerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 450);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.ConsoleLabel);
            this.Controls.Add(this.ConsoleTextBox);
            this.Controls.Add(this.PlayersLabel);
            this.Controls.Add(this.Players);
            this.Name = "ServerGUI";
            this.Text = "ServerGUI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerGUI_FormClosed);
            this.Load += new System.EventHandler(this.ServerGUI_Load);
            this.Shown += new System.EventHandler(this.ServerGUI_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Players;
        private System.Windows.Forms.Label PlayersLabel;
        private System.Windows.Forms.TextBox ConsoleTextBox;
        private System.Windows.Forms.Label ConsoleLabel;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.TextBox ipBox;
    }
}