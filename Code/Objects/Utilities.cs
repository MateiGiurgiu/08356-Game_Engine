using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game
{
    public static class Utilities
    {
        public static float DistanceFromPointToSegment(Vector3 P, Vector3 L1, Vector3 L2)
        {
            if ((P - L2).LengthSquared < (P - L1).LengthSquared)
            {
                Vector3 temp = L1;
                L1 = L2;
                L2 = temp;
            }

            Vector3 a = L2 - L1;
            Vector3 b = P - L1;

            bool outside = Vector3.Dot(a, b) < 0;

            if (outside) return b.Length;
            else
            {
                // project
                float AB = Vector3.Dot(a, b) / a.Length;
                Vector3 v = AB * a.Normalized();

                return ((L1 + v) - P).Length;
            }
        }

        public static float DistanceFromPointToSegment(Vector3 P, Vector3 L1, Vector3 L2, out Vector3 normal)
        {
            if ((P - L2).LengthSquared < (P - L1).LengthSquared)
            {
                Vector3 temp = L1;
                L1 = L2;
                L2 = temp;
            }

            Vector3 a = L2 - L1;
            Vector3 b = P - L1;

            bool outside = Vector3.Dot(a, b) < 0;

            if (outside)
            {
                normal = b.Normalized();
                return b.Length;
            }
            else
            {
                // project
                float AB = Vector3.Dot(a, b) / a.Length;
                Vector3 v = AB * a.Normalized();
                Vector3 o = (L1 + v) - P;

                normal = o.Normalized();
                return o.Length;
            }
        }

        public static bool IsProjectionOnSegment(Vector3 P, Vector3 L1, Vector3 L2)
        {
            Vector3 a = L2 - L1;
            Vector3 b = P - L1;

            return Vector3.Dot(a, b) >= 0f;
        }

        public static Vector3 ProjectPoint(Vector3 P, Vector3 L1, Vector3 L2)
        {
            Vector3 a = L2 - L1;
            Vector3 b = P - L1;
            float AB = Vector3.Dot(a, b) / a.Length;
            Vector3 v = AB * a.Normalized();
            return (L1 + v) - P;
        }


        public static bool ProjectPoint(Vector3 P, Vector3 L1, Vector3 L2, out Vector3 projectedPoint)
        {
            projectedPoint = Vector3.Zero;
            if (IsProjectionOnSegment(P, L1, L2))
            {
                Vector3 a = L2 - L1;
                Vector3 b = P - L1;
                float AB = Vector3.Dot(a, b) / a.Length;
                Vector3 v = AB * a.Normalized();
                projectedPoint = P - (L1 + v);
                return true;
            }
            return false;
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            return (a - b).Length;
        }

        public static float DistanceSquared(Vector3 a, Vector3 b)
        {
            return (a - b).LengthSquared;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
