using System.ServiceProcess;

namespace Microsoft
{
    internal class CustomService : ServiceBase
    {
        private Timer _timer;

        protected override void OnStart(string[] args)
        {
            WriteMessage("Starting..");
            _timer = new Timer(Work, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Start()
        {
                       OnStart(null);
        }

        public void Stop()
        {
                       OnStop();
        }

        protected override void OnStop()
        {
            WriteMessage("Stopping..");
        }

        private void Work(object? state)
        {
            WriteMessage("Working...");
        }

        private readonly string? _filename = "c:\\CustomService\\Microsoft.txt";
        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            File.AppendAllText(_filename, message + "\n");
        }



    }
}
