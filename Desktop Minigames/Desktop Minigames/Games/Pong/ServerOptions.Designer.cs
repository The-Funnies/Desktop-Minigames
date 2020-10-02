namespace Pong
{
    partial class ServerOptions
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
            this.options_label = new System.Windows.Forms.Label();
            this.radio_Free = new System.Windows.Forms.RadioButton();
            this.radio_2P = new System.Windows.Forms.RadioButton();
            this.radio_4P = new System.Windows.Forms.RadioButton();
            this.radio_8P = new System.Windows.Forms.RadioButton();
            this.tournament_label = new System.Windows.Forms.Label();
            this.ok_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // options_label
            // 
            this.options_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.options_label.Location = new System.Drawing.Point(109, 9);
            this.options_label.Name = "options_label";
            this.options_label.Size = new System.Drawing.Size(150, 52);
            this.options_label.TabIndex = 2;
            this.options_label.Text = "Server options";
            this.options_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radio_Free
            // 
            this.radio_Free.AutoSize = true;
            this.radio_Free.Location = new System.Drawing.Point(21, 127);
            this.radio_Free.Name = "radio_Free";
            this.radio_Free.Size = new System.Drawing.Size(164, 30);
            this.radio_Free.TabIndex = 3;
            this.radio_Free.TabStop = true;
            this.radio_Free.Text = "Free play (2 players)\r\nInfinite games with spectators\r\n";
            this.radio_Free.UseVisualStyleBackColor = true;
            // 
            // radio_2P
            // 
            this.radio_2P.AutoSize = true;
            this.radio_2P.Location = new System.Drawing.Point(228, 111);
            this.radio_2P.Name = "radio_2P";
            this.radio_2P.Size = new System.Drawing.Size(67, 17);
            this.radio_2P.TabIndex = 4;
            this.radio_2P.TabStop = true;
            this.radio_2P.Text = "2 players";
            this.radio_2P.UseVisualStyleBackColor = true;
            // 
            // radio_4P
            // 
            this.radio_4P.AutoSize = true;
            this.radio_4P.Location = new System.Drawing.Point(228, 134);
            this.radio_4P.Name = "radio_4P";
            this.radio_4P.Size = new System.Drawing.Size(67, 17);
            this.radio_4P.TabIndex = 5;
            this.radio_4P.TabStop = true;
            this.radio_4P.Text = "4 players";
            this.radio_4P.UseVisualStyleBackColor = true;
            // 
            // radio_8P
            // 
            this.radio_8P.AutoSize = true;
            this.radio_8P.Location = new System.Drawing.Point(228, 157);
            this.radio_8P.Name = "radio_8P";
            this.radio_8P.Size = new System.Drawing.Size(67, 17);
            this.radio_8P.TabIndex = 6;
            this.radio_8P.TabStop = true;
            this.radio_8P.Text = "8 players";
            this.radio_8P.UseVisualStyleBackColor = true;
            // 
            // tournament_label
            // 
            this.tournament_label.AutoSize = true;
            this.tournament_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tournament_label.Location = new System.Drawing.Point(225, 80);
            this.tournament_label.Name = "tournament_label";
            this.tournament_label.Size = new System.Drawing.Size(95, 17);
            this.tournament_label.TabIndex = 7;
            this.tournament_label.Text = "Tournament";
            // 
            // ok_Button
            // 
            this.ok_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok_Button.Location = new System.Drawing.Point(144, 202);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(82, 33);
            this.ok_Button.TabIndex = 8;
            this.ok_Button.Text = "play!";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.Ok_Button_Click);
            // 
            // ServerOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 247);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.tournament_label);
            this.Controls.Add(this.radio_8P);
            this.Controls.Add(this.radio_4P);
            this.Controls.Add(this.radio_2P);
            this.Controls.Add(this.radio_Free);
            this.Controls.Add(this.options_label);
            this.Name = "ServerOptions";
            this.Text = "ServerOptions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label options_label;
        private System.Windows.Forms.RadioButton radio_Free;
        private System.Windows.Forms.RadioButton radio_2P;
        private System.Windows.Forms.RadioButton radio_4P;
        private System.Windows.Forms.RadioButton radio_8P;
        private System.Windows.Forms.Label tournament_label;
        private System.Windows.Forms.Button ok_Button;
    }
}