using System.Numerics;
using System.Text;
using ImGuiNET;
using MonoProject.EditorComponent;
using MonoProject.EngineComponents;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics.Metrics;
namespace MonoProject.ImGuiComponent
{
    static class Layouts
    {
        public static void MainBarOutput(ImGuiSubWindow subW)
        {
             ImGui.BeginMainMenuBar();
                {
                    if(ImGui.BeginMenu("Project"))
                    {
                        if(ImGui.MenuItem("New Project...")) subW.type = SubWindowType.NewProjectW;
                        ImGui.MenuItem("Open project...");
                        ImGui.EndMenu();
                    }
                    if(ImGui.BeginMenu("Import"))
                    {
                        

                        ImGui.EndMenu();
                    }
                    if(ImGui.MenuItem("Help")) subW.type = SubWindowType.HelpW;
                    
                    ImGui.EndMainMenuBar();
                }
        }

        public static void SceneHierarchyOutput(ImGuiSubWindow subW, List<IFigure> figures)
        {
            if (ImGui.Button("Add"))
            {
                subW.type = SubWindowType.AddFigureW;
            }
            
            int counter = 0;
            foreach (var fig in figures)
            {
                if(ImGui.Selectable("Polfog " + counter,  fig.IsSelected))
                {
                    fig.IsSelected = !fig.IsSelected;
                }
                counter++;
            }

            //need cycle to spawn scene objects
            if(ImGui.TreeNode("Scene"))
            {
                ImGui.Selectable("ada0");
            }
        }

        public static void GameHierarchyOutput()
        {
            //need cycle to spawn Controller objects

        }
        private static System.Numerics.Vector3 trans = System.Numerics.Vector3.Zero;
        private static System.Numerics.Vector3 rot = System.Numerics.Vector3.Zero;
        private static System.Numerics.Vector3 sc = new System.Numerics.Vector3(1f, 1f, 1f);
        private static IFigure fig = null;
        public static void InspectorOutput(List<IFigure> figures)
        {   
            foreach (var f in figures)
            {
                if(f.IsSelected == true && f != fig) 
                {
                    fig = f;
                    break;
                }
            }
            if (fig == null) return;

            trans = Tools.ToSystemVector(fig.Translation);
            rot = Tools.ToSystemVector(fig.Rotation);
            sc = Tools.ToSystemVector(fig.Scale);
            
            ImGui.InputFloat3("Translate", ref trans);
            ImGui.InputFloat3("Rotate", ref rot);
            ImGui.InputFloat3("Scale", ref sc);

            fig.Translation = Tools.ToXnaVector(trans);
            fig.Rotation = Tools.ToXnaVector(rot);
            fig.Scale = Tools.ToXnaVector(sc);
            fig.ApplyTransform();
            
        }

        public static void NewProjectOutput(byte[] path, byte[] name)
        {
            ImGui.InputText("Target folder", path, 100);
            ImGui.InputText("Project Name", name, 100);
            if (ImGui.Button("Create"))
            {
                ProjectHandler.CreateProject(Encoding.ASCII.GetString(Tools.CropByte(path)), Encoding.ASCII.GetString(Tools.CropByte(name)));
                ProjectHandler.OpenProject();
            }  
            ImGui.Text($"Status: \n{ProjectHandler.status}".TrimEnd(':'));
        }

        public static void AddFigureOutput(EditorManager em)
        {
            if (ImGui.Selectable("Polygon")) em.AddFigure(new PolygonFigure(new Microsoft.Xna.Framework.Vector3(0, 0, 0), 5, 3));
        }
        public static void HelpOutput()
        {
              ImGui.Text("Hi!");
        }


        
    }
}