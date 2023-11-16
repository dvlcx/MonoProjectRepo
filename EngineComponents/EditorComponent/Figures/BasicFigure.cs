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
        public Color Color {get; set;}

        public Matrix WorldMatrix {get; protected set;}
        public BoundingOrientedBox OBoundingBox {get; protected set;}
        public bool IsSelected {get; set;}
        protected Vector3 position;
        protected int vertexCount;
        protected int[] _indices;
        protected VertexPositionColor[] verticesColor;
        protected VertexPositionTexture[] verticesText;
        protected VertexBuffer vertexBuffer;
        
        protected BasicFigure(Vector3 pos)
        {
            position = pos;
            SetUpVertices();
        }


        public abstract void LoadFigureContent();
        public abstract void DrawFigure(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p);
        public abstract void DrawFigure(GraphicsDevice gr, Effect effect, Matrix v, Matrix p);
        protected abstract void SetUpVertices();
        protected abstract void SetUpIndeces();
        public abstract void ApplyTransform();
        public abstract void ApplyColor(Color c);
        public abstract void ApplyTexture();
        public abstract void UnloadFigureContent();
    }
}