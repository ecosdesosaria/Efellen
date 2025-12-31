using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class DruidismFormMapping
    {
        public string FormId;
        public double RequiredDruidism;
        public double BaseChance;

        public DruidismFormMapping(string formId, double required, double chance)
        {
            FormId = formId;
            RequiredDruidism = required;
            BaseChance = chance;
        }

        public static DruidismFormMapping GetMapping(BaseCreature c)
        {
            if (c is RandomSerpent)
                return new DruidismFormMapping("Anaconda", 80.0, 0.04);

            if (c is ElderBlackBearRiding || c is ElderBrownBearRiding || c is ElderPolarBearRiding)
                return new DruidismFormMapping("Direbear", 85.0, 0.04);
                        
            if (c is CaveBearRiding)
                return new DruidismFormMapping("Direbear", 85.0, 0.06);

            if (c is Stalker)
                return new DruidismFormMapping("Stalker", 85.0, 0.05);

            if (c is Scorpion)
                return new DruidismFormMapping("Giant Scorpion", 90.0, 0.03);

            if (c is DeadlyScorpion)
                return new DruidismFormMapping("Giant Scorpion", 90.0, 0.04);

            if (c is Gorakong)
                return new DruidismFormMapping("Gorakong", 100.0, 0.04);

            if (c is Gorilla)
                return new DruidismFormMapping("Gorakong", 100.0, 0.02);

            if( c is BlackWolf || c is GreyWolf || c is TimberWolf )
                return new DruidismFormMapping("Worg", 105.0, 0.02);
       
            if( c is WereWolf || c is WhiteWolf || c is WinterWolf || c is WolfDire || c is WolfMan)
                return new DruidismFormMapping("Worg", 105.0, 0.04);
       
            if (c is Worg)
                return new DruidismFormMapping("Worg", 105.0, 0.05);

            if (c is GriffonRiding)
                return new DruidismFormMapping("Griffon", 110.0, 0.03);
            
            if (c is WarGriffon)
                return new DruidismFormMapping("Griffon", 110.0, 0.04);

            if (c is Stegosaurus || c is PackStegosaurus)
                return new DruidismFormMapping("Stegosaurus", 115.0, 0.04);


            if (c is MonstrousSpider)
                return new DruidismFormMapping("Monstrous Spider", 120.0, 0.03);

            if (c is Tyranasaur)
                return new DruidismFormMapping("Tyrannosaurus", 120.0, 0.02);

            return null;
        }
    }
}
