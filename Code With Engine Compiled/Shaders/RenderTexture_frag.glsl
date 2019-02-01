#version 330
 
in vec2 v_TexCoord;
uniform sampler2D s_texture;

out vec4 Color;

void main()
{
    Color = texture2D(s_texture, vec2(v_TexCoord.x, 1-v_TexCoord.y));
}