using System;
using System.Drawing;
using Lab5.Core.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab5.Core.Primitives
{
    public class Sphere : IDrawable
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
            Vector3 normal, position;

            for (uint j = 0; j < _precision / 2; j++)
            {
                theta1 = (j * twoPIThroughPrecision) - halfPI;
                theta2 = ((j + 1) * twoPIThroughPrecision) - halfPI;

                GL.Begin(BeginMode.TriangleStrip);
                GL.Color3(_color);
                for (uint i = 0; i <= _precision; i++)
                {
                    theta3 = i * twoPIThroughPrecision;

                    normal.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                    normal.Y = (float)Math.Sin(theta2);
                    normal.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                    position.X = _center.X + _radius * normal.X;
                    position.Y = _center.Y + _radius * normal.Y;
                    position.Z = _center.Z + _radius * normal.Z;

                    GL.Normal3(normal);
                    GL.TexCoord2(i * oneThroughPrecision, 2.0f * (j + 1) * oneThroughPrecision);
                    GL.Vertex3(position);

                    normal.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                    normal.Y = (float)Math.Sin(theta1);
                    normal.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                    position.X = _center.X + _radius * normal.X;
                    position.Y = _center.Y + _radius * normal.Y;
                    position.Z = _center.Z + _radius * normal.Z;

                    GL.Normal3(normal);
                    GL.TexCoord2(i * oneThroughPrecision, 2.0f * j * oneThroughPrecision);
                    GL.Vertex3(position);
                }
                GL.End();
            }
        }
    }
}
