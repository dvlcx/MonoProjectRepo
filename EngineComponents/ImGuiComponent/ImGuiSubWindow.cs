using System;
using Num = System.Numerics;
using ImGuiNET;

namespace MonoProject.ImGuiComponent
{
    class ImGuiSubWindow : ImGuiWindow
    {
        private const ImGuiWindowFlags CMFlags = ImGuiWindowFlags.NoResize;
        public ImGuiSubWindow(ImGuiWindowFlags flags, Num.Vector2 pos, Num.Vector2 size, String title) : base(flags, pos, size, title)
        {
            this.flags = base.flags | CMFlags;
        }
        public override void LayoutRealize(Action lo)
        {
            ImGui.SetNextWindowPos(pos, ImGuiCond.Appearing);
            ImGui.SetNextWindowSize(size, ImGuiCond.Appearing);
            ImGui.Begin(title, flags);
            lo();
            ImGui.End();
        } 
        public void LayoutRealize(Action lo, ref bool smex)
        {
            ImGui.SetNextWindowPos(pos, ImGuiCond.Appearing);
            ImGui.SetNextWindowSize(size, ImGuiCond.Appearing);
            ImGui.Begin(title, ref smex, flags);
            lo();
            ImGui.End();
        } 

    }
}