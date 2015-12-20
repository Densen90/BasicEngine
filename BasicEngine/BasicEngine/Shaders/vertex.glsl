#version 330
 
in vec3 vPosition;
in vec3 nPosition;
out vec4 color;
uniform mat4 modelview;
 
void main()
{
    gl_Position = modelview * vec4(vPosition, 1.0);
 
	vec3 col = vec3(1.0, 0.2, 0.5);
	//vec3 light = normalize(vec3(0.1, 0.1, -1.0));
	//col *= dot(light, nPosition);
    color = vec4( col, 1.0);
}