using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDraw
{
    public struct RGBAColor
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public RGBAColor(float r, float g, float b, float a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public static implicit operator RawColor4(RGBAColor c)
        {
            return new RawColor4(c.R, c.G, c.B, c.A);
        }
    }
}
