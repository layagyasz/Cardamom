#version 430 core

#define AMBIENT 0.2f

out vec4 out_color;

in vec4 vert_color;
in vec2 vert_tex_coord;
in vec3 vert_internal_coord;
in vec3 vert_normal;
in vec2 vert_bump_tex_coord;

layout(binding = 0) uniform sampler2D diffuse_texture;
layout(binding = 1) uniform sampler2D bump_texture;

vec2 as_spherical(vec3 v)
{
    return vec2(atan(length(v.xz), v.y), atan(v.z, v.x));
}

vec3 as_cartesian(vec2 v)
{
    return vec3(sin(v.x) * cos(v.y), cos(v.x), sin(v.x) * sin(v.y));
}

void main()
{
    vec3 bump_normal =  2 * texture(bump_texture, vert_tex_coord / textureSize(bump_texture, 0)).rgb - 1;
    vec3 normal = as_cartesian(as_spherical(bump_normal) + as_spherical(vert_normal) - vec2(1.57f , 1.57f));
    float light = AMBIENT + max(0, dot(normal, vec3(-1, 0, 0)));

    vec4 diffuse = vert_color * texture(diffuse_texture, vert_tex_coord / textureSize(diffuse_texture, 0)); 
    out_color = vec4(light * diffuse.xyz, 1);
}