using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Prism.Services
{
    public class RssReaderService: IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer timer;
        
        public RssReaderService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            TimeSpan interval = TimeSpan.FromHours(24);
            // nextRun is next time when it's midday. Today if it's before 12:00 or tomorrow if 12:00 passed
            var nextRun = DateTime.Today.AddHours(12);
            if (nextRun < DateTime.Now)
            {
                nextRun = nextRun.AddDays(1);
            }
            var now = DateTime.Now;
            var firstInterval = nextRun.Subtract(now);
            Console.WriteLine(firstInterval);
            Console.WriteLine(nextRun);
            
            Action action = () =>
            {
                var t1 = Task.Delay(firstInterval);
                t1.Wait();
                ReadAndStore(null);
                timer = new Timer(ReadAndStore, null, TimeSpan.Zero, interval);
            };

            Task.Run(action);    
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void ReadAndStore(object state)
        {
            using(var scope = _serviceProvider.CreateScope()) {
                var rssService = scope.ServiceProvider.GetService<IRssService>();
                rssService?.ReadAllAndStore();
            }
        }
    }
}