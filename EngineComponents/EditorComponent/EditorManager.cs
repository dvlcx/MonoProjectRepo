using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.EditorComponent;

namespace MonoProject.EngineComponents
{
    class EditorManager : DrawableGameComponent
    {
        private Game _game;
        private SpriteBatch _spriteBatch;
        private BasicEffect _effect;
        private EditorCam _editorCam;
        private SpriteFont _font;
        private BaseAxis _axes;
        
        private List<IFigure> _figures;
        private IFigure _selectedFig;
        public EditorManager(Game game) : base (game)
        {
            _game = game;
        }


        public override void Initialize()
        {
            _figures = new List<IFigure>();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _effect = new BasicEffect(GraphicsDevice);
            _editorCam = new EditorCam(GraphicsDevice.Viewport.AspectRatio);
            _axes = new BaseAxis();
            _axes.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _font = _game.Content.Load<SpriteFont>("Content//font");
            base.LoadContent();
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawFigures();
            
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, $"{_editorCam.GetData()}", new Vector2(310,20), Color.Black);
            _spriteBatch.End();
            _axes.Draw(GraphicsDevice, _effect, _editorCam.ViewMatrix, _editorCam.ProjectionMatrix, Matrix.Identity);

            base.Draw(gameTime);
        }
        
        public void AddFigure(IFigure fig)
        {
            _figures.Add(fig);
        }
        
        private void DrawFigures()
        {
            foreach (var fig in _figures)
            {
                fig.DrawFigure(GraphicsDevice, _effect, _editorCam.ViewMatrix, _editorCam.ProjectionMatrix, fig.WorldMatrix);
            }
        }




    }
}