#version 330
 
in vec2 v_TexCoord;
in vec3 v_Normal;
uniform sampler2D s_texture;

uniform vec3 uLightDir;
uniform vec3 uViewDir;
uniform vec4 uTintCol;
out vec4 Color;

float ComputeLight()
{
	float light = 0;
	float diffuse, specular;
	vec3 reflectedVector;

	// ==== DIRECTIONAL LIGHT ==== 
	diffuse				= max(dot(v_Normal, -uLightDir), 0); 
	reflectedVector		= reflect(-uLightDir, v_Normal);
	specular			= pow(max(dot(reflectedVector, uViewDir), 0), 32); 
	light				= diffuse * 0.7 + 0.3;

	return light;
}

void main()
{
    vec4 col = texture2D(s_texture, v_TexCoord);
	Color = vec4(ComputeLight() * col.rgb, 1) * uTintCol;
}