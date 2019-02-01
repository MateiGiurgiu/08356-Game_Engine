#version 330
 
in vec2 v_TexCoord;
uniform sampler2D s_texture;
uniform vec4 uTintCol;

out vec4 Color;

void main()
{
    Color = texture2D(s_texture, vec2(v_TexCoord.x, v_TexCoord.y)) * uTintCol;
}
