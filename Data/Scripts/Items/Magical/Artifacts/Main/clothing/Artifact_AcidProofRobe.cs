using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class Artifact_AcidProofRobe : GiftRobe
	{
		public DateTime TimeUsed;

		[CommandProperty(AccessLevel.Owner)]
		public DateTime Time_Used { get { return TimeUsed; } set { TimeUsed = value; InvalidateProperties(); } }

		[Constructable]
		public Artifact_AcidProofRobe()
		{
			Name = "Acidic Robe";
			Hue = 1167;
			Resistances.Fire = 30;
			Resistances.Poison = 30;
			Attributes.RegenStam = 5;
			Attributes.BonusDex = 5;
			Attributes.Luck = 50;
			Attributes.ReflectPhysical = 15;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Soaked in acid" );
		}

		public override void OnDoubleClick( Mobile from )
		{
			DateTime TimeNow = DateTime.Now;
			long ticksThen = TimeUsed.Ticks;
			long ticksNow = TimeNow.Ticks;
			int minsThen = (int)TimeSpan.FromTicks(ticksThen).TotalMinutes;
			int minsNow = (int)TimeSpan.FromTicks(ticksNow).TotalMinutes;
			int CanFillBottle = 30 - ( minsNow - minsThen );

			if ( CanFillBottle > 0 )
			{
				from.SendMessage( "Você pode extrair ácido em " + CanFillBottle + " minutos." );
			}
			else
			{
				if (!from.Backpack.ConsumeTotal(typeof(Bottle), 1))
				{
					from.SendMessage("Você precisa de uma garrafa vazia para extrair o ácido.");
				}
				else
				{
					from.PlaySound( 0x240 );
					from.AddToBackpack( new BottleOfAcid() );
					from.SendMessage( "Você extrai um pouco de ácido do tecido da vestimenta." );
					TimeUsed = DateTime.Now;
				}
			}
		}

		public override bool OnDragLift( Mobile from )
		{
			if ( from is PlayerMobile )
			{
				from.SendMessage( "Você pode usar esta vestimenta para extrair ácido de seu tecido." );
			}

			return true;
		}

		public Artifact_AcidProofRobe( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
            writer.Write( TimeUsed );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();
			TimeUsed = reader.ReadDateTime();
		}
	}
}
