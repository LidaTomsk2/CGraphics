using System;
using System.Drawing;
using Color = System.Drawing.Color;

namespace Lab3
{
    internal class FilteringHelper
    {
        public static Color GetBilinearColor(Bitmap bmp, double x, double y)
        {
            int xl = (int) x, xh = xl + 1;
            int yl = (int) y, yh = yl + 1;

            var c1 = MultiplyColor(bmp.GetPixel(xl, yl), xh - x);
            var c2 = MultiplyColor(bmp.GetPixel(xh, yl), x - xl);
            var c3 = MultiplyColor(SumColor(c1, c2), yh - y);

            var c4 = MultiplyColor(bmp.GetPixel(xl, yh), xh - x);
            var c5 = MultiplyColor(bmp.GetPixel(xh, yh), x - xl);
            var c6 = MultiplyColor(SumColor(c4, c5), y - yl);

            return SumColor(c3, c6);
        }

        private static Color MultiplyColor(Color color, double k)
        {
            return Color.FromArgb((int) (color.A*k), (int) (color.R*k), (int) (color.G*k), (int) (color.B*k));
        }

        private static Color SumColor(Color c1, Color c2)
        {
            return Color.FromArgb(c1.A + c2.A, c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);
        }

        private static Bitmap GetMipMap(Bitmap bmp, int reductLevel)
        {
            
        }
    }
}
