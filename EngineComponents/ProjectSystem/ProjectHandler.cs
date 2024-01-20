using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MonoProject.ProjectSystem
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

            GetCreationString(path, name, out string fileName, out string arguments);
            Process process = new Process();
            process.StartInfo  = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            process.Start();
            status = process.StandardOutput.ReadLine() + process.StandardError.ReadLine();
            if (Directory.Exists(arguments.Substring(arguments.IndexOf("-o") + 3))) 
            {
                Projects prs = DeserializeProjects();
                if(prs == null) prs = new Projects();
                prs.ProjectList.Add(new Project(name, path));
                SerializeProjects(prs);
                OpenProject(path, name);
            }
        }

        private static void GetCreationString(string path, string name, out string fileName, out string arguments)
        {
            path = path.TrimStart(' ');
            path = path.TrimEnd(@"/\ ".ToCharArray());
            if(OperatingSystem.IsWindows())
            {
                fileName = "dotnet.exe";
                arguments = $"new mgdesktopgl -o {path}\\{name}";
            }   
            else
            {
                fileName = "dotnet";
                arguments = $"new mgdesktopgl -o {path}/{name}";    
            }
        }

        public static void OpenProject(string path, string name)
        {
            if (path is null || name is null || path.Length == 0 || name.Length == 0) return;
            currentProject = new Project(name, path);
        }
        
        public static void SerializeProjects(Projects prs)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Projects));
            FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate);
            xml.Serialize(fs, prs);
        }

        public static Projects DeserializeProjects()
        {
            XmlSerializer xml = new XmlSerializer(typeof(Projects));
            using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
            {
                if(fs.Length == 0) return null;
                return (Projects)xml.Deserialize(fs);
            }
        }


        public static void Save()
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