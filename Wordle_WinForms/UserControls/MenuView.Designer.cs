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
            DefaultGameButton = new Button();
            CustomGameButton = new Button();
            ExitButton = new Button();
            SuspendLayout();
            // 
            // DefaultGameButton
            // 
            DefaultGameButton.Location = new Point(300, 95);
            DefaultGameButton.Name = "DefaultGameButton";
            DefaultGameButton.Size = new Size(160, 45);
            DefaultGameButton.TabIndex = 0;
            DefaultGameButton.Text = "Play Wordle";
            DefaultGameButton.UseVisualStyleBackColor = true;
            DefaultGameButton.Click += DefaultGameButton_Click;
            // 
            // CustomGameButton
            // 
            CustomGameButton.Location = new Point(300, 195);
            CustomGameButton.Name = "CustomGameButton";
            CustomGameButton.Size = new Size(160, 45);
            CustomGameButton.TabIndex = 1;
            CustomGameButton.Text = "Custom game";
            CustomGameButton.UseVisualStyleBackColor = true;
            CustomGameButton.Click += CustomGameButton_Click;
            // 
            // ExitButton
            // 
            ExitButton.Location = new Point(300, 295);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new Size(160, 45);
            ExitButton.TabIndex = 2;
            ExitButton.Text = "Exit";
            ExitButton.UseVisualStyleBackColor = true;
            ExitButton.Click += ExitButton_Click;
            // 
            // MenuView
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ExitButton);
            Controls.Add(CustomGameButton);
            Controls.Add(DefaultGameButton);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "MenuView";
            Size = new Size(744, 461);
            ResumeLayout(false);
        }

        #endregion

        private Button DefaultGameButton;
        private Button CustomGameButton;
        private Button ExitButton;
    }
}
