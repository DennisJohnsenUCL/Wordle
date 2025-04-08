namespace Wordle_Console
{
    public class Program
    {
        private static void Main()
        {
            //>> pass to game controller class
            //>> Frame game content
            //>> For InputHandler: Separate out into common methods
            //>> For all Models: Assess whether they should be structs, records, or struct record
            //>> Go over parameters, use shorter names in general
            //>> Use dependency injection? With interfaces
            //>> Set up mock tests?

            var controller = new GameController();
            controller.Run();
        }
    }
}
