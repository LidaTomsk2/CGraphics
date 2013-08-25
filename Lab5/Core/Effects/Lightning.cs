using System.Drawing;
using Lab5.Core.Interfaces;
using Lab5.Core.Primitives;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab5.Core.Effects
{
    /// <summary>
    /// Освещение (пока только работает 1 экземпляр Light0)
    /// </summary>
    public class Lightning : IDrawable
    {
        private Vector4 _position;
        public Vector4 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _sphere = new Sphere(new Vector3(_position.X, _position.Y, _position.Z), 0.1f, 10, Color.White);
            }
        }

        private Sphere _sphere;
        public bool IsAttenuationEnabled { get; set; }

        public Lightning(Vector4 position)
        {
            Position = position;
            IsAttenuationEnabled = false;
        }

        public void Draw()
        {
            GL.Light(LightName.Light0, LightParameter.Position, Position);
            if (IsAttenuationEnabled)
            {
                // убывание интенсивности с расстоянием
                // задано функцией f(d) = 1.0 / (0.4 * d * d + 0.2 * d)
                GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 0f);
                GL.Light(LightName.Light0, LightParameter.LinearAttenuation, .1f);
                GL.Light(LightName.Light0, LightParameter.QuadraticAttenuation, .1f);
            }
            else
            {

                GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 1);
                GL.Light(LightName.Light0, LightParameter.LinearAttenuation, 0);
                GL.Light(LightName.Light0, LightParameter.QuadraticAttenuation, 0);
            }
            
            GL.Disable(EnableCap.Lighting);
            _sphere.Draw();
            GL.Enable(EnableCap.Lighting);
        }
    }
}
