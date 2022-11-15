
namespace Chess
{
    partial class Form2Players
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
            this.components = new System.ComponentModel.Container();
            this.btnExit = new System.Windows.Forms.Button();
            this.notationTextBox = new System.Windows.Forms.TextBox();
            this.timerWhite = new System.Windows.Forms.Timer(this.components);
            this.timerBlack = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.Location = new System.Drawing.Point(637, 785);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(190, 30);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "СОХРАНИТЬ И ВЫЙТИ";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.SaveAndGoToMenu);
            // 
            // notationTextBox
            // 
            this.notationTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.notationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.notationTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.notationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.notationTextBox.Location = new System.Drawing.Point(1050, 50);
            this.notationTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.notationTextBox.MaxLength = 65535;
            this.notationTextBox.Multiline = true;
            this.notationTextBox.Name = "notationTextBox";
            this.notationTextBox.ReadOnly = true;
            this.notationTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.notationTextBox.Size = new System.Drawing.Size(350, 700);
            this.notationTextBox.TabIndex = 3;
            this.notationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timerWhite
            // 
            this.timerWhite.Interval = 1000;
            this.timerWhite.Tick += new System.EventHandler(this.TickWhite);
            // 
            // timerBlack
            // 
            this.timerBlack.Interval = 1000;
            this.timerBlack.Tick += new System.EventHandler(this.TickBlack);
            // 
            // Form2Players
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1440, 900);
            this.Controls.Add(this.notationTextBox);
            this.Controls.Add(this.btnExit);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form2Players";
            this.Text = "Chess";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExitApp);
            this.Load += new System.EventHandler(this.Form2Players_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form2Players_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form2Players_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox notationTextBox;
        private System.Windows.Forms.Timer timerWhite;
        private System.Windows.Forms.Timer timerBlack;
    }
}