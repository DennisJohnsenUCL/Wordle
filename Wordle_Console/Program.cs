namespace Wordle_Console
{
    public class Program
    {
        private static void Main()
        {
            //>> For all Models: Assess whether they should be structs, records, or struct record

            var inputHandler = new InputHandler();
            var renderer = new Renderer();
            var controller = new GameController(inputHandler, renderer);
            controller.Run();
        }
    }
}
