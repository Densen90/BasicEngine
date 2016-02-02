#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vertexPosition_modelspace;
layout(location = 1) in vec3 vertexNormal_modelspace;

out vec3 vCol;
out vec3 PosWorldspace;
out vec3 NormalCameraspace;
out vec3 EyeDirCameraspace;
out vec3 LightDirCameraspace;

uniform mat4 mvpMatrix;
uniform mat4 M;
uniform mat4 V;
uniform vec3 LightPosWorldspace;

void main()
{
	vCol = vec3(0.5, 0.3, 0.1);

	// Output position of the vertex, in clip space : MVP * position
	gl_Position = mvpMatrix * vec4(vertexPosition_modelspace, 1);

	// Position of the vertex, in worldspace : M * position
	PosWorldspace = (M * vec4(vertexPosition_modelspace, 1)).xyz;

	// Vector that goes from the vertex to the camera, in camera space.
	// In camera space, the camera is at the origin (0,0,0).
	vec3 vertexPosition_cameraspace = ( M * V * vec4(vertexPosition_modelspace,1)).xyz;
	EyeDirCameraspace = vec3(0,0,0) - vertexPosition_cameraspace;

	// Vector that goes from the vertex to the light, in camera space. M is ommited because it's identity.
	vec3 LightPosition_cameraspace = ( V * vec4(LightPosWorldspace,1)).xyz;
	LightDirCameraspace = LightPosition_cameraspace + EyeDirCameraspace;
	
	// Normal of the the vertex, in camera space
	NormalCameraspace = ( M * V * vec4(vertexNormal_modelspace,0)).xyz; // Only correct if ModelMatrix does not scale the model ! Use its inverse transpose if not.
}