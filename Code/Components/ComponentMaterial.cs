using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    public class ComponentMaterial : IComponent
    {
        private Texture texture;
        private Shader shader;
        public Color4 tintColor = Color4.White;

        public ComponentMaterial(string textureName = "defualt_texture.png", string shaderName = "DefaultBlinn")
        {
            this.texture = ResourceManager.LoadTexture(textureName);
            this.shader = ResourceManager.LoadShader(shaderName);
        }

        public ComponentMaterial(Texture texture, string shaderName = "DefaultBlinn")
        {
            this.texture = texture;
            this.shader = ResourceManager.LoadShader(shaderName);
        }

        public Texture Texture => texture;
        public Shader Shader => shader;

        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_MATERIAL;
    }
}
