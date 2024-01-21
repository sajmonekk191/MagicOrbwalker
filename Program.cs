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

            PrintStart();

            // Menu //
            Thread lobby = new Thread(LobbyShow);
            lobby.Start();
            // Menu //

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
        #region -Menu-

        private static void PrintStart()
        {
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
        }
        private static void LobbyShow()
        {
        start:
            Console.Clear();
            PrintStart();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("");
            Console.WriteLine("1. Cheat");
            Console.WriteLine("2. Settings");
            Console.WriteLine("3. Exit");
            Console.WriteLine("");
            Console.Write("Select an option: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CheatMenu();
                    break;
                case "2":
                    SettingsMenu();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    ErrorMessage("Invalid option, please try again.");
                    Thread.Sleep(500);
                    LobbyShow();
                    break;
            }
        }
        private static void CheatMenu()
        {
            Console.Clear();
            PrintStart();
            Console.WriteLine("Cheat Menu:");
            Console.WriteLine("");
            Console.WriteLine("1. Set Extra Windup");
            if (Values.ShowAttackRange) SucessMessage("2. Toggle Show Range");
            else ErrorMessage("2. Toggle Show Range");
            if (Values.AttackChampionOnly) SucessMessage("3. Toggle Attack Champion Only");
            else ErrorMessage("3. Toggle Attack Champion Only");
            Console.WriteLine("4. Back to Main Menu");
            Console.WriteLine("");
            Console.Write("Select an option: ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    SetExtraWindup();
                    break;
                case "2":
                    if(Values.ShowAttackRange) Values.ShowAttackRange = false;
                    else Values.ShowAttackRange = true;
                    CheatMenu();
                    break;
                case "3":
                    if (Values.AttackChampionOnly) Values.AttackChampionOnly = false;
                    else Values.AttackChampionOnly = true;
                    CheatMenu();
                    break;
                case "4":
                    LobbyShow();
                    break;
                default:
                    ErrorMessage("Invalid option, please try again.");
                    Thread.Sleep(500);
                    CheatMenu();
                    break;
            }
        }
        private static void SettingsMenu()
        {
            Console.Clear();
            PrintStart();
            Console.WriteLine("Settings Menu:");
            if (Values.DrawingsEnabled) SucessMessage("1. Drawings");
            else ErrorMessage("1. Drawings");
            Console.WriteLine("2. Back to Main Menu");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    if(Values.DrawingsEnabled) Values.DrawingsEnabled = false;
                    else Values.DrawingsEnabled = true;
                    SettingsMenu();
                    break;
                case "2":
                    LobbyShow();
                    break;
                default:
                    ErrorMessage("Invalid option, please try again.");
                    Thread.Sleep(500);
                    SettingsMenu();
                    break;
            }
        }

        static void SetExtraWindup()
        {
            Console.WriteLine("");
            Console.Write("Enter Extra Windup value (in milliseconds): ");
            if (float.TryParse(Console.ReadLine(), out float extraWindup))
            {
                //Values.extraWindup = extraWindup;
                SucessMessage($"Extra Windup set to: {extraWindup}ms");
                Thread.Sleep(500);
                CheatMenu();
            }
            else
            {
                ErrorMessage("Invalid input. Please enter a number.");
                Thread.Sleep(500);
                CheatMenu();
            }
        }

        private static void ErrorMessage(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        private static void SucessMessage(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        #endregion -Menu-

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
                    Values.originalMousePosition = new Point(Cursor.Position.X, Cursor.Position.Y);
                    SpecialFunctions.ClickAt(Values.EnemyPosition);

                    SpecialFunctions.AAtick = Environment.TickCount;
                    int windupDelay = await SpecialFunctions.GetAttackWindup();
                    SpecialFunctions.MoveCT = Environment.TickCount + windupDelay;

                    SpecialFunctions.SetCursorPos(Values.originalMousePosition.X, Values.originalMousePosition.Y);
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
