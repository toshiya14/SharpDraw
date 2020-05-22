using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDraw
{
    public interface IDrawBoard : IDisposable
    {
        int Width { get; }
        int Height { get; }
        int DPI { get; }
        IPaintTool GetPaintTool();

    }
}
