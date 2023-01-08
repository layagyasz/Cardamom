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

vec4 quaternion_conjugate(vec4 q)
{ 
  return vec4(-q.x, -q.y, -q.z, q.w); 
}
  
vec4 quaternion_multiply(vec4 q1, vec4 q2)
{ 
  vec4 qr;
  qr.x = (q1.w * q2.x) + (q1.x * q2.w) + (q1.y * q2.z) - (q1.z * q2.y);
  qr.y = (q1.w * q2.y) - (q1.x * q2.z) + (q1.y * q2.w) + (q1.z * q2.x);
  qr.z = (q1.w * q2.z) + (q1.x * q2.y) - (q1.y * q2.x) + (q1.z * q2.w);
  qr.w = (q1.w * q2.w) - (q1.x * q2.x) - (q1.y * q2.y) - (q1.z * q2.z);
  return qr;
}

vec4 quaternion_rotate(vec3 v, vec4 q)
{
    return quaternion_multiply(quaternion_multiply(q, vec4(v, 0)), quaternion_conjugate(q));
}

vec3 combine_normals_quat(vec3 surface_normal, vec3 bump_normal)
{
    const float inv_sqrt_2 = 0.70710678f;
    return quaternion_rotate(
        bump_normal,
        vec4(
            inv_sqrt_2 * sqrt(1 - surface_normal.z) * normalize(vec3(-surface_normal.y, surface_normal.x, 0)), 
            inv_sqrt_2 * sqrt(1 + surface_normal.z))).xyz;
}

vec2 as_spherical(vec3 v)
{
    return vec2(atan(length(v.xz), v.y), atan(v.z, v.x));
}

vec3 as_cartesian(vec2 v)
{
    return vec3(sin(v.x) * cos(v.y), cos(v.x), sin(v.x) * sin(v.y));
}

vec3 combine_normals_tan(vec3 surface_normal, vec3 bump_normal)
{
    const float pi_over_2 = 1.57079632f;
    return as_cartesian(as_spherical(surface_normal) + as_spherical(bump_normal) - vec2(pi_over_2, pi_over_2));
}


void main()
{
    vec3 bump_normal =  2 * texture(bump_texture, vert_tex_coord / textureSize(bump_texture, 0)).rgb - 1;
    vec3 normal = combine_normals_tan(normalize(vert_normal), bump_normal);
    float light = AMBIENT + max(0, dot(normal, vec3(0, 0, -1)));

    vec4 diffuse = vert_color * texture(diffuse_texture, vert_tex_coord / textureSize(diffuse_texture, 0)); 
    out_color = vec4(light * diffuse.rgb, 1);
}