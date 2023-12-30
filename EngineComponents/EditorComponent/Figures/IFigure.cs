using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MonoProject.EditorComponent
{
    interface IFigure
    {
        public String Name {get; set;}
        public int Length {get; set;}
        public int Width {get; set;}
        public int Height {get; set;}
        public Vector3 Translation {get; set;}
        public Vector3 Rotation {get; set;}
        public Vector3 Scale {get; set;}
        public Color Color{get; set;}
        virtual public Matrix WorldMatrix {get {return WorldMatrix;} protected set {WorldMatrix = value;}}
        virtual public BoundingOrientedBox OBoundingBox {get {return OBoundingBox;} protected set {OBoundingBox = value;}}
        public bool IsSelected {get; set;}
        void LoadFigureContent();
        void DrawFigure(GraphicsDevice gr, BasicEffect effect, Matrix v, Matrix p);
        void DrawFigure(GraphicsDevice gr, Effect effect, Matrix v, Matrix p);
        void ApplyTransform();
        void ApplyResize(Matrix wm);
        void ApplyColor(Color c);
        void UnloadFigureContent();
    }
}