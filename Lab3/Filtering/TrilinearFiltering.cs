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

        public TrilinearFiltering(Bitmap bmp, Matrix<double> warpMatrix)
        {
            _bmp = bmp;
            _warpMatrix = warpMatrix;
            _mipMaps = InitMipMaps(32);
        }

        public Bitmap GetFilteredImage()
        {
            var resultBmp = new Bitmap(_bmp.Width, _bmp.Height);
            
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

                    //var c1M = mm1.GetPixel((int) xNew / k1M, (int) yNew / k1M);
                    //var c2M = mm2.GetPixel((int) xNew / k2M, (int) yNew / k2M);
                    //var resultColor = c1M.Multiply(k2M - k).Add(c2M.Multiply(k - k1M)).Divide(k1M);
                    var resultColor = GetCorrectColor(i, j); // О_О
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
            var mipMaps = new List<Bitmap>();

            var startLevel = 1;
            while (startLevel < maxLevel)
            {
                mipMaps.Add(GetMipMap(startLevel));
                startLevel *= 2;
            }

            return mipMaps.ToArray();
        }

        private Color GetCorrectColor(int i, int j)
        {
            var xNew = _warpMatrix[0, 0] + _warpMatrix[0, 1] * i + _warpMatrix[0, 2] * j;
            i++;
            var xNew2 = _warpMatrix[0, 0] + _warpMatrix[0, 1] * i + _warpMatrix[0, 2] * j;

            i--;
            var yNew = _warpMatrix[1, 0] + _warpMatrix[1, 1] * i + _warpMatrix[1, 2] * j;
            j++;
            var yNew2 = _warpMatrix[1, 0] + _warpMatrix[1, 1] * i + _warpMatrix[1, 2] * j;

            var kx = Math.Abs(xNew - xNew2);
            var ky = Math.Abs(yNew - yNew2);

            var k = (kx + ky)/2;
            var lowM = (int) k;
            var highM = (int) k + 1;

            return new Color(); // TODO: СДЕЛАТЬ ЧТО-ТО
        }
    }
}
