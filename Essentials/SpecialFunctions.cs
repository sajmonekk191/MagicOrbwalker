using System.Diagnostics;
using System.Runtime.InteropServices;
using MagicOrbwalker1.Essentials.API;

namespace MagicOrbwalker1.Essentials
{
    class SpecialFunctions
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags);

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        public static bool IsTargetProcessFocused(string processName)
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
        public static void ClickAt(Point location)
        {
            SetCursorPos(location.X, location.Y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN);
            mouse_event(MOUSEEVENTF_RIGHTUP);
        }

        public static void Click()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN);
            mouse_event(MOUSEEVENTF_RIGHTUP);
        }


        public static int AAtick;
        public static int MoveCT;

        public static async Task<int> GetAttackWindup()
        {
            float windup = Values.getWindup();
            var apiClient = new API.API();
            Values.attackSpeed = await apiClient.GetAttackSpeedAsync();
            int finalWindUP = (int)((1 / Values.attackSpeed * 1000) * windup);
            return finalWindUP;
        }

        public static async Task<int> GetAttackDelay()
        {
            var apiClient = new API.API();
            Values.attackSpeed = await apiClient.GetAttackSpeedAsync();
            return (int)(1000.0f / Values.attackSpeed);
        }

        public static async Task<bool> CanAttack()
        {
            int attackDelay = await GetAttackDelay();
            return AAtick + attackDelay < Environment.TickCount;
        }

        public static bool CanMove()
        {
            if (Values.SelectedChamp == "Kalista")
            {
                return true;
            }
            else
            {
                return MoveCT <= Environment.TickCount;
            }
        }
    }
}
