using BarabanPanel.Models;
using BaseClassesLyb;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using WpfBaseLyb;

namespace BarabanPanel.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        public GetInRitmViewModel _getInRitmViewModel { get; }
        public GetInMelodyViewModel _getInMelodyViewModel { get; }

        private string _directory = Directory.GetCurrentDirectory();

        private bool _isLooping = false;


        private void LoopSound()
        {
            while (_isLooping)
            {
                try
                {
                    //_soundPlayer.PlayLooping();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка воспроизведения звука: " + ex.Message);
                }
            }
        }
        #region Комманды

        public SimpleCommand ToggleLoop { get; }

        private bool CanToggleLoopExecute() => true;

        private void ToggleLoopExecute()
        {
            _isLooping = !_isLooping;

            if (_isLooping)
            {
                Task.Run(() => LoopSound());
            }
        }

        
        private SoundManager _soundPlayer;
        public CommandBase MakeSound { get; }
        private bool CanMakeSoundExecute(object p) => true;

        private void MakeSoundExecute(object filePath)
        {
            if (filePath is string path)
            {
                try
                {
                    string soundPath = Path.Combine(_directory, path + ".wav");
                    _soundPlayer.PlaySoundAsync(soundPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка воспроизведения звука: " + ex.Message);
                }
            }
        }
        #endregion
        public ViewModelMainWindow(GetInRitmViewModel getInRitm, GetInMelodyViewModel getInMelody, UserStatisticViewModel userStat) 
        {
            _soundPlayer = new SoundManager();
            MakeSound = new RegularCommand(MakeSoundExecute, CanMakeSoundExecute);
            ToggleLoop = new SimpleCommand(ToggleLoopExecute, CanToggleLoopExecute);
        }
    }
}
