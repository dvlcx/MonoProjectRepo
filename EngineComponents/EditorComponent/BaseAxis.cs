using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoProject.EditorComponent
{
    class BaseAxis
    {
        private static readonly float _axisLength = 100000f;
        private readonly Vector3 _xAxisStart = new Vector3(-_axisLength, 0, 0);
        private readonly Vector3 _xAxisEnd = new Vector3(_axisLength, 0, 0);
        private readonly Vector3 _yAxisStart = new Vector3(0, -_axisLength, 0);
        private readonly Vector3 _yAxisEnd = new Vector3(0, _axisLength, 0);
        private readonly Vector3 _zAxisStart = new Vector3(0, 0, -_axisLength);
        private readonly Vector3 _zAxisEnd = new Vector3(0, 0, _axisLength);
        private readonly Color _xColor = Color.Red;
        private readonly Color _yColor = Color.Green;
        private readonly Color _zColor = Color.Blue;


        private VertexPositionColor[] _axisVertices;
        private VertexBuffer _vertexBuffer;

        public void Initialize(GraphicsDevice gr)
        {
            _axisVertices = new VertexPositionColor[]
            {
                new VertexPositionColor(_xAxisStart, _xColor),
                new VertexPositionColor(_xAxisEnd, _xColor),
                new VertexPositionColor(_yAxisStart, _yColor),
                new VertexPositionColor(_yAxisEnd, _yColor),
                new VertexPositionColor(_zAxisStart, _zColor),
                new VertexPositionColor(_zAxisEnd, _zColor)
            };

            _vertexBuffer = new VertexBuffer(gr, typeof(VertexPositionColor), 6, BufferUsage.WriteOnly);
            _vertexBuffer.SetData<VertexPositionColor>(_axisVertices);
        }

        public void Draw(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p, Matrix w)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            gr.RasterizerState = rs;
            effect.VertexColorEnabled = true;
            effect.View = v;
            effect.Projection = p;
            effect.World = w;
            gr.SetVertexBuffer(_vertexBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gr.DrawPrimitives(PrimitiveType.LineList, 0, 3);
            }
        }
    }
}
