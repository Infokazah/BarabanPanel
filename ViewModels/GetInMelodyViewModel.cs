using BarabanPanel.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarabanPanel.ViewModels
{
    class GetInMelodyViewModel
    {
        private ViewModelMainWindow _MainViewModel;

        public GetInMelodyViewModel() : this(null)
        {
            
        }
        public GetInMelodyViewModel(ViewModelMainWindow MainModel)
        {
            _MainViewModel = MainModel;
            
        }
    }
}
