#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vertexPosition_modelspace;
layout(location = 1) in vec3 vertexNormal;

out vec3 vCol;

uniform mat4 mvpMatrix;

void main()
{
	vec4 pos = mvpMatrix * vec4(vertexPosition_modelspace, 1);

	vec3 lightDir = normalize(vec3(-1,1,1));
	vec3 lightCol = vec3(0.9, 0.2, 0.2);
	vec3 materialCol = vec3(0.3, 0.8, 0.5);

	float ambient = clamp(dot(lightDir, vertexNormal), 0.2, 1);

	vCol = materialCol*lightCol*ambient;

    gl_Position = pos;
}