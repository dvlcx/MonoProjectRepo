using System;
using Num = System.Numerics;
using ImGuiNET;

namespace MonoProject.ImGuiComponent
{
    abstract class ImGuiWindow
    {
        protected delegate void Layout();
        protected ImGuiWindowFlags flags;
        protected Num.Vector2 pos;
        protected Num.Vector2 size;
        protected String title;
        public ImGuiWindow(ImGuiWindowFlags flags, Num.Vector2 pos, Num.Vector2 size, String title)
        {
            this.flags = flags;
            this.pos = pos;
            this.size = size;
            this.title = title;
        }

        public abstract void LayoutRealize(); 
    }
}