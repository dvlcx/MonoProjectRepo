using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace MonoProject
{
    public struct BoundingOrientedBox : IEquatable<BoundingOrientedBox>
    {

        public const int CornerCount = 8;
        const float RAY_EPSILON = 1e-20F;
        public Vector3 Center;
        public Vector3 HalfExtent;
        public Quaternion Orientation;
        public BoundingOrientedBox(Vector3 center, Vector3 halfExtents, Quaternion orientation)
        {
            Center = center;
            HalfExtent = halfExtents;
            Orientation = orientation;
        }

        public static BoundingOrientedBox CreateFromBoundingBox(BoundingBox box)
        {
            Vector3 mid = (box.Min + box.Max) * 0.5f;
            Vector3 halfExtent = (box.Max - box.Min) * 0.5f;
            return new BoundingOrientedBox(mid, halfExtent, Quaternion.Identity);
        }

        public BoundingOrientedBox Transform(Quaternion rotation, Vector3 translation)
        {
            return new BoundingOrientedBox(Vector3.Transform(Center, rotation) + translation,
                                            HalfExtent,
                                            Orientation * rotation);
        }

        public BoundingOrientedBox Transform(Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            return new BoundingOrientedBox(Vector3.Transform(Center * scale, rotation) + translation,
                                            HalfExtent * scale,
                                            Orientation * rotation);
        }

        public bool Equals(BoundingOrientedBox other)
        {
            return (Center == other.Center && HalfExtent == other.HalfExtent && Orientation == other.Orientation);
        }
        
        public override bool Equals(Object obj)
        {
            if (obj != null && obj is BoundingOrientedBox)
            {
                BoundingOrientedBox other = (BoundingOrientedBox)obj;
                return (Center == other.Center && HalfExtent == other.HalfExtent && Orientation == other.Orientation);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Center.GetHashCode() ^ HalfExtent.GetHashCode() ^ Orientation.GetHashCode();
        }

        public static bool operator==(BoundingOrientedBox a, BoundingOrientedBox b)
        {
            return Equals(a, b);
        }

        public static bool operator!=(BoundingOrientedBox a, BoundingOrientedBox b)
        {
            return !Equals(a, b);
        }

        public override string ToString()
        {
            return "{Center:" + Center.ToString() +
                   " Extents:" + HalfExtent.ToString() +
                   " Orientation:" + Orientation.ToString() + "}";
        }

        public bool Intersects(ref BoundingBox box)
        {
            Vector3 boxCenter = (box.Max + box.Min) * 0.5f;
            Vector3 boxHalfExtent = (box.Max - box.Min) * 0.5f;

            Matrix mb = Matrix.CreateFromQuaternion(Orientation);
            mb.Translation = Center - boxCenter;

            return ContainsRelativeBox(ref boxHalfExtent, ref HalfExtent, ref mb) != ContainmentType.Disjoint;
        }

        public ContainmentType Contains(ref BoundingBox box)
        {
            Vector3 boxCenter = (box.Max + box.Min) * 0.5f;
            Vector3 boxHalfExtent = (box.Max - box.Min) * 0.5f;

            Quaternion relOrient;
            Quaternion.Conjugate(ref Orientation, out relOrient);

            Matrix relTransform = Matrix.CreateFromQuaternion(relOrient);
            relTransform.Translation = Vector3.TransformNormal(boxCenter - Center, relTransform);

            return ContainsRelativeBox(ref HalfExtent, ref boxHalfExtent, ref relTransform);
        }

        public static ContainmentType Contains(ref BoundingBox boxA, ref BoundingOrientedBox oboxB)
        {
            Vector3 boxA_halfExtent = (boxA.Max - boxA.Min) * 0.5f;
            Vector3 boxA_center = (boxA.Max + boxA.Min) * 0.5f;
            Matrix mb = Matrix.CreateFromQuaternion(oboxB.Orientation);
            mb.Translation = oboxB.Center - boxA_center;

            return BoundingOrientedBox.ContainsRelativeBox(ref boxA_halfExtent, ref oboxB.HalfExtent, ref mb);
        }

        public bool Intersects(ref BoundingOrientedBox other)
        {
            return Contains(ref other) != ContainmentType.Disjoint;
        }

        public ContainmentType Contains(ref BoundingOrientedBox other)
        {
            Quaternion invOrient;
            Quaternion.Conjugate(ref Orientation, out invOrient);
            Quaternion relOrient;
            Quaternion.Multiply(ref invOrient, ref other.Orientation, out relOrient);

            Matrix relTransform = Matrix.CreateFromQuaternion(relOrient);
            relTransform.Translation = Vector3.Transform(other.Center - Center, invOrient);

            return ContainsRelativeBox(ref HalfExtent, ref other.HalfExtent, ref relTransform);
        }

        public ContainmentType Contains(BoundingFrustum frustum)
        {
            BoundingFrustum temp = ConvertToFrustum();
            return temp.Contains(frustum);
        }

        public bool Intersects(BoundingFrustum frustum)
        {
            return (Contains(frustum) != ContainmentType.Disjoint);
        }

        public static ContainmentType Contains(BoundingFrustum frustum, ref BoundingOrientedBox obox)
        {
            return frustum.Contains(obox.ConvertToFrustum());
        }

        public ContainmentType Contains(ref BoundingSphere sphere)
        {
            // Transform the sphere into local box space
            Quaternion iq = Quaternion.Conjugate(Orientation);
            Vector3 localCenter = Vector3.Transform(sphere.Center - Center, iq);

            float dx = Math.Abs(localCenter.X) - HalfExtent.X;
            float dy = Math.Abs(localCenter.Y) - HalfExtent.Y;
            float dz = Math.Abs(localCenter.Z) - HalfExtent.Z;

            float r = sphere.Radius;
            if (dx <= -r && dy <= -r && dz <= -r)
                return ContainmentType.Contains;


            dx = Math.Max(dx, 0.0f);
            dy = Math.Max(dy, 0.0f);
            dz = Math.Max(dz, 0.0f);

            if(dx*dx + dy*dy + dz*dz >= r*r)
                return ContainmentType.Disjoint;

            return ContainmentType.Intersects;
        }

        public bool Intersects(ref BoundingSphere sphere)
        {
            Quaternion iq = Quaternion.Conjugate(Orientation);
            Vector3 localCenter = Vector3.Transform(sphere.Center - Center, iq);

            float dx = Math.Abs(localCenter.X) - HalfExtent.X;
            float dy = Math.Abs(localCenter.Y) - HalfExtent.Y;
            float dz = Math.Abs(localCenter.Z) - HalfExtent.Z;

            dx = Math.Max(dx, 0.0f);
            dy = Math.Max(dy, 0.0f);
            dz = Math.Max(dz, 0.0f);
            float r = sphere.Radius;

            return dx * dx + dy * dy + dz * dz < r * r;
        }

        public static ContainmentType Contains(ref BoundingSphere sphere, ref BoundingOrientedBox box)
        {
            Quaternion iq = Quaternion.Conjugate(box.Orientation);
            Vector3 localCenter = Vector3.Transform(sphere.Center - box.Center, iq);
            localCenter.X = Math.Abs(localCenter.X);
            localCenter.Y = Math.Abs(localCenter.Y);
            localCenter.Z = Math.Abs(localCenter.Z);

            float rSquared = sphere.Radius * sphere.Radius;
            if ((localCenter + box.HalfExtent).LengthSquared() <= rSquared)
                return ContainmentType.Contains;

            Vector3 d = localCenter - box.HalfExtent;

            d.X = Math.Max(d.X, 0.0f);
            d.Y = Math.Max(d.Y, 0.0f);
            d.Z = Math.Max(d.Z, 0.0f);

            if (d.LengthSquared() >= rSquared)
                return ContainmentType.Disjoint;

            return ContainmentType.Intersects;
        }

        public bool Contains(ref Vector3 point)
        {
            Quaternion qinv = Quaternion.Conjugate(Orientation);
            Vector3 plocal = Vector3.Transform(point - Center, qinv);

            return Math.Abs(plocal.X) <= HalfExtent.X &&
                   Math.Abs(plocal.Y) <= HalfExtent.Y &&
                   Math.Abs(plocal.Z) <= HalfExtent.Z;
        }

        public float? Intersects(ref Ray ray)
        {
            Matrix R = Matrix.CreateFromQuaternion(Orientation);

            Vector3 TOrigin = Center - ray.Position;

            float t_min = -float.MaxValue;
            float t_max = float.MaxValue;

            // X-case
            float axisDotOrigin = Vector3.Dot(R.Right, TOrigin);
            float axisDotDir = Vector3.Dot(R.Right, ray.Direction);

            if (axisDotDir >= -RAY_EPSILON && axisDotDir <= RAY_EPSILON)
            {
                if ((-axisDotOrigin - HalfExtent.X) > 0.0 || (-axisDotOrigin + HalfExtent.X) > 0.0f)
                    return null;
            }
            else
            {
                float t1 = (axisDotOrigin - HalfExtent.X) / axisDotDir;
                float t2 = (axisDotOrigin + HalfExtent.X) / axisDotDir;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                if (t1 > t_min)
                    t_min = t1;

                if (t2 < t_max)
                    t_max = t2;

                if (t_max < 0.0f || t_min > t_max)
                    return null;
            }

            // Y-case
            axisDotOrigin = Vector3.Dot(R.Up, TOrigin);
            axisDotDir = Vector3.Dot(R.Up, ray.Direction);

            if (axisDotDir >= -RAY_EPSILON && axisDotDir <= RAY_EPSILON)
            {
                if ((-axisDotOrigin - HalfExtent.Y) > 0.0 || (-axisDotOrigin + HalfExtent.Y) > 0.0f)
                    return null;
            }
            else
            {
                float t1 = (axisDotOrigin - HalfExtent.Y) / axisDotDir;
                float t2 = (axisDotOrigin + HalfExtent.Y) / axisDotDir;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                if (t1 > t_min)
                    t_min = t1;

                if (t2 < t_max)
                    t_max = t2;

                if (t_max < 0.0f || t_min > t_max)
                    return null;
            }

            axisDotOrigin = Vector3.Dot(R.Forward, TOrigin);
            axisDotDir = Vector3.Dot(R.Forward, ray.Direction);

            if (axisDotDir >= -RAY_EPSILON && axisDotDir <= RAY_EPSILON)
            {
                if ((-axisDotOrigin - HalfExtent.Z) > 0.0 || (-axisDotOrigin + HalfExtent.Z) > 0.0f)
                    return null;
            }
            else
            {
                float t1 = (axisDotOrigin - HalfExtent.Z) / axisDotDir;
                float t2 = (axisDotOrigin + HalfExtent.Z) / axisDotDir;

                if (t1 > t2)
                {
                    float temp = t1;
                    t1 = t2;
                    t2 = temp;
                }

                if (t1 > t_min)
                    t_min = t1;

                if (t2 < t_max)
                    t_max = t2;

                if (t_max < 0.0f || t_min > t_max)
                    return null;
            }

            return t_min;
        }

        public PlaneIntersectionType Intersects(ref Plane plane)
        {
            float dist = plane.DotCoordinate(Center);

            Vector3 localNormal = Vector3.Transform(plane.Normal, Quaternion.Conjugate(Orientation));

            float r = Math.Abs(HalfExtent.X*localNormal.X)
                    + Math.Abs(HalfExtent.Y*localNormal.Y)
                    + Math.Abs(HalfExtent.Z*localNormal.Z);

            if(dist > r)
            {
                return PlaneIntersectionType.Front;
            }
            else if(dist < -r)
            {
                return PlaneIntersectionType.Back;
            }
            else
            {
                return PlaneIntersectionType.Intersecting;
            }
        }

        public Vector3[] GetCorners()
        {
            Vector3[] corners = new Vector3[CornerCount];
            GetCorners(corners, 0);
            return corners;
        }

        public void GetCorners(Vector3[] corners, int startIndex)
        {
            Matrix m = Matrix.CreateFromQuaternion(Orientation);
            Vector3 hX = m.Left * HalfExtent.X;
            Vector3 hY = m.Up * HalfExtent.Y;
            Vector3 hZ = m.Backward * HalfExtent.Z;

            int i = startIndex;
            corners[i++] = Center - hX + hY + hZ;
            corners[i++] = Center + hX + hY + hZ;
            corners[i++] = Center + hX - hY + hZ;
            corners[i++] = Center - hX - hY + hZ;
            corners[i++] = Center - hX + hY - hZ;
            corners[i++] = Center + hX + hY - hZ;
            corners[i++] = Center + hX - hY - hZ;
            corners[i++] = Center - hX - hY - hZ;
        }

        public static ContainmentType ContainsRelativeBox(ref Vector3 hA, ref Vector3 hB, ref Matrix mB)
        {
            Vector3 mB_T = mB.Translation;
            Vector3 mB_TA = new Vector3(Math.Abs(mB_T.X), Math.Abs(mB_T.Y), Math.Abs(mB_T.Z));

            Vector3 bX = mB.Right;      // x-axis of box B
            Vector3 bY = mB.Up;         // y-axis of box B
            Vector3 bZ = mB.Backward;   // z-axis of box B
            Vector3 hx_B = bX * hB.X;   // x extent of box B
            Vector3 hy_B = bY * hB.Y;   // y extent of box B
            Vector3 hz_B = bZ * hB.Z;   // z extent of box B

            float projx_B = Math.Abs(hx_B.X) + Math.Abs(hy_B.X) + Math.Abs(hz_B.X);
            float projy_B = Math.Abs(hx_B.Y) + Math.Abs(hy_B.Y) + Math.Abs(hz_B.Y);
            float projz_B = Math.Abs(hx_B.Z) + Math.Abs(hy_B.Z) + Math.Abs(hz_B.Z);
            if (mB_TA.X + projx_B <= hA.X && mB_TA.Y + projy_B <= hA.Y && mB_TA.Z + projz_B <= hA.Z)
                return ContainmentType.Contains;

            if (mB_TA.X >= hA.X + Math.Abs(hx_B.X) + Math.Abs(hy_B.X) + Math.Abs(hz_B.X))
                return ContainmentType.Disjoint;

            if (mB_TA.Y >= hA.Y + Math.Abs(hx_B.Y) + Math.Abs(hy_B.Y) + Math.Abs(hz_B.Y))
                return ContainmentType.Disjoint;

            if (mB_TA.Z >= hA.Z + Math.Abs(hx_B.Z) + Math.Abs(hy_B.Z) + Math.Abs(hz_B.Z))
                return ContainmentType.Disjoint;

            // Check for separation along the axes box B, hx_B/hy_B/hz_B
            if (Math.Abs(Vector3.Dot(mB_T, bX)) >= Math.Abs(hA.X * bX.X) + Math.Abs(hA.Y * bX.Y) + Math.Abs(hA.Z * bX.Z) + hB.X)
                return ContainmentType.Disjoint;

            if (Math.Abs(Vector3.Dot(mB_T, bY)) >= Math.Abs(hA.X * bY.X) + Math.Abs(hA.Y * bY.Y) + Math.Abs(hA.Z * bY.Z) + hB.Y)
                return ContainmentType.Disjoint;

            if (Math.Abs(Vector3.Dot(mB_T, bZ)) >= Math.Abs(hA.X * bZ.X) + Math.Abs(hA.Y * bZ.Y) + Math.Abs(hA.Z * bZ.Z) + hB.Z)
                return ContainmentType.Disjoint;

            Vector3 axis;

            axis = new Vector3(0, -bX.Z, bX.Y);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.Y * axis.Y) + Math.Abs(hA.Z * axis.Z) + Math.Abs(Vector3.Dot(axis, hy_B)) + Math.Abs(Vector3.Dot(axis, hz_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(0, -bY.Z, bY.Y);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.Y * axis.Y) + Math.Abs(hA.Z * axis.Z) + Math.Abs(Vector3.Dot(axis, hz_B)) + Math.Abs(Vector3.Dot(axis, hx_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(0, -bZ.Z, bZ.Y);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.Y * axis.Y) + Math.Abs(hA.Z * axis.Z) + Math.Abs(Vector3.Dot(axis, hx_B)) + Math.Abs(Vector3.Dot(axis, hy_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(bX.Z, 0, -bX.X);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.Z * axis.Z) + Math.Abs(hA.X * axis.X) + Math.Abs(Vector3.Dot(axis, hy_B)) + Math.Abs(Vector3.Dot(axis, hz_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(bY.Z, 0, -bY.X);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.Z * axis.Z) + Math.Abs(hA.X * axis.X) + Math.Abs(Vector3.Dot(axis, hz_B)) + Math.Abs(Vector3.Dot(axis, hx_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(bZ.Z, 0, -bZ.X);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.Z * axis.Z) + Math.Abs(hA.X * axis.X) + Math.Abs(Vector3.Dot(axis, hx_B)) + Math.Abs(Vector3.Dot(axis, hy_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(-bX.Y, bX.X, 0);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.X * axis.X) + Math.Abs(hA.Y * axis.Y) + Math.Abs(Vector3.Dot(axis, hy_B)) + Math.Abs(Vector3.Dot(axis, hz_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(-bY.Y, bY.X, 0);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.X * axis.X) + Math.Abs(hA.Y * axis.Y) + Math.Abs(Vector3.Dot(axis, hz_B)) + Math.Abs(Vector3.Dot(axis, hx_B)))
                return ContainmentType.Disjoint;

            axis = new Vector3(-bZ.Y, bZ.X, 0);
            if (Math.Abs(Vector3.Dot(mB_T, axis)) >= Math.Abs(hA.X * axis.X) + Math.Abs(hA.Y * axis.Y) + Math.Abs(Vector3.Dot(axis, hx_B)) + Math.Abs(Vector3.Dot(axis, hy_B)))
                return ContainmentType.Disjoint;

            return ContainmentType.Intersects;
        }

        public BoundingFrustum ConvertToFrustum()
        {
            Quaternion invOrientation;
            Quaternion.Conjugate(ref Orientation, out invOrientation);
            float sx = 1.0f / HalfExtent.X;
            float sy = 1.0f / HalfExtent.Y;
            float sz = .5f / HalfExtent.Z;
            Matrix temp;
            Matrix.CreateFromQuaternion(ref invOrientation, out temp);
            temp.M11 *= sx; temp.M21 *= sx; temp.M31 *= sx;
            temp.M12 *= sy; temp.M22 *= sy; temp.M32 *= sy;
            temp.M13 *= sz; temp.M23 *= sz; temp.M33 *= sz;
            temp.Translation = Vector3.UnitZ*0.5f + Vector3.TransformNormal(-Center, temp);

            return new BoundingFrustum(temp);
        }
    }
}