#version 330 core

out vec4 out_color;

in vec4 vert_color;
in vec2 vert_tex_coord;
in vec2 vert_internal_coord;

uniform sampler2D texture0;

void main()
{
    out_color = vert_color * texture(texture0, vert_tex_coord / textureSize(texture0, 0));
}