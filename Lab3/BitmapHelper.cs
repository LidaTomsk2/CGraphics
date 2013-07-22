using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Lab3
{
    class BitmapHelper
    {
        public static BitmapImage GetBitmapImage(Bitmap bitmap)
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

        public static Bitmap CreatePicture(Bitmap bmp, Matrix<double> warpMatrix)
        {
            var resultBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < resultBmp.Width; i++)
            {
                for (int j = 0; j < resultBmp.Height; j++)
                {
                    var xNew = warpMatrix[0, 0] + warpMatrix[0, 1] * i + warpMatrix[0, 2] * j;
                    var yNew = warpMatrix[1, 0] + warpMatrix[1, 1] * i + warpMatrix[1, 2] * j;
                    if (xNew < 0 || Math.Ceiling(xNew) >= resultBmp.Width ||
                        yNew < 0 || Math.Ceiling(yNew) >= resultBmp.Height) continue; // костыль
                    //var color = bmp.GetPixel((int)xNew, (int)yNew);
                    var color = FilteringHelper.GetBilinearColor(bmp, xNew, yNew);
                    resultBmp.SetPixel(i, j, color);
                }
            }
            return resultBmp;
        }
    }
}
