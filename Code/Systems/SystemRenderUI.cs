using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemRenderUI : ISystem
    {
        public string Name => "SystemRenderUI";

        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_UI);

        private Matrix4 projection;
        private Matrix4 view = Matrix4.Identity;

        public SystemRenderUI(float width, float height)
        {
            projection = Matrix4.CreateOrthographic(width, height, 0.01f, 10);
            projection = Matrix4.CreateOrthographicOffCenter(0, width, 0, height, 0.01f, 10);
            view = Matrix4.LookAt(new Vector3(0, 0, 2), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        public void BeforeAction()
        {
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                ComponentUI ui = entity.GetComponent<ComponentUI>();
                if (!ui.enabled) return;
                ComponentTransform transform = entity.transform;

                Draw(transform.GetTransformMatrix(), ui.Geometry, ui);
            }
        }

        public void Draw(Matrix4 model, Geometry geometry, ComponentUI material)
        {
            GL.UseProgram(material.Shader.ShaderProgramID);
            // set texture
            GL.Uniform1(material.Shader.uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, material.Texture.TextureID);

            // send tint color
            if (material.Shader.uniform_tint_color != -1)
            {
                GL.Uniform4(material.Shader.uniform_tint_color, material.tintColor);
            }

            // MVP matrices
            GL.UniformMatrix4(material.Shader.uniform_mat_model, false, ref model);
            GL.UniformMatrix4(material.Shader.uniform_mat_view, false, ref view);
            GL.UniformMatrix4(material.Shader.uniform_mat_proj, false, ref projection);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
