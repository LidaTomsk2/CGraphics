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

        // Interpolate() and PreCompute() are adapted from:
        // NUMERICAL RECIPES IN C: THE ART OF SCIENTIFIC COMPUTING
        // ISBN 0-521-43108-5, page 113, section 3.3.

        public double Interpolate(double x)
        {
            if (_y2 == null)
            {
                PreCompute();
            }

            var xa = _points.Keys;
            var ya = _points.Values;

            int n = ya.Count;
            var klo = 0;     // We will find the right place in the table by means of
            var khi = n - 1; // bisection. This is optimal if sequential calls to this

            while (khi - klo > 1)
            {
                // routine are at random values of x. If sequential calls
                int k = (khi + klo) >> 1;// are in order, and closely spaced, one would do better

                if (xa[k] > x)
                {
                    khi = k; // to store previous values of klo and khi and test if
                }
                else
                {
                    klo = k;
                }
            }

            double h = xa[khi] - xa[klo];
            double a = (xa[khi] - x) / h;
            double b = (x - xa[klo]) / h;

            // Cubic spline polynomial is now evaluated.
            return a * ya[klo] + b * ya[khi] +
                ((a * a * a - a) * _y2[klo] + (b * b * b - b) * _y2[khi]) * (h * h) / 6.0;
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
                // This is the decomposition loop of the tridiagonal algorithm. 
                // y2 and u are used for temporary storage of the decomposed factors.
                double wx = xa[i + 1] - xa[i - 1];
                double sig = (xa[i] - xa[i - 1]) / wx;
                double p = sig * _y2[i - 1] + 2.0;

                _y2[i] = (sig - 1.0) / p;

                double ddydx =
                    (ya[i + 1] - ya[i]) / (xa[i + 1] - xa[i]) -
                    (ya[i] - ya[i - 1]) / (xa[i] - xa[i - 1]);

                u[i] = (6.0 * ddydx / wx - sig * u[i - 1]) / p;
            }

            _y2[n - 1] = 0;

            // This is the backsubstitution loop of the tridiagonal algorithm
            for (int i = n - 2; i >= 0; --i)
            {
                _y2[i] = _y2[i] * _y2[i + 1] + u[i];
            }
        }
    }
}
