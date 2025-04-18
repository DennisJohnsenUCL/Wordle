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
            WordleLabel = new Label();
            GuessesLabel = new Label();
            BackButton = new Button();
            NewGameButton = new Button();
            WordlePanel = new Wordle_WinForms.CustomControls.WordleFlowPanel();
            SuspendLayout();
            // 
            // WordleLabel
            // 
            WordleLabel.AutoSize = true;
            WordleLabel.Location = new Point(657, 6);
            WordleLabel.Name = "WordleLabel";
            WordleLabel.Size = new Size(0, 30);
            WordleLabel.TabIndex = 1;
            // 
            // GuessesLabel
            // 
            GuessesLabel.AutoSize = true;
            GuessesLabel.Font = new Font("Segoe UI", 12F);
            GuessesLabel.Location = new Point(512, 58);
            GuessesLabel.Name = "GuessesLabel";
            GuessesLabel.Size = new Size(0, 21);
            GuessesLabel.TabIndex = 2;
            // 
            // BackButton
            // 
            BackButton.Location = new Point(3, 3);
            BackButton.Name = "BackButton";
            BackButton.Size = new Size(71, 43);
            BackButton.TabIndex = 3;
            BackButton.TabStop = false;
            BackButton.Text = "Back";
            BackButton.UseVisualStyleBackColor = true;
            BackButton.Click += BackButton_Click;
            // 
            // NewGameButton
            // 
            NewGameButton.AutoSize = true;
            NewGameButton.Location = new Point(613, 388);
            NewGameButton.Name = "NewGameButton";
            NewGameButton.Size = new Size(128, 40);
            NewGameButton.TabIndex = 4;
            NewGameButton.TabStop = false;
            NewGameButton.Text = "New game";
            NewGameButton.UseVisualStyleBackColor = true;
            NewGameButton.Visible = false;
            NewGameButton.Click += NewGameButton_Click;
            // 
            // WordlePanel
            // 
            WordlePanel.AutoScroll = true;
            WordlePanel.FlowDirection = FlowDirection.TopDown;
            WordlePanel.Location = new Point(259, 28);
            WordlePanel.Name = "WordlePanel";
            WordlePanel.Size = new Size(226, 400);
            WordlePanel.TabIndex = 5;
            // 
            // GameView
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(WordlePanel);
            Controls.Add(NewGameButton);
            Controls.Add(BackButton);
            Controls.Add(GuessesLabel);
            Controls.Add(WordleLabel);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "GameView";
            Size = new Size(744, 460);
            KeyDown += GameView_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label WordleLabel;
        private Label GuessesLabel;
        private Button BackButton;
        private Button NewGameButton;
        private CustomControls.WordleFlowPanel WordlePanel;
    }
}
