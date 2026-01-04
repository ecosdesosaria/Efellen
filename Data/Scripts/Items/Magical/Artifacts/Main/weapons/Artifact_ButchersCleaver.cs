using System;
using Server;

namespace Server.Items
{
	public class Artifact_ButchersCleaver : GiftCleaver
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_ButchersCleaver()
		{
			Hue = 0x845;
            SkillBonuses.SetValues(0, SkillName.Anatomy, 20);
			ItemID = 0x2AB6;
			Name = "Butcher's Cleaver";
			Attributes.WeaponSpeed = 30;
			Attributes.BonusHits = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Feasts on the fallen" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damage)
        {
            base.OnHit(attacker, defender, damage);

            if (attacker == null || defender == null)
                return;

            if (!defender.Alive || defender.Hits <= 0)
            {
                if (Utility.Random(100) < 10)
                {
                    int hits = Utility.RandomMinMax(10, 35);
                    attacker.Hits += hits;
                    attacker.SendMessage(33, "The Butcher's Cleaver gorges on the fallen enemy!");
                }
            }
        }

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			phys = 100;
			fire = cold = pois = nrgy = chaos = direct = 0;
		}

		public Artifact_ButchersCleaver( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}