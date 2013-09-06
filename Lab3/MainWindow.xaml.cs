using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Lab3.Filtering;
using Microsoft.Win32;
using Brushes = System.Windows.Media.Brushes;
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

        private static Rectangle GetRect(MouseEventArgs args)
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
                canvas2.Children.RemoveAt(0);
            }
            _secondPoints.Add(mouseButtonEventArgs.GetPosition(img2));
            var rect = GetRect(mouseButtonEventArgs);
            canvas2.Children.Add(rect);
        }

        private void MenuItemOpenClick(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog { Filter = "Изображения (*.bmp, *.jpg)|*.bmp;*.jpg" };
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
            if (_firstPoints.Count != 3 || _secondPoints.Count != 3) return;

            using (var src = new Bitmap(_url.AbsolutePath))
            {
                var warpMatr = WarpMatrix.GetWarpMatrix(_firstPoints, _secondPoints);

                var filterType = FilterType.Bilinear;
                IFilter filter;
                switch (filterType)
                {
                    case FilterType.None:
                        filter = new NoneFiltering(src, warpMatr);
                        break;

                    case FilterType.Bilinear:
                        filter = new BilinealFiltering(src, warpMatr);
                        break;

                    case FilterType.Trilinear:
                        filter = new TrilinearFiltering(src, warpMatr);
                        break;

                    default:
                        filter = new NoneFiltering(src, warpMatr);
                        break;
                }

                img2.Source = ImageCreator.GetImage(filter);
            }
            ClearCanvas();
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