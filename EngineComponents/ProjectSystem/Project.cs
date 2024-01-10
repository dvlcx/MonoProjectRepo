using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;
using System.Collections.Generic;


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

        public Project(){}
        public Project(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }
    }
}