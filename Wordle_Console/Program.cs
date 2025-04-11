namespace Wordle_Console
{
    public class Program
    {
        private static void Main()
        {
            var inputHandler = new InputHandler();
            var renderer = new Renderer();
            var controller = new AppController(inputHandler, renderer);

            controller.Run();
        }
    }
}
