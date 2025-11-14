using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace TopShelfService
{
    internal class CustomService : ServiceControl
    {
        private CancellationTokenSource CancellationTokenSource { get; set; }
        public bool Start(HostControl hostControl)
        {
            WriteMessage("Starting...\n");
            CancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                while (!CancellationTokenSource.Token.IsCancellationRequested)
                {
                    WriteMessage("I am working...\n");
                    await Task.Delay(5000);
                }
            }, CancellationTokenSource.Token);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            WriteMessage("Stopping...\n");
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            return true;
        }

        private string _fileName = "c:\\CustomService\\TopshelfService.txt";
        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            File.AppendAllText(_fileName, $"{DateTime.Now.ToShortTimeString()}: {message}");
        }
    }
}
