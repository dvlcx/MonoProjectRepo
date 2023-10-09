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
        private EditorManager _editorManager;

        //settings
        private bool _imGuiShow = true;
        public static bool IsSmthHovered = false;
        public static bool IsSmthFocused = false;

        //main widgets
        private ImGuiMainMenuBar _mainBar; //upper main menu bar
        private ImGuiMainWindow _inspectorWindow; //game object inspector (for transform)
        private ImGuiMainWindow _sceneHierarchyWindow; //scene inspector (node tree of scene objects)
        private ImGuiMainWindow _gameHierarchyWindow; //game inspector (node tree of scenes)

        //sub widgets shit
        private ImGuiSubWindow _subWindow; 
        private byte[] ProjectPath = new byte[100];
        private byte[] ProjectName = new byte[50];


        private Texture2D _xnaTexture;
        private IntPtr _imGuiTexture;
        
        public ImGuiManager(Game game, EditorManager em) : base (game)
        {
            _imGuiRenderer = new ImGuiRenderer(game);
            _imGuiRenderer.RebuildFontAtlas();
            _editorManager = em;
        }
        
        public override void Initialize()
        {
            _mainBar = new ImGuiMainMenuBar();
            _sceneHierarchyWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(0,19), new Num.Vector2(200, 800), "Scene Objects");
            _gameHierarchyWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(0,819), new Num.Vector2(200, 291), "Game Structure");
            _inspectorWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(1620,19), new Num.Vector2(300, 600), "Inspector");
            _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None, Num.Vector2.Zero, Num.Vector2.Zero, null, SubWindowType.Null);

            _xnaTexture = Tools.CreateTexture(GraphicsDevice, 300, 150, pixel =>
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
            IsSmthHovered = ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow) | ImGui.IsAnyItemHovered();
            IsSmthFocused = ImGui.IsWindowFocused(ImGuiFocusedFlags.AnyWindow);
        }

        public override void Draw(GameTime gameTime)
        {            
            if(_imGuiShow)
            {
            _imGuiRenderer.BeforeLayout(gameTime);
            _mainBar.LayoutRealize(() => Layouts.MainBarOutput(_subWindow));
            _sceneHierarchyWindow.LayoutRealize(() => Layouts.SceneHierarchyOutput(_subWindow, _editorManager.Figures));
            _gameHierarchyWindow.LayoutRealize(() => Layouts.GameHierarchyOutput());
            _inspectorWindow.LayoutRealize(() => Layouts.InspectorOutput(_editorManager.InspectedFig));
            CallSubWindows();
            _imGuiRenderer.AfterLayout();
            }

            
            base.Draw(gameTime);
        }
        
        private void CallSubWindows()
        {
            if(_subWindow.type == SubWindowType.AddFigureW)
            {
                _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(500, 500), new Num.Vector2(200,200), "Add Figure", SubWindowType.AddFigureW);
                _subWindow.LayoutRealize(() => Layouts.AddFigureOutput(_editorManager));
            }
            else if(_subWindow.type == SubWindowType.NewProjectW)
            {
                _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(800, 200), new Num.Vector2(500,800), "New Project", SubWindowType.NewProjectW);
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