namespace Wordle_WinForms.CustomControls
{
    internal class LetterLabel : Label
    {
        public LetterLabel()
        {
            BackColor = Color.LightGray;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using Pen pen = new(Color.Black, 1);
            e.Graphics.DrawRectangle(pen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }
    }
}
