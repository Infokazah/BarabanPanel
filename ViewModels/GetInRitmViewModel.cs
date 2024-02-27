using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using BaseClassesLyb;
using WpfBaseLyb;

namespace BarabanPanel.ViewModels
{
    internal class GetInRitmViewModel : ViewModelBase
    {
        #region Атрибуты
        private bool _inRitm = false;
        public ViewModelMainWindow _MainViewModel { get; set; }

        

        private SoundPlayer _ritmSoundPlayer;

        private SoundPlayer _barabanSoundPlayer;

        private DateTime lastClickTime;


        private double _currentTemp = 1;
        public double CurrentTemp
        {
            get
            {
                return _currentTemp;
            }
            set
            {
                if(value>0.3 && value<5)
                    _currentTemp = value;
                OnPropertyChanged(nameof(CurrentTemp));
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

        private string _currentText = "Start";

        public string CurrentText
        {
            get
            {
                return _currentText;
            }
            set
            {
                _currentText = value;
                OnPropertyChanged(nameof(CurrentText));
            }
        }
        #endregion
        //асинхронный таймер
        private async Task UpdateTimeAsync()
        {
            while (_inRitm == true)
            {
                TimeSpan elapsedTime = DateTime.Now - lastClickTime;
                CurrentTime = elapsedTime.TotalSeconds;
                await Task.Delay(1);
            }

        }
        

        #region Комманды
        public SimpleCommand StartCommand { get; }
        private bool CanStartExecute() => true;

        private void StartExecute()
        {
            _inRitm = !_inRitm;
            if(_inRitm)
            {
                CurrentText = "End";
            }
            else
            {
                CurrentText = "Start";
            }
            
            lastClickTime = DateTime.Now;
            UpdateTimeAsync();
        }

        public SimpleCommand CheckTime { get; }
        private bool CanCheckTimeExecute() => true;

        private void CheckTimeExecute()
        {
            _barabanSoundPlayer.Play();
            if(_inRitm == true)
            {
                TimeSpan elapsedTime = DateTime.Now - lastClickTime;

            if (elapsedTime.TotalSeconds > CurrentTemp + 0.05 || elapsedTime.TotalSeconds < CurrentTemp - 0.05)
            {
                MessageBox.Show("Ты не попал в тайминг");
                _inRitm = false;
                CurrentText = "Start";
                lastClickTime = DateTime.Now;
            }
            else
            {
                lastClickTime = DateTime.Now;
            }
            }
            
        }
        #endregion

        /*public GetInRitmViewModel() : this(null)
        {
            CheckTime = new SimpleCommand(CheckTimeExecute, CanCheckTimeExecute);
        }*/
        public GetInRitmViewModel() 
        {
            CheckTime = new SimpleCommand(CheckTimeExecute, CanCheckTimeExecute);
            StartCommand = new SimpleCommand(StartExecute, CanStartExecute);
            CurrentTime = 0;
            _ritmSoundPlayer = new SoundPlayer(Path.Combine(Directory.GetCurrentDirectory(), "GetInRitm.wav"));
            _barabanSoundPlayer = new SoundPlayer(Path.Combine(Directory.GetCurrentDirectory(), "GetInRitmBar.wav"));
        }
    }
}
