using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;

namespace MonoProject.ImGuiComponent
{
    class ImGuiMainMenuBar : IImGui
    {
        public void LayoutRealize(Action lo) => lo();
    }
}