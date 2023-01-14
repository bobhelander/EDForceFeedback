using Journals;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EDForceFeedback
{
    public partial class Form1 : Form
    {
        private Client _client;
        private Settings _settings;
        public Form1(Client client, Settings settings)
        {
            InitializeComponent();

            _client = client;
            _settings = settings;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(async () => await _client.Initialize(_settings).ConfigureAwait(false)).ContinueWith(t =>
            {
                var result = "Uknown state";
                if (t.IsCanceled) result = "Task cancelled";
                else if (t.IsFaulted) result = $"Exception {t.Exception.InnerException?.Message}";
                else result = _client.StatusText;

                statusLabel.Invoke((MethodInvoker)delegate {
                    statusLabel.Text = result;
                });
            });

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _client.TestEffect();
        }
    }
}