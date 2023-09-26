using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;
using MonoProject.EditorComponent;

namespace MonoProject.EngineComponents
{
    class EditorManager : DrawableGameComponent
    {
        private BasicEffect _effect;
        private EditorCam _editorCam;
        private AxisObject _axes;
        public EditorManager(Game game) : base (game)
        {
            
        }

         public override void Initialize()
        {
            _effect = new BasicEffect(GraphicsDevice);
            _editorCam = new EditorCam(GraphicsDevice.Viewport.AspectRatio);
            _axes = new AxisObject();
            _axes.Initialize(GraphicsDevice);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            _editorCam.UpdatePos(ks, ms);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);
            _axes.Draw(GraphicsDevice, _effect, _editorCam.ViewMatrix, _editorCam.ProjectionMatrix, Matrix.Identity);
            base.Draw(gameTime);
        }
        



    }
}