using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    public class ComponentUI : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_UI;

        public Color4 tintColor = Color4.White;

        private Shader shader;
        public Shader Shader => shader;

        private Texture texture;
        public Texture Texture => texture;

        private Geometry geometry;
        public Geometry Geometry => geometry;

        public bool enabled = true;

        public ComponentUI(string textureName = "defualt_texture.png", string shaderName = "SimpleUI")
        {
            this.texture = ResourceManager.LoadTexture(textureName);
            this.geometry = ResourceManager.LoadGeometry("UI Quad.obj");
            this.shader = ResourceManager.LoadShader(shaderName);
        }

        public ComponentUI(Texture texture, string shaderName = "SimpleUI")
        {
            this.texture = texture;
            this.geometry = ResourceManager.LoadGeometry("UI Quad.obj");
            this.shader = ResourceManager.LoadShader(shaderName);
        }
    }
}
