using BarabanPanel.Models;
using BarabanPanel.Services;
using BarabanPanel.Services.Interfaces;
using BaseClassesLyb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using UserStatisticDb;
using WpfBaseLyb;

namespace BarabanPanel.ViewModels
{
    class GetInMelodyViewModel : ViewModelBase
    {
        private SoundPlayer _soundPlayer;
        private JsonReader _reader;
        private string _directory = Directory.GetCurrentDirectory();

        #region Атрибуты мдля мелодии

        private string _melodyName;
        public string MelodyName
        {
            get => _melodyName;

            set
            {
                if (value != null)
                    _melodyName = value;
            }
        }

        private Melody _currentMelody;
        public Melody CurrentMelody
        {
            get => _currentMelody;

            set
            {
                if (value != null)
                    _currentMelody = value;
            }
        }

        private IEnumerable<string> _melodies;

        public IEnumerable<string> Melodies
        {
            get
            {
                return _melodies;
            }
            set 
            {
                _melodies = value;
                OnPropertyChanged(nameof(Melodies));
            }
        }
        #endregion
        public SimpleCommand StartMelody { get; }
        private bool CanStartMelodyExecute() => true;

        private void StartMelodyExecute()
        {
           if(MelodyName != null)
           {
                CurrentMelody = _reader.GetMelody(MelodyName);
                GetNextPartOfMelody();
                lastClickTime = DateTime.Now;
                _inRitm = true;
                UpdateTimeAsync();
            }
        }


        #region Процесс проигрыша мелодии

        private string _barabanName;
        public string BarabanName
        {
            get => _barabanName;

            set
            {
                _barabanName = value;
                OnPropertyChanged(nameof(BarabanName));
            }
        }
        private int _barabanParamNumber = 0;

        private DateTime lastClickTime;

        private bool _inRitm = false;

        private double _currentTemp;
        public double CurrentTemp
        {
            get
            {
                return _currentTemp;
            }
            set
            {
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

        public CommandBase CheckTime { get; }
        private bool CanCheckTimeExecute(object p) => true;

        private void CheckTimeExecute(object barabanName)
        {
            if (barabanName != null)
            {

                _soundPlayer.SoundLocation = Path.Combine(_directory,barabanName.ToString() + ".wav");
                _soundPlayer.Play();
                if(barabanName.ToString() == BarabanName)
                {
                    TimeSpan elapsedTime = DateTime.Now - lastClickTime;

                    if (elapsedTime.TotalSeconds > _currentTemp + 0.1 || elapsedTime.TotalSeconds < _currentTemp - 0.1)
                    {
                        MessageBox.Show("Ты не попал в тайминг");
                        _inRitm = false;
                        _barabanParamNumber = 0;
    }
                    else
                    {
                        GetNextPartOfMelody();
                        lastClickTime = DateTime.Now;
                    }
                }
                else
                {
                    MessageBox.Show("Не тот барабан");
                    _inRitm = false;
                    _barabanParamNumber = 0;
                }

                
            }

        }

        private async Task UpdateTimeAsync()
        {
            while (_inRitm == true)
            {
                TimeSpan elapsedTime = DateTime.Now - lastClickTime;
                CurrentTime = elapsedTime.TotalSeconds;
                await Task.Delay(1);
            }

        }

        private void GetNextPartOfMelody()
        {
            if(_barabanParamNumber <= CurrentMelody.BarabanParts.Count -1)
            {
                CurrentTemp = CurrentMelody.BarabanParts[_barabanParamNumber].BarabanTaiming;
                BarabanName = CurrentMelody.BarabanParts[_barabanParamNumber].BarabanNumber;
                _barabanParamNumber++;
            }
            else
            {
                MessageBox.Show("Мелодия пройдена");

                _inRitm = false;
            }
            
        }
        #endregion
        /*public GetInMelodyViewModel() : this(null)
        {
            JsonReader reader = new JsonReader();
            Melodies = reader.GetDictionaryNames();
            StartMelody = new SimpleCommand(StartMelodyExecute, CanStartMelodyExecute);
        }*/
        public GetInMelodyViewModel(IJsonReader reader)
        {
            _reader = (JsonReader?)reader;
            _soundPlayer = new SoundPlayer();
            Melodies = _reader.GetDictionaryNames();
            StartMelody = new SimpleCommand(StartMelodyExecute, CanStartMelodyExecute);
            CheckTime = new RegularCommand(CheckTimeExecute, CanCheckTimeExecute);
            BarabanName = "0";
        }
    }
}
