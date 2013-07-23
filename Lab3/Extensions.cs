using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Lab3
{
    static class Extensions
    {
        /// <summary>
        /// Умножить цвет на коэффициент
        /// </summary>
        /// <param name="color"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Color Multiply(this Color color, double k)
        {
            color = Color.FromArgb((int) (color.A*k), (int) (color.R*k), (int) (color.G*k), (int) (color.B*k));
            return color;
        }


        /// <summary>
        /// Увеличить цвет на параметры другого цвета
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Color Add(this Color c1, Color c2)
        {
            c1 = Color.FromArgb(c1.A + c2.A, c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);
            return c1;
        }

        /// <summary>
        /// Поделить параметры цвета на коэффициент
        /// </summary>
        /// <param name="color"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Color Divide(this Color color, double k)
        {
            color = Color.FromArgb((int) (color.A/k), (int) (color.R/k), (int) (color.G/k), (int) (color.B/k));
            return color;
        }

        public static BitmapImage ConvertToBitmapImage(this Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
    }
}
