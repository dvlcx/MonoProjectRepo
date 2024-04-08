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
        public Projects Projects {get; private set;} = null;
        public Project CurrentProject {get; private set;} = null;
        public string Status { get; private set; } = "...";
        private Action ToUpdate {get; set;} = null;
        public ProjectManager(Game game) : base (game)
        {

        }
        
        public void LoadContent()
        {

        }

        public override void Initialize()
        {
            Projects = DeserializeProjects();
            Projects ??= new Projects();
            CheckProjects();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            ToUpdate?.Invoke();
            if(ToUpdate is not null) ToUpdate = null;

            base.Update(gameTime);
        }

        public void CreateProject(string path, string name)
        {
            if(!Directory.Exists(path)) 
            {
                Status = "There is no such folder!";
                return;
            }
            else if(name == "") 
            {
                Status = "Empty name!";
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
            Status = process.StandardOutput.ReadLine() + process.StandardError.ReadLine();
            if (Directory.Exists(arguments.Substring(arguments.IndexOf("-o") + 3))) 
            {
                Projects.ProjectList.Add(new Project(name, path));
                SerializeProjects();
            }
        }
        public void CreateProjectNextFrame(string path, string name) =>
            ToUpdate += () => CreateProject(path, name);


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

        public void OpenProject(Project pr)
        {
            if (string.IsNullOrEmpty(pr.Path) || string.IsNullOrEmpty(pr.Name)) return;
            CurrentProject = pr;
        }
        public void OpenProjectNextFrame(Project pr) =>
            ToUpdate += () => OpenProject(pr);

        public void DeleteProject(Project pr)
        {

        }

        private void SerializeProjects()
        {
            XmlSerializer xml = new XmlSerializer(typeof(Projects));
            using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, Projects);
            }
        }

        public Projects DeserializeProjects()
        {
            XmlSerializer xml = new XmlSerializer(typeof(Projects));
            using (FileStream fs = new FileStream("Projects.xml", FileMode.OpenOrCreate))
            {
                if(fs.Length == 0) return null;
                Projects prs = (Projects)xml.Deserialize(fs);
                return prs;
            }
        }
        public void DeserializeProjectsNextFrame() =>
            ToUpdate += () => DeserializeProjects(); 

        private void CheckProjects()
        {
            Projects.ProjectList.ForEach((x) => x.Exists = Directory.Exists(x.GetFullString()) ?  true : false);
        }
        public void CheckProjectsNextFrame() =>
            ToUpdate += () => CheckProjects();
    }
}