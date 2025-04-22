namespace Wordle_WinForms.UserControls
{
    partial class MenuView
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
            defaultGameButton = new Button();
            customGameButton = new Button();
            exitButton = new Button();
            SuspendLayout();
            // 
            // defaultGameButton
            // 
            defaultGameButton.Location = new Point(211, 63);
            defaultGameButton.Name = "defaultGameButton";
            defaultGameButton.Size = new Size(160, 45);
            defaultGameButton.TabIndex = 0;
            defaultGameButton.Text = "Play Wordle";
            defaultGameButton.UseVisualStyleBackColor = true;
            defaultGameButton.Click += DefaultGameButton_Click;
            // 
            // customGameButton
            // 
            customGameButton.Location = new Point(211, 163);
            customGameButton.Name = "customGameButton";
            customGameButton.Size = new Size(160, 45);
            customGameButton.TabIndex = 1;
            customGameButton.Text = "Custom game";
            customGameButton.UseVisualStyleBackColor = true;
            customGameButton.Click += CustomGameButton_Click;
            // 
            // exitButton
            // 
            exitButton.Location = new Point(211, 263);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(160, 45);
            exitButton.TabIndex = 2;
            exitButton.Text = "Exit";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += ExitButton_Click;
            // 
            // MenuView
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(exitButton);
            Controls.Add(customGameButton);
            Controls.Add(defaultGameButton);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "MenuView";
            Size = new Size(582, 376);
            ResumeLayout(false);
        }

        #endregion

        private Button defaultGameButton;
        private Button customGameButton;
        private Button exitButton;
    }
}
