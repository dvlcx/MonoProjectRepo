using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.EngineComponents;
using MonoProject.ImGuiComponent;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MonoProject.EditorComponent
{
    public class PolygonFigure : BasicFigure, IFigure
    {
        public float Length {get; private set;}
        public float Width  {get; private set;}
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
                if(EditorManager.IsPointEditMode)
                {
                    foreach(var i in indices)
                    {
                        DrawVertexCircle(gr, verticesColor[i].Position);
                    }
                }
            }
            else 
            {
                effect.CurrentTechnique.Passes[0].Apply();
                gr.DrawUserIndexedPrimitives(PrimitiveType.PointList, verticesColor, 0, verticesColor.Length,
                 indices, 0, indices.Length/3, VertexPositionColor.VertexDeclaration);
            }
        }
        private void DrawVertexCircle(GraphicsDevice gr, Vector3 pos)
        {
            int radius = 10;
            Texture2D texture = new Texture2D(gr, radius, radius);
            Color[] colorData = new Color[radius*radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 posi = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);

            VertexPositionTexture[] verts = new VertexPositionTexture[4];
            int[] inds = new int[6];
            verts[0] = new VertexPositionTexture(new Vector3(1f/2f,0,1f/2f), new Vector2(1, 1));
            verts[1] = new VertexPositionTexture(new Vector3(1f/2f,0,-(1f/2f)), new Vector2(0, -1));
            verts[2] = new VertexPositionTexture(new Vector3(-(1f/2f),0,1f/2f), new Vector2(-1, 0));
            verts[3] = new VertexPositionTexture(new Vector3(-(1f/2f),0,-(1f/2f)), new Vector2(-1, -1));
            gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts, 0, inds.Length,
                    indices, 0, indices.Length/3, VertexPositionColor.VertexDeclaration);
            
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