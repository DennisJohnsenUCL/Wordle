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
            WordleRowsFlowPanel = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // WordleRowsFlowPanel
            // 
            WordleRowsFlowPanel.AutoScroll = true;
            WordleRowsFlowPanel.FlowDirection = FlowDirection.TopDown;
            WordleRowsFlowPanel.Location = new Point(258, 28);
            WordleRowsFlowPanel.Name = "WordleRowsFlowPanel";
            WordleRowsFlowPanel.Size = new Size(228, 400);
            WordleRowsFlowPanel.TabIndex = 0;
            // 
            // GameView
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(WordleRowsFlowPanel);
            Font = new Font("Segoe UI", 16F);
            Margin = new Padding(5, 6, 5, 6);
            Name = "GameView";
            Size = new Size(744, 461);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel WordleRowsFlowPanel;
    }
}
