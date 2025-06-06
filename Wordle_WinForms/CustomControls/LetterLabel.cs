﻿namespace Wordle_WinForms.CustomControls
{
    public class LetterLabel : Label
    {
        public LetterLabel()
        {
            TextAlign = ContentAlignment.MiddleCenter;
            BackColor = Color.White;
            Size = new Size(40, 40);
            Font = new Font(Font.FontFamily, 20f, FontStyle.Bold);
            AutoSize = false;
            Padding = new Padding(3, 0, 0, 3);
            Margin = new Padding(0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                Color.Black, 2, ButtonBorderStyle.Solid,
                Color.Black, 2, ButtonBorderStyle.Solid,
                Color.Black, 2, ButtonBorderStyle.Solid,
                Color.Black, 2, ButtonBorderStyle.Solid);
        }
    }
}
