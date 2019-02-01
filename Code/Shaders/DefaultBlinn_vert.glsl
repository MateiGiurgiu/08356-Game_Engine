#version 330
 
in vec3 a_Position;
in vec3 a_Normal;
in vec2 a_TexCoord;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec2 v_TexCoord;
out vec3 v_Normal;

void main()
{
	mat4 PVM = (uProjection * uView * uModel);
	//mat4 MVP = (uModel * uView * uProjection);
    gl_Position =  PVM * vec4(a_Position, 1.0);
	v_Normal = normalize(mat3(transpose(inverse(uModel))) * a_Normal);
	//v_Normal = normalize(a_Normal * mat3(transpose(inverse(uModel))));
	//v_Normal = a_Normal;
    v_TexCoord = a_TexCoord;
}