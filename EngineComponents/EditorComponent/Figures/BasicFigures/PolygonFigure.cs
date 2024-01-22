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
        public int Length {get; set;}
        public int Width  {get; set;}
        public int Height {get; set;} = 1;
        public Texture2D Texture {get; private set;}
        private static int _counter = 0;
        public PolygonFigure()
        {
            base.Name = "Polygon" + _counter;
            base.Translation = Vector3.Zero;
            base.Rotation = Vector3.Zero;
            base.Scale = new Vector3(1f, 1f, 1f);
            base.Color = Color.White;
            Length = 2;
            Width = 1;
            WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            SetUpVertices();
            SetUpBoundingBox();
            SetUpIndeces();
            _counter++;
        }

        public PolygonFigure(Matrix wm, int l, int w)
        {
            base.Name = "Polygon" + _counter;
            base.Translation = Vector3.Zero;
            base.Rotation = Vector3.Zero;
            base.Scale = new Vector3(1f, 1f, 1f);
            base.Color = Color.White;
            WorldMatrix = wm;
            Length = l;
            Width = w;
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
             _indices, 0, _indices.Length/3, VertexPositionColor.VertexDeclaration);
        }

        public override void DrawFigure(GraphicsDevice gr, Effect effect, Matrix v, Matrix p)
        {
            effect.Parameters["View"].SetValue(v);
            effect.Parameters["Projection"].SetValue(p);
            effect.Parameters["World"].SetValue(WorldMatrix);
            
            //pray for geometry shader support in future
            if (IsSelected) 
            {
                effect.CurrentTechnique.Passes[0].Apply();
                gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verticesColor, 0, verticesColor.Length,
                 _indices, 0, _indices.Length/3, VertexPositionColor.VertexDeclaration);
                
                effect.CurrentTechnique.Passes[1].Apply();
                gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verticesColor, 0, verticesColor.Length,
                 _indices, 0, _indices.Length/3, VertexPositionColor.VertexDeclaration);

                if(EditorManager.IsPointEditMode)
                {
                   effect.CurrentTechnique.Passes[2].Apply();
                gr.DrawUserIndexedPrimitives(PrimitiveType.PointList, verticesColor, 0, verticesColor.Length,
                 _indices, 0, _indices.Length, VertexPositionColor.VertexDeclaration);
                }
            }
            else 
            {
                effect.CurrentTechnique.Passes[0].Apply();
                gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verticesColor, 0, verticesColor.Length,
                 _indices, 0, _indices.Length/3, VertexPositionColor.VertexDeclaration);
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
            gr.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, verts, 0, verts.Length,
                    _indices, 0, _indices.Length/3, VertexPositionColor.VertexDeclaration);
            
        }

        private Vector3 _halfExtentStart;
        protected override void SetUpVertices()
        {
            base.verticesColor = new VertexPositionColor[(Length + 1) * (Width + 1)];
            float shiftX = 0.5f * Width;
            float shiftZ = 0.5f * Length;

            int counter = 0;
            for (int z = 0; z < Length + 1; z++)
            {
                for (int x = 0; x < Width + 1; x++)
                {
                verticesColor[counter].Position = new Vector3(x, 0, -z);                    
                verticesColor[counter].Color = Color;
                verticesColor[counter].Position.X -= shiftX;
                verticesColor[counter].Position.Z += shiftZ;

                    counter++;
                }
            }
        }

        private void SetUpBoundingBox()
        {
            BoundingBox bb = new BoundingBox(Vector3.Transform(base.verticesColor[0].Position+new Vector3(0, -0.1f, 0), WorldMatrix),
            Vector3.Transform(base.verticesColor[verticesColor.Length-1].Position+new Vector3(0, 0.1f, 0), WorldMatrix));
            OBoundingBox = BoundingOrientedBox.CreateFromBoundingBox(bb);
            _halfExtentStart = OBoundingBox.HalfExtent;
        }
        protected override void SetUpIndeces()
        {            
            _indices = new int[Width * Length * 6];

            int counter = 0;
            for (int y = 0; y < Length; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int lowerLeft = x + y * (Width +1);
                    int lowerRight = (x + 1) + y * (Width + 1);
                    int topLeft = x + (y + 1) * (Width + 1);
                    int topRight = (x + 1) + (y + 1) * (Width + 1);

                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = lowerLeft;

                    _indices[counter++] = topLeft;
                    _indices[counter++] = topRight;
                    _indices[counter++] = lowerRight;
                }
            }

        }
        
        public override void ApplyTransform()
        {
            WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up) * Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) * Matrix.CreateTranslation(Translation);
            base.OBoundingBox = new BoundingOrientedBox(Translation, _halfExtentStart*Scale, Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z));
        }

        public void ApplyResize(Matrix wm)
        {
            WorldMatrix = wm;
            SetUpVertices();
            SetUpBoundingBox();
            SetUpIndeces();
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