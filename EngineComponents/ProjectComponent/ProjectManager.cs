using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using System.Linq;
using Num = System.Numerics;
using ImGuiNET;
using MonoProject.ImGuiComponent;

using System.IO;
using System.Data;
using System.Xml.Serialization;
using System.Diagnostics;

namespace MonoProject.EngineComponents
{
    sealed class ProjectManager : GameComponent
    {
        private Game _game;
        public Project CurrentProject {get; private set;} = null;
        public Action ToUpdate {get; set;} = null;

        public ProjectManager(Game game) : base (game)
        {

        }
        
        protected void LoadContent()
        {

        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            ToUpdate?.Invoke();
            if(ToUpdate is not null) ToUpdate = null;

            base.Update(gameTime);
        }
        public void CreateProject(string path, string name, out string status)
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
            }
        }

        private void GetCreationString(string path, string name, out string fileName, out string arguments)
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

        public void OpenProject(string path, string name)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(name)) return;
            CurrentProject = new Project(name, path);
        }
        
        public void SerializeProjects(Projects prs)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Projects));
            using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, prs);
            }
        }

        public Projects DeserializeProjects()
        {
            XmlSerializer xml = new XmlSerializer(typeof(Projects));
            using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
            {
                if(fs.Length == 0) return null;
                Projects prs = (Projects)xml.Deserialize(fs);
                CheckProjects(prs);
                return prs;
            }
        }


        public void CheckProjects(Projects prs)
        {
            prs.ProjectList.ForEach((x) => x.Exists = Directory.Exists(x.GetFullString()) ?  true : false);
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