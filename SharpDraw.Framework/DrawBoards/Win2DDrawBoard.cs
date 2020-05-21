using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDraw.DrawBoards
{
    public class Win2DDrawBoard : IDrawBoard
    {
        private const float dpi = 96f;

        CanvasDevice device;
        CanvasRenderTarget offScreen;
        CanvasDrawingSession session;

        public float Width { get; private set; }
        public float Height { get; private set; }

        public Win2DDrawBoard(float width, float height)
        {
        }

        public void Dispose()
        {
            session?.Dispose();
        }
    }
}
