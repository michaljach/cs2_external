using GameOverlay.Windows;
using HAX.Classes;
using System;
using System.Numerics;
using Color = System.Drawing.Color;
using Graphics = GameOverlay.Drawing.Graphics;

namespace HAX
{
    public class UI
    {
        private readonly GraphicsWindow _window;

        public UI()
        {
            var graphics = new Graphics
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true,
                UseMultiThreadedFactories = false,
                VSync = true,
                WindowHandle = IntPtr.Zero
            };

            // it is important to set the window to visible (and topmost) if you want to see it!
            _window = new GameOverlay.Windows.StickyWindow(Memory.Process.MainWindowHandle, graphics)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = 144,
                X = Main.ScreenRect.left,
                Y = Main.ScreenRect.top,
                Width = Main.ScreenSize.Width,
                Height = Main.ScreenSize.Height
            };

            _window.DrawGraphics += _window_DrawGraphics;
        }

        public void Run()
        {
            // creates the window and setups the graphics
            _window.Create();
        }

        private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.ClearScene();

            DrawTextWithOutline("HAX by yaszko", 10, 25, 25, Color.Brown, Color.Black, true);

            foreach (Entity entity in Main.entityList)
            {
                Vector2 HeadPosition = entity.GetBonePosition2D(Bones.HEAD);
                float BoxHeight = HeadPosition.Y - entity.Position2D.Y;
                float BoxWidth = (BoxHeight / 2) * 1.25f; //little bit wider box
                Color drawcolor = Color.Blue;

                if (Main.showBoxes)
                {
                    DrawOutlineBox(entity.Position2D.X - (BoxWidth / 2), entity.Position2D.Y, BoxWidth, BoxHeight, drawcolor);
                }

                if (Main.showHP)
                {
                    DrawTextWithOutline("HP: " + entity.Health, HeadPosition.X + (BoxWidth / 2), HeadPosition.Y - 16, 12, Color.Red, Color.Black);
                }

                if (Main.showBones)
                {
                    DrawBones(entity);
                }
            }

            #region drawing functions
            void DrawText(string text, float x, float y, int size, Color color, bool bold = false, bool italic = false)
            {
                if (InScreenPos(x, y))
                {
                    gfx.DrawText(gfx.CreateFont("Arial", size, bold, italic), GetBrushColor(color), x, y, text);
                }
            }

            void DrawTextWithOutline(string text, float x, float y, int size, Color color, Color outlinecolor, bool bold = true, bool italic = false)
            {
                DrawText(text, x - 1, y + 1, size, outlinecolor, bold, italic);
                DrawText(text, x + 1, y + 1, size, outlinecolor, bold, italic);
                DrawText(text, x, y, size, color, bold, italic);
            }

            void DrawLine(float fromx, float fromy, float tox, float toy, Color color, float thiccness = 2.0f)
            {
                gfx.DrawLine(GetBrushColor(color), fromx, fromy, tox, toy, thiccness);
            }

            void DrawOutlineBox(float x, float y, float width, float height, Color color, float thiccness = 2.0f)
            {
                gfx.OutlineRectangle(GetBrushColor(Color.FromArgb(0, 0, 0)), GetBrushColor(color), x, y, x + width, y + height, thiccness);
            }

            void DrawBones(Entity Entity)
            {
                Vector2 HeadPosition = Entity.GetBonePosition2D(Bones.HEAD);
                Vector2 Neck = Entity.GetBonePosition2D(Bones.NECK);
                Vector2 Pelvis = Entity.GetBonePosition2D(Bones.PELVIS);
                Vector2 AnkleR = Entity.GetBonePosition2D(Bones.ANKLE_R);
                Vector2 AnkleL = Entity.GetBonePosition2D(Bones.ANKLE_L);
                Vector2 LegUpperL = Entity.GetBonePosition2D(Bones.LEG_UPPER_L);
                Vector2 LegUpperR = Entity.GetBonePosition2D(Bones.LEG_UPPER_R);
                Vector2 ArmUpperR = Entity.GetBonePosition2D(Bones.ARM_UPPER_R);
                Vector2 ArmLowerR = Entity.GetBonePosition2D(Bones.ARM_LOWER_R);
                Vector2 ArmUpperL = Entity.GetBonePosition2D(Bones.ARM_UPPER_L);
                Vector2 ArmLowerL = Entity.GetBonePosition2D(Bones.ARM_LOWER_L);
                Vector2 HandR = Entity.GetBonePosition2D(Bones.HAND_R);
                Vector2 HandL = Entity.GetBonePosition2D(Bones.HAND_L);

                DrawLine(HeadPosition.X, HeadPosition.Y, Neck.X, Neck.Y, Color.Red);
                DrawLine(Neck.X, Neck.Y, ArmUpperR.X, ArmUpperR.Y, Color.Red);
                DrawLine(Neck.X, Neck.Y, ArmUpperL.X, ArmUpperL.Y, Color.Red);
                DrawLine(Neck.X, Neck.Y, Pelvis.X, Pelvis.Y, Color.Red);
                DrawLine(Pelvis.X, Pelvis.Y, LegUpperR.X, LegUpperR.Y, Color.Red);
                DrawLine(Pelvis.X, Pelvis.Y, LegUpperL.X, LegUpperL.Y, Color.Red);
                DrawLine(AnkleL.X, AnkleL.Y, LegUpperL.X, LegUpperL.Y, Color.Red);
                DrawLine(AnkleR.X, AnkleR.Y, LegUpperR.X, LegUpperR.Y, Color.Red);
                DrawLine(ArmUpperR.X, ArmUpperR.Y, ArmLowerR.X, ArmLowerR.Y, Color.Red);
                DrawLine(ArmLowerR.X, ArmLowerR.Y, HandR.X, HandR.Y, Color.Red);
                DrawLine(ArmUpperL.X, ArmUpperL.Y, ArmLowerL.X, ArmLowerL.Y, Color.Red);
                DrawLine(ArmLowerL.X, ArmLowerL.Y, HandL.X, HandL.Y, Color.Red);
            }

            GameOverlay.Drawing.SolidBrush GetBrushColor(Color color)
            {
                return gfx.CreateSolidBrush(color.R, color.G, color.B, color.A);
            }
            #endregion
        }

        public static bool InScreenPos(float x, float y)
        {
            if (x < Main.ScreenSize.Width && x >= 0 && y < Main.ScreenSize.Height && y >= 0)
                return true;
            else
                return false;
        }
    }
}
