using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab2.DrawElements.Lines
{
    public class LineAdorner : Adorner
    {
        bool IsStartPoint = false;
        bool IsControlModeOn = false;
        Size size = new Size(10, 10);
        SnapToGrid snap = new SnapToGrid();

        public LineAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(LineAdorner_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(LineAdorner_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(LineAdorner_MouseMove);
        }

        void LineAdorner_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
                IsControlModeOn = true;

            Line line = this.AdornedElement as Line;
            LineItem lineItem = line.DataContext as LineItem;
            Point p = snap.Snap(e.GetPosition(line), SnapToGrid.SnapMode.Move);

            double dStart = 0.0;
            double dEnd = 0.0;

            if (!this.IsMouseCaptured)
            {
                dStart = Math.Sqrt(Math.Pow(lineItem.StartPoint.X - p.X, 2) + Math.Pow(lineItem.StartPoint.Y - p.Y, 2));
                dEnd = Math.Sqrt(Math.Pow(lineItem.EndPoint.X - p.X, 2) + Math.Pow(lineItem.EndPoint.Y - p.Y, 2));
            }

            if (IsControlModeOn)
            {
                if (this.IsMouseCaptured)
                {
                    if (IsStartPoint)
                        lineItem.StartPoint = p;
                    else
                        lineItem.EndPoint = p;

                    this.InvalidateVisual();
                    this.ReleaseMouseCapture();

                    IsControlModeOn = false;
                }
                else
                {
                    if (dStart < dEnd)
                        IsStartPoint = true;
                    else
                        IsStartPoint = false;

                    this.InvalidateVisual();
                    this.CaptureMouse();
                }
            }
            else
            {
                if (!this.IsMouseCaptured)
                {
                    if (dStart < dEnd)
                        IsStartPoint = true;
                    else
                        IsStartPoint = false;

                    this.InvalidateVisual();
                    this.CaptureMouse();
                }
            }
        }

        void LineAdorner_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsControlModeOn)
            {
                if (this.IsMouseCaptured)
                {
                    Line line = this.AdornedElement as Line;
                    LineItem lineItem = line.DataContext as LineItem;
                    Point p = snap.Snap(e.GetPosition(line), SnapToGrid.SnapMode.Move);

                    if (IsStartPoint)
                        lineItem.StartPoint = p;
                    else
                        lineItem.EndPoint = p;

                    this.InvalidateVisual();
                    this.ReleaseMouseCapture();
                }
            }
        }

        void LineAdorner_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                if (this.AdornedElement.GetType() == typeof(Line))
                {
                    Line line = this.AdornedElement as Line;
                    LineItem lineItem = line.DataContext as LineItem;
                    Point p = snap.Snap(e.GetPosition(line), SnapToGrid.SnapMode.Move);

                    // mode: move start or end point

                    //*
                    if (IsStartPoint)
                    {
                        line.X1 = p.X;
                        line.Y1 = p.Y;
                        lineItem.StartPoint = p;
                    }
                    else
                    {
                        line.X2 = p.X;
                        line.Y2 = p.Y;
                        lineItem.EndPoint = p;
                    }
                    //*/

                    // mode: move line

                    /*
                    if (IsStartPoint)
                    {
                        double dX = lineItem.StartPoint.X - p.X;
                        double dY = lineItem.StartPoint.Y - p.Y;
                        Point pEnd = new Point(lineItem.EndPoint.X - dX, lineItem.EndPoint.Y - dY);

                        lineItem.StartPoint = p;
                        lineItem.EndPoint = pEnd;
                    }
                    else
                    {
                        double dX = lineItem.EndPoint.X - p.X;
                        double dY = lineItem.EndPoint.Y - p.Y;
                        Point pStart = new Point(lineItem.StartPoint.X - dX, lineItem.StartPoint.Y - dY);

                        lineItem.EndPoint = p;
                        lineItem.StartPoint = pStart;
                    }
                    */

                    this.InvalidateVisual();
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.AdornedElement.GetType() == typeof(Line))
            {
                Line line = this.AdornedElement as Line;
                LineItem lineItem = line.DataContext as LineItem;

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                SolidColorBrush penBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                Pen pen = new Pen(penBrush, 1.0);

                Point p1 = new Point(lineItem.StartPoint.X, lineItem.StartPoint.Y);
                Point p2 = new Point(lineItem.EndPoint.X, lineItem.EndPoint.Y);

                p1.Offset(-size.Width / 2, -size.Height / 2);
                p2.Offset(-size.Width / 2, -size.Height / 2);

                Rect r1 = new Rect(p1, size);
                Rect r2 = new Rect(p2, size);

                double halfPenWidth = pen.Thickness / 2;

                GuidelineSet g1 = new GuidelineSet();
                g1.GuidelinesX.Add(r1.Left + halfPenWidth);
                g1.GuidelinesX.Add(r1.Right + halfPenWidth);
                g1.GuidelinesY.Add(r1.Top + halfPenWidth);
                g1.GuidelinesY.Add(r1.Bottom + halfPenWidth);
                drawingContext.PushGuidelineSet(g1);

                drawingContext.DrawRectangle(brush, pen, r1);
                drawingContext.Pop();

                GuidelineSet g2 = new GuidelineSet();
                g2.GuidelinesX.Add(r2.Left + halfPenWidth);
                g2.GuidelinesX.Add(r2.Right + halfPenWidth);
                g2.GuidelinesY.Add(r2.Top + halfPenWidth);
                g2.GuidelinesY.Add(r2.Bottom + halfPenWidth);
                drawingContext.PushGuidelineSet(g2);

                drawingContext.DrawRectangle(brush, pen, r2);
                drawingContext.Pop();
            }

            base.OnRender(drawingContext);
        }
    }

    class SnapToGrid
    {
        private Size gridSizeModeCreate = new Size(13.5, 13.5);
        private Size gridSizeModeMove = new Size(13.5, 13.5);
        private double gridOffsetX = -1.0;
        private double gridOffsetY = -1.0;

        public enum SnapMode
        {
            Create,
            Move,
            Line
        }

        public Size GridSizeModeCreate
        {
            get { return gridSizeModeCreate; }
            set
            {
                gridSizeModeCreate = value;
            }
        }

        public Size GridSizeModeMove
        {
            get { return gridSizeModeMove; }
            set
            {
                gridSizeModeMove = value;
            }
        }

        public double GridOffsetX
        {
            get { return gridOffsetX; }
            set
            {
                gridOffsetX = value;
            }
        }

        public double GridOffsetY
        {
            get { return gridOffsetY; }
            set
            {
                gridOffsetY = value;
            }
        }

        private Point Calculate(Point p, Size s)
        {
            double snapX = p.X + ((Math.Round(p.X / s.Width) - p.X / s.Width) * s.Width);
            double snapY = p.Y + ((Math.Round(p.Y / s.Height) - p.Y / s.Height) * s.Height);

            return new Point(snapX + gridOffsetX, snapY + gridOffsetY);
        }

        public Point Snap(Point p, SnapMode mode)
        {
            if (mode == SnapMode.Create)
                return Calculate(p, gridSizeModeCreate);
            else if (mode == SnapMode.Move)
                return Calculate(p, gridSizeModeMove);
            else
                return new Point(0, 0);
        }
    }

    public class LineItem : NotifyObject
    {
        private Point startPoint;
        private Point endPoint;

        public Point StartPoint
        {
            get
            {
                return startPoint;
            }
            set
            {
                if (startPoint != value)
                {
                    startPoint = value;
                    OnPropertyChanged("StartPoint");
                }
            }
        }

        public Point EndPoint
        {
            get
            {
                return endPoint;
            }
            set
            {
                if (endPoint != value)
                {
                    endPoint = value;
                    OnPropertyChanged("EndPoint");
                }
            }
        }
    }

    public class NotifyObject : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyChanged)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
