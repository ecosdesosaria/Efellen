using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Regions;

namespace Server.Items
{
    public class EssenceOfLolthsHatred : Item
    {
        [Constructable]
        public EssenceOfLolthsHatred() : this(1)
        {
        }

        [Constructable]
        public EssenceOfLolthsHatred(int amount) : base(0x2FF7)
        {
            Stackable = true;
            Weight = 0.01;
            Hue = 1316;
            Amount = amount;
            Name = "Essense of Lolth's Hatred";
        }

        public override string DefaultDescription{ get{ return "This strange idol shimmers with Drow malice. It seems to want to move with a will of its own towards the sacrificial pool in the depths of Fanaedar"; } }

        public EssenceOfLolthsHatred(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
                return;
            }

            Region reg = from.Region;
            if (reg == null || !reg.IsPartOf("Fanaedar"))
            {
                from.SendMessage("You must be in Fanaedar to use this.");
                return;
            }

            Point3D targetCoord = new Point3D(640, 2868, 0);
            if (from.GetDistanceToSqrt(targetCoord) > 2)
            {
                from.SendMessage("You are too far away from the pool of sacrifices.");
                return;
            }

            int totalEssense = 0;
            Item[] items = from.Backpack.FindItemsByType(typeof(EssenceOfLolthsHatred));
            for (int i = 0; i < items.Length; i++)
            {
                totalEssense += items[i].Amount;
            }

            if (totalEssense < 20)
            {
                from.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "You have not done enough to warrant Lolth's attention");
                return;
            }

            from.SendMessage("Target the artifact weapon that you want to consecrate to Lolth");
            from.Target = new LolthConsecrateTarget(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        private class LolthConsecrateTarget : Target
        {
            private Mobile m_From;

            public LolthConsecrateTarget(Mobile from) : base(2, false, TargetFlags.None)
            {
                m_From = from;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item item = (Item)targeted;

                    bool isValidType = (item is BaseGiftAxe || item is BaseGiftSword || 
                                       item is BaseGiftKnife || item is BaseGiftBashing || 
                                       item is BaseGiftWhip || item is BaseGiftPoleArm || 
                                       item is BaseGiftRanged || item is BaseGiftSpear || 
                                       item is BaseGiftStaff);

                    if (!isValidType)
                    {
                        from.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Lolth is uninterested in that item.");
                        return;
                    }

                    BaseWeapon weapon = item as BaseWeapon;
                    if (weapon == null)
                    {
                        from.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Lolth is uninterested in that item.");
                        return;
                    }

                    bool modified = false;
                    int minDam = weapon.MinDamage;
                    int maxDam = weapon.MaxDamage;

                    if (minDam > 25 || maxDam > 30)
                    {
                        weapon.MinDamage = 25;
                        weapon.MaxDamage = 30;
                        IGiftable giftable = item as IGiftable;
                        if (giftable != null)
                        {
                            if (item is BaseGiftSword)
                                ((BaseGiftSword)item).Points += 50;
                            else if (item is BaseGiftAxe)
                                ((BaseGiftAxe)item).Points += 50;
                            else if (item is BaseGiftKnife)
                                ((BaseGiftKnife)item).Points += 50;
                            else if (item is BaseGiftBashing)
                                ((BaseGiftBashing)item).Points += 50;
                            else if (item is BaseGiftWhip)
                                ((BaseGiftWhip)item).Points += 50;
                            else if (item is BaseGiftPoleArm)
                                ((BaseGiftPoleArm)item).Points += 50;
                            else if (item is BaseGiftRanged)
                                ((BaseGiftRanged)item).Points += 50;
                            else if (item is BaseGiftSpear)
                                ((BaseGiftSpear)item).Points += 50;
                            else if (item is BaseGiftStaff)
                                ((BaseGiftStaff)item).Points += 50;

                            modified = true;
                        }
                    }
                    else
                    {
                        if (minDam < 17)
                            minDam = 17;
                        if (maxDam < 20)
                            maxDam = 20;

                        if (minDam >= 17)
                            minDam += Utility.RandomMinMax(1, 3);

                        if (maxDam >= 20)
                        {
                            maxDam += Utility.RandomMinMax(1, 3);
                            if (maxDam <= minDam + 1)
                                maxDam = minDam + 2;
                        }

                        weapon.MinDamage = minDam;
                        weapon.MaxDamage = maxDam;
                        modified = true;
                    }

                    if (modified)
                    {
                        item.Hue = Utility.RandomDrowHue();

                        int newKarma = from.Karma - 10000;
                        if (newKarma < -15000)
                            newKarma = -15000;
                        from.Karma = newKarma;

                        int toConsume = 20;
                        Item[] Essenses = from.Backpack.FindItemsByType(typeof(EssenceOfLolthsHatred));
                        for (int i = 0; i < Essenses.Length && toConsume > 0; i++)
                        {
                            Item Essense = Essenses[i];
                            int available = Essense.Amount;
                            if (available >= toConsume)
                            {
                                Essense.Consume(toConsume);
                                toConsume = 0;
                            }
                            else
                            {
                                Essense.Delete();
                                toConsume -= available;
                            }
                        }
                        Effects.SendLocationEffect(from.Location, from.Map, 0x3709, 30, 10, 1316, 0);
                        from.PlaySound(0x208);
                        Effects.SendLocationEffect(new Point3D(from.X, from.Y, from.Z + 5), from.Map, 0x3728, 13, 10, 1316, 0);
                        from.PlaySound(0x1F1);
                        from.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Lolth's tendrils caress the weapon...");
                    }
                }
                else
                {
                    from.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Lolth is uninterested in that item.");
                }
            }
        }
    }
}