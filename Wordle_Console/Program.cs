namespace Wordle_Console
{
    public class Program
    {
        private static void Main()
        {
            //>> For InputHandler: Separate out into common methods
            //>> For all Models: Assess whether they should be structs, records, or struct record
            //>> Set up mock tests?
            //>> Any methods to remove from interfaces and make private and or static? If not used in GameController

            var inputHandler = new InputHandler();
            var renderer = new Renderer();
            var controller = new GameController(inputHandler, renderer);
            controller.Run();
        }
    }
}
