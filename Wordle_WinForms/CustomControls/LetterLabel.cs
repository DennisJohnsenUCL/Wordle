namespace Wordle_WinForms.CustomControls
{
    public class LetterLabel : Label
    {
        public LetterLabel()
        {
            TextAlign = ContentAlignment.MiddleCenter;
            BackColor = Color.White;
            Size = new Size(40, 40);
            Font = new Font(Font.FontFamily, 20f, FontStyle.Bold);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using Pen pen = new(Color.Black, 2);
            float halfPen = pen.Width / 2f;
            e.Graphics.DrawRectangle(
                pen,
                halfPen,
                halfPen,
                ClientSize.Width - pen.Width,
                ClientSize.Height - pen.Width
            );
        }
    }
}
