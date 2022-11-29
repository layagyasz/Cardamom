#version 330 core

out vec4 out_color;

in vec4 vert_color;
in vec2 vert_tex_coord;

uniform vec2 size;
uniform vec4 border_color[4];
uniform float border_width[4];

int get_border(vec2 pixel_coord)
{
    if (pixel_coord.x < border_width[0])
    {
        return 1;
    }
    if (pixel_coord.x > size.x - border_width[2])
    {
        return 1;
    }
    if (pixel_coord.y < border_width[1])
    {
        return 1;
    }
    if (pixel_coord.y > size.y - border_width[3])
    {
        return 1;
    }
    return 0;
}

void main()
{
    switch (get_border(size * vert_tex_coord))
    {
        case -1:
            discard;
            break;
        case 0:
            out_color = vert_color;
            break;
        case 1:
            out_color = 
                border_color[0] * (1 - vert_tex_coord.x) * (1 - vert_tex_coord.y) +
                border_color[1] * vert_tex_coord.x * (1 - vert_tex_coord.y) +
                border_color[2] * vert_tex_coord.x * vert_tex_coord.y +
                border_color[3] * (1 - vert_tex_coord.x) * vert_tex_coord.y;
                break;
    }
}