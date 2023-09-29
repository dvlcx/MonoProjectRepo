using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.ImGuiComponent;
using System;
using System.IO;

namespace MonoProject.EditorComponent
{
    public abstract class BasicFigure : IFigure
    {
        public Matrix WorldMatrix {get; set;}
        protected Vector3 position;
        protected int vertexCount;
        protected int[] indices;
        protected VertexPositionColor[] verticesColor;
        protected VertexPositionTexture[] verticesText;
        protected VertexBuffer vertexBuffer;
        
        protected BasicFigure(Vector3 pos, int h, int w)
        {
            position = pos;
            SetUpVertices(h, w);
        }


        public abstract void LoadFigureContent();
        public abstract void DrawFigure(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p, Matrix w);
        protected abstract void SetUpVertices(int h, int w);
        protected abstract void SetUpIndeces();
        public abstract void ApplyTexture();
        public abstract void UnloadFigureContent();
    }
}