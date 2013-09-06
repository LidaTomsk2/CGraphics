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

        private void DrawPixel(int x, int y)
        {
            _bmp.FillRectangle(x, y, x + PixelSize - 1, y + PixelSize - 1, CurrentColor);
        }

        public void DrawLineBrezenham(int x1, int y1, int x2, int y2)
        {
            bool changeFlag;
            int x = x1, y = y1;
            int dx = Math.Abs(x2 - x1), dy = Math.Abs(y2 - y1);
            int sx = Math.Sign(x2 - x1)*PixelSize, sy = Math.Sign(y2 - y1)*PixelSize;

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
                DrawPixel(x, y);
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
                    e -= 2 * dx;
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
            DrawPixel(x, y);
        }

        public void DrawLineTrippleAliasing(int x1, int y1, int x2, int y2)
        {
            var fromX = Math.Min(x1, x2);
            var toX = Math.Max(x1, x2);

            var tempColor = CurrentColor;
            for (int curX = fromX; curX < toX; curX += PixelSize)
            {
                CurrentColor = tempColor;
                var curYFloat = y1 + (y2 - y1)*((curX - x1)/(float)(x2 - x1));
                var curYRounded = Math.Round(curYFloat);
                var c = curYRounded - curYFloat;

                var b0 = (Math.Sqrt(Math.Pow((y2 - y1)/(float) (x2 - x1), 2) + 1 - 1)) / 2f;
                double b1, b2, b3;
                if (c >= 0)
                {
                    b1 = b0 + c;
                    b3 = b0*(1 - 2*c);
                }
                else
                {
                    b1 = b0*(1 + 2*c);
                    b3 = b0 - c;
                }
                b2 = 1 + Math.Abs(c)*(2*b0 - 1);

                CurrentColor = new Color {A = (byte)(255*b1), R = CurrentColor.R, G = CurrentColor.G, B = CurrentColor.B};
                DrawPixel(curX, (int)curYRounded-PixelSize);
                CurrentColor = new Color {A = (byte)(255*b2), R = CurrentColor.R, G = CurrentColor.G, B = CurrentColor.B};
                DrawPixel(curX, (int)curYRounded);
                CurrentColor = new Color {A = (byte)(255*b3), R = CurrentColor.R, G = CurrentColor.G, B = CurrentColor.B};
                DrawPixel(curX, (int)curYRounded+PixelSize);
            }
            CurrentColor = tempColor;
        }

        public void DrawLineWu()
        {
            
        }
    }
}
