namespace DotNet
{
        public class Worker : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await WriteMessage("Starting...\n");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {

                    await WriteMessage("Working...\n");

                    await Task.Delay(5000, stoppingToken);
                }
            }
            finally
            {
                await WriteMessage("Stopping...\n");
            }
        }


        private readonly string? _filename = "c:\\CustomService\\DotNet.txt";
        private Task WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            return File.AppendAllTextAsync(_filename, message);
        }
    }
}
