using EnlightedWorkService.Services;

namespace EnlightedWorkService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFetchData _fetchData;

        public Worker(ILogger<Worker> logger,IFetchData fetchData)
        {
            _logger = logger;
            _fetchData = fetchData;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                //call the data fetcher
                _fetchData.FetchFloorData();

                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}