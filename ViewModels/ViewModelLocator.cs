using BarabanPanel.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BarabanPanel.ViewModels
{
    internal class ViewModelLocator
    {
        public ViewModelMainWindow MainWindowModel => App.Host.Services.GetRequiredService<ViewModelMainWindow>();
        public GetInRitmViewModel GetInRitm => App.Host.Services.GetRequiredService<GetInRitmViewModel>();
        public GetInMelodyViewModel GetInMelody => App.Host.Services.GetRequiredService<GetInMelodyViewModel>();
        public JsonReader jsonReader => App.Host.Services.GetRequiredService<JsonReader>();
        public UserStatisticViewModel UserStatisticModel => App.Host.Services.GetRequiredService<UserStatisticViewModel>();
    }
}
