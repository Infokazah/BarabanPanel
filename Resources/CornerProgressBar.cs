using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BarabanPanel.Resources
{
    class CornerProgressBar : ProgressBar
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CornerProgressBar), new PropertyMetadata(new CornerRadius()));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty RectangleCornerRadiusProperty =
            DependencyProperty.Register("RectangleCornerRadius", typeof(double), typeof(CornerProgressBar), new PropertyMetadata(new double()));

        public double RectangleCornerRadius
        {
            get { return (double)GetValue(RectangleCornerRadiusProperty); }
            set { SetValue(RectangleCornerRadiusProperty, value); }
        }


        static CornerProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CornerProgressBar), new FrameworkPropertyMetadata(typeof(CornerProgressBar)));
        }


    }
}
