using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    /// <summary>
    /// Shaders must use the name convetion: ShaderName_vert.glsl and ShaderName_frag.glsl and they must be placed on the Shader folder
    /// </summary>
    public class Shader
    {
        public readonly int ShaderProgramID;
        public readonly int VertexShaderID;
        public readonly int FragmentShaderID;

        // shader variables
        public readonly int attribute_vpos;
        public readonly int attribute_vtex;
        public readonly int uniform_mat_model;
        public readonly int uniform_mat_view;
        public readonly int uniform_mat_proj;
        public readonly int uniform_stex;
        public readonly int uniform_light_dir;
        public readonly int uniform_view_dir;
        public readonly int uniform_tint_color;
        public readonly int Uniform_view_pos;

        public Shader(string shaderName)
        {
            string vertexShaderFile = string.Format(@"Shaders/{0}_vert.glsl", shaderName);
            string fragmentShaderFile = string.Format(@"Shaders/{0}_frag.glsl", shaderName);

            StreamReader streamReader;
            int result;

            // load vertex shader
            streamReader = new StreamReader(vertexShaderFile);
            VertexShaderID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShaderID, streamReader.ReadToEnd());
            streamReader.Close();
            GL.CompileShader(VertexShaderID);
            GL.GetShader(VertexShaderID, ShaderParameter.CompileStatus, out result);
            if (result == 0) throw new Exception("Failed to compile vertex shader!" + GL.GetShaderInfoLog(VertexShaderID));

            // load fragment shader
            streamReader = new StreamReader(fragmentShaderFile);
            FragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShaderID, streamReader.ReadToEnd());
            streamReader.Close();
            GL.CompileShader(FragmentShaderID);
            GL.GetShader(FragmentShaderID, ShaderParameter.CompileStatus, out result);
            if (result == 0) throw new Exception("Failed to compile vertex shader!" + GL.GetShaderInfoLog(FragmentShaderID));

            // create final shader program
            ShaderProgramID = GL.CreateProgram();
            GL.AttachShader(ShaderProgramID, VertexShaderID);
            GL.AttachShader(ShaderProgramID, FragmentShaderID);
            GL.LinkProgram(ShaderProgramID);

            attribute_vpos      = GL.GetAttribLocation(ShaderProgramID, "a_Position");
            attribute_vtex      = GL.GetAttribLocation(ShaderProgramID, "a_TexCoord");
            uniform_mat_model   = GL.GetUniformLocation(ShaderProgramID, "uModel");
            uniform_mat_view    = GL.GetUniformLocation(ShaderProgramID, "uView");
            uniform_mat_proj    = GL.GetUniformLocation(ShaderProgramID, "uProjection");
            uniform_stex        = GL.GetUniformLocation(ShaderProgramID, "s_texture");
            uniform_light_dir   = GL.GetUniformLocation(ShaderProgramID, "uLightDir");
            uniform_view_dir    = GL.GetUniformLocation(ShaderProgramID, "uViewDir");
            uniform_tint_color  = GL.GetUniformLocation(ShaderProgramID, "uTintCol");
            Uniform_view_pos = GL.GetUniformLocation(ShaderProgramID, "EyePos");
        }

        public void Delete()
        {
            GL.DetachShader(ShaderProgramID, VertexShaderID);
            GL.DetachShader(ShaderProgramID, FragmentShaderID);
            GL.DeleteShader(VertexShaderID);
            GL.DeleteShader(FragmentShaderID);
            GL.DeleteProgram(ShaderProgramID);
        }
    }
}
