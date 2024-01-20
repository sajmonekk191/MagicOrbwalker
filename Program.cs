// udìlat attack range 
// udìlat console UI
// udìlat additional windup to UI


using System.Diagnostics;
using System.Runtime.InteropServices;
using MagicOrbwalker1.Essentials;
using MagicOrbwalker1.Essentials.API;

namespace MagicOrbwalker1
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        [STAThread]
        static async Task Main()
        {
            AllocConsole();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
 
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(@"
  __  __             _       ___       _                  _ _             
 |  \/  | __ _  __ _(_) ___ / _ \ _ __| |____      ____ _| | | _____ _ __ 
 | |\/| |/ _` |/ _` | |/ __| | | | '__| '_ \ \ /\ / / _` | | |/ / _ \ '__|
 | |  | | (_| | (_| | | (__| |_| | |  | |_) \ V  V / (_| | |   <  __/ |   
 |_|  |_|\__,_|\__, |_|\___|\___/|_|  |_.__/ \_/\_/ \__,_|_|_|\_\___|_|   
               |___/                                                      
");
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("Orbwalker is running in background!");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Hold Space to Activate");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ResetColor();

            // Drawings //
            Thread overlay = new Thread(makeoverlay);
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
                            OrbwalkEnemy().Wait();
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
                Values.EnemyPosition = await ScreenCapture.GetEnemyPosition();
                if (Values.EnemyPosition != Point.Empty && Values.EnemyPosition != new Point(0, 0))
                {
                    Values.originalPosition = new Point(Cursor.Position.X, Cursor.Position.Y);
                    SpecialFunctions.ClickAt(Values.EnemyPosition);

                    SpecialFunctions.AAtick = Environment.TickCount;
                    int windupDelay = await SpecialFunctions.GetAttackWindup();
                    SpecialFunctions.MoveCT = Environment.TickCount + windupDelay;

                    SpecialFunctions.SetCursorPos(Values.originalPosition.X, Values.originalPosition.Y);
                    var apiClient = new API();
                    float attackSpeed = await apiClient.GetAttackSpeedAsync();
                    if (attackSpeed < 1.75)
                    {
                        Thread.Sleep(150);
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

                Values.originalPosition = Cursor.Position;
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
