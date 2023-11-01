using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.ImGuiComponent;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MonoProject.EditorComponent
{
    public class PolygonFigure : BasicFigure, IFigure
    {
        public Texture2D Texture {get; private set;}
        private static int _counter = 0;
        public PolygonFigure(Vector3 pos) : base(pos)
        {
            base.Name = "Polygon" + _counter;
            base.Translation = pos;
            base.Rotation = Vector3.Zero;
            base.Scale = new Vector3(1f, 1f, 1f);
            base.Color = Color.White;
            WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            SetUpVertices();
            SetUpBoundingBox();
            SetUpIndeces();
            _counter++;
        }

        public override void LoadFigureContent()
        {
            
        }

        public override void DrawFigure(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p)
        {
            effect.View = v;
            effect.Projection = p;
            effect.World = WorldMatrix;
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }
            gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verticesColor, 0, verticesColor.Length,
             indices, 0, indices.Length/3, VertexPositionColor.VertexDeclaration);
        }

        public override void DrawFigure(GraphicsDevice gr, Effect effect, Matrix v, Matrix p)
        {
            effect.Parameters["View"].SetValue(v);
            effect.Parameters["Projection"].SetValue(p);
            effect.Parameters["World"].SetValue(WorldMatrix);
            
            //pray for geometry shader support in future
            if (IsSelected) 
            {
                foreach (var pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verticesColor, 0, verticesColor.Length,
                    indices, 0, indices.Length/3, VertexPositionColor.VertexDeclaration);
                }
            }
            else 
            {
                effect.CurrentTechnique.Passes[0].Apply();
                gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verticesColor, 0, verticesColor.Length,
                 indices, 0, indices.Length/3, VertexPositionColor.VertexDeclaration);
            }
        }

        private Vector3 _halfExtentStart;
        protected override void SetUpVertices()
        {
            base.verticesColor = new VertexPositionColor[4];
            base.verticesColor[0] = new VertexPositionColor(new Vector3(1f/2f,0,1f/2f), base.Color);
            base.verticesColor[1] = new VertexPositionColor(new Vector3(1f/2f,0,-(1f/2f)), base.Color);
            base.verticesColor[2] = new VertexPositionColor(new Vector3(-(1f/2f),0,1f/2f), base.Color);
            base.verticesColor[3] = new VertexPositionColor(new Vector3(-(1f/2f),0,-(1f/2f)), base.Color);
        }
        private void SetUpBoundingBox()
        {
            BoundingBox bb = new BoundingBox(Vector3.Transform(base.verticesColor[0].Position+new Vector3(0, -0.1f, 0), WorldMatrix),
            Vector3.Transform(base.verticesColor[3].Position+new Vector3(0, 0.1f, 0), WorldMatrix));
            OBoundingBox = BoundingOrientedBox.CreateFromBoundingBox(bb);
            _halfExtentStart = OBoundingBox.HalfExtent;
        }
        protected override void SetUpIndeces()
        {
            indices = new int[6] {0, 1, 2, 2, 3, 1};
        }
        
        public override void ApplyTransform()
        {
            WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up) * Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) * Matrix.CreateTranslation(Translation) * Matrix.CreateScale(Scale);
            base.OBoundingBox = new BoundingOrientedBox(Translation, _halfExtentStart*Scale, Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z));
        }
        public override void ApplyColor(Color c)
        {
            Color = c;
            for (int i = 0; i < verticesColor.GetLength(0); i++)
            {
                verticesColor[i].Color = c;
            }
        }

        public override void ApplyTexture()
        {
            
        }
        public override void UnloadFigureContent()
        {
            
        }
        
        

    }
}