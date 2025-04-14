namespace Wordle_WinForms.UserControls
{
    partial class WordleRow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LetterLabel1 = new Wordle_WinForms.CustomControls.LetterLabel();
            LetterLabel2 = new Wordle_WinForms.CustomControls.LetterLabel();
            LetterLabel3 = new Wordle_WinForms.CustomControls.LetterLabel();
            LetterLabel4 = new Wordle_WinForms.CustomControls.LetterLabel();
            LetterLabel5 = new Wordle_WinForms.CustomControls.LetterLabel();
            SuspendLayout();
            // 
            // LetterLabel1
            // 
            LetterLabel1.BackColor = Color.White;
            LetterLabel1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            LetterLabel1.Location = new Point(2, 2);
            LetterLabel1.Name = "LetterLabel1";
            LetterLabel1.Padding = new Padding(3, 0, 0, 3);
            LetterLabel1.Size = new Size(40, 40);
            LetterLabel1.TabIndex = 0;
            LetterLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LetterLabel2
            // 
            LetterLabel2.BackColor = Color.White;
            LetterLabel2.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            LetterLabel2.Location = new Point(48, 2);
            LetterLabel2.Name = "LetterLabel2";
            LetterLabel2.Padding = new Padding(3, 0, 0, 3);
            LetterLabel2.Size = new Size(40, 40);
            LetterLabel2.TabIndex = 1;
            LetterLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LetterLabel3
            // 
            LetterLabel3.BackColor = Color.White;
            LetterLabel3.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            LetterLabel3.Location = new Point(94, 2);
            LetterLabel3.Name = "LetterLabel3";
            LetterLabel3.Padding = new Padding(3, 0, 0, 3);
            LetterLabel3.Size = new Size(40, 40);
            LetterLabel3.TabIndex = 2;
            LetterLabel3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LetterLabel4
            // 
            LetterLabel4.BackColor = Color.White;
            LetterLabel4.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            LetterLabel4.Location = new Point(140, 2);
            LetterLabel4.Name = "LetterLabel4";
            LetterLabel4.Padding = new Padding(3, 0, 0, 3);
            LetterLabel4.Size = new Size(40, 40);
            LetterLabel4.TabIndex = 3;
            LetterLabel4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LetterLabel5
            // 
            LetterLabel5.BackColor = Color.White;
            LetterLabel5.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            LetterLabel5.Location = new Point(186, 2);
            LetterLabel5.Name = "LetterLabel5";
            LetterLabel5.Padding = new Padding(3, 0, 0, 3);
            LetterLabel5.Size = new Size(40, 40);
            LetterLabel5.TabIndex = 4;
            LetterLabel5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // WordleRow
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(LetterLabel1);
            Controls.Add(LetterLabel2);
            Controls.Add(LetterLabel3);
            Controls.Add(LetterLabel4);
            Controls.Add(LetterLabel5);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "WordleRow";
            Size = new Size(228, 44);
            ResumeLayout(false);
        }

        #endregion

        private CustomControls.LetterLabel LetterLabel1;
        private CustomControls.LetterLabel LetterLabel2;
        private CustomControls.LetterLabel LetterLabel3;
        private CustomControls.LetterLabel LetterLabel4;
        private CustomControls.LetterLabel LetterLabel5;
    }
}
