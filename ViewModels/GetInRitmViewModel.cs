using BarabanPanel.Infrastructure.Commands.Base;
using BarabanPanel.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using BarabanPanel.Views;
using System.Windows;
using BarabanPanel.Infrastructure.Commands;

namespace BarabanPanel.ViewModels
{
    internal class GetInRitmViewModel : ViewModelBase
    {
        private bool _inRitm = true;
        private ViewModelMainWindow _MainViewModel { get; }

        private DateTime lastClickTime;
        public CommandBase CheckTime { get; }
        private bool CanCheckTimeExecute(object p) => true;

        private void CheckTimeExecute(object filePath)
        {

            TimeSpan elapsedTime = DateTime.Now - lastClickTime;

            if (elapsedTime.TotalSeconds > 4 || elapsedTime.TotalSeconds < 3)
            {
                MessageBox.Show("Ты не попал в тайминг");
                lastClickTime = DateTime.Now;
            }
            else
            {
                MessageBox.Show("Красава чувствуешь");
                lastClickTime = DateTime.Now;
            }
        }
        private async Task UpdateTimeAsync()
        {
            while (_inRitm = true) 
            {
                TimeSpan elapsedTime = DateTime.Now - lastClickTime;
                CurrentTime = elapsedTime.TotalSeconds;
                await Task.Delay(1);
            }
            
        }

        private double _currentTime;

        public double CurrentTime
        {
            get
            {
                return _currentTime;
            }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public GetInRitmViewModel() : this(null)
        {
            CheckTime = new RegularCommand(CheckTimeExecute, CanCheckTimeExecute);
            CurrentTime = 30;
            UpdateTimeAsync();
        }
        public GetInRitmViewModel(ViewModelMainWindow MainModel) 
        {
            _MainViewModel = MainModel;
            CheckTime = new RegularCommand(CheckTimeExecute, CanCheckTimeExecute);
            CurrentTime = 0;
            UpdateTimeAsync();
        }
        
    }
}
