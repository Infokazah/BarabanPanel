using BarabanPanel.Infrastructure.Commands;
using BarabanPanel.Infrastructure.Commands.Base;
using BarabanPanel.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BarabanPanel.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        public GetInRitmViewModel _getInRitmViewModel { get; }
        public GetInMelodyViewModel _getInMelodyViewModel { get; }

        private bool _isLooping = false;


        private void LoopSound()
        {
            while (_isLooping)
            {
                try
                {
                    _soundPlayer.PlayLooping();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка воспроизведения звука: " + ex.Message);
                }
            }
        }
        #region Комманды

        public CommandBase ToggleLoop { get; }

        private bool CanToggleLoopExecute(object p) => true;

        private void ToggleLoopExecute(object parameter)
        {
            _isLooping = !_isLooping;

            if (_isLooping)
            {
                Task.Run(() => LoopSound());
            }
        }

        private SoundPlayer _soundPlayer;
        public CommandBase MakeSound { get; }
        private bool CanMakeSoundExecute(object p) => true;

        private void MakeSoundExecute(object filePath)
        {
            if (filePath is string path)
            {
                try
                {
                    _soundPlayer.SoundLocation=path;
                    _soundPlayer.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка воспроизведения звука: " + ex.Message);
                }
            }
        }
        #endregion
        public ViewModelMainWindow() 
        {
            _getInRitmViewModel = new GetInRitmViewModel(this);
            _getInMelodyViewModel = new GetInMelodyViewModel(this); 
            _soundPlayer = new SoundPlayer();
            MakeSound = new RegularCommand(MakeSoundExecute, CanMakeSoundExecute);
            ToggleLoop = new RegularCommand(ToggleLoopExecute, CanToggleLoopExecute);
        }
    }
}
