using FileStorage.BLL;
using FileStorage.ConsoleUI.ConsoleUtils;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.ConsoleUI.Controllers;
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
        public IServiceProvider GetContainer()
        {
            ServiceCollection container = new ServiceCollection();
            container.AddTransient<IFileRepository, FileRepository>();
            container.AddTransient<IStorageRepository, StorageRepository>();
            container.AddTransient<IUserRepository, UserRepository>();
            container.AddTransient<IConsolePrinter, ConsolePrinter>();
            container.AddTransient<IController, Controller>();
            container.AddMediatR(BusinessLayerAssembly.Value);

            string logPath = ConfigurationManager.AppSettings["LogPath"];
            LoggerConfiguration serilogLogger = new LoggerConfiguration();
            serilogLogger.WriteTo.File(logPath);
            container.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddSerilog(serilogLogger.CreateLogger(), true);
            });

            return container.BuildServiceProvider();
        }
    }
}
