using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;

namespace MonoProject
{
    static class ProjectManager
    {
        public static Project currentProject = null;
        public static string status = "...";
        public static void CreateProject(string path, string name)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
            FileName = "dotnet.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Arguments = $"new mgdesktopgl -o {path}\\{name}"
            };
            process.StartInfo = startInfo;
            process.Start();
            status = process.StandardOutput.ReadLine()+process.StandardError.ReadLine();
            
        }

        public static void LoadProject()
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