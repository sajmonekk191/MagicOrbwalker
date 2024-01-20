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
        public static float Windup = 20.192f;
        public static float BaseWindup = 0.658f;
        public static string SelectedChamp;
        public static bool? IsChampionDead;
        // Main Hodnoty //

        // Champion Windup //
        public static float ashewu = 21.93f;
        public static float caitlynwu = 17.708f;
        public static float corkiwu = 10.00f;
        public static float dravenwu = 15.614f;
        public static float ezrealwu = 18.839f;
        public static float jinxwu = 16.875f;
        public static float kaisawu = 16.108f;
        public static float kalistawu = 36.000f;
        public static float kaylewu = 19.355f;
        public static float kindredwu = 17.544f;
        public static float kogmawu = 16.622f;
        public static float lucianwu = 15.00f;
        public static float mfwu = 14.801f;
        public static float olafwu = 23.438f;
        public static float quinnwu = 17.544f;
        public static float samirawu = 15.00f;
        public static float sennawu = 31.25f;
        public static float sivirwu = 12.00f;
        public static float tristanawu = 14.801f;
        public static float twitchwu = 20.192f;
        public static float varuswu = 17.544f;
        public static float vaynewu = 17.544f;
        public static float xayahwu = 17.687f;
        public static float xerathwu = 25.074f;
        // Champion Windup //

        // Champion Windup Modifier //
        public static float everyonebwm = 1f;
        public static float kaisabwm = 0.75f;
        public static float getWindup()
        {
            return Windup;
        }

        // Champion Windup Modifier //
    }
}
