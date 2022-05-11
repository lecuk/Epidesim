#version 330 core

layout (location = 0) in vec2 inPosition; 
layout (location = 1) in vec4 inColor;
layout (location = 2) in float inRadius;

out vec2 outPosition;
out vec4 outColor;
out float outRadius;

void main()
{
	gl_Position = vec4(inPosition.x, inPosition.y, 0.0, 1.0);
	outPosition = inPosition;
	outColor = inColor;
	outRadius = inRadius;
}