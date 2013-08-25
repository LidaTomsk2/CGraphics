using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab5.Core.Interfaces;
using OpenTK.Graphics.OpenGL;

namespace Lab5.Core.Effects
{
    public class Fog
    {
        public void Enable()
        {
            GL.Enable(EnableCap.Fog);
            GL.Fog(FogParameter.FogMode, (int)FogMode.Exp2);
            GL.Fog(FogParameter.FogColor, new[] {.5f, .5f, .5f, 1});
            GL.Fog(FogParameter.FogDensity, .1f);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
        }

        public void Disable()
        {
            GL.Disable(EnableCap.Fog);
        }
    }
}
