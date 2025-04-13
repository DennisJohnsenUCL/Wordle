namespace Wordle_WinForms
{
    internal static class Program
    {
        //>> Deal with screen size / DPI / resolution things
        //>> Add named methods for events down the line, and unsub in Dispose
        //>> Make a factory class for CreateWordleGameFromOptions, use in both places that have the method

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}