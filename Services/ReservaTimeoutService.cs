using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestauranteTuristicoApp.Models;

namespace RestauranteTuristicoApp.Services
/// <summary>
/// Supervisa y cancela automáticamente las reservas expiradas.
/// </summary>
{
    public class ReservaTimeoutService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReservaTimeoutService> _logger;

        public ReservaTimeoutService(IServiceProvider serviceProvider, ILogger<ReservaTimeoutService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ReservaTimeoutService se ha iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppWebContext>>();
                    using var db = dbFactory.CreateDbContext();

                    // Obtener parámetro de timeout (por defecto 65 minutos: 1 hora + 5 min de tolerancia)
                    int minutosLimite = 65;
                    var config = await db.ConfiguracionRestaurante
                        .FirstOrDefaultAsync(c => c.Clave == "tiempo_limite_llegada_minutos", stoppingToken);

                    if (config != null && int.TryParse(config.Valor, out int val) && val > 0)
                    {
                        minutosLimite = val;
                    }

                    var reservaService = scope.ServiceProvider.GetRequiredService<ReservaService>();
                    int canceladas = await reservaService.CancelarReservasExpiradasAsync(minutosLimite);

                    if (canceladas > 0)
                    {
                        _logger.LogInformation("ReservaTimeoutService: {Count} reservas canceladas por exceder límite de {Minutos} min.", canceladas, minutosLimite);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error ejecutando verificación de timeout de reservas.");
                }

                // Ejecutar cada minuto (60,000 ms)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
