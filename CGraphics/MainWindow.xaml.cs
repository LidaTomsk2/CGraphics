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

namespace CGraphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLab1OpenClick(object sender, RoutedEventArgs e)
        {
            Hide();
            var lab1 = new Lab1.MainWindow();
            lab1.ShowDialog();
            Show();
        }

        private void BtnLab2OpenClick(object sender, RoutedEventArgs e)
        {
            Hide();
            var lab2 = new Lab2.MainWindow();
            lab2.ShowDialog();
            Show();
        }

        private void BtnLab3OpenClick(object sender, RoutedEventArgs e)
        {
            Hide();
            var lab3 = new Lab3.MainWindow();
            lab3.ShowDialog();
            Show();
        }

        private void BtnLab4OpenClick(object sender, RoutedEventArgs e)
        {
            Hide();
            var lab4 = new Lab4.MainWindow();
            lab4.ShowDialog();
            Show();
        }

        private void BtnLab5OpenClick(object sender, RoutedEventArgs e)
        {
            Hide();
            using (var lab5 = new Lab5.MainWindow(8))
            {
                lab5.Run();
            }
            Show();
        }
    }
}
