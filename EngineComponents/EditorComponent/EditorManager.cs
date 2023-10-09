using System.Collections.Generic;
using System.Linq;
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



        public List<IFigure> Figures {get; private set;}
        private IFigure _inspectorFig;
        public IFigure InspectedFig 
        {
            get {return _inspectorFig;} 
            private set {
                if(InspectedFig != value)
                {
                    _inspectorFig = value;
                    IsInspectedChanged = true;
                }
            }
        }
        public static bool IsInspectedChanged = false;
        public EditorManager(Game game) : base (game)
        {
            _game = game; 
        }


        public override void Initialize()
        {
            Figures = new List<IFigure>();
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
            if(ms.LeftButton == ButtonState.Pressed && !ImGuiManager.IsSmthHovered)
            {
                if(!SelectFigure()) InspectedFig = null;
            }
            
            if(ks.IsKeyDown(Keys.Delete) && !ImGuiManager.IsSmthFocused)
            {
                DeleteFigure(InspectedFig);
                InspectedFig = null;
            }
            
            foreach(var fig in Figures) if(fig.IsSelected) InspectedFig = fig;
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
        
        public void AddFigure(IFigure fig) => Figures.Add(fig);
        public void DeleteFigure(IFigure fig) => Figures.Remove(fig);
        
        private void DrawFigures()
        {
            foreach (var fig in Figures)
            {
                fig.DrawFigure(GraphicsDevice, _effect, _editorCam.ViewMatrix, _editorCam.ProjectionMatrix, fig.WorldMatrix);
            }
        }

        private bool SelectFigure()
        {
            Point msPos = Mouse.GetState().Position;
            Vector3 rayStart = GraphicsDevice.Viewport.Unproject(new Vector3(msPos.X, msPos.Y, 0f),
             _editorCam.ProjectionMatrix, _editorCam.ViewMatrix, Matrix.Identity);
            Vector3 rayEnd = GraphicsDevice.Viewport.Unproject(new Vector3(msPos.X, msPos.Y, 0.1f),
             _editorCam.ProjectionMatrix, _editorCam.ViewMatrix, Matrix.Identity);
            Vector3 dir = rayEnd - rayStart;
            dir.Normalize();
            Ray selectRay = new Ray(rayStart, dir);
            bool isFound = false;
            foreach (var fig in Figures)
            {
                if (selectRay.Intersects(fig.BoundingBox) > 0f && !isFound) 
                {
                    fig.IsSelected = true;
                    isFound = true;
                }
                else fig.IsSelected = false;
            }
            return isFound;
        }

        private void SelectFigure(string figName)
        {
            
        }




    }
}