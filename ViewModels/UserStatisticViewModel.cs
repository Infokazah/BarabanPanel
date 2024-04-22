using BaseClassesLyb;
using Microsoft.EntityFrameworkCore;
using Rep_interfases;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using UserStatisticDb.Entityes;
using BarabanPanel.Services.Extensions;
using BarabanPanel.Services.Interfaces;

namespace BarabanPanel.ViewModels
{
    public class UserStatisticViewModel : ViewModelBase
    {
        private readonly IRepository<MelStat> _MelStatRepository;
        private readonly IRepository<TempStat> _TempStatRepository;

        private readonly CollectionViewSource _MelStatViewSource;
        private readonly CollectionViewSource _TempStatViewSource;

        private ObservableCollection<MelStat> _MelStatCollection;
        public ObservableCollection<MelStat> MelStatCollection
        {
            get => _MelStatCollection;

            set
            {
                if (Set(ref _MelStatCollection, value))
                    _MelStatViewSource.Source = value;
            }
        }

        private ObservableCollection<TempStat> _TempStatCollection;
        public ObservableCollection<TempStat> TempStatCollection
        {
            get => _TempStatCollection;

            set
            {
                if (Set(ref _TempStatCollection, value))
                    _MelStatViewSource.Source = value;
            }
        }
        private async Task DownloadDataAsync()
        {
            MelStatCollection = (await _MelStatRepository.Items.ToArrayAsync()).ToObservableCollection();

            TempStatCollection = (await _TempStatRepository.Items.ToArrayAsync()).ToObservableCollection();
        }
        public UserStatisticViewModel( IRepository<MelStat> melodiestats,
            IRepository<TempStat> tempstats, IDataService dataservice)
        {
            _MelStatRepository = melodiestats;
            _TempStatRepository = tempstats;

            _MelStatViewSource = new();
            _TempStatViewSource = new();

            DownloadDataAsync();
        }
    }
}
