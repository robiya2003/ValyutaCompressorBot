using SearchFileBot;

namespace ValyutaCompressorBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ControlClass controlClass = new ControlClass();
            await controlClass.EssentialFunction();
        }
    }
}
