namespace Wordle_WinForms.UserControls
{
    partial class GameView
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
            wordleLabel = new Label();
            guessesLabel = new Label();
            backButton = new Button();
            newGameButton = new Button();
            wordlePanel = new Wordle_WinForms.CustomControls.WordleFlowPanel();
            alphabetPanel = new AlphabetPanel();
            SuspendLayout();
            // 
            // wordleLabel
            // 
            wordleLabel.AutoSize = true;
            wordleLabel.Location = new Point(82, 11);
            wordleLabel.Name = "wordleLabel";
            wordleLabel.Size = new Size(0, 30);
            wordleLabel.TabIndex = 1;
            // 
            // guessesLabel
            // 
            guessesLabel.AutoSize = true;
            guessesLabel.Font = new Font("Segoe UI", 12F);
            guessesLabel.Location = new Point(100, 175);
            guessesLabel.Name = "guessesLabel";
            guessesLabel.Size = new Size(0, 21);
            guessesLabel.TabIndex = 2;
            // 
            // backButton
            // 
            backButton.Location = new Point(5, 5);
            backButton.Name = "backButton";
            backButton.Size = new Size(71, 43);
            backButton.TabIndex = 3;
            backButton.TabStop = false;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Click += BackButton_Click;
            // 
            // newGameButton
            // 
            newGameButton.AutoSize = true;
            newGameButton.Location = new Point(5, 331);
            newGameButton.Name = "newGameButton";
            newGameButton.Size = new Size(128, 40);
            newGameButton.TabIndex = 4;
            newGameButton.TabStop = false;
            newGameButton.Text = "New game";
            newGameButton.UseVisualStyleBackColor = true;
            newGameButton.Click += NewGameButton_Click;
            // 
            // wordlePanel
            // 
            wordlePanel.AutoScroll = true;
            wordlePanel.FlowDirection = FlowDirection.TopDown;
            wordlePanel.Location = new Point(314, 22);
            wordlePanel.Name = "wordlePanel";
            wordlePanel.Size = new Size(246, 332);
            wordlePanel.TabIndex = 5;
            wordlePanel.WrapContents = false;
            // 
            // alphabetPanel
            // 
            alphabetPanel.Font = new Font("Segoe UI", 16F);
            alphabetPanel.Location = new Point(21, 207);
            alphabetPanel.Margin = new Padding(0);
            alphabetPanel.Name = "alphabetPanel";
            alphabetPanel.Size = new Size(268, 79);
            alphabetPanel.TabIndex = 6;
            alphabetPanel.TabStop = false;
            // 
            // GameView
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(alphabetPanel);
            Controls.Add(wordlePanel);
            Controls.Add(newGameButton);
            Controls.Add(backButton);
            Controls.Add(guessesLabel);
            Controls.Add(wordleLabel);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "GameView";
            Size = new Size(582, 376);
            KeyDown += GameView_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label wordleLabel;
        private Label guessesLabel;
        private Button backButton;
        private Button newGameButton;
        private CustomControls.WordleFlowPanel wordlePanel;
        private AlphabetPanel alphabetPanel;
    }
}
