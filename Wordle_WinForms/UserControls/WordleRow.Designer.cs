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
            letterLabel1 = new Wordle_WinForms.CustomControls.LetterLabel();
            letterLabel2 = new Wordle_WinForms.CustomControls.LetterLabel();
            letterLabel3 = new Wordle_WinForms.CustomControls.LetterLabel();
            letterLabel4 = new Wordle_WinForms.CustomControls.LetterLabel();
            letterLabel5 = new Wordle_WinForms.CustomControls.LetterLabel();
            SuspendLayout();
            // 
            // letterLabel1
            // 
            letterLabel1.BackColor = Color.White;
            letterLabel1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            letterLabel1.Location = new Point(1, 0);
            letterLabel1.Margin = new Padding(0);
            letterLabel1.Name = "letterLabel1";
            letterLabel1.Padding = new Padding(3, 0, 0, 3);
            letterLabel1.Size = new Size(40, 40);
            letterLabel1.TabIndex = 0;
            letterLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // letterLabel2
            // 
            letterLabel2.BackColor = Color.White;
            letterLabel2.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            letterLabel2.Location = new Point(47, 0);
            letterLabel2.Margin = new Padding(0);
            letterLabel2.Name = "letterLabel2";
            letterLabel2.Padding = new Padding(3, 0, 0, 3);
            letterLabel2.Size = new Size(40, 40);
            letterLabel2.TabIndex = 1;
            letterLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // letterLabel3
            // 
            letterLabel3.BackColor = Color.White;
            letterLabel3.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            letterLabel3.Location = new Point(93, 0);
            letterLabel3.Margin = new Padding(0);
            letterLabel3.Name = "letterLabel3";
            letterLabel3.Padding = new Padding(3, 0, 0, 3);
            letterLabel3.Size = new Size(40, 40);
            letterLabel3.TabIndex = 2;
            letterLabel3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // letterLabel4
            // 
            letterLabel4.BackColor = Color.White;
            letterLabel4.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            letterLabel4.Location = new Point(139, 0);
            letterLabel4.Margin = new Padding(0);
            letterLabel4.Name = "letterLabel4";
            letterLabel4.Padding = new Padding(3, 0, 0, 3);
            letterLabel4.Size = new Size(40, 40);
            letterLabel4.TabIndex = 3;
            letterLabel4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // letterLabel5
            // 
            letterLabel5.BackColor = Color.White;
            letterLabel5.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            letterLabel5.Location = new Point(185, 0);
            letterLabel5.Margin = new Padding(0);
            letterLabel5.Name = "letterLabel5";
            letterLabel5.Padding = new Padding(3, 0, 0, 3);
            letterLabel5.Size = new Size(40, 40);
            letterLabel5.TabIndex = 4;
            letterLabel5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // WordleRow
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(letterLabel1);
            Controls.Add(letterLabel2);
            Controls.Add(letterLabel3);
            Controls.Add(letterLabel4);
            Controls.Add(letterLabel5);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(0, 0, 0, 6);
            Name = "WordleRow";
            Size = new Size(226, 40);
            ResumeLayout(false);
        }

        #endregion

        private CustomControls.LetterLabel letterLabel1;
        private CustomControls.LetterLabel letterLabel2;
        private CustomControls.LetterLabel letterLabel3;
        private CustomControls.LetterLabel letterLabel4;
        private CustomControls.LetterLabel letterLabel5;
    }
}
