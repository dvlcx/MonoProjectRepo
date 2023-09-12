using System.Text;
using ImGuiNET;

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
                        ImGui.MenuItem("Save");
                        ImGui.MenuItem("Save as...");
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
        public static void HelpOutput()
        {
              ImGui.Text("Hi!");
        }

        public static void MainInspectorOutput()
        {

            

        }
        
    }
}