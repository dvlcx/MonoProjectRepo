using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.ImGuiComponent;
using System;
using System.IO;

namespace MonoProject.EditorComponent
{
    public abstract class BasicFigure 
    {
        public String Name {get; set;}
        public Vector3 Translation {get; set;}
        public Vector3 Rotation {get; set;}
        public Vector3 Scale {get; set;}
        public Matrix WorldMatrix {get; protected set;}
        public BoundingBox BoundingBox {get; protected set;}
        public bool IsSelected {get; set;}
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
        public abstract void ApplyTransform();
        public abstract void ApplyColor(Color c);
        public abstract void ApplyTexture();
        public abstract void UnloadFigureContent();
    }
}