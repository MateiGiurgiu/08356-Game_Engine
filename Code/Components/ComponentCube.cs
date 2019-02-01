using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    public class ComponentCube : IComponent
    {
        public ComponentTypes ComponentType => ComponentTypes.COMPONENT_CUBE;

        private Shader shader;
        public Shader Shader => shader;

        private Texture texture;
        public Texture Texture => texture;

        private Geometry geometry;
        public Geometry Geometry => geometry;

        public ComponentCube(string[] texture, string shaderName = "Skybox")
        {
            this.texture = new TextureCube(texture);

            this.geometry = ResourceManager.LoadGeometry("Skybox.obj");
            this.shader = ResourceManager.LoadShader(shaderName);
        }
    }
}
