namespace WordleSolver.Controllers
{
    internal class AppController
    {
        private readonly IEnumerable<SolverController> _controllers;

        public AppController(IEnumerable<SolverController> controllers)
        {
            _controllers = controllers;
        }

        public void Run()
        {
            foreach (var controller in _controllers)
            {
                var result = controller.Run();
                Console.WriteLine(result + "\n");
            }
        }
    }
}
