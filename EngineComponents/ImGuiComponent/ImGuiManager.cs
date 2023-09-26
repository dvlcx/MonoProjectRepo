using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Num = System.Numerics;
using ImGuiNET;
using MonoProject.ImGuiComponent;
using System.Text;

namespace MonoProject.EngineComponents
{
    sealed class ImGuiManager : DrawableGameComponent
    {
        private ImGuiRenderer _imGuiRenderer;
        //settings shit
        private bool _imGuiShow = true;

        //main widgets shit
        private ImGuiMainMenuBar _mainBar; //upper main menu bar
        private ImGuiMainWindow _mainInspectorWindow; //game object inspector
        private ImGuiMainWindow _sceneHierarchyWindow; //game object inspector
        private ImGuiMainWindow _gameHierarchyWindow; //game object inspector

        //sub widgets shit
        private ImGuiSubWindow _subWindow; 
        private byte[] ProjectPath = new byte[100];
        private byte[] ProjectName = new byte[50];


        private Texture2D _xnaTexture;
        private IntPtr _imGuiTexture;
        public ImGuiManager(Game game) : base (game)
        {
            _imGuiRenderer = new ImGuiRenderer(game);
            _imGuiRenderer.RebuildFontAtlas();
        }
        
        public override void Initialize()
        {
            _mainBar = new ImGuiMainMenuBar();
            _mainInspectorWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(0,19), new Num.Vector2(300,600), "Inspector");
            _gameHierarchyWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(0,19), new Num.Vector2(300,600), "Game Hierarchy");
            _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None,Num.Vector2.Zero, Num.Vector2.Zero, null, SubWindowType.Null);

            _xnaTexture = ImGuiTools.CreateTexture(GraphicsDevice, 300, 150, pixel =>
			{
				var red = (pixel % 300) / 2;
				return new Color(red, 1, 1);
			});

			_imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Tab)) _imGuiShow = false;
            else _imGuiShow = true;
            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {            
            if(_imGuiShow)
            {
            _imGuiRenderer.BeforeLayout(gameTime);
            _mainBar.LayoutRealize(() => Layouts.MainBarOutput(_subWindow));
            _gameHierarchyWindow.LayoutRealize(() => Layouts.GameHierarchyOutput());
            //_mainInspectorWindow.LayoutRealize(() => Layouts.MainInspectorOutput());
            CallSubWindows();
            _imGuiRenderer.AfterLayout();
            }

            
            base.Draw(gameTime);
        }
        private void CallSubWindows()
        {
            if(_subWindow.type == SubWindowType.NewProjectW)
            {
                _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(800, 200), new Num.Vector2(500,800), "New project", SubWindowType.NewProjectW);
                _subWindow.LayoutRealize(() => Layouts.NewProjectOutput(ProjectPath, ProjectName));
            }
            else if(_subWindow.type == SubWindowType.HelpW)
            {
                _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(800, 400), new Num.Vector2(500,100), "Help", SubWindowType.HelpW);
                _subWindow.LayoutRealize(() => Layouts.HelpOutput());
            }
        }


    
 
    }
}