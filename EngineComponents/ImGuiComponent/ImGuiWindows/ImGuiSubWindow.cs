using System;
using Num = System.Numerics;
using ImGuiNET;
using MonoProject.EngineComponents;

namespace MonoProject.ImGuiComponent
{
    class ImGuiSubWindow : ImGuiWindow
    {
        private bool _status;
        public bool Status { get {return _status;} private set{_status = value;}}
        public SubWindowType Type = SubWindowType.Null;
        private const ImGuiWindowFlags CMFlags = ImGuiWindowFlags.NoResize;
        private Layout _layout = null;

        public ImGuiSubWindow(ImGuiWindowFlags flags, Num.Vector2 pos, Num.Vector2 size, String title, SubWindowType type, Action lo) : base(flags, pos, size, title)
        {
            _layout = new Layout(lo);
            this.Type = type;
            this.flags = base.flags | CMFlags;
            Status = true;
        }
        
        public override void LayoutRealize()
        {
            ImGui.SetNextWindowPos(pos, ImGuiCond.Appearing);
            ImGui.SetNextWindowSize(size, ImGuiCond.Appearing);
            ImGui.Begin(title, ref _status, flags);
            _layout();
            ImGui.End();
            UpdateStatus();
        } 


        private void UpdateStatus()
        {
            if(_status == false) Type = SubWindowType.Null;
        }

    }
}