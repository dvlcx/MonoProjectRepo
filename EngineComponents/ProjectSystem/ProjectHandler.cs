using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

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
            if (Directory.Exists($"{path}\\{name}")) 
            {
                Projects prs = DeserializeProjects();
                if(prs == null) prs = new Projects();
                prs.ProjectList.Add(new Project(name, path));
                SerializeProjects(prs);

                OpenProject(path, name);
            }
        }
        
        public static void OpenProject(string path, string name)
        {
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