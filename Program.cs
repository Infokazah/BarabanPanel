using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BarabanPanel
{
    internal class Program: System.Windows.Application
    {
            private bool _contentLoaded;
            
            public static IHostBuilder CreateHostBuilder(string[] args)=>
                Host.CreateDefaultBuilder(args)
                    .UseContentRoot(App.CurrentDirectory) 
                    .ConfigureAppConfiguration((host, cfg) =>cfg
                        .SetBasePath(App.CurrentDirectory)
                        .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                        )
                    .ConfigureServices(App.ConfigureServices);
            
            /// <summary>
            /// Application Entry Point.
            /// </summary>
            [System.STAThreadAttribute()]
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.2.0")]
            public static void Main()
            {
                MVVMTrening.App app = new MVVMTrening.App();
                app.InitializeComponent();
                app.Run();
            }
        
    }
}
