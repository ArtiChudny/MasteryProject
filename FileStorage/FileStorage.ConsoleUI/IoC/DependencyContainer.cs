using FileStorage.BLL;
using FileStorage.ConsoleUI.ConsoleUtils;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.ConsoleUI.Controllers;
using FileStorage.DAL;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Configuration;

namespace FileStorage.ConsoleUI.IoC
{
    public class DependencyContainer
    {
        private readonly string _logPath = $"{ConfigurationManager.AppSettings["LogPath"]}/{DateTime.Today:yyy-MM-dd} Log.txt";

        public IServiceProvider GetContainer()
        {
            var container = new ServiceCollection();
            container.AddTransient<IFileRepository, FileRepository>();
            container.AddTransient<IStorageRepository, StorageRepository>();
            container.AddTransient<IUserRepository, UserRepository>();
            container.AddTransient<ICommandInfoRepository, CommandRepository>();
            container.AddTransient<IConsolePrinter, ConsolePrinter>();
            container.AddTransient<IController, Controller>();
            container.AddSingleton<CurrentUser>();
            container.AddMediatR(BusinessLayerAssembly.Value);
            container.AddDbContext<StorageContext>();

            var SerilogLogger = new LoggerConfiguration();
            SerilogLogger.WriteTo.File(_logPath);

            container.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);     
                builder.AddSerilog(SerilogLogger.CreateLogger(), dispose: true);                
            });

            return container.BuildServiceProvider();
        }
    }
}
