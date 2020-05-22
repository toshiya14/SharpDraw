using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDraw
{
    public struct FontSetting
    {
        public string Family;
        public float Size;
        public bool IsItalic;
        public FontWeight Weight;
        public FontStyle Style;
        public WordWrapping WordWrapping;
        public ParagraphAlignment ParagraphAlign;
        public TextAlignment Align;
    }

    public enum ParagraphAlignment
    {
        Near = 0,
        Far = 1,
        Center = 2
    }

    public enum TextAlignment
    {
        Leading = 0,
        Trailing = 1,
        Center = 2,
        Justified = 3
    }

    public enum FontWeight
    {
        Thin = 100,
        ExtraLight = 200,
        UltraLight = 200,
        Light = 300,
        SemiLight = 350,
        Normal = 400,
        Regular = 400,
        Medium = 500,
        DemiBold = 600,
        SemiBold = 600,
        Bold = 700,
        ExtraBold = 800,
        UltraBold = 800,
        Black = 900,
        Heavy = 900,
        ExtraBlack = 950,
        UltraBlack = 950
    }

    public enum FontStyle
    {
        Normal = 0,
        Oblique = 1,
        Italic = 2
    }

    public enum WordWrapping
    {
        Wrap = 0,
        NoWrap = 1,
        EmergencyBreak = 2,
        WholeWord = 3,
        Character = 4
    }
}
