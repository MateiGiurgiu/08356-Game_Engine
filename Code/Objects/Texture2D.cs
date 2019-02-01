using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace OpenGL_Game.Objects
{
    public class Texture2D : Texture
    {
        public Vector2 size;

        public override Vector2 GetTextureSize()
        {
            return size;
        }

        public Texture2D(string filename)
        {
            if (File.Exists(filename))
            {
                TextureID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, TextureID);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                Bitmap bmp;
                try
                {
                    bmp = new Bitmap(string.Format(filename));
                }
                catch (Exception e)
                {
                    throw new Exception("Error when importing texture: " + e.ToString());
                }
                size = new Vector2(bmp.Width, bmp.Height);
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                bmp.UnlockBits(bmp_data);
            }
            else
            {
                throw new FileNotFoundException("Unable to open \"" + filename + "\", does not exist.");
            }
        }
    }
}
