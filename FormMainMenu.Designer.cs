
namespace Chess
{
    partial class FormMainMenu
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn2players = new System.Windows.Forms.Button();
            this.btnLoadGame = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn2players
            // 
            this.btn2players.BackColor = System.Drawing.Color.White;
            this.btn2players.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn2players.Location = new System.Drawing.Point(520, 110);
            this.btn2players.Name = "btn2players";
            this.btn2players.Size = new System.Drawing.Size(400, 120);
            this.btn2players.TabIndex = 0;
            this.btn2players.Text = "ИГРАТЬ ВДВОЁМ";
            this.btn2players.UseVisualStyleBackColor = false;
            this.btn2players.Click += new System.EventHandler(this.btn2players_Click);
            // 
            // btnLoadGame
            // 
            this.btnLoadGame.BackColor = System.Drawing.Color.White;
            this.btnLoadGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnLoadGame.Location = new System.Drawing.Point(520, 280);
            this.btnLoadGame.Name = "btnLoadGame";
            this.btnLoadGame.Size = new System.Drawing.Size(400, 120);
            this.btnLoadGame.TabIndex = 1;
            this.btnLoadGame.Text = "ЗАГРУЗИТЬ ИГРУ";
            this.btnLoadGame.UseVisualStyleBackColor = false;
            this.btnLoadGame.Click += new System.EventHandler(this.btnLoadGame_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.White;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnSettings.Location = new System.Drawing.Point(520, 450);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(400, 120);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "НАСТРОЙКИ";
            this.btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnExit.Location = new System.Drawing.Point(520, 620);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(400, 120);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "ВЫХОД";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1424, 861);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnLoadGame);
            this.Controls.Add(this.btn2players);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FormMainMenu";
            this.Text = "Chess";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExitApp);
            this.Load += new System.EventHandler(this.FormMainMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn2players;
        private System.Windows.Forms.Button btnLoadGame;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnExit;
    }
}

