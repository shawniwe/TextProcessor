using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TextProcessor.DataAccessLayer;
using System.Text;
using TextProcessor;
using TextProcessor.Abstract.Interfaces;
using TextProcessor.Application;
using TextProcessor.Models;

Startup startup = new Startup();
startup.ConfigureServices();

using var scope = startup.Host.Services.CreateScope();
var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
// развертывание бд при старте
try
{
    Console.WriteLine("Применение миграции, подождите...");
    context?.Database.Migrate();
}
catch (Exception ex)
{
    Console.WriteLine("Ошибка развертывания миграции! Попробуйте развернуть миграцию вручную, а после перезапустите приложение (выполните команду \"update-database\" в консоли диспетчера пакетов nuget)");
    return;
}

Application app = new Application(startup.Host.Services.GetService<IRepository<Word>>());