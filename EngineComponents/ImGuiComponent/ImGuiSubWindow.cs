using System;
using Num = System.Numerics;
using ImGuiNET;

namespace MonoProject.ImGuiComponent
{
    class ImGuiSubWindow : ImGuiWindow
    {
        private bool _status = false;
        public SubWindowType type = SubWindowType.Null;
        private const ImGuiWindowFlags CMFlags = ImGuiWindowFlags.NoResize;
        public ImGuiSubWindow(ImGuiWindowFlags flags, Num.Vector2 pos, Num.Vector2 size, String title, SubWindowType type) : base(flags, pos, size, title)
        {
            this.type = type;
            this.flags = base.flags | CMFlags;
            this._status = true;
        }

        public override void LayoutRealize(Action lo)
        {
            ImGui.SetNextWindowPos(pos, ImGuiCond.Appearing);
            ImGui.SetNextWindowSize(size, ImGuiCond.Appearing);
            ImGui.Begin(title,ref _status, flags);
            lo();
            UpdateStatus();
            ImGui.End();
        } 

        private void UpdateStatus()
        {
            if(_status == false){
            type = SubWindowType.Null;
            }
        }

    }
}