using System.Drawing;
using Lab5.Core.Interfaces;
using OpenTK.Graphics.OpenGL;

namespace Lab5.Core.Primitives
{
    public class CoordGrid : IDrawable
    {
        private readonly float _size;

        public CoordGrid(float size)
        {
            _size = size;
        }

        public void Draw()
        {
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(-_size, 0, 0);
            GL.Vertex3(_size, 0, 0);
            GL.Vertex3(0, -_size, 0);
            GL.Vertex3(0, _size, 0);
            GL.Vertex3(0, 0, -_size);
            GL.Vertex3(0, 0, _size);
            GL.End();
        }
    }
}
