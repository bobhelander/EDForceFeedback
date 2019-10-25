using Journals;
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
        static private async Task Main(string[] _)
        {
            var fileName = $"{Directory.GetCurrentDirectory()}\\settings.json";
            var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName));
            var client = new Client();

            client.Initialize(settings);

            while (true)
                await Task.Delay(5000).ConfigureAwait(false);
        }
    }
}
