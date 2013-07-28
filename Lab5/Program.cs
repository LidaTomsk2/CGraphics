using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Lab5
{
    class Program
    {
        private static GameWindow _window;

        static void Main(string[] args)
        {
            _window = new GameWindow(640, 480) {Title = "Lab5"};

            _window.RenderFrame += WindowOnRenderFrame;
            _window.Run();
        }

        private static void WindowOnRenderFrame(object sender, FrameEventArgs frameEventArgs)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(BeginMode.Triangles);

            GL.Vertex2(-1, -1);
            GL.Vertex2(1, -1);
            GL.Vertex2(0, 1);

            GL.End();

            _window.SwapBuffers();
        }
    }
}
