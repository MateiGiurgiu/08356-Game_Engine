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
    public class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_TRANSFORM | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_MATERIAL);

        public string Name => "SystemRender";

        /// <summary>
        /// The camera used to render
        /// </summary>
        private readonly Camera camera = null;
        private readonly bool renderOnTexture = false;
        private readonly Light light;

        public SystemRender(Camera camera, Light light)
        {
            this.light = light;
            this.camera = camera;
            if (camera.activeRenderTexture != null) renderOnTexture = true;
        }

        public void BeforeAction()
        {
            GL.DepthMask(true);
            if (renderOnTexture)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, camera.activeRenderTexture.framebufferID);
            }
            else
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }

            GL.ClearColor(camera.clearColor);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.AlphaTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void OnAction(Entity entity)
        {
            if (camera == null) return;

            if ((entity.Mask & MASK) == MASK)
            {
                ComponentGeometry geometryComponent = entity.GetComponent<ComponentGeometry>();
                ComponentTransform transform = entity.transform;
                ComponentMaterial materialComponent = entity.GetComponent<ComponentMaterial>();
                ComponentSound sound = entity.GetComponent<ComponentSound>();
                if (sound != null)
                {
                    AL.Source(sound.souce[0], ALSource3f.Position, ref transform.position);
                    //sound.playsoundonce(1);
                }


                Draw(transform.GetTransformMatrix(), geometryComponent.Geometry(), materialComponent);
            }
        }

        public void Draw(Matrix4 model, Geometry geometry, ComponentMaterial material)
        {

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);

            GL.UseProgram(material.Shader.ShaderProgramID);

            // send texture to shader
            GL.Uniform1(material.Shader.uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, material.Texture.TextureID);

            // send tint color
            if (material.Shader.uniform_tint_color != -1)
            {
                GL.Uniform4(material.Shader.uniform_tint_color, material.tintColor);
            }

            // send matrices to the shader
            GL.UniformMatrix4(material.Shader.uniform_mat_model, false, ref model);
            Matrix4 viewMatrix = camera.GetViewMatrix();
            GL.UniformMatrix4(material.Shader.uniform_mat_view, false, ref viewMatrix);
            Matrix4 projectionMatrix = camera.GetProjectionMatrix();
            GL.UniformMatrix4(material.Shader.uniform_mat_proj, false, ref projectionMatrix);

            // light
            GL.Uniform3(material.Shader.uniform_light_dir, light.transform.forward);
            GL.Uniform3(material.Shader.uniform_view_dir, camera.transform.forward);

            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
