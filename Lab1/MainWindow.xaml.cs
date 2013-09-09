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

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly WriteableBitmap _bmp;
        private Point _fromPoint;
        private readonly LineDrawer _lineDrawer;
        private bool _isLeftBtnPressed = false;

        public MainWindow()
        {
            InitializeComponent();

            _bmp = BitmapFactory.New(640, 480);
            _bmp.Clear();
            image.Source = _bmp;
            _lineDrawer = new LineDrawer(_bmp);

            image.MouseLeftButtonDown += ImageOnMouseLeftButtonDown;
            image.MouseLeftButtonUp += ImageOnMouseLeftButtonUp;
            MouseMove += OnMouseMove;
            dudPixelSize.ValueChanged += DudPixelSizeValueChanged;
            cpLineColor.SelectedColorChanged += CpLineColorSelectedColorChanged;
            btnClear.Click += (sender, args) => _bmp.Clear();
        }

        private void ImageOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _isLeftBtnPressed = false;
        }

        private byte[] _prevBytes;
        private void ImageOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _isLeftBtnPressed = true;
            _fromPoint = mouseButtonEventArgs.GetPosition(image);
            _prevBytes = _bmp.ToByteArray();
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_isLeftBtnPressed == false) return;
            _bmp.FromByteArray(_prevBytes);

            var point = mouseEventArgs.GetPosition(image);
            if (point.X >= image.ActualWidth || point.X <= 0 || point.Y >= image.ActualHeight || point.Y <= 0) return;

            int x1 = (int)_fromPoint.X, y1 = (int)_fromPoint.Y;
            int x2 = (int)point.X, y2 = (int)point.Y;

            switch (cbAlgType.SelectedIndex)
            {
                case 0:
                    _lineDrawer.DrawLineBrezenham(x1, y1, x2, y2);
                    return;
                case 1:
                    _lineDrawer.DrawLineWu(x1, y1, x2, y2);
                    return;
                case 2:
                    _lineDrawer.DrawLineTrippleAliasing(x1, y1, x2, y2);
                    return;
            }
        }

        private void DudPixelSizeValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _lineDrawer.PixelSize = Convert.ToByte(dudPixelSize.Value);
        }

        private void CpLineColorSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            _lineDrawer.CurrentColor = cpLineColor.SelectedColor;
        }
    }
}
