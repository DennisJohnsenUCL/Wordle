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
            backButton = new Button();
            wordleLabel = new Label();
            startGameButton = new Button();
            wordleTextBox = new TextBox();
            guessesUpDown = new NumericUpDown();
            guessesLabel = new Label();
            InvalidWordleLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)guessesUpDown).BeginInit();
            SuspendLayout();
            // 
            // backButton
            // 
            backButton.Location = new Point(3, 3);
            backButton.Name = "backButton";
            backButton.Size = new Size(71, 43);
            backButton.TabIndex = 3;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += BackButton_Click;
            // 
            // wordleLabel
            // 
            wordleLabel.AutoSize = true;
            wordleLabel.Location = new Point(246, 118);
            wordleLabel.Name = "wordleLabel";
            wordleLabel.Size = new Size(89, 30);
            wordleLabel.TabIndex = 1;
            wordleLabel.Text = "Wordle:";
            wordleLabel.Click += OptionsView_Click;
            // 
            // startGameButton
            // 
            startGameButton.AutoSize = true;
            startGameButton.Location = new Point(308, 297);
            startGameButton.Name = "startGameButton";
            startGameButton.Size = new Size(128, 40);
            startGameButton.TabIndex = 2;
            startGameButton.Text = "Start game";
            startGameButton.UseVisualStyleBackColor = true;
            startGameButton.Click += StartGameButton_Click;
            // 
            // wordleTextBox
            // 
            wordleTextBox.CharacterCasing = CharacterCasing.Upper;
            wordleTextBox.Location = new Point(392, 115);
            wordleTextBox.MaxLength = 5;
            wordleTextBox.Name = "wordleTextBox";
            wordleTextBox.Size = new Size(120, 36);
            wordleTextBox.TabIndex = 0;
            wordleTextBox.TextChanged += WordleTextBox_TextChanged;
            wordleTextBox.KeyDown += WordleTextBox_KeyDown;
            wordleTextBox.Leave += WordleTextBox_Leave;
            // 
            // guessesUpDown
            // 
            guessesUpDown.Location = new Point(392, 199);
            guessesUpDown.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            guessesUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            guessesUpDown.Name = "guessesUpDown";
            guessesUpDown.Size = new Size(120, 36);
            guessesUpDown.TabIndex = 1;
            guessesUpDown.Value = new decimal(new int[] { 6, 0, 0, 0 });
            guessesUpDown.KeyDown += GuessesUpDown_KeyDown;
            // 
            // guessesLabel
            // 
            guessesLabel.AutoSize = true;
            guessesLabel.Location = new Point(246, 201);
            guessesLabel.Name = "guessesLabel";
            guessesLabel.Size = new Size(96, 30);
            guessesLabel.TabIndex = 6;
            guessesLabel.Text = "Guesses:";
            guessesLabel.Click += OptionsView_Click;
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
            Controls.Add(guessesLabel);
            Controls.Add(guessesUpDown);
            Controls.Add(wordleTextBox);
            Controls.Add(startGameButton);
            Controls.Add(wordleLabel);
            Controls.Add(backButton);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "OptionsView";
            Size = new Size(744, 461);
            Click += OptionsView_Click;
            ((System.ComponentModel.ISupportInitialize)guessesUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button backButton;
        private Label wordleLabel;
        private Button startGameButton;
        private TextBox wordleTextBox;
        private NumericUpDown guessesUpDown;
        private Label guessesLabel;
        private Label InvalidWordleLabel;
    }
}
