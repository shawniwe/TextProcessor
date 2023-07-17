using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TextProcessor.Abstract.Interfaces;
using TextProcessor.DataAccessLayer;
using TextProcessor.DataAccessLayer.Repositories;
using TextProcessor.Models;

namespace TextProcessor
{
    public class Startup
    {
        private IConfigurationRoot config;
        public IHost Host { get; private set; }
        private void EnableConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            this.config = configurationBuilder.Build();
        }

        public void ConfigureServices()
        {
            EnableConfiguration();
            
            string connectionString = this.config.GetConnectionString("DefaultConnection");
            var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
            hostBuilder.ConfigureLogging((_, loggining) =>
            {
                // отключение логгирования efcore в консольы
                loggining.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            });
            hostBuilder.ConfigureServices((_, services) =>
            {
                // Регистрация сервисов (в т. ч. сервиса контекста)
                services.AddTransient<IRepository<Word>, DefaultRepository<Word>>();
                services.AddSingleton<Application.Application>();
                services.AddDbContext<ApplicationDbContext>(o =>
                {
                    o.UseSqlServer(connectionString);
                });
            });

            Host = hostBuilder.Build();
        }
    }
}
