using BarabanPanel.Infrastructure.Commands;
using BarabanPanel.Infrastructure.Commands.Base;
using BarabanPanel.Models;
using BarabanPanel.Services;
using BarabanPanel.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BarabanPanel.ViewModels
{
    class GetInMelodyViewModel : ViewModelBase
    {
        private ViewModelMainWindow _MainViewModel;

        private JsonReader _reader;

        private string _melodyName;
        public string MelodyName 
        {
            get => _melodyName;

            set
            {   if(value != null)
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
        public CommandBase StartMelody { get; }
        private bool CanStartMelodyExecute(object p) => true;

        private void StartMelodyExecute(object melody)
        {
           if(melody != null)
           {
                _reader.GetMelody(melody.ToString());
           }
        }
        public CommandBase CheckTime { get; }
        private bool CanCheckTimeExecute(object p) => true;

        private void CheckTimeExecute(object filePath)
        {
           /* if (_inRitm == true)
            {
                TimeSpan elapsedTime = DateTime.Now - lastClickTime;

                if (elapsedTime.TotalSeconds > 4 || elapsedTime.TotalSeconds < 3)
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
            }*/

        }

        public GetInMelodyViewModel() : this(null)
        {
            JsonReader reader = new JsonReader();
            Melodies = reader.GetDictionaryNames();
            StartMelody = new RegularCommand(StartMelodyExecute, CanStartMelodyExecute);
        }
        public GetInMelodyViewModel(ViewModelMainWindow MainModel)
        {
            _MainViewModel = MainModel;
            _reader= new JsonReader();
            Melodies = _reader.GetDictionaryNames();
            StartMelody = new RegularCommand(StartMelodyExecute, CanStartMelodyExecute);
        }
    }
}
