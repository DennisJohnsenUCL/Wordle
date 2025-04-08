namespace Wordle_Console
{
    public class Program
    {
        private static void Main()
        {
            //>> Frame game content
            //>> For InputHandler: Separate out into common methods
            //>> For all Models: Assess whether they should be structs, records, or struct record
            //>> Set up mock tests?

            var inputHandler = new InputHandler();
            var renderer = new Renderer();
            var controller = new GameController(inputHandler, renderer);
            controller.Run();
        }
    }
}
