using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Objects;


namespace OpenGL_Game.Managers
{
    static class ResourceManager
    {
        static readonly Dictionary<string, Geometry> geometryDictionary = new Dictionary<string, Geometry>();
        static readonly Dictionary<string, Texture> textureDictionary   = new Dictionary<string, Texture>();
        static readonly Dictionary<string, Audio> soundDictionary       = new Dictionary<string, Audio>();
        static readonly Dictionary<string, Shader> shaderDictionary     = new Dictionary<string, Shader>();

        public static Shader LoadShader(string shaderName)
        {
            Shader shader;
            shaderDictionary.TryGetValue(shaderName, out shader);
            if(shader == null)
            {
                shader = new Shader(shaderName);
            }
            return shader;
        }

        public static Geometry LoadGeometry(string filename)
        {
            Geometry geometry;
            geometryDictionary.TryGetValue(filename, out geometry);
            if (geometry == null)
            {
                geometry = new Geometry(string.Format(@"Geometry/{0}", filename));
                geometryDictionary.Add(filename, geometry);
            }

            return geometry;
        }


        public static Texture LoadTexture(string filename)
        {
            Texture texture;
            textureDictionary.TryGetValue(filename, out texture);
            if(texture == null)
            {
                texture = new Texture2D(string.Format(@"Textures/{0}", filename));
                textureDictionary.Add(filename, texture);
            }

            return texture;
        }

        public static Audio Loadsound(string filename)
        {
            Audio file;

            //soundDictionary.TryGetValue(filename, out file);
            //if (file != null)
            //{
                file = new Audio();
                file.LoadSound(filename);


            //    soundDictionary.Add(filename, file);
            //}


            return file;
        }
    }
}
