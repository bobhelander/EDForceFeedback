using Journals;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDForceFeedback
{
    static public class Program
    {
        static private async Task Main(string[] args)
        {
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

            var client = new Client();

            await client.Initialize(settings).ConfigureAwait(false);

            while (true)
                await Task.Delay(5000).ConfigureAwait(false);
        }
    }
}
