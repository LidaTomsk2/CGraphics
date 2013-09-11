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

namespace Lab2.DrawElements.Controls
{
    /// <summary>
    /// Interaction logic for LineConnector.xaml
    /// </summary>
    public partial class LineConnector : UserControl
    {
        public LineFigure LeftFigure { get; set; }
        public LineFigure RightFigure { get; set; }
        public Point PosPoint { get; set; }

        public LineConnector(LineFigure leftFigure, LineFigure rightFigure, Point point)
        {
            LeftFigure = leftFigure;
            RightFigure = rightFigure;
            PosPoint = point;

            InitializeComponent();
            UpdateLinks();

            MouseMove += OnMouseMove;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        public void UpdateLinks()
        {
            UpdatePos();

            if (LeftFigure != null)
            {
                LeftFigure.RightLineConnector = this;
                Panel.SetZIndex(this, Math.Max(Panel.GetZIndex(this), Panel.GetZIndex(LeftFigure) + 1));
            }

            if (RightFigure != null)
            {
                RightFigure.LeftLineConnector = this;
                Panel.SetZIndex(this, Math.Max(Panel.GetZIndex(this), Panel.GetZIndex(RightFigure) + 1));
            }
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

            if (LeftFigure != null) LeftFigure.InvalidateVisual();
            if (RightFigure != null) RightFigure.InvalidateVisual();
        }

        private void UpdatePos()
        {
            Canvas.SetLeft(this, PosPoint.X - Width / 2);
            Canvas.SetTop(this, PosPoint.Y - Height / 2);
        }
    }
}
