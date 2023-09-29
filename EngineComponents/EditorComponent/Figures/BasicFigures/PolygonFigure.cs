using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoProject.ImGuiComponent;
using System;
using System.IO;

namespace MonoProject.EditorComponent
{
    public class PolygonFigure : BasicFigure
    {
        public PolygonFigure(Vector3 pos, int h, int w) : base(pos, h, w)
        {
            WorldMatrix = Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
            SetUpVertices(h, w);
            SetUpIndeces();
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
        }
        protected override void SetUpIndeces()
        {
            indices = new int[6] {0, 1, 2, 0, 3, 1};
        }
        public override void ApplyTexture()
        {
            
        }
        public override void UnloadFigureContent()
        {}
        
        

    }
}