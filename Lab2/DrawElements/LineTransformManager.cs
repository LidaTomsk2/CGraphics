using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
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
            line.MouseRightButtonDown += (sender, args) =>
                                         {
                                             if (Keyboard.IsKeyDown(Key.LeftAlt))
                                             {
                                                 TransformToSpline(line);   
                                             }
                                             else
                                             {
                                                 TransformToBezier(line);
                                             }
                                         };

            return line;
        }

        private Spline CreateSpline()
        {
            var spline = new Spline();
            spline.MouseLeftButtonDown += (sender, args) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftAlt))
                    ConvertSplineToLine(spline, args.GetPosition(_canvas));
            };

            return spline;
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
            var lines = _canvas.Children.OfType<LineFigure>().ToList();
            foreach (var lineFigure in lines)
            {
                _canvas.Children.Remove(lineFigure);
            }

            var spline = CreateSpline();

            var connector = _canvas.Children.OfType<LineConnector>().First(x => x.LeftFigure == null);
            var firstConnector = connector;
            var lastConnector = _canvas.Children.OfType<LineConnector>().First(x => x.RightFigure == null);

            var cons = new List<LineConnector>() {connector};
            while (!Equals(connector, lastConnector))
            {
                cons.Add(connector.RightFigure.RightLineConnector);
                connector.LeftFigure = spline;
                connector.UpdateLinks();
                connector = connector.RightFigure.RightLineConnector;
            }
            foreach (var lineCon in _canvas.Children.OfType<LineConnector>())
            {
                lineCon.RightFigure = spline;
                lineCon.UpdateLinks();
            }

            firstConnector.LeftFigure = null;
            firstConnector.RightFigure = spline;
            firstConnector.UpdateLinks();
            connector.LeftFigure = spline;
            connector.RightFigure = null;
            connector.UpdateLinks();
            

            spline.Connectors = cons;

            _canvas.Children.Add(spline);
        }

        private void ConvertSplineToLine(Spline spline, Point clickPoint)
        {
            LineConnector fromConnector = null, toConnector = null;
            for (int i = 0; i < spline.Connectors.Count - 1; i++)
            {
                if (spline.Connectors[i].PosPoint.X < clickPoint.X && spline.Connectors[i + 1].PosPoint.X > clickPoint.X)
                {
                    fromConnector = spline.Connectors[i];
                    toConnector = spline.Connectors[i + 1];
                    break;
                }
            }

            _canvas.Children.Remove(spline);
            var pointsLeftSpline = spline.Connectors.TakeWhile(x => !Equals(x, fromConnector)).ToList();
            pointsLeftSpline.Add(fromConnector);

            var pointsRightSpline = spline.Connectors.Except(pointsLeftSpline).ToList();

            // левый сплайн
            var leftSpline = CreateSpline();
            leftSpline.Connectors = pointsLeftSpline;
            var firstCon = pointsLeftSpline.First();
            firstCon.LeftFigure = null;
            firstCon.RightFigure = leftSpline;
            firstCon.UpdateLinks();
            foreach (var lineConnector in pointsLeftSpline.Where(x => !Equals(x, firstCon)))
            {
                lineConnector.LeftFigure = leftSpline;
                lineConnector.RightFigure = leftSpline;
                lineConnector.UpdateLinks();
            }
            _canvas.Children.Add(leftSpline);

            // правый сплайн
            var rightSpline = CreateSpline();
            rightSpline.Connectors = pointsRightSpline;
            firstCon = pointsRightSpline.First();
            firstCon.RightFigure = rightSpline;
            firstCon.UpdateLinks();
            foreach (var lineConnector in pointsRightSpline.Where(x => !Equals(x, firstCon)))
            {
                lineConnector.LeftFigure = rightSpline;
                lineConnector.RightFigure = rightSpline;
                lineConnector.UpdateLinks();
            }
            var lastCon = pointsRightSpline.Last();
            lastCon.RightFigure = null;
            lastCon.UpdateLinks();
            _canvas.Children.Add(rightSpline);

            // строим линию
            var line = CreateLine();
            line.LeftLineConnector = fromConnector;
            line.LeftLineConnector.LeftFigure = leftSpline;
            line.LeftLineConnector.RightFigure = line;
            line.LeftLineConnector.UpdateLinks();

            line.RightLineConnector = toConnector;
            line.RightLineConnector.LeftFigure = line;
            line.RightLineConnector.RightFigure = rightSpline;
            line.RightLineConnector.UpdateLinks();
            _canvas.Children.Add(line);
        }
    }
}
