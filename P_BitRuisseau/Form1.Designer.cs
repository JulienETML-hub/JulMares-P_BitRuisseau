namespace P_BitRuisseau
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MediaTheque = new Label();
            btnToMediaPlayer = new Button();
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            UpLoadFichier = new Button();
            ListeFichiersLocaux = new ListView();
            ListeCommu = new ListView();
            button1 = new Button();
            SuspendLayout();
            // 
            // MediaTheque
            // 
            MediaTheque.AutoSize = true;
            MediaTheque.Font = new Font("Segoe UI", 20F);
            MediaTheque.Location = new Point(297, 26);
            MediaTheque.Name = "MediaTheque";
            MediaTheque.Size = new Size(180, 37);
            MediaTheque.TabIndex = 0;
            MediaTheque.Text = "MediaTheque";
            // 
            // btnToMediaPlayer
            // 
            btnToMediaPlayer.Location = new Point(12, 26);
            btnToMediaPlayer.Name = "btnToMediaPlayer";
            btnToMediaPlayer.Size = new Size(82, 23);
            btnToMediaPlayer.TabIndex = 1;
            btnToMediaPlayer.Text = "Mediaplayer";
            btnToMediaPlayer.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(192, 109);
            label1.Name = "label1";
            label1.Size = new Size(81, 15);
            label1.TabIndex = 2;
            label1.Text = "Communauté";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(495, 109);
            label2.Name = "label2";
            label2.Size = new Size(88, 15);
            label2.TabIndex = 3;
            label2.Text = "Fichiers Locaux";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(489, 143);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Rechercher";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(182, 143);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Rechercher";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 5;
            // 
            // UpLoadFichier
            // 
            UpLoadFichier.Location = new Point(508, 328);
            UpLoadFichier.Name = "UpLoadFichier";
            UpLoadFichier.Size = new Size(75, 23);
            UpLoadFichier.TabIndex = 8;
            UpLoadFichier.Text = "upLoadFichiers";
            UpLoadFichier.UseVisualStyleBackColor = true;
            UpLoadFichier.Click += UpLoadFichier_Click;
            // 
            // ListeFichiersLocaux
            // 
            ListeFichiersLocaux.Location = new Point(418, 183);
            ListeFichiersLocaux.Name = "ListeFichiersLocaux";
            ListeFichiersLocaux.Size = new Size(250, 97);
            ListeFichiersLocaux.TabIndex = 9;
            ListeFichiersLocaux.UseCompatibleStateImageBehavior = false;
            ListeFichiersLocaux.SelectedIndexChanged += ListeFichiersLocaux_SelectedIndexChanged;
            // 
            // ListeCommu
            // 
            ListeCommu.Location = new Point(182, 183);
            ListeCommu.Name = "ListeCommu";
            ListeCommu.Size = new Size(121, 97);
            ListeCommu.TabIndex = 10;
            ListeCommu.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            button1.Location = new Point(336, 321);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 11;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(ListeCommu);
            Controls.Add(ListeFichiersLocaux);
            Controls.Add(UpLoadFichier);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnToMediaPlayer);
            Controls.Add(MediaTheque);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label MediaTheque;
        private Button btnToMediaPlayer;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button UpLoadFichier;
        private ListView ListeFichiersLocaux;
        private ListView ListeCommu;
        private Button button1;
    }
}
