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
    class Sphere : IDrawable
    {
        private Vector3 _center;
        private float _radius;
        private readonly uint _precision;
        private readonly Color _color;

        public Sphere(Vector3 center, float radius, uint precision, Color color)
        {
            _center = center;
            _radius = radius;
            _precision = precision;
            _color = color;
        }

        public void Draw()
        {
            if (_radius < 0f)
                _radius = -_radius;
            
            const float halfPI = MathHelper.PiOver2;
            var oneThroughPrecision = 1.0f / _precision;
            var twoPIThroughPrecision = MathHelper.Pi * 2 * oneThroughPrecision;

            float theta1, theta2, theta3;
            Vector3 Normal, Position;

            for (uint j = 0; j < _precision / 2; j++)
            {
                theta1 = (j * twoPIThroughPrecision) - halfPI;
                theta2 = ((j + 1) * twoPIThroughPrecision) - halfPI;

                GL.Begin(BeginMode.TriangleStrip);
                GL.Color3(_color);
                for (uint i = 0; i <= _precision; i++)
                {
                    theta3 = i * twoPIThroughPrecision;

                    Normal.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                    Normal.Y = (float)Math.Sin(theta2);
                    Normal.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                    Position.X = _center.X + _radius * Normal.X;
                    Position.Y = _center.Y + _radius * Normal.Y;
                    Position.Z = _center.Z + _radius * Normal.Z;

                    GL.Normal3(Normal);
                    GL.TexCoord2(i * oneThroughPrecision, 2.0f * (j + 1) * oneThroughPrecision);
                    GL.Vertex3(Position);

                    Normal.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                    Normal.Y = (float)Math.Sin(theta1);
                    Normal.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                    Position.X = _center.X + _radius * Normal.X;
                    Position.Y = _center.Y + _radius * Normal.Y;
                    Position.Z = _center.Z + _radius * Normal.Z;

                    GL.Normal3(Normal);
                    GL.TexCoord2(i * oneThroughPrecision, 2.0f * j * oneThroughPrecision);
                    GL.Vertex3(Position);
                }
                GL.End();
            }
        }
    }
}
