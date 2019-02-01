using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Systems
{
    public class SystemRenderSkybox : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_CUBE);

        public string Name => "SystemRenderSkybox";

        /// <summary>
        /// The camera used to render
        /// </summary>
        private readonly Camera camera = null;
        private readonly Light light;

        public SystemRenderSkybox(Camera camera, Light light)
        {
            this.camera = camera;
            this.light = light;
        }

        public void BeforeAction()
        {

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);


            GL.ClearColor(camera.clearColor);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.DepthMask(false);
            // GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void OnAction(Entity entity)
        {

            if ((entity.Mask & MASK) == MASK)
            {
                ComponentCube skyComponent = entity.GetComponent<ComponentCube>();
                ComponentTransform transform = entity.transform;

                Draw(transform.GetTransformMatrix(), skyComponent.Geometry, skyComponent.Texture, skyComponent.Shader);
            }
        }

        public void Draw(Matrix4 model, Geometry geometry, Texture texture, Shader shader)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            // GL.Enable(EnableCap.TextureCubeMap);
            GL.UseProgram(shader.ShaderProgramID);

            GL.Uniform1(shader.uniform_stex, 0);
            // GL.ActiveTexture(TextureUnit.Texture0);
            //GL.BindTexture(TextureTarget.TextureCubeMap, texture.TextureID);

            // send matrices to the shader
            GL.UniformMatrix4(shader.uniform_mat_model, false, ref model);
            Matrix4 viewMatrix = camera.GetViewMatrix();
            GL.UniformMatrix4(shader.uniform_mat_view, false, ref viewMatrix);
            Matrix4 projectionMatrix = camera.GetProjectionMatrix();
            GL.UniformMatrix4(shader.uniform_mat_proj, false, ref projectionMatrix);
            Vector3 viewposition = camera.GetViewMatrix().ExtractTranslation();
            GL.Uniform3(shader.Uniform_view_pos, viewposition);
            // light
            GL.Uniform3(shader.uniform_light_dir, light.transform.forward);
            GL.Uniform3(shader.uniform_view_dir, camera.transform.forward);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
