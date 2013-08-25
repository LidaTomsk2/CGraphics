using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab5.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Lab5.Core
{
    public class Camera
    {
        private const float CameraFovy = 45.0f;
        private const float CameraZfar = 1000.0f;
        private const float CameraZnear = 0.1f;

        private const float MouseOrbitSpeed = 0.30f; // крутить
        private const float MouseDollySpeed = 0.5f; // зум
        private const float MouseTrackSpeed = 0.012f; // двигать

        private float _gHeading, _gPitch, _dx, _dy;
        public ECameraMode CameraMode { get; set; }

        readonly float[] _gCameraPos = new float[3];
        readonly float[] _gTargetPos = new float[3];

        public Camera()
        {
            CameraMode = ECameraMode.CAMERA_NONE;
        }

        public void ResetCamera()
        {
            _gTargetPos[0] = 0; _gTargetPos[1] = 0; _gTargetPos[2] = 0;

            _gCameraPos[0] = _gTargetPos[0];
            _gCameraPos[1] = _gTargetPos[1] + 5;
            _gCameraPos[2] = _gTargetPos[2] + 15 + CameraZnear + 0.4f;

            _gPitch = 0.0f;
            _gHeading = 0.0f;
        }

        public void UpdateFromMouse(Point mousePrevious, Point mouseCurrent)
        {
            switch (CameraMode)
            {
                case ECameraMode.CAMERA_TRACK:
                    _dx = mouseCurrent.X - mousePrevious.X;
                    _dx *= MouseTrackSpeed;

                    _dy = mouseCurrent.Y - mousePrevious.Y;
                    _dy *= MouseTrackSpeed;

                    _gCameraPos[0] -= _dx;
                    _gCameraPos[1] += _dy;

                    _gTargetPos[0] -= _dx;
                    _gTargetPos[1] += _dy;

                    break;

                case ECameraMode.CAMERA_ORBIT:
                    _dx = mouseCurrent.X - mousePrevious.X;
                    _dx *= MouseOrbitSpeed;

                    _dy = mouseCurrent.Y - mousePrevious.Y;
                    _dy *= MouseOrbitSpeed;

                    _gHeading += _dx;
                    _gPitch += _dy;

                    if (_gPitch > 90.0f)
                        _gPitch = 90.0f;

                    if (_gPitch < -90.0f)
                        _gPitch = -90.0f;

                    break;
            }
        }

        public void Zoom(float delta)
        {
            _dy = delta;
            _dy *= MouseDollySpeed;

            _gCameraPos[2] -= _dy;

            if (_gCameraPos[2] < 1 + CameraZnear)
                _gCameraPos[2] = 1 + CameraZnear;

            if (_gCameraPos[2] > CameraZfar - 10)
                _gCameraPos[2] = CameraZfar - 10;
        }

        public void AffectResize(int width, int height)
        {
            if (width == 0 || height == 0) return;
            GL.Viewport(0, 0, width, height);
            var aspectRatio = width / (double)height;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Glu.Perspective(CameraFovy, aspectRatio, CameraZnear, CameraZfar);
        }

        public enum ECameraMode
        {
            CAMERA_NONE, CAMERA_TRACK, CAMERA_ORBIT
        }

        public void Update()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            Glu.LookAt(_gCameraPos[0], _gCameraPos[1], _gCameraPos[2],
                      _gTargetPos[0], _gTargetPos[1], _gTargetPos[2],
                      0.0f, 1.0f, 0.0f);

            GL.Rotate(_gPitch, 1.0f, 0.0f, 0.0f);
            GL.Rotate(_gHeading, 0.0f, 1.0f, 0.0f);
        }
    }
}
