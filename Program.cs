// udìlat aditional Windup

using System.Runtime.InteropServices;
using MagicOrbwalker1.Essentials;
using MagicOrbwalker1.Essentials.API;
using static MagicOrbwalker1.Essentials.Keyboard;

namespace MagicOrbwalker1
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        static bool cKeyPressed = false;
        static bool middlepressed = false;

        [STAThread]
        static async Task Main()
        {
            AllocConsole();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Menu //
            Thread LobbyHandle = new Thread(() => CNSL.LobbyShow());
            LobbyHandle.Start();
            // Menu //

            // Drawings //
            Thread overlay = new Thread(() => makeoverlay());
            overlay.Start();
            // Drawings //

            // Orbwalker Loop //

            while (true)
            {
                var apiClient = new API();
                Values.IsChampionDead = await apiClient.IsChampionOrEntityDeadAsync();
                Values.SelectedChamp = await apiClient.GetChampionNameAsync();

                if (SpecialFunctions.IsTargetProcessFocused("League of Legends"))
                {
                    bool isChampionDead = Values.IsChampionDead.HasValue ? Values.IsChampionDead.Value : true;
                    if (!isChampionDead && Values.MakeCorrectWindup())
                    {
                        Thread.Sleep(1);
                        if ((GetAsyncKeyState(Keys.Space) & 0x8000) != 0 && SpecialFunctions.AAtick < Environment.TickCount)
                        {
                            if (Values.ShowAttackRange && !cKeyPressed)
                            {
                                Keyboard.SendKeyDown(ScanCodeShort.KEY_C);
                                cKeyPressed = true;
                            }
                            if (Values.AttackChampionOnly && !middlepressed)
                            {
                                SendMiddleMouseDown();
                                middlepressed = true;
                            }
                            OrbwalkEnemy().Wait();
                        }
                        else if (Values.ShowAttackRange && cKeyPressed)
                        {
                            Keyboard.SendKeyUp(ScanCodeShort.KEY_C);
                            cKeyPressed = false;
                        }
                        else if (Values.AttackChampionOnly && middlepressed)
                        {
                            SendMiddleMouseUp();
                            middlepressed = false;
                        }
                    }
                }
            }

            // Orbwalker Loop //

        }

        private static void makeoverlay()
        {
            using (var overlay = new Drawings())
            {
                overlay.Run();
            }
        }

        private static Random rnd = new Random();
        private static async Task OrbwalkEnemy()
        {
            if (await SpecialFunctions.CanAttack())
            {
                Values.EnemyPosition = await ScreenCapture.GetEnemyPosition ();
                if (Values.EnemyPosition != Point.Empty && Values.EnemyPosition != new Point(0, 0))
                {
                    Values.originalMousePosition = new Point(Cursor.Position.X, Cursor.Position.Y);
                    SpecialFunctions.ClickAt(Values.EnemyPosition);

                    SpecialFunctions.AAtick = Environment.TickCount;
                    int windupDelay = await SpecialFunctions.GetAttackWindup();
                    SpecialFunctions.MoveCT = Environment.TickCount + windupDelay;

                    SpecialFunctions.SetCursorPos(Values.originalMousePosition.X, Values.originalMousePosition.Y);

                    if (Values.attackSpeed < 1.75)
                    {
                        Thread.Sleep(Values.SleepOnLowAS);
                    }

                }
                else
                {
                    SpecialFunctions.Click();
                }
            }
            else if (SpecialFunctions.CanMove() && SpecialFunctions.MoveCT <= Environment.TickCount)
            {
                SpecialFunctions.Click();

                Values.originalMousePosition = Cursor.Position;
                SpecialFunctions.MoveCT = Environment.TickCount + rnd.Next(50, 80);
            }
            else
            {
                Thread.Sleep(1);
                SpecialFunctions.Click();
            }
        }
    }
}
