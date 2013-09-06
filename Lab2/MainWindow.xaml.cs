using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab2.DrawElements.Lines;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var line = new Line {X1 = 10, Y1 = 10, X2 = 100, Y2 = 100, StrokeThickness = 2, Stroke = Brushes.Black};
            line.DataContext = new LineItem
                               {
                                   StartPoint = new Point(line.X1, line.Y1),
                                   EndPoint = new Point(line.X2, line.Y2)
                               };
            var adorner = new LineAdorner(line);
            DrawCanvas.Children.Add(line);
            DrawCanvas.Children.Add(adorner);
        }
    }
}
