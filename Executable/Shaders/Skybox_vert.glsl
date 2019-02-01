//#version 330

in vec3 a_Position;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

in vec2 a_TexCoord;
uniform vec4 EyePos;
varying vec3 texCoords;

void main(void)
{
	texCoords = normalize(a_Position);
	mat4 modifiedView = uView;
	modifiedView[0][3] = 0.0;
	modifiedView[1][3] = 0.0;
	modifiedView[2][3] = 0.0;
	mat4 PVM = (uProjection * modifiedView * uModel);
	vec3 pos = a_Position;
	pos.y -= 0.1;
    gl_Position = PVM * vec4(pos,1.0);
}
