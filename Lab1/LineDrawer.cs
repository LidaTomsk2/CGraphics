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

        private readonly WriteableBitmap _bmp;
        public LineDrawer(WriteableBitmap bmp)
        {
            _bmp = bmp;
        }

        private void DrawPixel(int x, int y)
        {
            _bmp.FillRectangle(x, y, x + PixelSize, y + PixelSize, Colors.Black);
        }

        private void DrawPixel(bool steep, int x, int y, float intense)
        {
            if (steep) Swap(ref x, ref y);

            var colorIntense = Convert.ToByte(255*intense);
            var color = Color.FromArgb(colorIntense, 0, 0, 0);
            _bmp.SetPixel(x, y, color);
        }

        private void Swap(ref int a, ref int b)
        {
            var z = a;
            a = b;
            b = z;
        }

        public void BresenhamLine(int x1, int y1, int x2, int y2)
        {
            var steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1); // Проверяем рост отрезка по оси x и по оси y
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
            int ystep = (y1 < y2) ? 1 : -1; // Выбираем направление роста координаты y
            int y = y1;
            for (int x = x1; x <= x2; x++)
            {
                DrawPixel(steep ? y : x, steep ? x : y); // Возвращаем координаты на место
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
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

            DrawPixel(steep, x1, y1, 1);
            DrawPixel(steep, x2, y2, 1);
            float dx = x2 - x1;
            float dy = y2 - y1;
            float gradient = dy / dx;
            float y = y1 + gradient;
            for (var x = x1 + 1; x <= x2 - 1; x++)
            {
                DrawPixel(steep, x, (int)y, 1 - (y - (int)y));
                DrawPixel(steep, x, (int)y + 1, y - (int)y);
                y += gradient;
            }
        }
    }
}
