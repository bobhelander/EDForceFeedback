using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Journals
{
    public static class Journal
    {
        public static async Task Watch()
        {
            var client = new Client();
            client.Initialize();

            while (true)
                await Task.Delay(5000);
        }
    }
}
