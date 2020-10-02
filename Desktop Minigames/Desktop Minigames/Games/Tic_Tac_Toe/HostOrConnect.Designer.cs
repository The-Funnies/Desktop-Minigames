namespace TicTacToe
{
    partial class HostOrConnect
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
            this.Host = new System.Windows.Forms.Button();
            this.Connect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Host
            // 
            this.Host.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Host.Location = new System.Drawing.Point(24, 105);
            this.Host.Name = "Host";
            this.Host.Size = new System.Drawing.Size(150, 125);
            this.Host.TabIndex = 4;
            this.Host.TabStop = false;
            this.Host.Text = "Host a server";
            this.Host.UseVisualStyleBackColor = true;
            this.Host.Click += new System.EventHandler(this.Host_Click);
            // 
            // Connect
            // 
            this.Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Connect.Location = new System.Drawing.Point(204, 105);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(150, 125);
            this.Connect.TabIndex = 3;
            this.Connect.TabStop = false;
            this.Connect.Text = "Connect to server";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // HostOrConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 325);
            this.Controls.Add(this.Host);
            this.Controls.Add(this.Connect);
            this.Name = "HostOrConnect";
            this.Text = "HostOrConnect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HostOrConnect_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Host;
        private System.Windows.Forms.Button Connect;
    }
}