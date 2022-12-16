﻿using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class SolidFace
    {
        public Vector3[] Vertices { get; }

        public SolidFace(Vector3[] vertices)
        {
            Vertices = vertices;
        }
    }
}
