using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Lab3.Filtering
{
    class NoneFiltering : IFilter
    {
        private readonly Bitmap _bmp;
        private readonly Matrix<double> _warpMatrix;

        public NoneFiltering(Bitmap bmp, Matrix<double> warpMatrix)
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

                    var color = _bmp.GetPixel((int) xNew, (int) yNew);
                    resultBmp.SetPixel(i, j, color);
                }
            }
            return resultBmp;
        }
    }
}
