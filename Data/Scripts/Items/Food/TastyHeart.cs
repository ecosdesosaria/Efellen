using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class TastyHeart : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Body; } }

		private string HeartName;

		[CommandProperty( AccessLevel.GameMaster )]
		public string Heart_Name { get { return HeartName; } set { HeartName = value; } }

		[Constructable]
		public TastyHeart() : this( null )
		{
		}

		[Constructable]
		public TastyHeart( string sName ) : base( 0x1CED )
		{
			if ( sName != null ){ HeartName = "the heart of " + sName; } else { HeartName = "heart"; }
			Name = HeartName;
			Weight = 0.1;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) ) 
			{
				from.SendMessage( "Isto deve estar em sua mochila para usar." );
				return;
			}
			else
			{
				if ( from.Hunger < 20 )
				{
					from.Hunger += 3;
					int iHunger = from.Hunger;

					if ( Server.Items.BaseRace.BloodDrinker( from.RaceID ) )
					{
						from.Thirst += 3;
						if ( iHunger < 5 )
							from.SendMessage( "Você come o coração, mas ainda precisa de mais sangue." );
						else if ( iHunger < 10 )
							from.SendMessage( "Você come o coração, mas ainda deseja mais sangue." );
						else if ( iHunger < 15 )
							from.SendMessage( "Você come o coração, mas ainda poderia se satisfazer com sangue." );
						else
							from.SendMessage( "Você come o coração, mas já se satisfez com sangue o suficiente." );
					}
					else if ( Server.Items.BaseRace.BrainEater( from.RaceID ) )
					{
						from.Thirst += 3;
						if ( iHunger < 5 )
							from.SendMessage( "Você come o coração, mas ainda precisa de mais cérebros." );
						else if ( iHunger < 10 )
							from.SendMessage( "Você come o coração, mas ainda deseja mais cérebros." );
						else if ( iHunger < 15 )
							from.SendMessage( "Você come o coração, mas ainda poderia se satisfazer com cérebros." );
						else
							from.SendMessage( "Você come o coração, e você não sente mais fome por cérebros." );
					}
					else
					{
						if ( iHunger < 5 )
							from.SendMessage( "Você come o coração, mas ainda está extremamente com fome." );
						else if ( iHunger < 10 )
							from.SendMessage( "Você come o coração, sentindo-se mais satisfeito." );
						else if ( iHunger < 15 )
							from.SendMessage( "Você come o coração, sentindo-se muito menos com fome." );
						else
							from.SendMessage( "Você come o coração, mas agora se sente bastante cheio." );
					}

					this.Consume();

					// Play a random "eat" sound
					from.PlaySound( Utility.Random( 0x3A, 3 ) );

					if ( from.Body.IsHuman && !from.Mounted )
						from.Animate( 34, 5, 1, true, false, 0 );

					int iHeal = (int)from.Skills[SkillName.Tasting].Value;
					int iHurt = from.HitsMax - from.Hits;

					if ( iHurt > 0 )
					{
						if ( iHeal > iHurt )
						{
							iHeal = iHurt;
						}

						from.Hits = from.Hits + iHeal;
					}

					Misc.Titles.AwardKarma( from, -50, true );
				}
				else
				{
					from.SendMessage( "Você não está com fome o suficiente para comer o " + HeartName + "." );
					from.Hunger = 20;
				}
			}
		}

		public TastyHeart(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
            writer.Write( HeartName );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
            HeartName = reader.ReadString();
		}
	}
}