using BaseClassesLyb;
using Microsoft.EntityFrameworkCore;
using Rep_interfases;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using UserStatisticDb.Entityes;
using BarabanPanel.Services.Extensions;
using BarabanPanel.Services.Interfaces;
using System;
using System.Windows;
using WpfBaseLyb;
using System.Linq;

namespace BarabanPanel.ViewModels
{
    public class UserStatisticViewModel : ViewModelBase
    {
        private readonly IRepository<MelStat> _MelStatRepository;
        private readonly IRepository<TempStat> _TempStatRepository;


        private ObservableCollection<MelStat> _MelStatCollection;
        public ObservableCollection<MelStat> MelStatCollection
        {
            get => _MelStatCollection;

            set
            {
                Set(ref _MelStatCollection, value);
            }
        }

        private ObservableCollection<TempStat> _TempStatCollection;
        public ObservableCollection<TempStat> TempStatCollection
        {
            get => _TempStatCollection;

            set
            {
                Set(ref _TempStatCollection, value);
            }
        }

        private Visibility ritmVisibility;

        public Visibility RitmVisibility
        {
            get => ritmVisibility;

            set
            {
                ritmVisibility = value;
                OnPropertyChanged(nameof(RitmVisibility));
            }
        }

        private Visibility melodyVisibility;

        public Visibility MelodyVisibility
        {
            get => melodyVisibility;

            set
            {
                melodyVisibility = value;
                OnPropertyChanged(nameof(MelodyVisibility));
            }
        }


        private DateTime date;

        public DateTime Date
        {
            get => date;

            set
            {
                date = value;
                MelStatCollection = _MelStatRepository.Items.ToArray().Where(i => i.Data.Month == date.Month).ToObservableCollection();
                TempStatCollection = _TempStatRepository.Items.ToArray().Where(i => i.Data.Month == date.Month).ToObservableCollection();
                OnPropertyChanged(nameof(Date));
            }
        }

        public SimpleCommand ChangeCommand { get; }
        private bool CanChangeExecute() => true;

        private void ChangeExecute()
        {
            if(MelodyVisibility == Visibility.Visible)
            {
                MelodyVisibility = Visibility.Hidden;
                RitmVisibility = Visibility.Visible;
            }
            else
            {
                MelodyVisibility = Visibility.Visible;
                RitmVisibility = Visibility.Hidden;
            }
        }

        public SimpleCommand CloseCommand { get; }
        private bool CanCloseExecute() => true;

        private void CloseExecute()
        {
            App.CurrentWindow.Close();
        }
        private async Task DownloadDataAsync()
        {
            MelStatCollection = (await _MelStatRepository.Items.ToArrayAsync()).ToObservableCollection();

            TempStatCollection = (await _TempStatRepository.Items.ToArrayAsync()).ToObservableCollection();
        }

       
        public UserStatisticViewModel( IRepository<MelStat> melodiestats,
            IRepository<TempStat> tempstats)
        {
            _MelStatRepository = melodiestats;
            _TempStatRepository = tempstats;
            ChangeCommand = new SimpleCommand(ChangeExecute,CanChangeExecute);
            CloseCommand = new SimpleCommand(CloseExecute, CanCloseExecute);
            date = DateTime.Now;
            DownloadDataAsync();
        }
    }
}
