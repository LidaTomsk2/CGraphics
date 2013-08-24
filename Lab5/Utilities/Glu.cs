using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab5.Utilities
{
    public class Glu
    {
        public static void Perspective(double fovy, double aspect, double zNear, double zFar)
        {
            var ymax = zNear * Math.Tan(fovy * Math.PI / 360.0);
            var ymin = -ymax;

            var xmin = ymin * aspect;
            var xmax = ymax * aspect;

            GL.Frustum(xmin, xmax, ymin, ymax, zNear, zFar);
        }

        public static void LookAt(float eyex, float eyey, float eyez, float centerx, float centery, float centerz,
            float upx, float upy, float upz)
        {
            Vector3 forward, up, right;
            var m = new float[16];

            forward.X = centerx - eyex;
            forward.Y = centery - eyey;
            forward.Z = centerz - eyez;

            up.X = upx;
            up.Y = upy;
            up.Z = upz;

            forward.Normalize();

            /* Side = tForward x tUp */
            Vector3.Cross(ref forward, ref up, out right);
            right.Normalize();

            /* Recompute tUp as: tUp = tRight x tForward */
            Vector3.Cross(ref right, ref forward, out up);

            // set right vector
            m[0] = right.X;
            m[1] = up.X;
            m[2] = -forward.X;
            m[3] = 0;
            // set up vector
            m[4] = right.Y;
            m[5] = up.Y;
            m[6] = -forward.Y;
            m[7] = 0;
            // set forward vector
            m[8] = right.Z;
            m[9] = up.Z;
            m[10] = -forward.Z;
            m[11] = 0;
            // set translation vector
            m[12] = 0;
            m[13] = 0;
            m[14] = 0;
            m[15] = 1;

            GL.MultMatrix(m);
            GL.Translate(-eyex, -eyey, -eyez);
        }
    }
}
