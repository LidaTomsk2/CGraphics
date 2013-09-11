using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Lab2.DrawElements.Controls;
using Lab2.MathHelper;

namespace Lab2.DrawElements.Lines
{
    public class Spline : LineFigure
    {
        public List<LineConnector> Connectors { get; set; }

        protected override void DrawGeometry(StreamGeometryContext context)
        {
            var interpolator = new SplineInterpolator();
            foreach (var lineConnector in Connectors)
            {
                interpolator.Add(lineConnector.PosPoint.X, lineConnector.PosPoint.Y);
            }

            var start = Connectors.Min(x => x.PosPoint.X);
            var end = Connectors.Max(x => x.PosPoint.X);

            var step = Math.Sign(end - start);
            var firstY = interpolator.Interpolate(start);
            context.BeginFigure(new Point(start, firstY), true, false);
            start += step;

            for (var x = start; x <= end; x += step)
            {
                var y = interpolator.Interpolate(x);
                context.LineTo(new Point(x, y), true, true);
            }
        }
    }
}
