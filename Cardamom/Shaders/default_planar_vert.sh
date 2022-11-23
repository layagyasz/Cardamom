#version 330 core

layout(location = 0) in vec2 in_position;  
layout(location = 1) in vec4 in_color;

uniform mat3 projection;
uniform mat3 view;

out vec4 vert_color;

void main(void)
{
	gl_Position = vec4(vec3(in_position, 1.0) * view * projection, 1.0); 
	vert_color = in_color;
}