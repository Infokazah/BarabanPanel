
using System.Windows.Controls;


namespace BarabanPanel.Views
{
    /// <summary>
    /// Логика взаимодействия для GetInRitmViewModel.xaml
    /// </summary>
    public partial class GetInRitmControll : UserControl
    {
        private bool _inRitm = false;
        public GetInRitmControll()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _inRitm = !_inRitm;

            if(_inRitm)
            {
                StartButton.Content = "End";
            }
            else
            {
                StartButton.Content = "Start";
            }
        }
    }
}
