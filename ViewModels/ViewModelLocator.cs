using BarabanPanel.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BarabanPanel.ViewModels
{
    internal class ViewModelLocator
    {
        public ViewModelMainWindow MainWindowModel => App.Host.Services.GetRequiredService<ViewModelMainWindow>();
        public JsonReader jsonReader => App.Host.Services.GetRequiredService<JsonReader>();
        public UserStatisticViewModel UserStatisticModel => App.Host.Services.GetRequiredService<UserStatisticViewModel>();
    }
}
