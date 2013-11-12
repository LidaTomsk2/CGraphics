 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab4
{
    public class LFigure : Shape
    {
        enum RotateDirection
        {
            Left,
            Right
        }

        public Rect FigureBound { get; set; }

        private readonly string _path;
        private readonly int _stepSize;
        private readonly double _angle;
        private Point _dirPoint;
        private bool _firstCall;

        public LFigure(string path, int stepSize, double angle)
        {
            _path = path;
            _stepSize = stepSize;
            _angle = angle;

            _firstCall = true;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };
                if (_firstCall)
                {
                    _firstCall = false;
                    return geometry;
                }
                using (var context = geometry.Open())
                {
                    InternalDrawGeometry(context);
                }

                geometry.Freeze();
                FigureBound = geometry.Bounds;
                return geometry;
            }
        }

        private void InternalDrawGeometry(StreamGeometryContext context)
        {
            _dirPoint = new Point(0, 0);
            double angle = 0;
            var curPoint = new Point(0, 0);

            var memSaveStack = new Stack<Tuple<Point, double>>();

            context.BeginFigure(curPoint, true, false);
            foreach (var symb in _path)
            {
                switch (symb)
                {
                    case 'F':
                        curPoint.X += _dirPoint.X*_stepSize;
                        curPoint.Y += _dirPoint.Y*_stepSize;
                        context.LineTo(curPoint, true, true);
                        break;

                    case '+':
                        angle = Rotate(angle, RotateDirection.Right);
                        break;

                    case '-':
                        angle = Rotate(angle, RotateDirection.Left);
                        break;

                    case 'b':
                        curPoint.X += _dirPoint.X*_stepSize;
                        curPoint.Y += _dirPoint.Y*_stepSize;
                        context.BeginFigure(curPoint, true, false);
                        break;

                    case '[':
                        memSaveStack.Push(new Tuple<Point, double>(curPoint, angle));
                        break;

                    case ']':
                        var state = memSaveStack.Pop();
                        curPoint = state.Item1;
                        angle = state.Item2;
                        context.BeginFigure(curPoint, true, false);
                        break;
                }
            }

            context.Close();
        }
        
        private double Rotate(double angle, RotateDirection direction)
        {
            angle = direction == RotateDirection.Left ? angle - _angle : angle + _angle;

            if (angle >= 360 || angle <= -360)
            {
                angle = 0;
            }

            _dirPoint.X = Math.Cos(angle * Math.PI / 180);
            _dirPoint.Y = Math.Sin(angle * Math.PI / 180);
            return angle;
        }
    }
}
