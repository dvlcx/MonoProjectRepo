using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;

namespace MonoProject
{
    static class ProjectHandler
    {
        public static Project currentProject = null;
        public static void CreateProject(string path, string name, out string status)
        {
            if(!Directory.Exists(path)) 
            {
                status = "There is no such folder!";
                return;
            }
            else if(name == "") 
            {
                status = "Empty name!";
                return;
            }
            
            Process process = new Process();
            process.StartInfo  = new ProcessStartInfo
            {
                FileName = "dotnet.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = $"new mgdesktopgl -o {path}\\{name}"
            };
            process.Start();
            status = process.StandardOutput.ReadLine() + process.StandardError.ReadLine();
        }

        public static void OpenProject(string path)
        {
            
            
        }

        public static void BuildProject()
        {
            
            
        }

        public static void RunProject()
        {
            
            
        }



    }
}