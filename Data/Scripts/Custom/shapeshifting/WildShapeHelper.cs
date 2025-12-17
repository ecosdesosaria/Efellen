using Server;
using Server.Mobiles;

namespace Server.Items
{
    public static class WildShapeHelper
    {
        public static bool IsWildShaped(Mobile m)
        {
            PlayerMobile pm = m as PlayerMobile;
            if (pm == null)
                return false;
            // mildly hacky
            return pm.BodyMod != 0;
        }
    }
}
