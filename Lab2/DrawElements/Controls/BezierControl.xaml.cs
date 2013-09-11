using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lab2.DrawElements.Lines;

namespace Lab2.DrawElements.Controls
{
    /// <summary>
    /// Interaction logic for BezierControl.xaml
    /// </summary>
    public partial class BezierControl : UserControl
    {
        public BezierLine Figure { get; set; }
        public bool IsRight { get; set; }
        public Point PosPoint { get; set; }

        public BezierControl(BezierLine figure, bool isRight)
        {
            InitializeComponent();

            var leftPoint = figure.LeftLineConnector.PosPoint;
            var rightPoint = figure.RightLineConnector.PosPoint;
            Figure = figure;
            IsRight = isRight;

            if (IsRight)
            {
                figure.RightBezierControl = this;
            }
            else
            {
                figure.LeftBezierControl = this;
            }

            var mult = IsRight ? 2 : 1;
            PosPoint = new Point(leftPoint.X + (rightPoint.X - leftPoint.X)/3*mult,
                leftPoint.Y + (rightPoint.Y - leftPoint.Y)/3*mult);
            UpdatePos();

            MouseMove += OnMouseMove;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseCaptured) return;

            PosPoint = e.GetPosition((IInputElement)VisualParent);
            UpdatePos();
            
            Figure.InvalidateVisual();
        }

        private void UpdatePos()
        {
            Canvas.SetLeft(this, PosPoint.X - Width / 2);
            Canvas.SetTop(this, PosPoint.Y - Height / 2);
        }
    }
}
