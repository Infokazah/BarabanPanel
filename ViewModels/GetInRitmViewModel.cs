﻿using BarabanPanel.Infrastructure.Commands.Base;
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
        #region Атрибуты
        private bool _inRitm = false;
        private ViewModelMainWindow _MainViewModel { get; }

        private DateTime lastClickTime;

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
        public CommandBase StartCommand { get; }
        private bool CanStartExecute(object p) => true;

        private void StartExecute(object filePath)
        {
            _inRitm = !_inRitm;
            CurrentText = "End";
            lastClickTime = DateTime.Now;
            UpdateTimeAsync();
        }

        public CommandBase CheckTime { get; }
        private bool CanCheckTimeExecute(object p) => true;

        private void CheckTimeExecute(object filePath)
        {
            if(_inRitm == true)
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
            }
            
        }
        #endregion

        public GetInRitmViewModel() : this(null)
        {
            CheckTime = new RegularCommand(CheckTimeExecute, CanCheckTimeExecute);
        }
        public GetInRitmViewModel(ViewModelMainWindow MainModel) 
        {
            _MainViewModel = MainModel;
            CheckTime = new RegularCommand(CheckTimeExecute, CanCheckTimeExecute);
            StartCommand = new RegularCommand(StartExecute, CanStartExecute);
            CurrentTime = 0;
        }
        
    }
}
