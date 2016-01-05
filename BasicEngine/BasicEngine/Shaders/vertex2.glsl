#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vertexPosition_modelspace;

uniform mat4 mvpMatrix;

void main()
{
    gl_Position = mvpMatrix * vec4(vertexPosition_modelspace, 1);
}