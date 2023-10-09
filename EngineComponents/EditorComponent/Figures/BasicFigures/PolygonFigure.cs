using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.ImGuiComponent;
using System;
using System.IO;
using System.Text;

namespace MonoProject.EditorComponent
{
    public class PolygonFigure : BasicFigure
    {
        private static int _counter = 0;
        public PolygonFigure(Vector3 pos, int h, int w) : base(pos, h, w)
        {
            Name = "Polygon" + _counter;
            Translation = pos;
            Rotation = Vector3.Zero;
            Scale = new Vector3(1f, 1f, 1f);
            WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            SetUpVertices(h, w);
            SetUpIndeces();
            _counter++;
        }
        public override void LoadFigureContent()
        {
            
        }
        public override void DrawFigure(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p, Matrix w)
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


        protected override void SetUpVertices(int h, int w)
        {
            base.verticesColor = new VertexPositionColor[4];
            base.verticesColor[0] = new VertexPositionColor(new Vector3(h/2,0,w/2), Color.White);
            base.verticesColor[1] = new VertexPositionColor(new Vector3(h/2,0,-(w/2)), Color.White);
            base.verticesColor[2] = new VertexPositionColor(new Vector3(-(h/2),0,w/2), Color.White);
            base.verticesColor[3] = new VertexPositionColor(new Vector3(-(h/2),0,-(w/2)), Color.White);
            base.BoundingBox = new BoundingBox(Vector3.Transform(base.verticesColor[0].Position+new Vector3(0,-0.1f,0), WorldMatrix),
            Vector3.Transform(base.verticesColor[3].Position+new Vector3(0,0.1f,0), WorldMatrix));
        }
        protected override void SetUpIndeces()
        {
            indices = new int[6] {0, 1, 2, 2, 3, 1};
        }
        public override void ApplyTransform()
        {
            WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up) * Matrix.CreateTranslation(Translation) * Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) * Matrix.CreateScale(Scale);
            base.BoundingBox = new BoundingBox(Vector3.Transform(base.verticesColor[0].Position+new Vector3(0,-0.1f,0), WorldMatrix),
            Vector3.Transform(base.verticesColor[3].Position+new Vector3(0,0.1f,0), WorldMatrix));
        }
        
        public override void ApplyTexture()
        {
            
        }
        public override void UnloadFigureContent()
        {
            
        }
        
        

    }
}