using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.ImGuiComponent;
using System;
using System.IO;

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
            set {_currentZoom = MathHelper.Clamp(value, 4f, 20f);}
        }
        
        //arcball move
        private const float MOUSE_POWER = 0.02f;
        float yaw = -0.5f;
        float pitch = -0.5f;
        
        private readonly Vector3 _camTargStart = Vector3.Zero;
        private Vector3 _cameraPos;
        
        public EditorCam(float aspr)
        {
            _cameraPos = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f));

            _viewMatrix = Matrix.CreateLookAt(_cameraPos, _camTargStart, Vector3.Up);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspr,  0.0001f, float.MaxValue);
        }

        Point lastMousePosition;
        int lastScrollValue = 0;
        public void UpdatePos(KeyboardState ks, MouseState ms)
        {
            if(ks.IsKeyDown(Keys.NumPad0)) ResetCamera();
            
            //camera move
            if (ms.MiddleButton == ButtonState.Pressed)
            {
                if(lastMousePosition != ms.Position)
                {
                    Point delta = ms.Position - lastMousePosition;
                    if(delta.Y > 0)
                    {
                        pitch += MOUSE_POWER/2;
                    }
                    else if(delta.Y < 0)
                    {
                        pitch -= MOUSE_POWER/2;
                    }
                    
                    if(delta.X > 0)
                    {
                        yaw -= MOUSE_POWER;
                    }
                    else if(delta.X < 0)
                    {
                        yaw += MOUSE_POWER;
                    }
                }
            }

            

            //zooming
            if(ms.ScrollWheelValue > lastScrollValue)
            {   
                CurrentZoom += ZOOM_POWER;

            }
            else if(ms.ScrollWheelValue < lastScrollValue)
            {
                CurrentZoom -= ZOOM_POWER;

            }
            
            //camera change apply
            pitch = MathHelper.Clamp(pitch, -1.4f, 1.4f); 
            _cameraPos = Vector3.Transform(Vector3.Backward*CurrentZoom, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
            _viewMatrix = Matrix.CreateLookAt(_cameraPos, _camTargStart, Vector3.Up);

            //prev frame info save
            lastScrollValue = ms.ScrollWheelValue;
            lastMousePosition = ms.Position;
        }

        private void ResetCamera()
        {
            yaw = 0.5f;
            pitch = 0.5f;
            CurrentZoom = 1f;
            _cameraPos = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f));
            _viewMatrix = Matrix.CreateLookAt(_cameraPos, _camTargStart, Vector3.Up);
        }
        
        private void UpdateCam(float CurrentZoom, float yaw, float pitch)
        {

        }
        
        public string GetData()
        {
            Vector3 sc;
            Quaternion r;
            Vector3 tr;
            _viewMatrix.Decompose(out sc,out r,out tr);
            Vector3 direction = Vector3.Transform(Vector3.UnitX, r);
            float rot = (float)Math.Atan2(direction.Y, direction.X);
            string o = $"Position (y/p): {yaw}/{pitch}\nAngle: {MathHelper.ToDegrees(rot)}\nScale: {_currentZoom}";
            return o;
            
        }
    }
}
