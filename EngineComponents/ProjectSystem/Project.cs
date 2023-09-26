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
        string name {get; set;}
        string path {get; set;}
        public Project(string name, string path)
        {
            this.name = name;
            this.path = path;

        }

        
    }
}