using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    public class Camera
    {
        private Matrix4 projection = Matrix4.Identity;
        public ComponentTransform transform = new ComponentTransform();
        public Vector3 deltaPosition = Vector3.Zero;

        public Color4 clearColor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);
        public RenderTexture activeRenderTexture = null;

        #region Constructor

        public Camera(int height, int width, float fov)
        {
            ComputeCameraProjection(height, width, fov);
        }

        public Camera(int height, int width, float fov, float nearPlane, float farPlane)
        {
            ComputeCameraProjection(height, width, fov, nearPlane, farPlane);
        }

        public Camera(Matrix4 projection)
        {
            this.projection = projection;
        }

        #endregion

        public void ComputeCameraProjection(int height, int width, float fov)
        {
            float FOV = MathHelper.DegreesToRadians(fov);
            projection = Matrix4.CreatePerspectiveFieldOfView(FOV, ((float)width / (float)height), 1f, 200f);
        }

        public void ComputeCameraProjection(int height, int width, float fov, float nearPlane, float farPlane)
        {
            float FOV = MathHelper.DegreesToRadians(fov);
            projection = Matrix4.CreatePerspectiveFieldOfView(FOV, ((float)width / (float)height), nearPlane, farPlane);
        }

        #region Matrices

        public Matrix4 GetProjectionMatrix()
        {
            return projection;
        }

        public Matrix4 GetViewMatrix()
        {
            Vector3 position = transform.position + (transform.rotation * deltaPosition);
            Vector3 direction = position + transform.forward;
            Matrix4 m = Matrix4.LookAt(position, direction, Vector3.UnitY);
            return m;
        }
        #endregion
    }
}
