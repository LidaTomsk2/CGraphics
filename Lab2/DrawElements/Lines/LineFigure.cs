using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Lab2.DrawElements.Controls;

namespace Lab2.DrawElements.Lines
{
    public abstract class LineFigure : Shape
    {
        public Point FromPoint
        {
            get { return LeftLineConnector.PosPoint; }
        }

        public Point TargetPoint
        {
            get { return RightLineConnector.PosPoint; }
        }

        public LineConnector LeftLineConnector { get; set; }
        public LineConnector RightLineConnector { get; set; }

        protected LineFigure()
        {
            Stroke = Brushes.Black;
            StrokeThickness = 2;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var geometry = new StreamGeometry();
                using (var context = geometry.Open())
                {
                    DrawGeometry(context);
                }
                geometry.Freeze();
                return geometry;
            }
        }

        protected abstract void DrawGeometry(StreamGeometryContext context);
    }
}
