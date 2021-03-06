#version 330 core

//vertex color given from vertex shader
in vec3 vCol;
in vec3 PosWorldspace;
in vec3 NormalCameraspace;
in vec3 EyeDirCameraspace;
in vec3 LightDirCameraspace;
in vec2 uv;

// Ouput data
out vec3 color;

uniform mat4 MV;
uniform vec3 LightPosWorldspace;
uniform sampler2D MainTexture;

vec3 calculateLighting(vec3 color)
{
	vec3 MaterialDiffuseColor = color;
	vec3 MaterialAmbientColor = vec3(0.1,0.1,0.1) * MaterialDiffuseColor;
	vec3 MaterialSpecularColor = vec3(0.3,0.3,0.3);

	// Light emission properties
	// You probably want to put them as uniforms
	vec3 LightColor = vec3(1,1,1);
	float LightPower = 50.0f;

	// Distance to the light
	float distance = length( LightPosWorldspace - PosWorldspace );

	// Normal of the computed fragment, in camera space
	vec3 n = normalize( NormalCameraspace );
	// Direction of the light (from the fragment to the light)
	vec3 l = normalize( LightDirCameraspace );
	// Cosine of the angle between the normal and the light direction, 
	// clamped above 0
	//  - light is at the vertical of the triangle -> 1
	//  - light is perpendicular to the triangle -> 0
	//  - light is behind the triangle -> 0
	float cosTheta = clamp( dot( n,l ), 0,1 );
	
	// Eye vector (towards the camera)
	vec3 E = normalize(EyeDirCameraspace);
	// Direction in which the triangle reflects the light
	vec3 R = reflect(-l,n);
	// Cosine of the angle between the Eye vector and the Reflect vector,
	// clamped to 0
	//  - Looking into the reflection -> 1
	//  - Looking elsewhere -> < 1
	float cosAlpha = clamp( dot( E,R ), 0,1 );

	return	// Ambient : simulates indirect lighting
		MaterialAmbientColor +
		// Diffuse : "color" of the object
		MaterialDiffuseColor * LightColor * LightPower * cosTheta / (distance*distance) +
		// Specular : reflective highlight, like a mirror
		MaterialSpecularColor * LightColor * LightPower * pow(cosAlpha,5) / (distance*distance);
}

void main()
{
	vec3 texel = texture(MainTexture, uv).rgb;

	//TODO: How to check if Texture is existant --> texel = (0,0,0) could also be the middle of the texture
	color = (texel.x==0 && texel.y==0 && texel.z==0) ? calculateLighting(vCol) : calculateLighting(texel);
		
}