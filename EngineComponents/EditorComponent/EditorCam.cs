using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MonoProject.EditorComponent
{
    class EditorCam
    {
        private Matrix _viewMatrix;
        public Matrix ViewMatrix {
            get {return _viewMatrix;}
        }
        private Matrix _projectionMatrix;
        public Matrix ProjectionMatrix {
            get {return _projectionMatrix;}
        }

        
        private readonly Vector3 _camTargStart = Vector3.Zero;

        private Vector3 _cameraPos = new Vector3(100f, 100f, 100f);
        private Quaternion _cameraQuat;
        private Vector3 _cameraScale;
        public EditorCam(float aspr)
        {
            _viewMatrix = Matrix.CreateLookAt(_cameraPos, _camTargStart, Vector3.Up);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspr,  1f, 2000f);
        }

        int scrollValue = 0;
        public void UpdatePos(KeyboardState ks, MouseState ms)
        {
            if(ks.IsKeyDown(Keys.Up))
            {
                _viewMatrix *= Matrix.CreateTranslation(Vector3.Up);
            }
            if(ks.IsKeyDown(Keys.Down))
            {
                _viewMatrix *= Matrix.CreateTranslation(Vector3.Down);
            }
            if(ks.IsKeyDown(Keys.Left))
            {
                _viewMatrix *= Matrix.CreateTranslation(Vector3.Left);
            }
            if(ks.IsKeyDown(Keys.Right))
            {
                _viewMatrix *= Matrix.CreateTranslation(Vector3.Right);
            }
            
            if(ms.ScrollWheelValue > scrollValue)
            {
                _viewMatrix *= Matrix.CreateScale(5f);
            }
            else if(ms.ScrollWheelValue < scrollValue)
            {
                _viewMatrix *= Matrix.CreateScale(0.2f);
            }
            scrollValue = ms.ScrollWheelValue;
            
        }
        
    }
}
