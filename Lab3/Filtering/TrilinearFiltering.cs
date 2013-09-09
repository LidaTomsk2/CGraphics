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
            InitMipMaps(32);
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
                    var reduct = GetReductionLevel(i, j, xNew, yNew);

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
            var mipMaps = new List<Bitmap>();

            var startLevel = 1;
            while (startLevel < maxLevel)
            {
                mipMaps.Add(GetMipMap(startLevel));
                startLevel *= 2;
            }

            return mipMaps.ToArray();
        }

        private int GetReductionLevel(int targetI, int targetJ, double fromX, double fromY)
        {
            return 3;
        }

        private Color GetResultColor(int i, int j)
        {
            var reduct = 3;

            var xm = _warpMatrix[0, 0] + _warpMatrix[0, 1]*i + _warpMatrix[0, 2]*j;
            var ym = _warpMatrix[1, 0] + _warpMatrix[1, 1]*i + _warpMatrix[1, 2]*j;
            var im = _mipMaps[2].GetPixel((int)xm, (int)ym);

            var mm2 = _mipMaps[4];
            var xm2 = (int)(_warpMatrix[0, 0] + _warpMatrix[0, 1] * i + _warpMatrix[0, 2] * j);
            var ym2 = _warpMatrix[1, 0] + _warpMatrix[1, 1] * i + _warpMatrix[1, 2] * j;
            Color im2;
            if (xm2 < 0 || xm2 > mm2.Width) im2 = Color.Empty; else im2 = _mipMaps[4].GetPixel(xm2, (int)ym2);

            return im.Multiply(4 - reduct).Divide(2).Add(im2.Multiply(reduct - 2).Divide(2));
        }
    }
}
