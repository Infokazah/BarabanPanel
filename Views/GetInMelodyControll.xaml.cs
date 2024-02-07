using BarabanPanel.Models;
using BarabanPanel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarabanPanel.Views
{
    /// <summary>
    /// Логика взаимодействия для GetInMelodyControll.xaml
    /// </summary>
    public partial class GetInMelodyControll : UserControl
    {
        private GetInMelodyViewModel viewModel;
        private IEnumerable<string> paths;
        public GetInMelodyControll()
        {
            InitializeComponent();
            viewModel = (GetInMelodyViewModel)DataContext;
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null && viewModel.CurrentMelody != null)
            {
                Melody _melody = viewModel.CurrentMelody;
                foreach (var item in _melody.BarabanParts)
                {
                    try
                    {
                        SoundPlayer soundPlayer = new SoundPlayer();

                        // Установите путь к вашему звуковому файлу
                       

                        // Воспроизведите звук во время анимации
                        

                        Storyboard myAnimation = (Storyboard)FindName(item.BarabanNumber);

                        if (myAnimation != null)
                        {
                            myAnimation.Begin();
                            soundPlayer.SoundLocation = "путь_к_вашему_звуковому_файлу.wav";
                            soundPlayer.Play();
                        }
                        else
                        {
                            MessageBox.Show($"Анимация для элемента {item.BarabanNumber} не найдена.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при выполнении анимации: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("ViewModel или Melody не инициализированы.");
            }
        }*/
    }
}
