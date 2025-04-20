namespace Wordle_WinForms.CustomControls
{
    public class AlphabetLabel : Label
    {
        public AlphabetLabel()
        {
            AutoSize = false;
            Size = new Size(25, 25);
            BackColor = Color.White;
            TextAlign = ContentAlignment.MiddleCenter;
            Margin = new Padding(0);
            Padding = new Padding(0, 0, 0, 2);
            Name = "AlphabetLabel";
            Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
