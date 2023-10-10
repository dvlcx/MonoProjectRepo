using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImGuiNET;
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



        public static List<IFigure> Figures;
        public static bool ListChanged {get; set;}
        public static KeyboardState KeyboardState {get; private set;}
        public static MouseState MouseState {get; private set;}

        public EditorManager(Game game) : base (game)
        {
            _game = game; 
        }


        public override void Initialize()
        {
            ListChanged = false;
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
        private ButtonState _leftButtonLast = new ButtonState();
        public override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            
            _editorCam.UpdatePos(KeyboardState, MouseState);
            if(MouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton != _leftButtonLast && !ImGuiManager.IsSmthHovered)
            {
                SelectFigure(KeyboardState.IsKeyDown(Keys.LeftControl));
            }
            
            if(KeyboardState.IsKeyDown(Keys.Delete) && (!ImGuiManager.IsSmthFocused ))
            {
                DeleteFigure();
            }
            
            base.Update(gameTime);
            _leftButtonLast = MouseState.LeftButton;
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
        public void DeleteFigure() 
        {
            for (int i = 0; i < Figures.Count; i++) if(Figures[i].IsSelected)
            {
                Figures.Remove(Figures[i]);
            }
        }
        private void DrawFigures()
        {
            foreach (var fig in Figures)
            {
                fig.DrawFigure(GraphicsDevice, _effect, _editorCam.ViewMatrix, _editorCam.ProjectionMatrix, fig.WorldMatrix);
            }
        }

        private delegate void ActionRef<T>(ref T item);
        private void SelectFigure(bool ctrlPressed)
        {
            Point MouseStatePos = Mouse.GetState().Position;
            Vector3 rayStart = GraphicsDevice.Viewport.Unproject(new Vector3(MouseStatePos.X, MouseStatePos.Y, 0f),
             _editorCam.ProjectionMatrix, _editorCam.ViewMatrix, Matrix.Identity);
            Vector3 rayEnd = GraphicsDevice.Viewport.Unproject(new Vector3(MouseStatePos.X, MouseStatePos.Y, 0.08f),
             _editorCam.ProjectionMatrix, _editorCam.ViewMatrix, Matrix.Identity);
            Vector3 dir = rayEnd - rayStart;
            dir.Normalize();
            Ray selectRay = new Ray(rayStart, dir);
   
            int intersections = 0;
            float d = 1000;
            IFigure closest = Figures[0];
            IFigure soloSelect = Figures[0];

            ActionRef<IFigure> a = null;
            foreach (var fig in Figures)
            {
                bool check = selectRay.Intersects(fig.BoundingBox) > 0f;
                if(check && d > Vector3.Distance(fig.Translation, _editorCam.CameraPos))
                {
                    d = Vector3.Distance(fig.Translation, _editorCam.CameraPos);
                    closest = fig;
                }

                if (check && ctrlPressed) 
                {
                    intersections++;
                    soloSelect = fig;
                    a = (ref IFigure fi) => fi.IsSelected = !fi.IsSelected;
                }
                else if(check && !ctrlPressed)
                {
                    intersections++;
                    soloSelect = fig;
                    a = (ref IFigure fi) => 
                    {
                        fi.IsSelected = true;
                        foreach (var f in Figures) if(f != fi)
                        {
                            f.IsSelected = false;
                        }
                    };
                }
                else if(!check && ctrlPressed) 
                {
                    continue;
                }
                else if(!check && !ctrlPressed) foreach (var f in Figures)
                {
                    f.IsSelected = false;
                }
            }
            ListChanged = true;

            if(intersections > 1)
            {
                a.Invoke(ref closest);
                ListChanged = true;
            }  
            else if(intersections == 1)
            {
                a.Invoke(ref soloSelect);
                ListChanged = true;
            }
        }




    }
}