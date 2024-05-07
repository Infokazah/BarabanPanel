using BarabanPanel.Models;
using BarabanPanel.Services.Interfaces;
using BarabanPanel.Views;
using BaseClassesLyb;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NAudio.Wave;
using Rep_interfases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UserStatisticDb;
using UserStatisticDb.Entityes;
using WpfBaseLyb;

namespace BarabanPanel.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        private IJsonReader _reader;
        private string _directory = Directory.GetCurrentDirectory();
        private readonly Repository<MelStat> MelodyRepository;
        private readonly Repository<TempStat> TempRepository;
        private string _currentMode;

        public string CurrentMode
        {
            get => _currentMode;

            set
            {
                _currentMode = value;
                if (CurrentMode == "System.Windows.Controls.ComboBoxItem: Статистика")
                {
                    OpenStatistic();
                }
                if (CurrentMode == "System.Windows.Controls.ComboBoxItem: Повторение мелодии")
                {
                    MelodyVisibility = Visibility.Visible;
                }
                else
                {
                    MelodyVisibility = Visibility.Hidden;
                }

                if (CurrentMode == "System.Windows.Controls.ComboBoxItem: Поподание в ритм")
                {
                    RitmVisibility = Visibility.Visible;
                }
                else
                {
                    RitmVisibility = Visibility.Hidden;
                }
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

        bool _inRitm = false;
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
                if (value > 0.1 && value < 5)
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

        private int _score;

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }
        #region Ritm
        #region Комманды


        private void CheckRitmTimeExecute()
        {
            if (_inRitm == true)
            {
                TimeSpan elapsedTime = DateTime.Now - lastClickTime;

                if (elapsedTime.TotalSeconds > CurrentTemp + 0.1 || elapsedTime.TotalSeconds < CurrentTemp - 0.1)
                {
                    MessageBox.Show("Ты не попал в тайминг");
                    _inRitm = false;
                    TempStat stat = new TempStat()
                    {
                        Data = DateTime.Now,
                        Temp = CurrentTemp,
                        Score = Score
                    };
                    var temp = TempRepository.Items.FirstOrDefault(i => i.Temp == stat.Temp);
                    if(temp != null) 
                    {
                        if(temp.Score < stat.Score) 
                        {
                            TempRepository.Remove(temp);
                            TempRepository.Add(stat);
                            MessageBox.Show("Новый рекорд в данном темпе");
                        }
                    }
                    else
                    {
                        TempRepository.Add(stat);
                        MessageBox.Show("Новый рекорд в данном темпе");
                    }
                    lastClickTime = DateTime.Now;
                    Score = 0;
                }
                else
                {
                    lastClickTime = DateTime.Now;
                    Score++;
                }
            }

        }
        #endregion
        #region Методы
        private async Task UpdateTimeAsync()
        {
            while (_inRitm == true)
            {
                TimeSpan elapsedTime = DateTime.Now - lastClickTime;
                CurrentTime = elapsedTime.TotalSeconds;
                await Task.Delay(1);
            }

        }


        #endregion
        #endregion

        #region Melody
        #region Атрибуты
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

        private int _barabanParamNumber = 0;

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

        #endregion
        #region Комманды
        private void CheckMelodyTimeExecute(string barabanName)
        {
            if (barabanName != null)
            {
                if (barabanName.ToString() == BarabanName)
                {
                    TimeSpan elapsedTime = DateTime.Now - lastClickTime;

                    if (elapsedTime.TotalSeconds > _currentTemp + 0.1 || elapsedTime.TotalSeconds < _currentTemp - 0.1)
                    {
                        MessageBox.Show("Ты не попал в тайминг");
                        UpdateStat();
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
                    UpdateStat();
                }


            }

        }
        #endregion
        #region Методы

        private void GetNextPartOfMelody()
        {
            if (_barabanParamNumber <= CurrentMelody.BarabanParts.Count - 1)
            {
                CurrentTemp = CurrentMelody.BarabanParts[_barabanParamNumber].BarabanTaiming;
                BarabanName = CurrentMelody.BarabanParts[_barabanParamNumber].BarabanNumber;
                _barabanParamNumber++;
            }
            else
            {
                MessageBox.Show("Мелодия пройдена");
                UpdateStat();
                _inRitm = false;
            }

        }

        private void UpdateStat()
        {
            MelStat CurMelStat = MelodyRepository.Get(CurrentMelody.Id);
            CurMelStat.Data = DateTime.Now;
            CurMelStat.CompleteCount = $"{_barabanParamNumber}/{CurrentMelody.BarabanParts.Count}";
            MelodyRepository.Update(CurMelStat);
        }
        #endregion
        #endregion

        #region Комманды

        public CommandBase MakeSound { get; }
        private bool CanMakeSoundExecute(object p) => true;

        private async void MakeSoundExecute(object BarabanName)
        {
            if (BarabanName is string name)
            {
                try
                {
                    string soundPath = Path.Combine(_directory, name + ".wav");
                    Thread soundThread = new Thread(() => PlaySoundAsync(soundPath));
                    soundThread.Start();
                    if (_inRitm == true)
                    {
                        if (CurrentMode == "System.Windows.Controls.ComboBoxItem: Повторение мелодии")
                        {
                            CheckMelodyTimeExecute(name);
                        }

                        if (CurrentMode == "System.Windows.Controls.ComboBoxItem: Поподание в ритм")
                        {
                            CheckRitmTimeExecute();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка воспроизведения звука: " + ex.Message);
                }
            }
        }

        public async Task PlaySoundAsync(string path)
        {
            var _waveOut = new WaveOutEvent();
            try
            {
                using (var audioFile = new AudioFileReader(path))
                {
                    _waveOut.Init(audioFile);
                    _waveOut.Play();
                    await Task.Delay(audioFile.TotalTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка воспроизведения звука: " + ex.Message);
            }
        }



        private void OpenStatistic()
        {
            UserStatisticWindow stat = new UserStatisticWindow();
            stat.ShowDialog();  
        }

        public SimpleCommand StartCommand { get; }
        private bool CanStartExecute() => true;

        private void StartExecute()
        {
            if (CurrentMode == "System.Windows.Controls.ComboBoxItem: Повторение мелодии")
            {
                if (!MelodyName.IsNullOrEmpty())
                {
                    _barabanParamNumber = 0;
                    CurrentMelody = _reader.GetMelody(MelodyName);
                    GetNextPartOfMelody();
                    lastClickTime = DateTime.Now;
                    _inRitm = true;
                    UpdateTimeAsync();
                }
                else
                {
                    MessageBox.Show("Выберите мелодию");
                }
            }

            if (CurrentMode == "System.Windows.Controls.ComboBoxItem: Поподание в ритм")
            {
                _inRitm = !_inRitm;
                lastClickTime = DateTime.Now;
                UpdateTimeAsync();
            }
        }

        #endregion

        public ViewModelMainWindow(IJsonReader Reader, UserStatisticViewModel userStat, IRepository<MelStat> melStatRep, IRepository<TempStat> tempRep) 
        {
            MakeSound = new RegularCommand(MakeSoundExecute, CanMakeSoundExecute);
            StartCommand = new SimpleCommand(StartExecute, CanStartExecute);
            MelodyRepository = (Repository<MelStat>)melStatRep;
            TempRepository = (Repository<TempStat>)tempRep;
            _reader = Reader;
            Melodies = _reader.GetDictionaryNames();
            CurrentTime = 0;
            RitmVisibility = Visibility.Hidden;
            MelodyVisibility = Visibility.Hidden;
        }
    }
}
