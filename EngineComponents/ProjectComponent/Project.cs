using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace MonoProject
{
    [Serializable]
    public sealed class Projects
    {
        public List<Project> ProjectList {get; set;} = new List<Project>();
    }

    [Serializable]
    public sealed class Project
    {
        public string Name {get; set;}
        public string Path {get; set;}
        [XmlIgnore]
        public bool Exists {get; set;}

        public Project(){}
        public Project(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public string GetFullString()
        {
                if(OperatingSystem.IsWindows()) return this.Path + "\\" + this.Name; 
                else return this.Path + "/" + this.Name;
        }
    }
}