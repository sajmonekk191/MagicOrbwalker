using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags);

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [STAThread]
        static void Main()
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
            Console.Write("Enter your command: ");
            Thread orbwalkThread = new Thread(OrbwalkLoop);
            orbwalkThread.Start();
            Console.ReadLine();
        }

        private static void ClickAt(Point location)
        {
            SetCursorPos(location.X, location.Y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN);
            mouse_event(MOUSEEVENTF_RIGHTUP);
        }

        private static void Click()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN);
            mouse_event(MOUSEEVENTF_RIGHTUP);
        }

        private static bool IsTargetProcessFocused(string processName)
        {
            IntPtr activeWindowHandle = GetForegroundWindow();
            GetWindowThreadProcessId(activeWindowHandle, out int activeProcId);

            try
            {
                Process activeProcess = Process.GetProcessById(activeProcId);
                return activeProcess != null && activeProcess.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                Console.WriteLine("League of Legends Game not found!!");
                return false;
            }
        }
        private static async void OrbwalkLoop()
        {
            while (true)
            {
                var apiClient = new API();
                bool? isChampionOrEntityDead = await apiClient.IsChampionOrEntityDeadAsync();
                if (IsTargetProcessFocused("League of Legends"))
                {
                    if (!isChampionOrEntityDead.Value)
                    {
                        Thread.Sleep(10);
                        /*API apiclient = new API();
                        Values.SelectedChamp = await apiclient.GetChampionNameAsync();
                        Console.Title = "MagicOrbwalker      Playing: " + Values.SelectedChamp;*/
                        if ((GetAsyncKeyState(Keys.Space) & 0x8000) != 0)
                        {
                            OrbwalkEnemy().Wait();
                        }
                    }
                }
            }
        }
        private static int AAtick;
        private static int MoveCT;

        private static async Task OrbwalkEnemy()
        {
            if(await CanAttack())
            {
                Point originalPosition;

                Point? objectPosition = await ScreenCapture.GetEnemyPosition();

                if (objectPosition.HasValue && objectPosition.Value != new Point(0, 0))
                {
                    originalPosition = new Point(Cursor.Position.X, Cursor.Position.Y);
                    ClickAt(objectPosition.Value);
                    Thread.Sleep(5);

                    AAtick = Environment.TickCount;
                    int windupDelay = await GetAttackWindup();
                    MoveCT = Environment.TickCount + windupDelay;

                    SetCursorPos(originalPosition.X, originalPosition.Y);
                }
                else
                {
                    Click();
                }
            }
            else
            {
                Thread.Sleep(10);
                Click();
            }
        }
        private static async Task<int> GetAttackWindup()
        {
            float windup = Values.getWindup();
            var apiClient = new API();
            float attackSpeed = await apiClient.GetAttackSpeedAsync();
            int finalWindUP = (int)((1 / attackSpeed * 1000) * windup);
            return finalWindUP;
        }

        private static async Task<int> GetAttackDelay()
        {
            var apiClient = new API();
            float attackSpeed = await apiClient.GetAttackSpeedAsync();
            return (int)(1000.0f / attackSpeed);
        }

        private static async Task<bool> CanAttack()
        {
            int attackDelay = await GetAttackDelay();
            return AAtick + attackDelay + 24 / 2 < Environment.TickCount;
        }
    }
}
