#version 330 core

layout(location = 0) in vec2 in_position;  
layout(location = 1) in vec4 in_color;

out vec4 vert_color; // output a color to the fragment shader

void main(void)
{
	gl_Position = vec4(in_position, 0.0, 0.0); 
	vert_color = in_color;
}