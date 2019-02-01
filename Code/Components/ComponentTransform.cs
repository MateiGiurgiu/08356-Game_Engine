using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenGL_Game.Components
{
    public class ComponentTransform : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_TRANSFORM;

        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;
        public Vector3 scale = Vector3.One;
        private Matrix4 modelMatrix = Matrix4.Identity;

        public Vector3 right
        {
            get
            {
                return Vector3.Transform(Vector3.UnitX, rotation).Normalized();
            }
        }

        public Vector3 forward
        {
            get
            {
                return Vector3.Transform(Vector3.UnitZ, rotation).Normalized();
            }
        }

        public Vector3 up
        {
            get
            {
                return Vector3.Transform(Vector3.UnitY, rotation).Normalized();
            }
        }

        public Matrix4 GetTransformMatrix()
        {
            Matrix4 T = Matrix4.CreateTranslation(position);
            Matrix4 R = Matrix4.CreateFromQuaternion(rotation);
            Matrix4 S = Matrix4.CreateScale(scale);

            Matrix4 TRS = S * R * T;
            //Matrix4 TRS = T * R * S;
            return TRS;
        }

        public void LookAt(Vector3 target)
        {
            Vector3 direction = target - position;
            rotation = LookRotation(direction, Vector3.UnitY);
        }

        public void LookAt(Vector3 target, Vector3 up)
        {
            Vector3 direction = target - position;
            rotation = LookRotation(direction, up);
        }

        public void LookAtDir(Vector3 direction)
        {
            rotation = LookRotation(direction, Vector3.UnitY);
        }

        /// <summary>
        /// Sets the quaternion rotation variable from an angle in DEGREES
        /// </summary>
        /// <param name="eulerX"></param>
        /// <param name="eulerY"></param>
        /// <param name="eulerZ"></param>
        public void SetEulerAnglesRotation(Vector3 euler)
        {
            this.rotation = Quaternion.FromEulerAngles
                (
                    MathHelper.DegreesToRadians(euler.X),
                    MathHelper.DegreesToRadians(euler.Y),
                    MathHelper.DegreesToRadians(euler.Z)
                    );
        }

        /// <summary>
        /// Sets the quaternion rotation variable from an angle in DEGREES
        /// </summary>
        /// <param name="eulerX"></param>
        /// <param name="eulerY"></param>
        /// <param name="eulerZ"></param>
        public void SetEulerAnglesRotation(float eulerX, float eulerY, float eulerZ)
        {
            this.rotation = Quaternion.FromEulerAngles
                (
                    MathHelper.DegreesToRadians(eulerX),
                    MathHelper.DegreesToRadians(eulerY),
                    MathHelper.DegreesToRadians(eulerZ)
                    );
        }

        public void RotateAxis(Vector3 axis, float angleInDegrees)
        {
            Quaternion rotQuat = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angleInDegrees));
            rotation = rotQuat * rotation;
        }

        public void RotateY(float angleInDegrees)
        {
            Vector3 axis = Vector3.UnitY;
            Quaternion rotQuat = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angleInDegrees));
            rotation = rotQuat * rotation;
        }

        public void RotateX(float angleInDegrees)
        {
            Vector3 axis = Vector3.UnitX;
            Quaternion rotQuat = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angleInDegrees));
            rotation = rotQuat * rotation;
        }

        public void RotateZ(float angleInDegrees)
        {
            Vector3 axis = Vector3.UnitZ;
            Quaternion rotQuat = Quaternion.FromAxisAngle(axis, MathHelper.DegreesToRadians(angleInDegrees));
            rotation = rotQuat * rotation;
        }

        public void Translate(Vector3 amount)
        {
            position += amount;
        }

        public static Quaternion LookRotation(Vector3 forward, Vector3 up)
        {
            Vector3 vector = forward.Normalized();
            Vector3 vector2 = Vector3.Cross(up, vector).Normalized();
            Vector3 vector3 = Vector3.Cross(vector, vector2);
            var m00 = vector2.X;
            var m01 = vector2.Y;
            var m02 = vector2.Z;
            var m10 = vector3.X;
            var m11 = vector3.Y;
            var m12 = vector3.Z;
            var m20 = vector.X;
            var m21 = vector.Y;
            var m22 = vector.Z;

            double num8 = (m00 + m11) + m22;
            var quaternion = new Quaternion();
            if (num8 > 0.0)
            {
                var num = (float)Math.Sqrt(num8 + 1.0);
                quaternion.W = num * 0.5f;
                num = 0.5f / num;
                quaternion.X = (m12 - m21) * num;
                quaternion.Y = (m20 - m02) * num;
                quaternion.Z = (m01 - m10) * num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                var num7 = (float)Math.Sqrt(((1.0f + m00) - m11) - m22);
                var num4 = 0.5f / num7;
                quaternion.X = 0.5f * num7;
                quaternion.Y = (m01 + m10) * num4;
                quaternion.Z = (m02 + m20) * num4;
                quaternion.W = (m12 - m21) * num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                var num6 = (float)Math.Sqrt(((1.0 + m11) - m00) - m22);
                var num3 = 0.5f / num6;
                quaternion.X = (m10 + m01) * num3;
                quaternion.Y = 0.5f * num6;
                quaternion.Z = (m21 + m12) * num3;
                quaternion.W = (m20 - m02) * num3;
                return quaternion;
            }
            var num5 = (float)Math.Sqrt(((1.0 + m22) - m00) - m11);
            var num2 = 0.5f / num5;
            quaternion.X = (m20 + m02) * num2;
            quaternion.Y = (m21 + m12) * num2;
            quaternion.Z = 0.5f * num5;
            quaternion.W = (m01 - m10) * num2;
            return quaternion;
        }

    }
}
