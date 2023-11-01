using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoProject.EngineComponents;
using System;


namespace MonoProject.EditorComponent
{
    class EditorCam
    {
        private Matrix _viewMatrix;
        public Matrix ViewMatrix {
            get {return _viewMatrix;}
            private set {}
        }
        private Matrix _projectionMatrix;
        public Matrix ProjectionMatrix {
            get {return _projectionMatrix;}
            private set {}
        }
        
        //zoom
        private const float ZOOM_POWER = 0.2f;
        private float _currentZoom = 5f;
        private float CurrentZoom
        {
            get {return _currentZoom;}
            set {_currentZoom = MathHelper.Clamp(value, 1f, 20f);}
        }
        
        //arcball move
        private const float MOUSE_POWER = 0.01f;
        float yaw = 0.5f;
        float pitch = 0.5f;
        
        private readonly Vector3 _camTargStart;
        private readonly Vector3 _camPosStart;
        public Vector3 CameraPos {get; private set;}
        public Vector3 CameraTarget {get; private set;}
        
        
        public EditorCam(float aspr)
        {
            _camPosStart = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f));
            _camTargStart = Vector3.Zero;
            CameraPos = _camPosStart;
            CameraTarget = _camTargStart;
            _viewMatrix = Matrix.CreateLookAt(CameraPos, CameraTarget, Vector3.Up);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspr,  0.0001f, float.MaxValue);
        }

        private Point _lastMousePosition;
        private int _lastScrollValue = 0;
        public void UpdatePos(KeyboardState ks, MouseState ms)
        {
            if(ks.IsKeyDown(Keys.R) && !ImGuiManager.IsSmthFocused) ResetCamera();
            
            //camera move
            if (ms.MiddleButton == ButtonState.Pressed)
            {
                if(_lastMousePosition != ms.Position)
                {
                    Point delta = ms.Position - _lastMousePosition;
                    if(delta.Y > 0)
                    {
                        pitch -= MOUSE_POWER * delta.Y;
                    }
                    else if(delta.Y < 0)
                    {
                        pitch += MOUSE_POWER * -delta.Y;
                    }
                    
                    if(delta.X > 0)
                    {
                        yaw -= MOUSE_POWER * delta.X;
                    }
                    else if(delta.X < 0)
                    {
                        yaw += MOUSE_POWER * -delta.X;
                    }
                }
            }

            if(ms.ScrollWheelValue > _lastScrollValue)
            {   
                CurrentZoom += ZOOM_POWER;

            }
            else if(ms.ScrollWheelValue < _lastScrollValue)
            {
                CurrentZoom -= ZOOM_POWER;

            }

            pitch = MathHelper.Clamp(pitch, -1.4f, 1.4f); 
            CameraPos = Vector3.Transform(Vector3.Forward * CurrentZoom, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0)) + CameraTarget;
            _viewMatrix = Matrix.CreateLookAt(CameraPos, CameraTarget, Vector3.Up);

            _lastScrollValue = ms.ScrollWheelValue;
            _lastMousePosition = ms.Position;
        }

        private void ResetCamera()
        {
            yaw = 0.5f;
            pitch = 0.5f;
            CurrentZoom = 5f;
            CameraPos = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f));
            _viewMatrix = Matrix.CreateLookAt(_camPosStart, _camTargStart, Vector3.Up);
        }
        
        public void ChangeFocus(Vector3 nt)
        {
            CameraTarget = nt;
        }
        
        public string GetData()
        {
            Vector3 sc;
            Quaternion r;
            Vector3 tr;
            _viewMatrix.Decompose(out sc,out r,out tr);
            Vector3 direction = Vector3.Transform(Vector3.UnitX, r);
            float rot = (float)Math.Atan2(direction.Y, direction.X);
            string o = $"Position (y/p): {CameraPos}\nAngle: {MathHelper.ToDegrees(rot)}\nScale: {_currentZoom}";
            return o;
        }
    }
}
