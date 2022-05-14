#version 330 core

layout (location = 0) in vec3 inPosition; 
layout (location = 1) in vec4 inColor; 
layout (location = 2) in vec2 inTexCoord; 

uniform mat4 transform;

out vec2 texCoord;
out vec4 texColor;

void main()
{
	gl_Position = transform * vec4(inPosition, 1.0);
	texColor = inColor;
	texCoord = inTexCoord;
}