using System.Numerics;
using System.Text;
using ImGuiNET;
using MonoProject.EditorComponent;
using MonoProject.EngineComponents;

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

        }
        public static void NewProjectOutput(byte[] path, byte[] name)
        {
            ImGui.InputText("Target folder", path, 100);
            ImGui.InputText("Project Name", name, 100);
            if (ImGui.Button("Create"))
            {
                ProjectManager.CreateProject(Encoding.ASCII.GetString(ImGuiTools.CropByte(path)), Encoding.ASCII.GetString(ImGuiTools.CropByte(name)));
                ProjectManager.OpenProject();
            }  
            ImGui.Text($"Status: \n{ProjectManager.status}".TrimEnd(':'));
        }

        public static void AddFigureOutput(EditorManager em)
        {
            if (ImGui.Selectable("Polygon")) em.AddFigure(new PolygonFigure(new Vector3(0,2,0), 5, 3));
        }
        public static void HelpOutput()
        {
              ImGui.Text("Hi!");
        }


        
    }
}