using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDraw
{
    public interface IPaintTool: IDisposable
    {
        void DrawText(string text, FontSetting format, float x, float y, RGBAColor? color);
        void DrawText(string text, string family, float size, float x, float y, RGBAColor? color);
    }
}
