namespace Wordle_WinForms.CustomControls
{
    public class LetterLabel : Label
    {
        public LetterLabel()
        {
            SetStyle(ControlStyles.Selectable, false);
            TextAlign = ContentAlignment.MiddleCenter;
            BackColor = Color.White;
            Size = new Size(40, 40);
            Font = new Font(Font.FontFamily, 20f, FontStyle.Bold);
            AutoSize = false;
            Padding = new Padding(4, 0, 0, 4);
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
