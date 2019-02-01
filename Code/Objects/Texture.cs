using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OpenGL_Game.Objects
{
    public abstract class Texture
    {
        public int TextureID { protected set; get; }
        public abstract OpenTK.Vector2 GetTextureSize();
    }
}
