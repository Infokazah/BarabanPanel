using BarabanPanel.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarabanPanel.ViewModels
{
    internal static class RegistarViewModels
    {
        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services.AddSingleton<GetInMelodyViewModel>();
            services.AddSingleton<GetInRitmViewModel>();
            services.AddSingleton<ViewModelMainWindow>();
            return services;
        }
    }
}
