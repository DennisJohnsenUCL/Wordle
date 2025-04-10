namespace Wordle_Console
{
    public class Program
    {
        private static void Main()
        {
            //>> For all Models: Assess whether they should be structs, records, or struct record
            //>> WordleOptions default values null, if then can simplify creation in InputHandler?

            var inputHandler = new InputHandler();
            var renderer = new Renderer();
            var controller = new GameController(inputHandler, renderer);
            controller.Run();
        }
    }
}
