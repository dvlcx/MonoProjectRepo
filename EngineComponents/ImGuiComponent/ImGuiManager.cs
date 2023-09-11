using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Num = System.Numerics;
using ImGuiNET;
using MonoProject.ImGuiComponent;
using System.Text;

namespace MonoProject
{
    sealed class ImGuiManager : DrawableGameComponent
    {
        private ImGuiRenderer _imGuiRenderer;
        private byte[] ProjectName = new byte[100];
        //settings shit
        private Num.Vector3 backgroundColor = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);


        //main widgets shit
        private ImGuiMainMenuBar _mainBar; //upper main menu bar
        private ImGuiMainWindow _mainInspectorWindow; //game object inspector

        //sub widgets shit
        private ImGuiSubWindow _subWindow; 
        private bool helpShow = false;
        private bool projectCreationShow = false;
        
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
            

            _xnaTexture = ImGuiManager.CreateTexture(GraphicsDevice, 300, 150, pixel =>
			{
				var red = (pixel % 300) / 2;
				return new Color(red, 1, 1);
			});

			_imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(backgroundColor.X, backgroundColor.Y, backgroundColor.Z));
            
            _imGuiRenderer.BeforeLayout(gameTime);

            _mainBar.LayoutRealize(() => Layouts.MainBarOutput(ref projectCreationShow, ref helpShow));
            //_mainInspectorWindow.LayoutRealize(() => Layouts.MainInspectorOutput());
            CallSubWindows();
            _imGuiRenderer.AfterLayout();
            
            base.Draw(gameTime);
        }



        private void CallMainWindows(){

        }
        //sub window shit
        private void CallSubWindows()
        {
            if(projectCreationShow)
            {
                _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(800, 200), new Num.Vector2(500,800), "New project");
                _subWindow.LayoutRealize(() => Layouts.NewProjectOutput(ref ProjectName), ref projectCreationShow);
            }
            if(helpShow)
            {
                _subWindow = new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(800, 400), new Num.Vector2(500,100), "Help");
                _subWindow.LayoutRealize(() => Layouts.HelpOutput(), ref helpShow);
            }

        }

        private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
		{
			var texture = new Texture2D(device, width, height);

			Color[] data = new Color[width * height];
			for(var pixel = 0; pixel < data.Length; pixel++)
			{
				data[pixel] = paint(pixel);
			}

			texture.SetData(data);

			return texture;
		}
    }
}