using System;
using System.Collections.Generic;
using System.Drawing;
using Lab5.Core;
using Lab5.Core.Effects;
using Lab5.Core.Interfaces;
using Lab5.Core.Primitives;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Lab5
{
    public class MainWindow : GameWindow
    {
        private Point _mousePrevious, _mouseCurrent;
        private readonly IEnumerable<IDrawable> _drawables;
        private readonly Lightning _lightning = new Lightning(new Vector4(0, 5, 4, 0));
        private readonly Camera _camera = new Camera();
        private readonly Fog _fog = new Fog();

        public MainWindow(int samples)
            : base(800, 600, new OpenTK.Graphics.GraphicsMode(32, 16, 8, samples))
        {
            Title = "Lab5";
            VSync = VSyncMode.On;
            
            Keyboard.KeyDown += KeyboardKeyDown;
            Mouse.ButtonDown += MouseButtonDown;
            Mouse.ButtonUp += MouseButtonUp;
            Mouse.Move += MouseOnMove;
            Mouse.WheelChanged += (sender, args) => _camera.Zoom(args.Delta);
            Resize += (sender, args) => _camera.AffectResize(Width, Height);

            _camera.ResetCamera();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ColorMaterial);

            GL.ShadeModel(ShadingModel.Smooth);
            

            _drawables = new List<IDrawable>
                         {
                             new Cube(new Vector3(3, 1, 0), new Vector3(1, 1, 1), Color.Red),
                             new Sphere(new Vector3(1, 1, 0), 1f, 30, Color.Yellow),
                             new Sphere(new Vector3(-1, 1, 0), 1f, 30, Color.Yellow),
                             new Sphere(new Vector3(-1, 1, -3), 1f, 30, Color.Yellow),
                             new Sphere(new Vector3(-3, 1, 0), 1f, 30, Color.Yellow),
                             new Sphere(new Vector3(-5, 1, 0), 1f, 30, Color.Yellow),
                             new Sphere(new Vector3(-7, 1, 0), 1f, 30, Color.Yellow),
                             new CoordGrid(5f)
                         };
        }

        private void MouseOnMove(object sender, MouseMoveEventArgs e)
        {
            _mouseCurrent.X = Mouse.X;
            _mouseCurrent.Y = Mouse.Y;

            _camera.UpdateFromMouse(_mousePrevious, _mouseCurrent);

            _mousePrevious.X = _mouseCurrent.X;
            _mousePrevious.Y = _mouseCurrent.Y;
        }

        void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            _camera.CameraMode = Camera.ECameraMode.CAMERA_NONE;
        }

        void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    _camera.CameraMode = Camera.ECameraMode.CAMERA_TRACK;
                    break;
                case MouseButton.Right:
                    _camera.CameraMode = Camera.ECameraMode.CAMERA_ORBIT;
                    break;
            }

            _mouseCurrent.X = Mouse.X;
            _mouseCurrent.Y = Mouse.Y;
        }

        // разовые события
        void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Keypad1:
                    _lightning.Position = Vector4.Add(_lightning.Position, 
                        new Vector4(0, 0, 0, _lightning.Position.W == 0f ? 1 : -1));
                    break;
                case Key.Keypad2:
                    _lightning.IsAttenuationEnabled = !_lightning.IsAttenuationEnabled;
                    break;
                case Key.Keypad3:
                    if (GL.IsEnabled(EnableCap.Fog)) _fog.Disable(); else _fog.Enable();
                    break;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard.IsKeyDown(Key.Left))
            {
                _lightning.Position = Vector4.Add(_lightning.Position, new Vector4(-.1f, 0, 0, 0));
            }
            if (keyboard.IsKeyDown(Key.Right))
            {
                _lightning.Position = Vector4.Add(_lightning.Position, new Vector4(.1f, 0, 0, 0));
            }
            if (keyboard.IsKeyDown(Key.Up))
            {
                _lightning.Position = Vector4.Add(_lightning.Position, new Vector4(0, 0, -.1f, 0));
            }
            if (keyboard.IsKeyDown(Key.Down))
            {
                _lightning.Position = Vector4.Add(_lightning.Position, new Vector4(0, 0, .1f, 0));
            }
            if (keyboard.IsKeyDown(Key.PageUp))
            {
                _lightning.Position = Vector4.Add(_lightning.Position, new Vector4(0, .1f, 0, 0));
            }
            if (keyboard.IsKeyDown(Key.PageDown))
            {
                _lightning.Position = Vector4.Add(_lightning.Position, new Vector4(0, -.1f, 0, 0));
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.3f, 0.5f, 0.9f, 0.0f);

            _camera.Update();
            _lightning.Draw();

            foreach (var drawable in _drawables)
            {
                drawable.Draw();
            }

            GL.Color3(Color.White);
            GL.Begin(BeginMode.Quads);
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    GL.Vertex3(-10 + j, 0, -10 + i);
                    GL.Vertex3(-10 + j + 1, 0, -10 + i);
                    GL.Vertex3(-10 + j + 1, 0, -10 + i + 1);
                    GL.Vertex3(-10 + j, 0, -10 + i + 1);
                }
            }

            GL.End();
            
            SwapBuffers();
        }
    }
}
