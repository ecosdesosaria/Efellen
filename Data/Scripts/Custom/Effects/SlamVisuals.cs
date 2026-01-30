using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.EffectsUtil
{
    public static class SlamVisuals
    {
        public static void SlamVisual(Mobile from, int radius, int effectID, int hue)
        {
            if (from == null || from.Deleted || from.Map == null)
                return;

            SlamRing(from, from.Location, radius, effectID, hue);
        }

        public static void SlamRing(Mobile from, Point3D center, int radius, int effectID, int hue)
        {
            if (from == null || from.Deleted || from.Map == null)
                return;

            Map map = from.Map;
            int centerZ = center.Z;
            int delayMS = 90;

            for (int r = 1; r <= radius; r++)
            {
                int ringRadius = r;
                Timer.DelayCall(TimeSpan.FromMilliseconds(r * delayMS), delegate
                {
                    DoRing(center, ringRadius, effectID, hue, map, centerZ);
                });
            }
        }

        private static void DoRing(Point3D center, int radius, int effectID, int hue, Map map, int z)
        {
            int x0 = center.X;
            int y0 = center.Y;
            for (int x = -radius; x <= radius; x++)
            {
                int yRange = radius - Math.Abs(x);
                if (yRange >= 0)
                {
                    int y = yRange;
                    if (Math.Abs(Math.Abs(x) + Math.Abs(y) - radius) <= 1)
                    {
                        Point3D target = new Point3D(x0 + x, y0 + y, z);
                        Effects.SendLocationEffect(target, map, effectID, 15, hue, 0);
                    }
                    if (y != 0)
                    {
                        y = -yRange;
                        if (Math.Abs(Math.Abs(x) + Math.Abs(y) - radius) <= 1)
                        {
                            Point3D target = new Point3D(x0 + x, y0 + y, z);
                            Effects.SendLocationEffect(target, map, effectID, 15, hue, 0);
                        }
                    }
                }
            }
        }
    }
}