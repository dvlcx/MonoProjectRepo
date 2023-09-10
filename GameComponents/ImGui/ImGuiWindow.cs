using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;

namespace MonoProject
{
    abstract class ImGuiWindow
    {
        ImGuiWindowFlags flags;
        Num.Vector2 pos;
        Num.Vector2 size;
        String title;

        public ImGuiWindow(ImGuiWindowFlags flags, Num.Vector2 pos, Num.Vector2 size, String title)
        {
            this.flags = flags;
            this.pos = pos;
            this.size = size;
            this.title = title;
        }
        
    }
}