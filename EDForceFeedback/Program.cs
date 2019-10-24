using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDForceFeedback
{
    static public class Program
    {
        static private async Task Main(string[] _)
        {
            var client = new Journals.Client();

            client.Initialize();

            while (true)
                await Task.Delay(5000).ConfigureAwait(false);
        }
    }
}
