#version 330 core

out vec4 out_color;

in vec4 vert_color;

void main()
{
    out_color = vert_color;
}