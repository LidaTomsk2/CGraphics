using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lab2.DrawElements.Controls;
using Lab2.DrawElements.Lines;

namespace Lab2.DrawElements
{
    public class LineTransformManager
    {
        private readonly Canvas _canvas;

        public LineTransformManager(Canvas canvas)
        {
            _canvas = canvas;
        }

        private LineSimple CreateLine()
        {
            var line = new LineSimple();
            line.SplitPressEventHandler += LineOnSplitPressEventHandler;
            line.MouseRightButtonDown += (sender, args) => TransformToBezier(line);

            return line;
        }

        public LineSimple CreateLineSimple(Point fromPoint, Point targetPoint)
        {
            var line = CreateLine();
            var connector1 = new LineConnector(null, line, fromPoint);
            var connector2 = new LineConnector(line, null, targetPoint);

            _canvas.Children.Add(line);
            _canvas.Children.Add(connector1);
            _canvas.Children.Add(connector2);

            return line;
        }

        private void LineOnSplitPressEventHandler(object sender, EventArgs e)
        {
            var line = (LineSimple) sender;
            var clickPoint = ((MouseButtonEventArgs) e).GetPosition(_canvas);

            _canvas.Children.Remove(line);

            var newLineLeft = CreateLine();
            var newLineRight = CreateLine();

            var connector = new LineConnector(newLineLeft, newLineRight, clickPoint);
            
            line.LeftLineConnector.RightFigure = newLineLeft;
            line.LeftLineConnector.UpdateLinks();
            line.RightLineConnector.LeftFigure = newLineRight;
            line.RightLineConnector.UpdateLinks();

            _canvas.Children.Add(newLineLeft);
            _canvas.Children.Add(newLineRight);
            _canvas.Children.Add(connector);
        }

        public void TransformToLineSimple(LineFigure figure)
        {
            //TODO
        }

        public void TransformToBezier(LineFigure figure)
        {
            _canvas.Children.Remove(figure);

            var bezierLine = new BezierLine();
            figure.LeftLineConnector.RightFigure = bezierLine;
            figure.LeftLineConnector.UpdateLinks();
            figure.RightLineConnector.LeftFigure = bezierLine;
            figure.RightLineConnector.UpdateLinks();
            
            var bezCont1 = new BezierControl(bezierLine, false);
            var bezCont2 = new BezierControl(bezierLine, true);

            _canvas.Children.Add(bezierLine);
            _canvas.Children.Add(bezCont1);
            _canvas.Children.Add(bezCont2);
        }

        public void TransformToSpline(LineFigure figure)
        {
            //TODO
        }
    }
}
