using BarabanPanel.Data.StatisticDb;
using BarabanPanel.Services;
using BarabanPanel.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BarabanPanel
{

    public partial class App : Application
    {
        public static bool IsDesignMode { get; private set; } = true;

        private static IHost _host;
        public static IHost Host => _host ?? Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static Window ActivedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

        public static Window FocusedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);
        public static Window CurrentWindow => FocusedWindow ?? ActivedWindow;

        public static IServiceProvider Services => Host.Services;
        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesignMode = false;
            var host = Host;
            
            base.OnStartup(e);
            host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var host = Host;

            await host.StopAsync().ConfigureAwait(false);
            _host = null;
        }
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services) => services
            .AddDataBase(context.Configuration.GetSection("Database"))
            .RegisterServices()
            .RegisterViewModels();

        public static string CurrentDirectory => IsDesignMode ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string Path = null) => Path;
    }
}
