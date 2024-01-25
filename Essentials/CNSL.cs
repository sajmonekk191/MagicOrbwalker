using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOrbwalker1.Essentials
{
    class CNSL
    {
        public static void PrintStart()
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
        public static void LobbyShow()
        {
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
        public static void CheatMenu()
        {
            Console.Clear();
            PrintStart();
            Console.WriteLine("Cheat Menu:");
            Console.WriteLine("");
            Console.WriteLine("1. Set Sleep on Low AttackSpeed");
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
                    SetSleepOnAS();
                    break;
                case "2":
                    if (Values.ShowAttackRange) Values.ShowAttackRange = false;
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
        public static void SettingsMenu()
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
                    if (Values.DrawingsEnabled) Values.DrawingsEnabled = false;
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

        public static void SetSleepOnAS()
        {
            Console.WriteLine("");
            Console.WriteLine("Current Sleep on AttackSpeed is " + Values.SleepOnLowAS);
            Console.Write("Enter Sleep on AttackSpeed 0 > 1.75 (in milliseconds): ");
            if (int.TryParse(Console.ReadLine(), out int sleeponAS))
            {
                Values.SleepOnLowAS = sleeponAS;
                SucessMessage($"Sleep on low AttackSpeed set to: {sleeponAS}ms");
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

        public static void ErrorMessage(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        public static void SucessMessage(string msg)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
