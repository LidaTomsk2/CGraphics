using System;
using System.Collections.Generic;
using System.Drawing;
using Lab5.Primitives;
using Lab5.Utilities;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace Lab5
{
    public class MainWindow : GameWindow
    {
        private const float CameraFovy = 45.0f;
        private const float CameraZfar = 1000.0f;
        private const float CameraZnear = 0.1f;

        private const float MouseOrbitSpeed = 0.30f; // крутить
        private const float MouseDollySpeed = 0.5f; // зум
        private const float MouseTrackSpeed = 0.012f; // двигать

        private float _gHeading, _gPitch, _dx, _dy;
        private ECameraMode _cameraMode = ECameraMode.CAMERA_NONE;

        public static bool AvailableVboIbo = false;
        public static bool AvailableGlsl = false;

        private Point _mousePrevious, _mouseCurrent;
        readonly float[] _gCameraPos = new float[3];
        readonly float[] _gTargetPos = new float[3];
        private Vector4 _lightVector = new Vector4(0, 0, 4, 1);
        private readonly IEnumerable<IDrawable> _drawables; 

        public MainWindow(int samples)
            : base(800, 600, new OpenTK.Graphics.GraphicsMode(32, 16, 0, samples))
        {
            Title = "Lab5";

            VSync = VSyncMode.On;
            AvailableVboIbo = true;
            AvailableGlsl = true;

            Keyboard.KeyDown += KeyboardKeyDown;
            Mouse.ButtonDown += MouseButtonDown;
            Mouse.ButtonUp += MouseButtonUp;
            Mouse.Move += MouseMove;
            Mouse.WheelChanged += MouseOnWheelChanged;

            ResetCamera();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ColorMaterial);

            _drawables = new List<IDrawable>()
                         {
                             new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Red),
                             new Sphere(new Vector3(0, 0, 0), 3f, 30, Color.Yellow)
                         };    
        }

        private void MouseOnWheelChanged(object sender, MouseWheelEventArgs e)
        {
            _dy = e.Delta;
            _dy *= MouseDollySpeed;

            _gCameraPos[2] -= _dy;

            if (_gCameraPos[2] < 1 + CameraZnear)
                _gCameraPos[2] = 1 + CameraZnear;

            if (_gCameraPos[2] > CameraZfar - 10)
                _gCameraPos[2] = CameraZfar - 10;
        }

        void MouseMove(object sender, MouseMoveEventArgs e)
        {
            _mouseCurrent.X = Mouse.X;
            _mouseCurrent.Y = Mouse.Y;

            // Now use mouse_delta to move the camera

            switch (_cameraMode)
            {
                case ECameraMode.CAMERA_TRACK:
                    _dx = _mouseCurrent.X - _mousePrevious.X;
                    _dx *= MouseTrackSpeed;

                    _dy = _mouseCurrent.Y - _mousePrevious.Y;
                    _dy *= MouseTrackSpeed;

                    _gCameraPos[0] -= _dx;
                    _gCameraPos[1] += _dy;

                    _gTargetPos[0] -= _dx;
                    _gTargetPos[1] += _dy;

                    break;

                case ECameraMode.CAMERA_ORBIT:
                    _dx = _mouseCurrent.X - _mousePrevious.X;
                    _dx *= MouseOrbitSpeed;

                    _dy = _mouseCurrent.Y - _mousePrevious.Y;
                    _dy *= MouseOrbitSpeed;

                    _gHeading += _dx;
                    _gPitch += _dy;

                    if (_gPitch > 90.0f)
                        _gPitch = 90.0f;

                    if (_gPitch < -90.0f)
                        _gPitch = -90.0f;

                    break;
            }
            _mousePrevious.X = _mouseCurrent.X;
            _mousePrevious.Y = _mouseCurrent.Y;
        }

        void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            _cameraMode = ECameraMode.CAMERA_NONE;
        }

        void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    _cameraMode = ECameraMode.CAMERA_TRACK;
                    break;
                case MouseButton.Right:
                    _cameraMode = ECameraMode.CAMERA_ORBIT;
                    break;
            }
            _mousePrevious.X = Mouse.X;
            _mousePrevious.Y = Mouse.Y;
        }
        void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Left:
                    _lightVector = Vector4.Add(_lightVector, new Vector4(0.0f, 0.0f,0.2f, 0));
                    break;
            }
        }

        #region override OnXXX()

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnResize(EventArgs e)
        {
            if (Width != 0 && Height != 0)
            {
                GL.Viewport(0, 0, Width, Height);
                double aspectRatio = Width / (double)Height;

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();

                Glu.Perspective(CameraFovy, aspectRatio, CameraZnear, CameraZfar);
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.3f, 0.5f, 0.9f, 0.0f);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            Glu.LookAt(_gCameraPos[0], _gCameraPos[1], _gCameraPos[2],
                      _gTargetPos[0], _gTargetPos[1], _gTargetPos[2],
                      0.0f, 1.0f, 0.0f);

            GL.Rotate(_gPitch, 1.0f, 0.0f, 0.0f);
            GL.Rotate(_gHeading, 0.0f, 1.0f, 0.0f);

            foreach (var drawable in _drawables)
            {
                drawable.Draw();
            }

            GL.Begin(BeginMode.Lines);
            GL.Vertex3(-3, 0, 0);
            GL.Vertex3(3, 0, 0);
            GL.Vertex3(0, -3, 0);
            GL.Vertex3(0, 3, 0);
            GL.End();



            GL.Light(LightName.Light0, LightParameter.Position, _lightVector);
            SwapBuffers();
        }

        protected override void OnDisposed(EventArgs e)
        {
            base.OnDisposed(e);
        }
        #endregion override OnXXX()

        void ResetCamera()
        {
            _gTargetPos[0] = 0; _gTargetPos[1] = 0; _gTargetPos[2] = 0;

            _gCameraPos[0] = _gTargetPos[0];
            _gCameraPos[1] = _gTargetPos[1];
            _gCameraPos[2] = _gTargetPos[2] + 10 + CameraZnear + 0.4f;

            _gPitch = 0.0f;
            _gHeading = 0.0f;
        }
    }
    enum ECameraMode
    {
        CAMERA_NONE, CAMERA_TRACK, CAMERA_ORBIT
    }
}
