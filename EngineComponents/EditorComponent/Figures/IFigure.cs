using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MonoProject.EditorComponent
{
    interface IFigure
    {
        public String Name {get; set;}
        public Vector3 Translation {get; set;}
        public Vector3 Rotation {get; set;}
        public Vector3 Scale {get; set;}
        virtual public Matrix WorldMatrix {get {return WorldMatrix;} protected set {WorldMatrix = value;}}
        virtual public BoundingBox BoundingBox {get {return BoundingBox;} protected set {BoundingBox = value;}}
        public bool IsSelected {get; set;}
        void LoadFigureContent();
        void DrawFigure(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p, Matrix w);
        void ApplyTransform();
        void ApplyColor(Color c);
        void UnloadFigureContent();
    }
}