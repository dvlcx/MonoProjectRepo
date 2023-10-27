
using System;


namespace MonoProject.ImGuiComponent
{
    class ImGuiMainMenuBar
    {
        public Action LayoutRealize {get; private set;}
        public ImGuiMainMenuBar(Action lo)
        {
            LayoutRealize = lo;
        }
    }
}