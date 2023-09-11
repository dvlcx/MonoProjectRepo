using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;

namespace MonoProject.ImGuiComponent
{
    class ImGuiMainWindow : ImGuiWindow
    {
        private const ImGuiWindowFlags CMFlags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
        public ImGuiMainWindow(ImGuiWindowFlags flags, Num.Vector2 pos, Num.Vector2 size, String title) : base(flags, pos, size, title)
        {
            this.flags = base.flags | CMFlags;
        }
        public override void LayoutRealize(Action lo)
        {
            ImGui.SetNextWindowPos(pos, ImGuiCond.Always);
            ImGui.SetNextWindowSize(size, ImGuiCond.Always);
            ImGui.Begin(title, flags);
            lo();
            ImGui.End();
        } 
    }
}