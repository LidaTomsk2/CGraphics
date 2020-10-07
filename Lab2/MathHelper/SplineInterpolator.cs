using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.MathHelper
{
    public sealed class SplineInterpolator
    {
        private readonly SortedList<double, double> _points = new SortedList<double, double>();
        private double[] _y2;

        public int Count
        {
            get
            {
                return _points.Count;
            }
        }

        public void Add(double x, double y)
        {
            _points[x] = y;
            _y2 = null;
        }

        public void Clear()
        {
            _points.Clear();
        }

        public double Interpolate(double x)
        {
            if (_y2 == null)
            {
                PreCompute();
            }

            var xa = _points.Keys;
            var ya = _points.Values;

            int n = ya.Count;
            var klo = 0;
            var khi = n - 1;

            while (khi - klo > 1)
            {
                int k = (khi + klo) >> 1;

                if (xa[k] > x)
                {
                    khi = k;
                }
                else
                {
                    klo = k;
                }
            }

            var h = xa[khi] - xa[klo];
            var a = (xa[khi] - x) / h;
            var b = (x - xa[klo]) / h;

            return a * ya[klo] + b * ya[khi] +
                ((Math.Pow(a, 3) - a) * _y2[klo] + (b * b * b - b) * _y2[khi]) * (h * h) / 6.0;
        }

        private void PreCompute()
        {
            var n = _points.Count;
            var u = new double[n];
            var xa = _points.Keys;
            var ya = _points.Values;

            _y2 = new double[n];

            u[0] = 0;
            _y2[0] = 0;

            for (int i = 1; i < n - 1; ++i)
            {
                double wx = xa[i + 1] - xa[i - 1];
                var sig = (xa[i] - xa[i - 1]) / wx;
                var p = sig * _y2[i - 1] + 2.0;

                _y2[i] = (sig - 1.0) / p;

                var ddydx = (ya[i + 1] - ya[i])/(xa[i + 1] - xa[i]) -
                            (ya[i] - ya[i - 1])/(xa[i] - xa[i - 1]);

                u[i] = (3.0 * ddydx / wx - sig * u[i - 1]) / p;
            }

            _y2[n - 1] = 0;

            for (int i = n - 2; i >= 0; --i)
            {
                _y2[i] = _y2[i] * _y2[i + 1] + u[i];
            }
        }
    }
}
