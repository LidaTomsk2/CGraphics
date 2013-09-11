using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Lab2.DrawElements.Controls;

namespace Lab2.DrawElements.Lines
{
    public class BezierLine : LineFigure
    {
        public BezierControl LeftBezierControl { get; set; }
        public BezierControl RightBezierControl { get; set; }
        
        protected override void DrawGeometry(StreamGeometryContext context)
        {
            context.BeginFigure(FromPoint, true, false);
            
            for (float step = 0; step <= 1; step += 0.01f)
            {
                context.LineTo(GetPoint(step, FromPoint, TargetPoint), true, true);
            }
        }

        private Point GetPoint(double t, Point p0, Point p3)
        {
            var p1 = LeftBezierControl.PosPoint;
            var p2 = RightBezierControl.PosPoint;

            var cx = 3 * (p1.X - p0.X);
            var cy = 3 * (p1.Y - p0.Y);

            var bx = 3 * (p2.X - p1.X) - cx;
            var by = 3 * (p2.Y - p1.Y) - cy;

            var ax = p3.X - p0.X - cx - bx;
            var ay = p3.Y - p0.Y - cy - by;

            var cube = Math.Pow(t, 3);
            var square = Math.Pow(t, 2);

            var resX = (ax * cube) + (bx * square) + (cx * t) + p0.X;
            var resY = (ay * cube) + (by * square) + (cy * t) + p0.Y;
            
            return new Point(resX, resY);
        }
    }
}
