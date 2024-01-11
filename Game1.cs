using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;
using MonoProject.EngineComponents;
using MonoProject.ProjectSystem;
namespace MonoProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private ImGuiManager _imGuiManager;
        private EditorManager _editorManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.AllowUserResizing = true;
            Mouse.SetPosition( _graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/2);
            IsMouseVisible = true;
            Window.Title = ProjectHandler.currentProject?.Name;
        }

        protected override void Initialize()
        {
            _editorManager = new EditorManager(this);
            _imGuiManager = new ImGuiManager(this, _editorManager);
            
            Components.Add(_imGuiManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Window.Title = ProjectHandler.currentProject?.Name;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            base.Draw(gameTime);
        }





	}
}
