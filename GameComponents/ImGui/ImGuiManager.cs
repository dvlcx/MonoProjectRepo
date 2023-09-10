using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;


namespace MonoProject
{
    sealed class ImGuiManager : DrawableGameComponent
    {
        private ImGuiRenderer _imGuiRenderer;
        private ImGuiMainMenuBar _mainBar;
        private ImGuiMainWindow _mainInspectorWindow;
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

			// Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
			_imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(clear_color.X, clear_color.Y, clear_color.Z));

            _imGuiRenderer.BeforeLayout(gameTime);
            _mainBar.LayoutRealize(() => MainBarOutput());
            _mainInspectorWindow.LayoutRealize(() => MainInspectorOutput());
            _imGuiRenderer.AfterLayout();
            
            base.Draw(gameTime);
        }

        private float f = 0.0f;
        private bool show_test_window = false;
        private bool show_another_window = false;
        private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
        private byte[] _textBuffer = new byte[100];



        
        private void MainBarOutput()
        {
             if(ImGui.BeginMainMenuBar())
                {
                    if(ImGui.BeginMenu("Project"))
                    {
                        ImGui.MenuItem("New Project...");
                        ImGui.MenuItem("Open project...");
                        ImGui.EndMenu();
                    }
                    if(ImGui.BeginMenu("Import"))
                    {
                        

                        ImGui.EndMenu();
                    }
                    if(ImGui.MenuItem("Settings"))
                    {


                        
                    }
                    ImGui.EndMainMenuBar();
                }
        }
        private void MainInspectorOutput()
        {
            {

                ImGui.Text("Hello, world!");
                ImGui.SliderFloat("float", ref f, 0.0f, 1.0f, string.Empty);
                ImGui.ColorEdit3("clear color", ref clear_color);
                if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
                if (ImGui.Button("Another Window")) show_another_window = !show_another_window;
                ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)", 1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));

                ImGui.InputText("Text input", _textBuffer, 100);

                ImGui.Text("Texture sample");
                ImGui.Image(_imGuiTexture, new Num.Vector2(300, 150), Num.Vector2.Zero, Num.Vector2.One, Num.Vector4.One, Num.Vector4.One); // Here, the previously loaded texture is used

            }

            // 2. Show another simple window, this time using an explicit Begin/End pair
            if (show_another_window)
            {
                ImGui.SetNextWindowSize(new Num.Vector2(200, 100), ImGuiCond.FirstUseEver);
                ImGui.Begin("Another Window", ref show_another_window);
                ImGui.Text("Hello");
                ImGui.End();
            }

            // 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
            if (show_test_window)
            {
                ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref show_test_window);
            }

        }
        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
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