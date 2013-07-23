using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Lab3.Filtering
{
    class BilinealFiltering : IFilter
    {
        private readonly Bitmap _bmp;
        private readonly Matrix<double> _warpMatrix;

        public BilinealFiltering(Bitmap bmp, Matrix<double> warpMatrix)
        {
            _bmp = bmp;
            _warpMatrix = warpMatrix;
        }

        public Bitmap GetFilteredImage()
        {
            var resultBmp = new Bitmap(_bmp.Width, _bmp.Height);
            for (int i = 0; i < resultBmp.Width; i++)
            {
                for (int j = 0; j < resultBmp.Height; j++)
                {
                    var xNew = _warpMatrix[0, 0] + _warpMatrix[0, 1] * i + _warpMatrix[0, 2] * j;
                    var yNew = _warpMatrix[1, 0] + _warpMatrix[1, 1] * i + _warpMatrix[1, 2] * j;

                    if (xNew < 0 || Math.Ceiling(xNew) >= resultBmp.Width ||
                        yNew < 0 || Math.Ceiling(yNew) >= resultBmp.Height) continue;

                    var color = GetBilinearColor(xNew, yNew);
                    resultBmp.SetPixel(i, j, color);
                }
            }
            return resultBmp;
        }

        public Color GetBilinearColor(double x, double y)
        {
            int xl = (int)x, xh = xl + 1;
            int yl = (int)y, yh = yl + 1;

            var c1 = _bmp.GetPixel(xl, yl).Multiply(xh - x);
            var c2 = _bmp.GetPixel(xh, yl).Multiply(x - xl);
            var c3 = c1.Add(c2).Multiply(yh - y);

            var c4 = _bmp.GetPixel(xl, yh).Multiply(xh - x);
            var c5 = _bmp.GetPixel(xh, yh).Multiply(x - xl);
            var c6 = c4.Add(c5).Multiply(y - yl);

            return c3.Add(c6);
        }
    }
}
