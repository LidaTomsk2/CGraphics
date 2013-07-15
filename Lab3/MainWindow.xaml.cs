using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using Microsoft.Win32;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Drawing.Image;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Point> _firstPoints = new List<Point>();
        private readonly List<Point> _secondPoints = new List<Point>();
        private Uri _url;
        public MainWindow()
        {
            InitializeComponent();
            canvas1.MouseLeftButtonDown += Canvas1OnMouseLeftButtonDown;
            canvas2.MouseLeftButtonDown += Canvas2OnMouseLeftButtonDown;
        }

        private Rectangle GetRect(MouseButtonEventArgs args)
        {
            var rect = new Rectangle { Width = 5, Height = 5, Fill = Brushes.Black };
            Canvas.SetLeft(rect, args.GetPosition((Canvas)args.Source).X);
            Canvas.SetTop(rect, args.GetPosition((Canvas)args.Source).Y);
            return rect;
        }

        private void Canvas1OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_firstPoints.Count == 3)
            {
                _firstPoints.RemoveAt(0);
                canvas1.Children.RemoveAt(0);
            }
            _firstPoints.Add(mouseButtonEventArgs.GetPosition(img1));
            var rect = GetRect(mouseButtonEventArgs);
            canvas1.Children.Add(rect);
        }

        private void Canvas2OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_secondPoints.Count == 3)
            {
                _secondPoints.RemoveAt(0);
                canvas1.Children.RemoveAt(0);
            }
            _secondPoints.Add(mouseButtonEventArgs.GetPosition(img2));
            var rect = GetRect(mouseButtonEventArgs);
            canvas2.Children.Add(rect);
        }

        private void MenuItemOpenClick(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog {Filter = "Изображения (*.bmp, *.jpg)|*.bmp;*.jpg"};
            if (openDialog.ShowDialog().Value)
            {
                _url = new Uri(openDialog.FileName);
                img1.Source = new BitmapImage(_url);

                canvas1.Width = canvas2.Width = img1.Width;
                canvas1.Height = canvas2.Height = img1.Height;
            }
        }

        private void MenuItemWorkClick(object sender, RoutedEventArgs e)
        {
            if (_firstPoints.Count == 3 && _secondPoints.Count == 3)
            {
                using (var src = new Bitmap(_url.AbsolutePath))
                using (var bmp = new Bitmap(src.Width, src.Height))
                using (var gr = Graphics.FromImage(bmp))
                {
                    gr.Clear(System.Drawing.Color.White);
                    var matr = AffineTrasformations.GetWarpMatrix(_firstPoints, _secondPoints);
                    gr.MultiplyTransform(matr);
                    gr.DrawImage(src, new System.Drawing.Rectangle(0, 0, src.Width, src.Height));

                    var bi = new BitmapImage();
                    bi.BeginInit();
                    var ms = new MemoryStream();
                    bmp.Save(ms, ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms;
                    bi.EndInit();

                    img2.Source = bi;
                }
                ClearCanvas();
            }
        }

        private void ClearCanvas()
        {
            _firstPoints.Clear();
            _secondPoints.Clear();
            canvas1.Children.Clear();
            canvas2.Children.Clear();
        }
    }
}
