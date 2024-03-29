using Journals;
using Newtonsoft.Json;

namespace EDForceFeedback
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            var fileName = $"{Directory.GetCurrentDirectory()}\\settings.json";

            // Check if a settings file was specified
            if (args?.Length == 1)
            {
                if (args[0].CompareTo("-h") == 0 || args[0].CompareTo("help") == 0)
                {
                    Console.WriteLine("EDForceFeedBack: EDForceFeedback.exe is a console program that runs during a Elite Dangerous session.");
                    Console.WriteLine("It watches the ED log files and responds to game events by playing a force feedback editor (.ffe) file.");
                    Console.WriteLine();
                    Console.WriteLine("Usage:");
                    Console.WriteLine("EDForceFeedback.exe -h                   Output this help.");
                    Console.WriteLine(@"EDForceFeedback.exe c:\settings.json    Override the default settings file and use this instead.");
                    Console.WriteLine($"EDForceFeedback.exe                     Will default to the settings file {Directory.GetCurrentDirectory()}\\settings.json");
                    return;
                }
                else
                {
                    fileName = args[0];
                }
            }

            Console.WriteLine($"Using setting file: {fileName}");

            var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName));

            //var client = new Client();

            //await client.Initialize(settings).ConfigureAwait(false);

//          while (true)
//              await Task.Delay(5000).ConfigureAwait(false);

            Application.Run(new Form1(new Client(), settings));
        }
    }
}