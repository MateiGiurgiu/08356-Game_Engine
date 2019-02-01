#version 330

uniform samplerCube s_texture;
varying vec3 texCoords;

out vec4 Color;

void main(void)
{
	Color = texture(s_texture, texCoords);
}