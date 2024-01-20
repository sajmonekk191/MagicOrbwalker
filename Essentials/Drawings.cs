using GameOverlay.Drawing;
using GameOverlay.Windows;
using System.Runtime.InteropServices;
using SolidBrush = GameOverlay.Drawing.SolidBrush;

namespace MagicOrbwalker1.Essentials
{
    internal class Drawings : IDisposable
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);
        private readonly GraphicsWindow _graphicsWindow;
        private SolidBrush _OrbwalkerActivated;
        private SolidBrush _fontBrush;
        private SolidBrush _InfoBrush;
        private SolidBrush _LogoBrush;
        private GameOverlay.Drawing.Font _font;
        private GameOverlay.Drawing.Font _LogoFont;
        public Drawings()
        {
            var gfx = new GameOverlay.Drawing.Graphics
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };

            _graphicsWindow = new GraphicsWindow(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, gfx)
            {
                FPS = 20,
                IsTopmost = true,
                IsVisible = true
            };

            _graphicsWindow.DrawGraphics += DrawGraphics;
            _graphicsWindow.SetupGraphics += SetupGraphics;
            _graphicsWindow.DestroyGraphics += DestroyGraphics;
        }


        private void SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            _OrbwalkerActivated?.Dispose();
            _InfoBrush?.Dispose();
            _fontBrush?.Dispose();
            _LogoBrush?.Dispose();

            _OrbwalkerActivated = gfx.CreateSolidBrush(0, 255, 0, 88);
            _InfoBrush = gfx.CreateSolidBrush(0, 255, 255, 255);
            _fontBrush = gfx.CreateSolidBrush(0, 0, 0, 196);
            _LogoBrush = gfx.CreateSolidBrush(255, 255, 0, 196);

            _LogoFont?.Dispose();
            _font?.Dispose();

            _LogoFont = gfx.CreateFont("Monaco", 15, true);
            _font = gfx.CreateFont("Arial", 12);
        }

        private void DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.ClearScene();
            if (SpecialFunctions.IsTargetProcessFocused("League of Legends"))
            {
                // INFO //

                gfx.DrawTextWithBackground(_LogoFont, _fontBrush, _LogoBrush, (_graphicsWindow.Width / 2) - 62, 0, $"MagicOrbwalker");

                gfx.DrawTextWithBackground(_font, _fontBrush, _InfoBrush, 2, _graphicsWindow.Height - 16, $"ATK Speed: {Values.attackSpeed}");

                gfx.DrawTextWithBackground(_font, _fontBrush, _InfoBrush, 2, _graphicsWindow.Height - 35, $"ATK Range: {Values.attackRange}");

                gfx.DrawTextWithBackground(_font, _fontBrush, _InfoBrush, 2, _graphicsWindow.Height - 50, $"Windup: {Values.Windup}");
                // INFO //

                // Orbwalker ON/OFF //
                if ((GetAsyncKeyState(Keys.Space) & 0x8000) != 0)
                {
                    int centerX = _graphicsWindow.Width / 2;
                    int centerY = _graphicsWindow.Height / 2;

                    gfx.DrawTextWithBackground(_font, _fontBrush, _OrbwalkerActivated, (_graphicsWindow.Width / 2) + 20, (_graphicsWindow.Height / 2) - 10, $"Orbwalker: ON");
                }
                // Orbwalker ON/OFF //

                /*// Draw Cross //
                if (Values.EnemyPosition != new System.Drawing.Point(0, 0))
                {
                    int crosshairSize = 20;
                    float thickness = 5.0f;
                    var crosshairColor = new GameOverlay.Drawing.Color(255, 0, 0);

                    if (_crosshairBrush == null)
                        _crosshairBrush = gfx.CreateSolidBrush(crosshairColor);

                    gfx.DrawCrosshair(_crosshairBrush, new GameOverlay.Drawing.Point(Values.EnemyPosition.X, Values.EnemyPosition.Y), crosshairSize, thickness, CrosshairStyle.Gap);
                }
                // Draw Cross //*/
            }
        }

        private static void DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
        {
            // Clean up resources here
        }
        public void Run()
        {
            _graphicsWindow.Create();
            _graphicsWindow.Join();
        }

        public void Dispose()
        {
            _graphicsWindow?.Dispose();
        }

    }
}
