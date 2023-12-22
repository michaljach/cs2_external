using HAX.Classes;
using System;
using System.Drawing;
using System.Numerics;

namespace HAX
{
    internal class Utils
    {
        public static Vector2 WorldToScreen(Vector3 target)
        {
            Vector2 _worldToScreenPos;
            Vector3 to;
            float w = 0.0f;
            float[] viewmatrix = new float[16];

            for (int i = 0; i < 16; i++)
                viewmatrix[i] = Memory.ReadMemory<float>(Main.ClientAddress + Offsets.dwViewMatrix + (i * 0x4));


            to.X = viewmatrix[0] * target.X + viewmatrix[1] * target.Y + viewmatrix[2] * target.Z + viewmatrix[3];
            to.Y = viewmatrix[4] * target.X + viewmatrix[5] * target.Y + viewmatrix[6] * target.Z + viewmatrix[7];

            w = viewmatrix[12] * target.X + viewmatrix[13] * target.Y + viewmatrix[14] * target.Z + viewmatrix[15];

            // behind us
            if (w < 0.01f)
                return new Vector2(0, 0);

            to.X *= (1.0f / w);
            to.Y *= (1.0f / w);

            int width = Main.ScreenSize.Width;
            int height = Main.ScreenSize.Height;

            float x = width / 2;
            float y = height / 2;

            x += 0.5f * to.X * width + 0.5f;
            y -= 0.5f * to.Y * height + 0.5f;

            to.X = x;
            to.Y = y;

            _worldToScreenPos.X = to.X;
            _worldToScreenPos.Y = to.Y;
            return _worldToScreenPos;
        }


        public static Size GetScreenSize()
        {
            Rect rect;
            Memory.GetWindowRect(Main.WindowHandle, out rect);
            return new Size(rect.right - rect.left, rect.bottom - rect.top);
        }
    }
}
