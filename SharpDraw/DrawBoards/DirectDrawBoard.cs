using System;
using System.Collections.Generic;
using System.Text;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.WIC;

using DXGIDevice = SharpDX.DXGI.Device;
using DXGIFormat = SharpDX.DXGI.Format;

using D3DDevice = SharpDX.Direct3D11.Device;
using D3DDevice1 = SharpDX.Direct3D11.Device1;
using D3DDriverType = SharpDX.Direct3D.DriverType;
using D3DDeviceCreationFlags = SharpDX.Direct3D11.DeviceCreationFlags;

using D2DFactory = SharpDX.Direct2D1.Factory;
using D2DDevice = SharpDX.Direct2D1.Device;
using D2DContext = SharpDX.Direct2D1.DeviceContext;
using D2DTargetType = SharpDX.Direct2D1.RenderTargetType;
using D2DPixelFormat = SharpDX.Direct2D1.PixelFormat;
using D2DContextOptions = SharpDX.Direct2D1.DeviceContextOptions;
using D2DAlphaMode = SharpDX.Direct2D1.AlphaMode;
using D2DBitmapProps = SharpDX.Direct2D1.BitmapProperties1;
using D2DBitmapTarget = SharpDX.Direct2D1.Bitmap1;

using DWFactory = SharpDX.DirectWrite.Factory;

using WicRenderTarget = SharpDX.Direct2D1.WicRenderTarget;
using WicBitmap = SharpDX.WIC.Bitmap;
using WicFactory = SharpDX.WIC.ImagingFactory2;
using WicPixelFormat = SharpDX.WIC.PixelFormat;
using SharpDX.Direct3D11;
using System.Drawing;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace SharpDraw.DrawBoards
{
    public class DirectDrawBoard : IDrawBoard
    {
        private enum ContextState
        {
            Init,
            Drawing,
            Released
        }

        private D3DDevice defDev;
        private D3DDevice1 d3dDev;
        private D2DDevice d2dDev;
        private DXGIDevice dxgiDev;

        private WicFactory wicFac;
        private Guid wicpxFmt;

        private D2DContext d2dCtx;
        private DWFactory dwFac;
        private TextFormat dwFormat;

        private D2DPixelFormat d2dpxFmt;
        private D2DBitmapProps d2dbmpProps;

        private D2DBitmapTarget d2dbmpTarget;

        private ContextState ctxState;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int DPI { get; private set; }

        public DirectDrawBoard(int width, int height, int dpi = 96)
        {
            defDev = new D3DDevice(D3DDriverType.Hardware, D3DDeviceCreationFlags.BgraSupport | D3DDeviceCreationFlags.None | D3DDeviceCreationFlags.VideoSupport);
            d3dDev = defDev.QueryInterface<D3DDevice1>();
            dxgiDev = defDev.QueryInterface<DXGIDevice>();
            d2dDev = new D2DDevice(dxgiDev);

            wicFac = new WicFactory();
            d2dCtx = new D2DContext(d2dDev, D2DContextOptions.None);
            dwFac = new DWFactory();

            d2dpxFmt = new D2DPixelFormat(DXGIFormat.R8G8B8A8_UNorm, D2DAlphaMode.Premultiplied);
            wicpxFmt = WicPixelFormat.Format32bppRGBA;

            d2dbmpProps = new D2DBitmapProps(d2dpxFmt, dpi, dpi, BitmapOptions.Target | BitmapOptions.CannotDraw);
            d2dbmpTarget = new D2DBitmapTarget(d2dCtx, new Size2(width, height), d2dbmpProps);
            d2dCtx.Target = d2dbmpTarget;

            this.Height = height;
            this.Width = width;
            this.DPI = dpi;
            ctxState = ContextState.Init;
        }

        private void BeginDraw()
        {
            if (ctxState == ContextState.Init)
            {
                d2dCtx.BeginDraw();
                ctxState = ContextState.Drawing;
            }
        }

        private void EndDraw()
        {
            if (ctxState == ContextState.Drawing)
            {
                d2dCtx.EndDraw();
                ctxState = ContextState.Released;
            }
        }

        public void DrawText(string text, float size, string family, float x = 0, float y = 0, RawColor4? color = null)
        {
            BeginDraw();
            var textFmt = new TextFormat(dwFac, family, size);
            var textLayout = new TextLayout(dwFac, text, textFmt, 400f, 200f);
            var textBrush = new SolidColorBrush(d2dCtx, color ?? new RawColor4(0, 0, 0, 255));
            d2dCtx.DrawTextLayout(new RawVector2(x, y), textLayout, textBrush);
        }

        public void Dump(string path)
        {
            EndDraw();
            ctxState = ContextState.Init;

            using(var file = new WICStream(wicFac, path, SharpDX.IO.NativeFileAccess.Write))
            {
                using(var enc = new PngBitmapEncoder(wicFac))
                {
                    enc.Initialize(file);

                    using (var frameEnc = new BitmapFrameEncode(enc))
                    {
                        frameEnc.Initialize();
                        frameEnc.SetSize(Width, Height);
                        frameEnc.SetPixelFormat(ref wicpxFmt);

                        using(var imgEnc = new ImageEncoder(wicFac, d2dDev))
                        {
                            imgEnc.WriteFrame(d2dbmpTarget, frameEnc, new ImageParameters(d2dpxFmt, DPI, DPI, 0, 0, Width, Height));
                            frameEnc.Commit();
                            enc.Commit();
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            EndDraw();
            d2dbmpTarget.Dispose();
            dwFac.Dispose();
            d2dCtx.Dispose();
            wicFac.Dispose();
            dxgiDev.Dispose();
            d2dDev.Dispose();
            d3dDev.Dispose();
            defDev.Dispose();
        }
    }
}
