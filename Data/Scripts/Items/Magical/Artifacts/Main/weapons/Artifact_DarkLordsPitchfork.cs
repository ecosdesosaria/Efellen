using System;
using Server;
using Server.Spells.Eighth;
using Server.Spells;

namespace Server.Items
{
    public class Artifact_DarkLordsPitchfork : GiftPitchfork
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

        [Constructable]
        public Artifact_DarkLordsPitchfork()
        {
            Name = "Dark Lord's PitchFork";
            Hue = 1157;
            WeaponAttributes.HitFireArea = 50;
            WeaponAttributes.HitFireball = 50;
            WeaponAttributes.ResistFireBonus = 15;
            Attributes.SpellChanneling = 1;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Calls forth demons" );
		}

        public override bool OnEquip(Mobile from)
        {
            if (from.Karma > 0)
            {
                from.SendMessage("Este instrumento vil queima suas mãos e se recusa a ser empunhado por você!");
                return false;
            }

            return base.OnEquip(from);
        }

        public override void OnDoubleClick( Mobile from )
		{
			if ( Parent != from )
			{
				from.SendMessage( "Você precisa estar segurando o forcado para invocar um Demônio." );
			}
			else
			{
				new SummonDaemonSpell( from, this ).Cast();
			}
			return;
		}

        public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
        {
            phys = 0;
            cold = 0;
            fire = 100;
            nrgy = 0;
            pois = 0;
            chaos = 0;
            direct = 0;
        }
        public Artifact_DarkLordsPitchfork( Serial serial )
            : base( serial )
        {
        }
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 );
        }
        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
			ArtifactLevel = 2;
            int version = reader.ReadInt();
        }
    }
}
