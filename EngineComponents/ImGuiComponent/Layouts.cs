using ImGuiNET;

namespace MonoProject.ImGuiComponent
{
    static class Layouts
    {
        public static void MainBarOutput(ref bool projectCreationShow, ref bool helpShow)
        {
             ImGui.BeginMainMenuBar();
                {
                    if(ImGui.BeginMenu("Project"))
                    {
                        if(ImGui.MenuItem("New Project...")) projectCreationShow = !projectCreationShow;
                        ImGui.MenuItem("Open project...");
                        ImGui.MenuItem("Save");
                        ImGui.MenuItem("Save as...");
                        ImGui.EndMenu();
                    }
                    if(ImGui.BeginMenu("Import"))
                    {
                        

                        ImGui.EndMenu();
                    }
                    if(ImGui.MenuItem("Help")) helpShow = !helpShow;
                    
                    ImGui.EndMainMenuBar();
                }
        }

        public static void NewProjectOutput(ref byte[] name)
        {
            ImGui.InputText("Target folder", name, 100);
            if (ImGui.Button("Create")); //add folder add 
        }
        public static void HelpOutput()
        {
              ImGui.Text("Hi!");
        }
        /*
        //main window shit
        public static void MainInspectorOutput()
        {
            {
                ImGui.Text("Hello, world!");
                
                if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
                if (ImGui.Button("Another Window")) show_another_window = !show_another_window;
                ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)", 1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));

                ImGui.InputText("Text input", _textBuffer, 100);

                ImGui.Text("Texture sample");
                ImGui.Image(_imGuiTexture, new Num.Vector2(300, 150), Num.Vector2.Zero, Num.Vector2.One, Num.Vector4.One, Num.Vector4.One); // Here, the previously loaded texture is used
            }

            if (show_another_window)
            {
                ImGui.SetNextWindowSize(new Num.Vector2(200, 100), ImGuiCond.FirstUseEver);
                ImGui.Begin("Another Window", ref show_another_window);
                ImGui.Text("Hello");
                ImGui.End();
            }
            if (show_test_window)
            {
                ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref show_test_window);
            }
            

        }
        */
    }
}