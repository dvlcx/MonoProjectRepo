using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MonoProject.EditorComponent
{
    interface IFigure
    {
        public Matrix WorldMatrix {get; set;}
        void LoadFigureContent(); //load textures/color init
        void DrawFigure(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p, Matrix w);
        void UnloadFigureContent();
    }

}