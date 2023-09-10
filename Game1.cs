using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;

namespace MonoProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public ImGuiRenderer _imGuiRenderer;
        private ImGuiManager _imGuiManager;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Window.AllowUserResizing = true;
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _imGuiManager = new ImGuiManager(this);
            Components.Add(_imGuiManager);


            base.Initialize();
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            base.Draw(gameTime);
        }





	}
}
