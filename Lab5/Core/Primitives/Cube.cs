using System.Drawing;
using Lab5.Core.Interfaces;
using Lab5.Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab5.Core.Primitives
{
    public class Cube : IDrawable
    {
        private readonly Vector3 _center;
        private readonly Vector3 _size;
        private readonly Color _color;
        private float _rotateAngle;
        private readonly int _textureId;

        public bool IsRotating { get; set; }
        public bool IsTextured { get; set; }

        public Cube(Vector3 center, Vector3 size, Color color)
        {
            _center = center;
            _size = size;
            _color = color;
            _textureId = TextureManager.LoadTexture("cube.jpg");
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(_center);

            if (IsRotating)
            {
                _rotateAngle += .5f;
                if (_rotateAngle > 360) _rotateAngle -= 360;
            }
            GL.Rotate(_rotateAngle, 0, 1, 0);

            if (IsTextured)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, _textureId);
            }

            GL.Color3(_color);
            GL.Begin(BeginMode.Quads);
            
            // FRONT
            GL.TexCoord2(0, 1);
            GL.Vertex3(-_size.X, -_size.Y, _size.Z);
            GL.TexCoord2(1, 1);
            GL.Vertex3(_size.X, -_size.Y, _size.Z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(_size.X, _size.Y, _size.Z);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-_size.X, _size.Y, _size.Z);

            // BACK
            GL.TexCoord2(1, 1);
            GL.Vertex3(-_size.X, -_size.Y, -_size.Z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-_size.X, _size.Y, -_size.Z);
            GL.TexCoord2(0, 0);
            GL.Vertex3(_size.X, _size.Y, -_size.Z);
            GL.TexCoord2(0, 1);
            GL.Vertex3(_size.X, -_size.Y, -_size.Z);

            // LEFT
            GL.TexCoord2(1, 1);
            GL.Vertex3(-_size.X, -_size.Y, _size.Z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-_size.X, _size.Y, _size.Z);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-_size.X, _size.Y, -_size.Z);
            GL.TexCoord2(0, 1);
            GL.Vertex3(-_size.X, -_size.Y, -_size.Z);

            // RIGHT
            GL.TexCoord2(1, 1);
            GL.Vertex3(_size.X, -_size.Y, -_size.Z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(_size.X, _size.Y, -_size.Z);
            GL.TexCoord2(0, 0);
            GL.Vertex3(_size.X, _size.Y, _size.Z);
            GL.TexCoord2(0, 1);
            GL.Vertex3(_size.X, -_size.Y, _size.Z);

            // TOP
            GL.TexCoord2(0, 1);
            GL.Vertex3(-_size.X, _size.Y, _size.Z);
            GL.TexCoord2(1, 1);
            GL.Vertex3(_size.X, _size.Y, _size.Z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(_size.X, _size.Y, -_size.Z);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-_size.X, _size.Y, -_size.Z);

            // BOTTOM
            GL.TexCoord2(1, 1);
            GL.Vertex3(-_size.X, -_size.Y, _size.Z);
            GL.TexCoord2(1, 0);
            GL.Vertex3(-_size.X, -_size.Y, -_size.Z);
            GL.TexCoord2(0, 0);
            GL.Vertex3(_size.X, -_size.Y, -_size.Z);
            GL.TexCoord2(0, 1);
            GL.Vertex3(_size.X, -_size.Y, _size.Z);

            GL.End();

            GL.Disable(EnableCap.Texture2D);

            //// крыша ;D
            //GL.Begin(BeginMode.TriangleFan);
            //GL.Color3(Color.OrangeRed);
            //GL.Vertex3(0, 2*_size.Y, 0);
            //GL.Vertex3(-_size.X, _size.Y, -_size.Z);
            //GL.Vertex3(_size.X, _size.Y, -_size.Z);
            //GL.Vertex3(_size.X, _size.Y, _size.Z);
            //GL.Vertex3(-_size.X, _size.Y, _size.Z);
            //GL.Vertex3(-_size.X, _size.Y, -_size.Z);
            //GL.End();

            // возвращаем прежнее состояние
            GL.PopMatrix();
        }
    }
}
