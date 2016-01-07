#version 330 core

//vertex color given from vertex shader
in vec3 vCol;

// Ouput data
out vec3 color;

void main()
{

	// Output color = red 
	color = vCol;

}