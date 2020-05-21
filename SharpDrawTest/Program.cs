using System;
using System.IO;
using SharpDraw;
using SharpDraw.DrawBoards;

namespace SharpDrawTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var drawBoard = new DirectDrawBoard(500, 500))
            {
                drawBoard.DrawText("第一个绘图DEMO", 18f, "Microsoft YaHei", color: new RGBAColor(255, 255, 255, 255));
                drawBoard.Dump(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "demo.png"));
            }
        }
    }
}
