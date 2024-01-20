using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicOrbwalker1.Essentials
{
    class Values
    {
        // Orbwalker Components //
        public static bool Activated = false;
        public static Point originalPosition;
        public static Point EnemyPosition;
        public static Color EnemyPix = Color.FromArgb(52, 3, 0);
        public static Color EnemyPix1 = Color.FromArgb(53, 3, 0);
        // Orbwalker Components //

        // Main Hodnoty //
        public static float attackRange;
        public static float attackSpeed;
        public static float Windup;
        public static string? SelectedChamp;
        public static bool? IsChampionDead;
        // Main Hodnoty //

        // Champion Windup //
        public static float Ashe_wu = 21.93f;
        public static float Caitlyn_wu = 17.708f;
        public static float Corki_wu = 10.00f;
        public static float Draven_wu = 15.614f;
        public static float Ezreal_wu = 18.839f;
        public static float Jinx_wu = 16.875f;
        public static float Kaisa_wu = 16.108f;
        public static float Kalista_wu = 36.000f;
        public static float Kayle_wu = 19.355f;
        public static float Kindred_wu = 17.544f;
        public static float Kogmaw_wu = 16.622f;
        public static float Lucian_wu = 15.00f;
        public static float MissFortune_wu = 14.801f;
        //public static float Olaf_wu = 23.438f;
        public static float Quinn_wu = 17.544f;
        public static float Samira_wu = 15.00f;
        public static float Senna_wu = 31.25f;
        public static float Sivir_wu = 12.00f;
        public static float Tristana_wu = 14.801f;
        public static float Twitch_wu = 20.192f;
        public static float Varus_wu = 17.544f;
        public static float Vayne_wu = 17.544f;
        public static float Xayah_wu = 17.687f;
        //public static float Xerath_wu = 25.074f;
        public static float Other_wu = 15.0f;
        // Champion Windup //

        // Champion Windup Modifier //
        public static float Everyone_bwm = 1f;
        public static float Kalista_bwm = 0.75f;
        public static float Senna_bwm = 0.60f;
        public static float Graves_bwn = 0.10f;
        // Champion Windup Modifier //
        public static float getWindup()
        {
            return Windup;
        }

        public static bool MakeCorrectWindup()
        {
            var championWindups = new Dictionary<string, float>
        {
        {"Ashe", Ashe_wu},
        {"Caitlyn", Caitlyn_wu},
        {"Corki", Corki_wu},
        {"Draven", Draven_wu},
        {"Ezreal", Ezreal_wu},
        {"Jinx", Jinx_wu},
        {"Kaisa", Kaisa_wu},
        {"Kalista", Kalista_wu},
        {"Kayle", Kayle_wu},
        {"Kindred", Kindred_wu},
        {"Kogmaw", Kogmaw_wu},
        {"Lucian", Lucian_wu},
        {"MissFortune", MissFortune_wu},
        {"Quinn", Quinn_wu},
        {"Samira", Samira_wu},
        {"Senna", Senna_wu},
        {"Sivir", Sivir_wu},
        {"Tristana", Tristana_wu},
        {"Twitch", Twitch_wu},
        {"Varus", Varus_wu},
        {"Vayne", Vayne_wu},
        {"Xayah", Xayah_wu},
        // {"Xerath", Xerath_wu},
        };

            float defaultWindup = Other_wu;
            float championSpecificWindup = championWindups.TryGetValue(SelectedChamp, out float specificWindup) ? specificWindup : defaultWindup;

            if (Windup != championSpecificWindup)
            {
                Windup = championSpecificWindup;
                return true;
            }

            return true;
        }

    }
}
