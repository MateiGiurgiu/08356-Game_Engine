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
    public class TextureCube : Texture
    {
        public Vector2 size;

        public override Vector2 GetTextureSize()
        {
            return size;
        }

        public TextureCube(string[] filename)
        {
            TextureID = GL.GenTexture();


            GL.BindTexture(TextureTarget.TextureCubeMap, TextureID);

            TextureTarget[] target = new TextureTarget[]
            {
                TextureTarget.TextureCubeMapNegativeZ,
                TextureTarget.TextureCubeMapPositiveZ,
                TextureTarget.TextureCubeMapPositiveY,
                TextureTarget.TextureCubeMapNegativeY,
                TextureTarget.TextureCubeMapNegativeX,
                TextureTarget.TextureCubeMapPositiveX
            };

            for (int i = 0; i < filename.Length; i++)
            {
                if (File.Exists(filename[i]))
                {
                    Bitmap bmp;
                    try
                    {
                        bmp = new Bitmap(string.Format(filename[i]));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error when importing texture: " + e.ToString());
                    }
                    size = new Vector2(bmp.Width, bmp.Height);
                    BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(target[i], 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
                    GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
                    bmp.UnlockBits(bmp_data);
                }
                else
                {
                    throw new FileNotFoundException("Unable to open \"" + filename + "\", does not exist.");
                }
            }


            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        }
    }
}
