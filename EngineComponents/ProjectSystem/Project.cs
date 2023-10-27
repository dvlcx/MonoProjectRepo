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
    sealed class Project
    {
        public string Name {get; private set;}
        public string Path {get; private set;}
        
        public Project(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        
    }
}