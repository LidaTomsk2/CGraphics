using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab5.Primitives
{
    public class Cube : IDrawable
    {
        private readonly Vector3 _center;
        private readonly Vector3 _size;
        private readonly Color _color;

        public Cube(Vector3 center, Vector3 size, Color color)
        {
            _center = center;
            _size = size;
            _color = color;
        }

        public void Draw()
        {
            GL.Translate(-1 * _center);
            GL.Begin(BeginMode.Quads);
            GL.Color3(_color);
            
            // FRONT
            GL.Vertex3(-_size.X, -_size.Y, _size.Z);
            GL.Vertex3(_size.X, -_size.Y, _size.Z);
            GL.Vertex3(_size.X, _size.Y, _size.Z);
            GL.Vertex3(-_size.X, _size.Y, _size.Z);
            
            // BACK
            GL.Vertex3(-_size.X, -_size.Y, -_size.Z);
            GL.Vertex3(-_size.X, _size.Y, -_size.Z);
            GL.Vertex3(_size.X, _size.Y, -_size.Z);
            GL.Vertex3(_size.X, -_size.Y, -_size.Z);
            
            // LEFT
            GL.Vertex3(-_size.X, -_size.Y, _size.Z);
            GL.Vertex3(-_size.X, _size.Y, _size.Z);
            GL.Vertex3(-_size.X, _size.Y, -_size.Z);
            GL.Vertex3(-_size.X, -_size.Y, -_size.Z);

            // RIGHT
            GL.Vertex3(_size.X, -_size.Y, -_size.Z);
            GL.Vertex3(_size.X, _size.Y, -_size.Z);
            GL.Vertex3(_size.X, _size.Y, _size.Z);
            GL.Vertex3(_size.X, -_size.Y, _size.Z);

            // TOP
            GL.Vertex3(-_size.X, _size.Y, _size.Z);
            GL.Vertex3(_size.X, _size.Y, _size.Z);
            GL.Vertex3(_size.X, _size.Y, -_size.Z);
            GL.Vertex3(-_size.X, _size.Y, -_size.Z);

            // BOTTOM
            GL.Vertex3(-_size.X, -_size.Y, _size.Z);
            GL.Vertex3(-_size.X, -_size.Y, -_size.Z);
            GL.Vertex3(_size.X, -_size.Y, -_size.Z);
            GL.Vertex3(_size.X, -_size.Y, _size.Z);
            
            GL.End();
            GL.Translate(_center);
        }
    }
}
