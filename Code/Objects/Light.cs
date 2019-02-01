using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    // simple directional light
    public class Light : Entity
    {
        public Light(string name) : base(name) { }

        public float intensity = 1.0f;
        public Color4 color = Color4.LightYellow; 
    }
}
