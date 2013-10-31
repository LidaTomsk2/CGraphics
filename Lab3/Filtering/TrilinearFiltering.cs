using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Lab3.Filtering
{
    class TrilinearFiltering : IFilter
    {
        private readonly Bitmap _bmp;
        private readonly Matrix<double> _warpMatrix;
        private readonly Bitmap[] _mipMaps;
        private double _reduct;
        private int _lowRed, _topRed = 1;

        public TrilinearFiltering(Bitmap bmp, Matrix<double> warpMatrix)
        {
            _bmp = bmp;
            _warpMatrix = warpMatrix;
            _mipMaps = InitMipMaps(32);
            _reduct = GetReductionLevel(warpMatrix);
        }

        public Bitmap GetFilteredImage()
        {
            var resultBmp = new Bitmap(_bmp.Width, _bmp.Height);
            while (_topRed < _reduct) _topRed *= 2;
            _lowRed = _topRed / 2;

            // фильтрация
            for (int i = 0; i < resultBmp.Width; i++)
            {
                for (int j = 0; j < resultBmp.Height; j++)
                {
                    var xNew = _warpMatrix[0, 0] + _warpMatrix[0, 1]*i + _warpMatrix[0, 2]*j;
                    var yNew = _warpMatrix[1, 0] + _warpMatrix[1, 1]*i + _warpMatrix[1, 2]*j;

                    // проверка на запись в пустую память
                    if (xNew < 0 || Math.Ceiling(xNew) >= resultBmp.Width ||
                        yNew < 0 || Math.Ceiling(yNew) >= resultBmp.Height) continue;
                    
                    var resultColor = GetResultColor(i, j);
                    resultBmp.SetPixel(i, j, resultColor);
                }
            }

            return resultBmp;
        }

        private Bitmap GetMipMap(int reductLevel)
        {
            var nWidth = _bmp.Width / reductLevel;
            var nHeihgt = _bmp.Height / reductLevel;

            return new Bitmap(_bmp, nWidth, nHeihgt);
        }

        private Bitmap[] InitMipMaps(int maxLevel)
        {
            var mipMaps = new Bitmap[maxLevel];

            var startLevel = 1;
            while (startLevel < maxLevel)
            {
                mipMaps[startLevel] = GetMipMap(startLevel);
                startLevel *= 2;
            }

            return mipMaps;
        }

        public static double GetReductionLevel(Matrix<double> warpMatrix)
        {
            var i = 0;
            var j = 0;
            var xNew = warpMatrix[0, 0] + warpMatrix[0, 1] * i + warpMatrix[0, 2] * j;
            var yNew = warpMatrix[1, 0] + warpMatrix[1, 1] * i + warpMatrix[1, 2] * j;

            i = 1;
            j = 1;
            var xNew2 = warpMatrix[0, 0] + warpMatrix[0, 1] * i + warpMatrix[0, 2] * j;
            var yNew2 = warpMatrix[1, 0] + warpMatrix[1, 1] * i + warpMatrix[1, 2] * j;

            return Math.Sqrt(Math.Pow((int)xNew2 - (int)xNew, 2) + Math.Pow((int)yNew2 - (int)yNew, 2));
        }

        //private Color GetResultColor(int i, int j)
        //{
        //    var xm = _warpMatrix[0, 0] + _warpMatrix[0, 1]*i + _warpMatrix[0, 2]*j;
        //    var ym = _warpMatrix[1, 0] + _warpMatrix[1, 1]*i + _warpMatrix[1, 2]*j;
        //    if ((int) ym >= _mipMaps[_lowRed].Height || (int) ym < 0 || (int) xm >= _mipMaps[_lowRed].Width ||
        //        (int) xm < 0)
        //    {
        //        return _bmp.GetPixel((int) xm, (int) ym);
        //    }
        //    var im = _mipMaps[_lowRed].GetPixel((int)xm, (int)ym);

        //    var xm2 = _warpMatrix[0, 0] + _warpMatrix[0, 1] * i + _warpMatrix[0, 2] * j;
        //    var ym2 = _warpMatrix[1, 0] + _warpMatrix[1, 1] * i + _warpMatrix[1, 2] * j;
        //    if ((int) ym2 >= _mipMaps[_topRed].Height || (int) ym2 < 0 || (int) xm2 >= _mipMaps[_topRed].Width ||
        //        (int) xm2 < 0)
        //    {
        //        return _bmp.GetPixel((int)xm2, (int)ym2);
        //    }
        //    var im2 = _mipMaps[_topRed].GetPixel((int)xm2, (int)ym2);

        //    return Extensions.GetTrilinearColor(im, im2, _topRed, _lowRed, _reduct);
        //}

        private Color GetResultColor(int i, int j)
        {
            var x = _warpMatrix[0, 0] + _warpMatrix[0, 1]*i + _warpMatrix[0, 2]*j;
            var y = _warpMatrix[1, 0] + _warpMatrix[1, 1]*i + _warpMatrix[1, 2]*j;

            var xm = x/_lowRed - 1;
            var ym = y/_lowRed - 1;
            var im = _mipMaps[_lowRed].GetPixel((int)xm, (int)ym);

            var xm2 = x/_topRed - 1;
            var ym2 = y/_topRed - 1;
            var im2 = _mipMaps[_topRed].GetPixel((int)xm2, (int)ym2);

            return Extensions.GetTrilinearColor(im, im2, _topRed, _lowRed, _reduct);
        }
    }
}
