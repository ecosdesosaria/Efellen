using System;
using Server;
using Server.Items;

namespace Server.Custom
{
    public class RichesSystem
    {
        private static readonly WealthTier[] WealthTiers = new WealthTier[]
        {                             //first is min value, second is max value
                          // tier  r1       r2        r3        r4         r5
            new WealthTier(1,      10, 20,  25, 50,   50, 100,  100, 200,  150, 200),
            new WealthTier(2,      30, 60,  50, 100,  100, 200, 200, 300,  300, 400),
            new WealthTier(3,      60, 80,  100, 200, 200, 400, 400, 600,  600, 800),
            new WealthTier(4,      70, 140, 150, 300, 500, 600, 700, 1000, 1000, 1200),
            new WealthTier(5,      80, 160, 200, 400, 400, 800, 800, 1200, 1200, 1600)
        };

        public static void SpawnRiches(Mobile target, int wealthLevel, int radius = 12)
        {
            if (target == null || target.Map == null)
                return;

            if (wealthLevel < 1)
                wealthLevel = 1;

            if (wealthLevel > 5)
                wealthLevel = 5;

            Map map = target.Map;
            WealthTier tier = WealthTiers[wealthLevel - 1];

            for (int x = -radius; x <= radius; ++x)
            {
                for (int y = -radius; y <= radius; ++y)
                {
                    double dist = Math.Sqrt(x * x + y * y);

                    if (dist <= radius)
                        new RichesSpawnTimer(map, target.X + x, target.Y + y, tier).Start();
                }
            }
        }

         private class WealthTier
        {
            private int m_Level;
            private int m_R1Min;
            private int m_R1Max;
            private int m_R2Min;
            private int m_R2Max;
            private int m_R3Min;
            private int m_R3Max;
            private int m_R4Min;
            private int m_R4Max;
            private int m_R5Min;
            private int m_R5Max;

            public int Level { get { return m_Level; } }
            public int R1Min { get { return m_R1Min; } }
            public int R1Max { get { return m_R1Max; } }
            public int R2Min { get { return m_R2Min; } }
            public int R2Max { get { return m_R2Max; } }
            public int R3Min { get { return m_R3Min; } }
            public int R3Max { get { return m_R3Max; } }
            public int R4Min { get { return m_R4Min; } }
            public int R4Max { get { return m_R4Max; } }
            public int R5Min { get { return m_R5Min; } }
            public int R5Max { get { return m_R5Max; } }

            public WealthTier(int level, int r1Min, int r1Max, int r2Min, int r2Max, 
                            int r3Min, int r3Max, int r4Min, int r4Max, int r5Min, int r5Max)
            {
                m_Level = level;
                m_R1Min = r1Min;
                m_R1Max = r1Max;
                m_R2Min = r2Min;
                m_R2Max = r2Max;
                m_R3Min = r3Min;
                m_R3Max = r3Max;
                m_R4Min = r4Min;
                m_R4Max = r4Max;
                m_R5Min = r5Min;
                m_R5Max = r5Max;
            }

            public int GetR1()
            {
                return (int)(Utility.RandomMinMax(R1Min, R1Max) * (MyServerSettings.GetGoldCutRate() * .01));
            }

            public int GetR2()
            {
                return (int)(Utility.RandomMinMax(R2Min, R2Max) * (MyServerSettings.GetGoldCutRate() * .01));
            }

            public int GetR3()
            {
                return (int)(Utility.RandomMinMax(R3Min, R3Max) * (MyServerSettings.GetGoldCutRate() * .01));
            }

            public int GetR4()
            {
                return (int)(Utility.RandomMinMax(R4Min, R4Max) * (MyServerSettings.GetGoldCutRate() * .01));
            }

            public int GetR5()
            {
                return (int)(Utility.RandomMinMax(R5Min, R5Max) * (MyServerSettings.GetGoldCutRate() * .01));
            }
        }

        private class RichesSpawnTimer : Timer
        {
            private Map m_Map;
            private int m_X, m_Y;
            private WealthTier m_Tier;

            public RichesSpawnTimer(Map map, int x, int y, WealthTier tier) 
                : base(TimeSpan.FromSeconds(Utility.RandomDouble() * 5.0))
            {
                m_Map = map;
                m_X = x;
                m_Y = y;
                m_Tier = tier;
            }

            protected override void OnTick()
            {
                int z = m_Map.GetAverageZ(m_X, m_Y);
                bool canFit = m_Map.CanFit(m_X, m_Y, z, 6, false, false);

                for (int i = -3; !canFit && i <= 3; ++i)
                {
                    canFit = m_Map.CanFit(m_X, m_Y, z + i, 6, false, false);

                    if (canFit)
                        z += i;
                }

                if (!canFit)
                    return;

                Item item = GenerateRandomItem(m_Tier);

                if (item != null)
                {
                    item.MoveToWorld(new Point3D(m_X, m_Y, z), m_Map);

                    if (0.5 >= Utility.RandomDouble())
                    {
                        PlaySpawnEffect(item);
                    }
                }
            }

            private Item GenerateRandomItem(WealthTier tier)
            {
                int r1 = tier.GetR1();
                int r2 = tier.GetR2();
                int r3 = tier.GetR3();
                int r4 = tier.GetR4();
                int r5 = tier.GetR5();

                switch (Utility.Random(21))
                {
                    case 0: return new Crystals(r1);
                    case 1: return new DDGemstones(r2);
                    case 2: return new DDJewels(r2);
                    case 3: return new DDGoldNuggets(r3);
                    case 4: return new Gold(r3);
                    case 5: return new Gold(r3);
                    case 6: return new Gold(r3);
                    case 7: return new DDSilver(r4);
                    case 8: return new DDSilver(r4);
                    case 9: return new DDSilver(r4);
                    case 10: return new DDSilver(r4);
                    case 11: return new DDSilver(r4);
                    case 12: return new DDSilver(r4);
                    case 13: return new DDCopper(r5);
                    case 14: return new DDCopper(r5);
                    case 15: return new DDCopper(r5);
                    case 16: return new DDCopper(r5);
                    case 17: return new DDCopper(r5);
                    case 18: return new DDCopper(r5);
                    case 19: return new DDCopper(r5);
                    case 20: return new DDCopper(r5);
                    default: return null;
                }
            }

            private void PlaySpawnEffect(Item item)
            {
                switch (Utility.Random(3))
                {
                    case 0: // Fire column
                        Effects.SendLocationParticles(
                            EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 
                            0x3709, 10, 30, 5052
                        );
                        Effects.PlaySound(item, item.Map, 0x208);
                        break;
                    case 1: // Explosion
                        Effects.SendLocationParticles(
                            EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 
                            0x36BD, 20, 10, 5044
                        );
                        Effects.PlaySound(item, item.Map, 0x307);
                        break;
                    case 2: // Ball of fire
                        Effects.SendLocationParticles(
                            EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 
                            0x36FE, 10, 10, 5052
                        );
                        break;
                }
            }
        }
    }
}