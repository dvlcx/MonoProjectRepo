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
    sealed class Scene 
    {
        public string Name {get; private set;}
        
        public Scene(string name, string path)
        {
            this.Name = name;
        }

        
    }
}