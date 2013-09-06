using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Lab5.Utilities
{
    public class TextureManager
    {
        private static readonly Dictionary<string, int> Textures = new Dictionary<string, int>(); 

        /// <summary>
        /// Сгенерировать текстуру (или получить, если уже генерировали)
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static int LoadTexture(string imgPath)
        {
            if (Textures.ContainsKey(imgPath)) return Textures[imgPath];

            var id = GL.GenTexture();
            Textures.Add(imgPath, id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            var bmp = new Bitmap(imgPath);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return id;
        }
    }
}
