namespace Wordle_WinForms.CustomControls
{
    public class LetterLabel : Label
    {
        public LetterLabel()
        {
            BackColor = Color.White;
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
