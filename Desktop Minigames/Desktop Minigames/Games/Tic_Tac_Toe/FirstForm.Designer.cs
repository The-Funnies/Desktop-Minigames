using System.Drawing;

namespace TicTacToe
{
    partial class FirstForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstForm));
            this.Single = new System.Windows.Forms.Button();
            this.Multiplayer = new System.Windows.Forms.Button();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Single
            // 
            this.Single.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Single.Location = new System.Drawing.Point(241, 186);
            this.Single.Name = "Single";
            this.Single.Size = new System.Drawing.Size(150, 125);
            this.Single.TabIndex = 0;
            this.Single.TabStop = false;
            this.Single.Text = "Play against computer";
            this.Single.UseVisualStyleBackColor = true;
            this.Single.Click += new System.EventHandler(this.Single_Click);
            // 
            // Multiplayer
            // 
            this.Multiplayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Multiplayer.Location = new System.Drawing.Point(38, 186);
            this.Multiplayer.Name = "Multiplayer";
            this.Multiplayer.Size = new System.Drawing.Size(150, 125);
            this.Multiplayer.TabIndex = 1;
            this.Multiplayer.TabStop = false;
            this.Multiplayer.Text = "Play against friend";
            this.Multiplayer.UseVisualStyleBackColor = true;
            this.Multiplayer.Click += new System.EventHandler(this.Multiplayer_Click);
            // 
            // ImageBox
            // 
            this.ImageBox.BackColor = System.Drawing.Color.Transparent;
            this.ImageBox.Image = Desktop_Minigames.Properties.Resources.TicTacToeImage;
            this.ImageBox.Location = new System.Drawing.Point(135, 25);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(158, 143);
            this.ImageBox.TabIndex = 2;
            this.ImageBox.TabStop = false;
            // 
            // FirstForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 361);
            this.Controls.Add(this.ImageBox);
            this.Controls.Add(this.Multiplayer);
            this.Controls.Add(this.Single);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FirstForm";
            this.Text = "Tic Tac Toe";
            this.Load += new System.EventHandler(this.FirstForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Single;
        private System.Windows.Forms.Button Multiplayer;
        private System.Windows.Forms.PictureBox ImageBox;
    }
}

