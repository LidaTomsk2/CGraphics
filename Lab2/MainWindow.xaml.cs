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
using Lab2.DrawElements;
using Lab2.DrawElements.Controls;
using Lab2.DrawElements.Lines;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LineTransformManager _lineManager;
        public MainWindow()
        {
            InitializeComponent();
            _lineManager = new LineTransformManager(DrawCanvas);

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _lineManager.CreateLineSimple(new Point(30, 30), new Point(100, 30));
        }
    }
}
