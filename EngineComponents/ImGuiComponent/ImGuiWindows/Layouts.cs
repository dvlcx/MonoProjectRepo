using System.Numerics;
using System.Text;
using ImGuiNET;
using MonoProject.EditorComponent;
using MonoProject.EngineComponents;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using System;


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

        public static void SceneHierarchyOutput(ImGuiSubWindow subW)
        {
            if (ImGui.Button("Add"))
            {
                subW.type = SubWindowType.AddFigureW;
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

        public static void GameHierarchyOutput()
        {
            //need cycle to spawn Controller objects

        }
        
        public static void InspectorOutput()
        {
            if(EditorManager.Figures.Count == 0) return;
            List<IFigure> selectedFigs = EditorManager.Figures.Where(fig => fig.IsSelected).ToList();
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

            if(trans!=transOrigin || rot!=rotOrigin || sc!=scOrigin)
            foreach(var fig in selectedFigs)
            {
                fig.Translation += Tools.ToXnaVector(trans) - Tools.ToXnaVector(transOrigin);
                fig.Rotation += Tools.ToXnaVector(rot) - Tools.ToXnaVector(rotOrigin);
                fig.Scale += Tools.ToXnaVector(sc) - Tools.ToXnaVector(scOrigin);
                fig.ApplyTransform();
            }

            if(color != colorOrigin) foreach (var fig2 in selectedFigs) fig2.ApplyColor(new Microsoft.Xna.Framework.Color(color.X, color.Y, color.Z));
            
            if(!name.SequenceEqual(nameOrigin)) selectedFigs[0].Name = Encoding.ASCII.GetString(Tools.CropByte(name));
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
            if (ImGui.Selectable("Polygon")) em.AddFigure(new PolygonFigure(new Microsoft.Xna.Framework.Vector3(0, 0, 0), 2, 2));
        }
        public static void HelpOutput()
        {
              ImGui.Text("Hi!");
        }


        
    }
}