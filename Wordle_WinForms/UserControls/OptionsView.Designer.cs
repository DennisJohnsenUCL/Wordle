namespace Wordle_WinForms.UserControls
{
    partial class OptionsView
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
            BackButton = new Button();
            WordleLabel = new Label();
            StartGameButton = new Button();
            WordleTextBox = new TextBox();
            GuessesUpDown = new NumericUpDown();
            GuessesLabel = new Label();
            InvalidWordleLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)GuessesUpDown).BeginInit();
            SuspendLayout();
            // 
            // BackButton
            // 
            BackButton.Location = new Point(3, 3);
            BackButton.Name = "BackButton";
            BackButton.Size = new Size(71, 43);
            BackButton.TabIndex = 3;
            BackButton.Text = "Back";
            BackButton.UseVisualStyleBackColor = true;
            BackButton.Click += BackButton_Click;
            // 
            // WordleLabel
            // 
            WordleLabel.AutoSize = true;
            WordleLabel.Location = new Point(246, 118);
            WordleLabel.Name = "WordleLabel";
            WordleLabel.Size = new Size(89, 30);
            WordleLabel.TabIndex = 1;
            WordleLabel.Text = "Wordle:";
            // 
            // StartGameButton
            // 
            StartGameButton.AutoSize = true;
            StartGameButton.Location = new Point(308, 297);
            StartGameButton.Name = "StartGameButton";
            StartGameButton.Size = new Size(128, 40);
            StartGameButton.TabIndex = 2;
            StartGameButton.Text = "Start game";
            StartGameButton.UseVisualStyleBackColor = true;
            StartGameButton.Click += StartGameButton_Click;
            // 
            // WordleTextBox
            // 
            WordleTextBox.CharacterCasing = CharacterCasing.Upper;
            WordleTextBox.Location = new Point(392, 115);
            WordleTextBox.MaxLength = 5;
            WordleTextBox.Name = "WordleTextBox";
            WordleTextBox.Size = new Size(120, 36);
            WordleTextBox.TabIndex = 0;
            WordleTextBox.TextChanged += WordleTextBox_TextChanged;
            WordleTextBox.KeyDown += WordleTextBox_KeyDown;
            WordleTextBox.Leave += WordleTextBox_Leave;
            // 
            // GuessesUpDown
            // 
            GuessesUpDown.Location = new Point(392, 199);
            GuessesUpDown.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            GuessesUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            GuessesUpDown.Name = "GuessesUpDown";
            GuessesUpDown.Size = new Size(120, 36);
            GuessesUpDown.TabIndex = 1;
            GuessesUpDown.Value = new decimal(new int[] { 6, 0, 0, 0 });
            GuessesUpDown.KeyDown += GuessesUpDown_KeyDown;
            // 
            // GuessesLabel
            // 
            GuessesLabel.AutoSize = true;
            GuessesLabel.Location = new Point(246, 201);
            GuessesLabel.Name = "GuessesLabel";
            GuessesLabel.Size = new Size(96, 30);
            GuessesLabel.TabIndex = 6;
            GuessesLabel.Text = "Guesses:";
            // 
            // InvalidWordleLabel
            // 
            InvalidWordleLabel.AutoSize = true;
            InvalidWordleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            InvalidWordleLabel.ForeColor = Color.Red;
            InvalidWordleLabel.Location = new Point(392, 154);
            InvalidWordleLabel.Name = "InvalidWordleLabel";
            InvalidWordleLabel.Size = new Size(0, 21);
            InvalidWordleLabel.TabIndex = 7;
            // 
            // OptionsView
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(InvalidWordleLabel);
            Controls.Add(GuessesLabel);
            Controls.Add(GuessesUpDown);
            Controls.Add(WordleTextBox);
            Controls.Add(StartGameButton);
            Controls.Add(WordleLabel);
            Controls.Add(BackButton);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "OptionsView";
            Size = new Size(744, 461);
            ((System.ComponentModel.ISupportInitialize)GuessesUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BackButton;
        private Label WordleLabel;
        private Button StartGameButton;
        private TextBox WordleTextBox;
        private NumericUpDown GuessesUpDown;
        private Label GuessesLabel;
        private Label InvalidWordleLabel;
    }
}
