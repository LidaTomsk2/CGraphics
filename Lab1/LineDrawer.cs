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
        public Color CurrentColor { get; set; }

        private readonly WriteableBitmap _bmp;
        public LineDrawer(WriteableBitmap bmp)
        {
            _bmp = bmp;
            CurrentColor = Colors.Black;
            PixelSize = 1;
        }

        private void DrawPixel(int x, int y, double br)
        {
            var color = new Color { A = (byte)(255 * br), R = CurrentColor.R, G = CurrentColor.G, B = CurrentColor.B };
            _bmp.FillRectangle(x, y, x + PixelSize - 1, y + PixelSize - 1, color);
        }

        public void DrawLineBrezenham(int x1, int y1, int x2, int y2)
        {
            bool changeFlag;
            int x = x1, y = y1;
            int dx = Math.Abs(x2 - x1), dy = Math.Abs(y2 - y1);
            int sx = Math.Sign(x2 - x1) * PixelSize, sy = Math.Sign(y2 - y1) * PixelSize;

            if (dy > dx)
            {
                var z = dx;
                dx = dy;
                dy = z;
                changeFlag = true;
            }
            else
            {
                changeFlag = false;
            }

            var e = 2 * dy - dx;
            for (int i = 1; i <= dx; i += PixelSize)
            {
                DrawPixel(x, y, 1);
                while (e >= 0)
                {
                    if (changeFlag)
                    {
                        x += sx;
                    }
                    else
                    {
                        y += sy;
                    }
                    e -= 2 * Math.Sqrt(dx);
                }
                if (changeFlag)
                {
                    y += sy;
                }
                else
                {
                    x += sx;
                }
                e += 2 * dy;
            }
            DrawPixel(x, y, 1);
        }

        public void DrawLineTrippleAliasing(int x1, int y1, int x2, int y2)
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

            for (int curX = x1; curX < x2; curX += PixelSize)
            {
                double curYFloat = y1 + (y2 - y1) * ((curX - x1) / (float)(x2 - x1));
                var curYRounded = Math.Round(curYFloat);
                var c = curYRounded - curYFloat;
                curYRounded = Math.Round((int)curYFloat / PixelSize) * PixelSize;

                var b0 = (int)(Math.Sqrt(Math.Pow((y2 - y1) / (float)(x2 - x1), 2))) / 2f;
                double b1, b3;
                if (c >= 0)
                {
                    b1 = b0 + c;
                    b3 = b0 * (1 - 2 * c);
                }
                else
                {
                    b1 = b0 * (1 + 2 * c);
                    b3 = b0 - c;
                }
                var b2 = 1 + Math.Abs(c) * (2 * b0 - 1);

                DrawPixel(steep, curX, (int)curYRounded - PixelSize, b1);
                DrawPixel(steep, curX, (int)curYRounded, b2);
                DrawPixel(steep, curX, (int)curYRounded + PixelSize, b3);
            }
        }

        public void DrawLineWu(int x1, int y1, int x2, int y2)
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
            var gradient = dy / dx;

            var y = gradient;
            var yPos = y1;
            for (var x = x1 + 1; x <= x2 - 1; x += PixelSize)
            {
                var br = y - (int) y;
                DrawPixel(steep, x, yPos, 1 - br);
                DrawPixel(steep, x, yPos + PixelSize, br);
                y += gradient;

                if (Math.Abs(y) > 1)
                {
                    y = (int)(Math.Abs(y) - 1)*Math.Sign(gradient);
                    yPos += PixelSize*Math.Sign(gradient);
                }
            }
        }
        
        private static void Swap<T>(ref T lhs, ref T rhs)
        {
            var temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public void DrawPixel(bool steep, int x, int y, double br)
        {
            if (!steep)
            {
                DrawPixel(x, y, br);
            }
            else
            {
                DrawPixel(y, x, br);
            }
        }
    }
}
