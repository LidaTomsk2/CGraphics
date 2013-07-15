using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lab1
{
    public class LineDrawer
    {
        public int PixelSize { get; set; }
        public Color Color { get; set; }

        private readonly WriteableBitmap _bmp;
        public LineDrawer(WriteableBitmap bmp)
        {
            _bmp = bmp;
            Color = Colors.Black;
            PixelSize = 1;
        }

        private void DrawPixel(int x, int y)
        {
            _bmp.FillRectangle(x, y, x + PixelSize - 1, y + PixelSize - 1, Color);
        }

        private void DrawPixel(bool steep, int x, int y, float intense)
        {
            if (steep) Swap(ref x, ref y);

            var colorIntense = Convert.ToByte(Color.A*intense);
            var color = Color.FromArgb(colorIntense, Color.R, Color.G, Color.B);
            _bmp.FillRectangle(x, y, x + PixelSize - 1, y + PixelSize - 1, color);
        }

        private void Swap(ref int a, ref int b)
        {
            var z = a;
            a = b;
            b = z;
        }

        public void BresenhamLine(int x1, int y1, int x2, int y2)
        {
            var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            // Отражаем линию по диагонали, если угол наклона слишком большой
            if (steep)
            {
                Swap(ref x1, ref y1);
                Swap(ref x2, ref y2);
            }
            // Если линия растёт не слева направо, то меняем начало и конец отрезка местами
            if (x1 > x2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
            }
            int dx = x2 - x1;
            int dy = Math.Abs(y2 - y1);
            int error = dx / 2; // Оптимизация с умножением на dx, чтобы избавиться от лишних дробей
            int ystep = (y1 < y2) ? 1 : -1;
            int y = y1;
            for (int x = x1; x <= x2; x += PixelSize)
            {
                DrawPixel(steep ? y : x, steep ? x : y);
                error -= dy;
                if (error < 0)
                {
                    y += ystep * PixelSize;
                    error += dx;
                }
            }
        }

        public void WuLine(int x1, int y1, int x2, int y2)
        {
            var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                Swap(ref x1, ref y1);
                Swap(ref x2, ref y2);
            }
            if (x1 > x2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
            }

            //DrawPixel(steep, x1, y1, 1);
            //DrawPixel(steep, x2, y2, 1);
            float dx = x2 - x1;
            float dy = y2 - y1;
            float gradient = dy / dx;
            float y = y1 + gradient;
            for (var x = x1 + 1; x <= x2 - 1; x += PixelSize)
            {
                DrawPixel(steep, x, (int)y, 1 - (y - (int)y));
                DrawPixel(steep, x, (int)y + PixelSize, y - (int)y);
                y += gradient*PixelSize;
            }
        }
    }
}
