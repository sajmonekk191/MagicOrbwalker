using Hazdryx.Drawing;
using System.Drawing.Imaging;
using MagicOrbwalker1.Essentials.API;

namespace MagicOrbwalker1.Essentials
{
    class ScreenCapture
    {
        public static async Task<Point> GetEnemyPosition()
        {
            var apiClient = new API.API();
            Values.attackRange = await apiClient.GetAttackRangeAsync();
            if (Values.attackRange != 0)
            {
                Rectangle rect = CalculateRectangle(Values.attackRange);
                return new Point((Size)PixelSearchEnemy(rect, Values.EnemyPix, Values.EnemyPix1, Values.EnemyPixBS, Values.EnemyPixBS1));
            }
            return new Point(0, 0);
        }
        private static Rectangle CalculateRectangle(double clientRange)
        {
            if (clientRange >= 100 && clientRange <= 225)
                return new Rectangle(450, 70, 910, 750);
            else if (clientRange >= 500 && clientRange <= 650)
                return new Rectangle(450, 70, 910, 750);
            else if (clientRange >= 650 && clientRange <= 850)
                return new Rectangle(385, 35, 1100, 875);
            else if (clientRange >= 850)
                return new Rectangle(200, 0, 1600, 900);

            return Rectangle.Empty;
        }
        public static Point PixelSearchEnemy(Rectangle rect, Color pixelColor, Color pixelColor1, Color PixelColorBS, Color PixelColorBS1)
        {
            Point playerPos = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
            if (rect.IsEmpty)
                return new Point(0, 0);

            List<Point> points = PixelSearchEnemies(rect, pixelColor, pixelColor1, PixelColorBS, PixelColorBS1, playerPos);
            Point closestPoint = new Point(0, 0);
            int closestDistance = int.MaxValue;

            foreach (Point p in points)
            {
                int distance = SquareDistance(playerPos, p);
                if (distance < closestDistance)
                {
                    closestPoint = p;
                    closestDistance = distance;
                }
            }

            return closestPoint;
        }
        private static int SquareDistance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }

        public static List<Point> PixelSearchEnemies(Rectangle rect, Color PixelColor, Color PixelColor1, Color PixelColorBS, Color PixelColorBS1, Point PlayerPos)
        {
            int offsetX = 65;
            int offsetY = 95;

            if (Screen.PrimaryScreen.Bounds.Width != 1920 || Screen.PrimaryScreen.Bounds.Height != 1080)
            {
                double XRatio = Screen.PrimaryScreen.Bounds.Width / 1920;
                double YRatio = Screen.PrimaryScreen.Bounds.Height / 1080;
                rect.X = (int)(rect.X * XRatio);
                rect.Y = (int)(rect.Y * YRatio);
                rect.Width = (int)(rect.Width * XRatio);
                rect.Height = (int)(rect.Height * YRatio);
                offsetX = (int)(offsetX * XRatio);
                offsetY = (int)(offsetY * YRatio);

            }
            int searchvalueNormal = PixelColor.ToArgb(); // EnemyPix
            int searchvalueNormal1 = PixelColor1.ToArgb(); // EnemyPix1
            int searchvalueBS = PixelColorBS.ToArgb(); // EnemyPixBS
            int searchvalueBS1 = PixelColorBS1.ToArgb(); // EnemyPixBS1

            List<Point> Points = new List<Point>();
            object lockObj = new object();

            Bitmap BMP = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppRgb);
            using (Graphics GFX = Graphics.FromImage(BMP))
            {
                GFX.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }
            using (FastBitmap bitmap = new FastBitmap(BMP))
            {
                Parallel.For(0, bitmap.Length - 1, i =>
                {
                    int currentPixel = bitmap.GetI(i);
                    int nextPixel = bitmap.GetI(i + 1);

                    // Check for normal enemy pixel followed by its pair
                    if (currentPixel == searchvalueNormal && nextPixel == searchvalueNormal1)
                    {
                        AddPointToList(i, bitmap, rect, offsetX, offsetY, lockObj, Points);
                    }

                    // Check for black shield pixel followed by its pair
                    else if (currentPixel == searchvalueBS && nextPixel == searchvalueBS1)
                    {
                        AddPointToList(i, bitmap, rect, offsetX, offsetY, lockObj, Points);
                    }
                });
            }

            return Points;
        }
        private static void AddPointToList(int index, FastBitmap bitmap, Rectangle rect, int offsetX, int offsetY, object lockObj, List<Point> Points)
        {
            int x = index % bitmap.Width;
            int y = index / bitmap.Width;
            if (InCircle(x, y, rect))
            {
                lock (lockObj)
                {
                    Points.Add(new Point(x + rect.X + offsetX, y + rect.Y + offsetY));
                }
            }
        }
        public static bool InCircle(int X, int Y, Rectangle rect)
        {
            double ratio = (double)rect.Width / rect.Height;
            double r = rect.Height / 2;
            double y = rect.Height / 2 - Y;
            double x = (rect.Width / 2 - X) / ratio;
            if (x * x + y * y <= r * r)
            {
                return true;
            }
            return false;
        }
    }
}
