using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using System.Linq;
using Num = System.Numerics;
using ImGuiNET;
using MonoProject.ImGuiComponent;
using MonoProject.EditorComponent;

using MonoProject.EngineComponents;
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
        public static bool IsAnySubWindowActive = false;

        //main widgets
        private ImGuiMainMenuBar _mainBar; //upper main menu bar
        private ImGuiMainWindow _inspectorWindow; //game object inspector (for transform)
        private ImGuiMainWindow _sceneHierarchyWindow; //scene inspector (node tree of scene objects)
        private ImGuiMainWindow _gameHierarchyWindow; //game inspector (node tree of scenes)

        //sub widgets shit
        private ImGuiSubWindow _subWindow; 



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
            _mainBar = new ImGuiMainMenuBar(() => MainBarOutput(_subWindow));
            _sceneHierarchyWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(0,19), new Num.Vector2(200, 800),
             "Scene Objects", () => SceneHierarchyOutput(_subWindow));
            _gameHierarchyWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(0,819), new Num.Vector2(200, 291),
             "Game Structure", () => MainBarOutput(_subWindow));
            _inspectorWindow = new ImGuiMainWindow(ImGuiWindowFlags.None, new Num.Vector2(1620,19), new Num.Vector2(300, 600),
             "Inspector", () => InspectorOutput());

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
            IsSmthHovered = ImGui.IsWindowHovered(ImGuiHoveredFlags.AnyWindow | ImGuiHoveredFlags.AllowWhenBlockedByPopup) | ImGui.IsAnyItemHovered();
            IsSmthFocused = ImGui.IsAnyItemActive();
            if(_subWindow != null) IsAnySubWindowActive = true;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {            
            if(_imGuiShow)
            {
            _imGuiRenderer.BeforeLayout(gameTime);
            _mainBar.LayoutRealize();
            _sceneHierarchyWindow.LayoutRealize();
            _gameHierarchyWindow.LayoutRealize();
            _inspectorWindow.LayoutRealize();
            if(_subWindow != null && !_subWindow.Status) ChangeSubWindow(SubWindowType.Null);
            else _subWindow?.LayoutRealize();

            _imGuiRenderer.AfterLayout();
            }

            
            base.Draw(gameTime);
        }

        #region MainControls
        private void MainBarOutput(ImGuiSubWindow subW)
        {
            ImGui.BeginMainMenuBar();
            {
                if(ImGui.BeginMenu("Project"))
                {
                    if(ImGui.MenuItem("New Project...")) 
                    {
                        ChangeSubWindow(SubWindowType.NewProjectW);
                        _status = "...";
                    }
                    ImGui.MenuItem("Open project...");
                    ImGui.EndMenu();
                }
                if(ImGui.BeginMenu("Import"))
                {
                        

                    ImGui.EndMenu();
                }
                if(ImGui.MenuItem("Help")) ChangeSubWindow(SubWindowType.HelpW);;
                    
                ImGui.EndMainMenuBar();
            }
        }
        private void SceneHierarchyOutput(ImGuiSubWindow subW)
        {
            if (ImGui.Button("Add"))
            {
               ChangeSubWindow(SubWindowType.AddFigureW);
            }
            
            foreach (var fig in EditorManager.Figures)
            {
                if(ImGui.Selectable(fig.Name,  fig.IsSelected))
                {
                    fig.IsSelected = !fig.IsSelected;
                    if (!EditorManager.KeyboardState.IsKeyDown(Keys.LeftControl))
                    foreach(var f in EditorManager.Figures) if (f != fig && f.IsSelected)
                    {
                        f.IsSelected = !f.IsSelected;
                    }
                }
            }

            //need cycle to spawn scene objects
            if(ImGui.TreeNode("Scene"))
            {
                ImGui.Selectable("ada0");
            }
        }
        public static void InspectorOutput()
        {
            if(EditorManager.Figures.Count == 0) return;
            System.Collections.Generic.List<IFigure> selectedFigs = EditorManager.Figures.Where(fig => fig.IsSelected).ToList();
            if (selectedFigs.Count == 0) return;
            if (EditorManager.ListChanged) 
            {
                EditorManager.ListChanged = false;
                return;
            }

            byte[] name = new byte[100];
            System.Numerics.Vector3 trans = System.Numerics.Vector3.Zero;
            System.Numerics.Vector3 rot = System.Numerics.Vector3.Zero;
            System.Numerics.Vector3 sc = new System.Numerics.Vector3(1f);
            System.Numerics.Vector3 color = System.Numerics.Vector3.Zero;

            selectedFigs.ForEach(fig => {
                trans += Tools.ToSystemVector(fig.Translation); 
                rot = Tools.ToSystemVector(fig.Rotation); 
                sc = Tools.ToSystemVector(fig.Scale); 
                color = Tools.ToSystemVector(fig.Color.ToVector3());
            });
            if(selectedFigs.Count == 1) Encoding.ASCII.GetBytes(selectedFigs[0].Name).CopyTo(name, 0);

            trans /= selectedFigs.Count;
            byte[] nameOrigin = new byte[100];
            name.CopyTo(nameOrigin, 0);
            System.Numerics.Vector3 transOrigin = trans;
            System.Numerics.Vector3 rotOrigin = rot;
            System.Numerics.Vector3 scOrigin = sc;
            System.Numerics.Vector3 colorOrigin = color;

            if(selectedFigs.Count == 1) ImGui.InputText("Name", name, 100);
            ImGui.DragFloat3("Translate", ref trans, 0.01f);
            ImGui.DragFloat3("Rotate", ref rot, 0.01f);
            ImGui.DragFloat3("Scale", ref sc, 0.01f);
            ImGui.ColorEdit3("Color", ref color);

            if(trans!=transOrigin || rot!=rotOrigin || sc!=scOrigin) foreach(var fig in selectedFigs)
            {
                fig.Translation += Tools.ToXnaVector(trans) - Tools.ToXnaVector(transOrigin);
                fig.Rotation += Tools.ToXnaVector(rot) - Tools.ToXnaVector(rotOrigin);
                fig.Scale += Tools.ToXnaVector(sc) - Tools.ToXnaVector(scOrigin);
                fig.ApplyTransform();
            }

            if(color != colorOrigin) foreach (var fig2 in selectedFigs) fig2.ApplyColor(new Microsoft.Xna.Framework.Color(color.X, color.Y, color.Z));
            
            if(!name.SequenceEqual(nameOrigin)) selectedFigs[0].Name = Encoding.ASCII.GetString(Tools.CropByte(name));
        }
        private void GameHierarchyOutput()
        {
            //need cycle to spawn Controller objects

        }
        #endregion
        
        #region SubControls
        private void ChangeSubWindow(SubWindowType swt)
        {
            _subWindow = swt switch
            {
                SubWindowType.AddFigureW => new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(500, 500), new Num.Vector2(200,200),
                 "Add Figure", SubWindowType.AddFigureW, () => AddFigureOutput(_editorManager)),
                SubWindowType.NewProjectW => new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(700, 200), new Num.Vector2(550,500),
                 "New Project", SubWindowType.NewProjectW, () => NewProjectOutput()),
                SubWindowType.HelpW => new ImGuiSubWindow(ImGuiWindowFlags.None, new Num.Vector2(800, 400), new Num.Vector2(500,100),
                  "Help", SubWindowType.HelpW, () => HelpOutput()),
                SubWindowType.Null => null
            };
        }

        private byte[] _path = new byte[100];
        private byte[] _name = new byte[50];
        private string _status;
        private void NewProjectOutput()
        {
            ImGui.InputText("Target folder", _path, 100);
            ImGui.InputText("Project Name", _name, 50);
            if (ImGui.Button("Create"))
            {
                ProjectHandler.CreateProject(Encoding.ASCII.GetString(Tools.CropByte(_path)), Encoding.ASCII.GetString(Tools.CropByte(_name)), out _status);
                //  ProjectHandler.OpenProject();
                Array.Clear(_path);
                Array.Clear(_name);
            }  
            ImGui.Text($"Status: \n{_status}".TrimEnd(':'));
        } 
        private void AddFigureOutput(EditorManager em)
        {
            if (ImGui.Selectable("Polygon")) em.AddFigure(new PolygonFigure(new Microsoft.Xna.Framework.Vector3(0, 0, 0), 2, 2));
        }
    
    
        private void HelpOutput()
        {
              ImGui.Text("Hi!");
        }
        #endregion
    }

}