namespace Wordle_WinForms
{
    internal static class Program
    {
        //>> Deal with screen size / DPI / resolution things
        //>> Add named methods for events down the line, and unsub in Dispose
        //>> Make a factory class for CreateWordleGameFromOptions, use in both places that have the method
        //>> Animate letters when invalid word
        //>> Animate row popping in
        //>> Align wordle flow panel vertical distance with horizontal
        //>> Add alphabet
        //>> Add Main Menu button when game over
        //>> Add options menu
        //>> Add new game shortcut when game over
        //>> Add game controller to handle all non-UI logic strictly, with events
        //>> Add a back button
        //>> Find a better way to stop tabbing to other views (hide, close, dispose, whatever)

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