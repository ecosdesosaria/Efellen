using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class ClockworkAssembly : Item
	{
		public override string DefaultName
		{
			get { return "clockwork assembly"; }
		}

		[Constructable]
		public ClockworkAssembly() : base( 0x1EA8 )
		{
			Weight = 5.0;
			Hue = 1102;
		}

		public ClockworkAssembly( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				return;
			}

			double tinkerSkill = from.Skills[SkillName.Tinkering].Value;

			if ( tinkerSkill < 60.0 )
			{
				from.SendMessage( "Você precisa ter pelo menos 60.0 de habilidade em tinkering para construir um golem." );
				return;
			}
			else if ( (from.Followers + 4) > from.FollowersMax )
			{
				from.SendLocalizedMessage( 1049607 ); // You have too many followers to control that creature.
				return;
			}

			double scalar;

			if ( tinkerSkill >= 100.0 )
				scalar = 1.0;
			else if ( tinkerSkill >= 90.0 )
				scalar = 0.9;
			else if ( tinkerSkill >= 80.0 )
				scalar = 0.8;
			else if ( tinkerSkill >= 70.0 )
				scalar = 0.7;
			else
				scalar = 0.6;

			Container pack = from.Backpack;

			if ( pack == null )
				return;

			int res = pack.ConsumeTotal(
				new Type[]
				{
					typeof( PowerCrystal ),
					typeof( IronIngot ),
					typeof( BronzeIngot ),
					typeof( Gears )
				},
				new int[]
				{
					1,
					50,
					50,
					5
				} );

			switch ( res )
			{
				case 0:
				{
					from.SendMessage( "Você deve ter um cristal de energia para construir o golem." );
					break;
				}
				case 1:
				{
					from.SendMessage( "Você deve ter 50 lingotes de ferro para construir o golem." );
					break;
				}
				case 2:
				{
					from.SendMessage( "Você deve ter 50 lingotes de bronze para construir o golem." );
					break;
				}
				case 3:
				{
					from.SendMessage( "Você deve ter 5 engrenagens para construir o golem." );
					break;
				}
				default:
				{
					Golem g = new Golem( true, scalar );

					if ( g.SetControlMaster( from ) )
					{
						Delete();

						g.MoveToWorld( from.Location, from.Map );
						from.PlaySound( 0x241 );
					}

					break;
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}