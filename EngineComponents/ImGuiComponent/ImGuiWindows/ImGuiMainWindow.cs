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
        private Layout _layout;
        private const ImGuiWindowFlags CMFlags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.DockNodeHost;
        public ImGuiMainWindow(ImGuiWindowFlags flags, Num.Vector2 pos, Num.Vector2 size, String title, Action lo) : base(flags, pos, size, title)
        {
            _layout = new Layout(lo);
            this.flags = base.flags | CMFlags;
        }
        public override void LayoutRealize()
        {
            ImGui.SetNextWindowPos(pos, ImGuiCond.Always);
            ImGui.SetNextWindowSize(size, ImGuiCond.Always);
            ImGui.Begin(title, flags);
            _layout();
            ImGui.End();
        } 
    }
}