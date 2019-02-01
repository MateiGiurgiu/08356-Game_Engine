using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;

namespace OpenGL_Game.Objects
{
    public class RenderTexture : Texture
    {
        public int framebufferID;

        private Vector2 size;

        public override Vector2 GetTextureSize()
        {
            return size;
        }

        public RenderTexture(int width, int height)
        {
            size = new Vector2(width, height);

            framebufferID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferID);

            TextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureID, 0);

            int depth = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, depth);
            

            //DrawBuffersEnum[] drawBuffers = new DrawBuffersEnum[]{ DrawBuffersEnum.ColorAttachment0 };
            //GL.DrawBuffers(1, drawBuffers);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) !=  FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Something wrong happened when setting frambe buffer");
            }

        }
    }
}
