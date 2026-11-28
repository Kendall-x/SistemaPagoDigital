using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SistemaPagoDigital.Services
{
    public class ExpiracionCobrosService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ExpiracionCobrosService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var cobroService = scope.ServiceProvider
                        .GetRequiredService<CobroService>();

                    await cobroService.ActualizarCobrosExpiradosAsync();
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(30),
                    stoppingToken);
            }
        }
    }
}