namespace WebApiAuthors.Services
{
    public class Registry: IHostedService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _archiveName = "resgistre.txt";
        private Timer _timer;

        
        public Registry(IWebHostEnvironment env)
        {
            this._env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            WriteArchive("Proceso Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            WriteArchive("Proceso Finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            WriteArchive("Proceso en Ejecución: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void WriteArchive(string message)
        {
            var route = $@"{_env.ContentRootPath}\wwwroot\{_archiveName}";
            using(StreamWriter writer = new StreamWriter(route, append: true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
