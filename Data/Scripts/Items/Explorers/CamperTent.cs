using System;
using Server;
using System.Collections.Generic;
using System.Collections;
using Server.Mobiles;
using Server.Items;
using Server.Regions;
using Server.Network;
using Server.Multis;
using Server.Misc;
using Server.ContextMenus;
using Server.Gumps;
using Server.Commands;

namespace Server.Items 
{
	public enum CamperTentEffect
	{
		Charges
	}

	public class CampersTent : Item
	{
		public override string DefaultDescription{ get{ return "Esta é uma barraca de acampamento que você pode usar para escapar dos perigos da terra e descansar. Você só pode usar essas barracas se tiver pelo menos 40 na habilidade de acampamento, e elas eventualmente se desgastam com o uso. Se clicar duas vezes na barraca enquanto ela estiver em sua mochila, você montará a barraca para si mesmo. Ninguém poderá segui-lo para dentro da barraca a menos que tenha uma barraca e a habilidade apropriada. Se você colocar a barraca no chão e clicar duas vezes nela, então outros poderão usar a barraca para descansar, pois poderão clicar duas vezes na barraca para segui-lo para dentro. A barraca enrolada original será colocada de volta em sua mochila, enquanto a barraca montada será deixada para trás e permanecerá por apenas cerca de 30 segundos, então seus companheiros devem se apressar e segui-lo para dentro. Se alguém quiser sair da barraca, basta clicar duas vezes na entrada por onde entrou. Qualquer um pode ficar na barraca o tempo que quiser, mas retornará ao local onde usou a barraca quando sair."; } }

		private CamperTentEffect m_CamperTentEffect;
		private int m_Charges;

		[CommandProperty( AccessLevel.GameMaster )]
		public CamperTentEffect Effect
		{
			get{ return m_CamperTentEffect; }
			set{ m_CamperTentEffect = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get{ return m_Charges; }
			set{ m_Charges = value; InvalidateProperties(); }
		}

		[Constructable]
		public CampersTent() : base( 0x0A59 )
		{
			Name = "barraca de acampamento";
			Weight = 5.0; 
			Charges = 10;
			Hue = Utility.RandomList( 0x96D, 0x96E, 0x96F, 0x970, 0x971, 0x972, 0x973, 0x974, 0x975, 0x976, 0x977, 0x978, 0x979, 0x97A, 0x97B, 0x97C, 0x97D, 0x97E );		
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Monte uma barraca segura onde descansar");
			list.Add( 1049644, "Usável por quem tem habilidade em acampamento");
        }

		public override void OnDoubleClick( Mobile from )
		{
			bool inCombat = ( from.Combatant != null && from.InRange( from.Combatant.Location, 20 ) && from.Combatant.InLOS( from ) );

			int CanUseTent = 0;

			if ( from.Skills[SkillName.Camping].Value < 40 )
			{
				from.SendMessage( "Você deve ser um explorador novato para usar esta barraca." );
				return;
			}
			else if ( from.Region.IsPartOf( typeof( PublicRegion ) ) )
			{
				from.SendMessage( "Esta é uma barraca de acampamento muito bonita." ); 
				return;
			}
			else if ( Server.Misc.Worlds.IsOnBoat( from ) )
			{
				from.SendMessage( "Você não pode montar esta barraca perto de um barco." );
				return;
			}
			else if ( inCombat )
			{
				from.SendMessage( "Você não pode montar uma barraca enquanto está em combate." );
				return;
			}
			else if ( ( from.Region.IsPartOf( typeof( BardDungeonRegion ) ) || from.Region.IsPartOf( typeof( DungeonRegion ) ) ) && from.Skills[SkillName.Camping].Value >= 90 )
			{
				CanUseTent = 1;
			}
			else if (	from.Skills[SkillName.Camping].Value < 90 && 
						!Server.Misc.Worlds.IsMainRegion( Server.Misc.Worlds.GetRegionName( from.Map, from.Location ) ) && 
						!from.Region.IsPartOf( typeof( OutDoorRegion ) ) && 
						!from.Region.IsPartOf( typeof( OutDoorBadRegion ) ) && 
						!from.Region.IsPartOf( typeof( VillageRegion ) ) )
			{
				from.SendMessage( "Você está apenas habilidoso o suficiente para usar esta barraca ao ar livre." ); 
				return;
			}
			else if (	from.Skills[SkillName.Camping].Value >= 90 && 
						!from.Region.IsPartOf( typeof( DungeonRegion ) ) && 
						!from.Region.IsPartOf( typeof( BardDungeonRegion ) ) && 
						!Server.Misc.Worlds.IsMainRegion( Server.Misc.Worlds.GetRegionName( from.Map, from.Location ) ) && 
						!from.Region.IsPartOf( typeof( OutDoorRegion ) ) && 
						!from.Region.IsPartOf( typeof( OutDoorBadRegion ) ) && 
						!from.Region.IsPartOf( typeof( VillageRegion ) ) )
			{
				from.SendMessage( "Você só pode usar esta barraca ao ar livre ou em masmorras." ); 
				return;
			}
			else
			{
				CanUseTent = 1;
			}

			if ( CanUseTent > 0 && from.CheckSkill( SkillName.Camping, 0.0, 125.0 ) )
			{
				if ( IsChildOf( from.Backpack ) && Charges > 0 ) 
				{
					Server.Items.Kindling.RaiseCamping( from );
					ConsumeCharge( from );

					PlayerMobile pc = (PlayerMobile)from;
					string sX = from.X.ToString();
					string sY = from.Y.ToString();
					string sZ = from.Z.ToString();
					string sMap = Worlds.GetMyMapString( from.Map );
					string sZone = "the Camping Tent";
						if ( from.Region.IsPartOf( typeof( DungeonRegion ) ) || from.Region.IsPartOf( typeof( BardDungeonRegion ) ) ){ sZone = "the Dungeon Room"; }

					string doors = sX + "#" + sY + "#" + sZ + "#" + sMap + "#" + sZone;

					((PlayerMobile)from).CharacterPublicDoor = doors;

					Point3D loc = new Point3D( 3710, 3971, 0 );
						if ( from.Region.IsPartOf( typeof( DungeonRegion ) ) ){ loc = new Point3D( 3687, 3333, 0 ); }
						else if ( from.Region.IsPartOf( typeof( BardDungeonRegion ) ) ){ loc = new Point3D( 3687, 3333, 0 ); }
						else if ( from.Skills[SkillName.Camping].Value > 66 ){ loc = new Point3D( 3792, 3967, 0 ); }

					TentTeleport( from, loc, Map.Sosaria, 0x057, sZone, "enter" );
					return;
				}
				else if ( from.InRange( this.GetWorldLocation(), 3 ) && Charges > 0 )
				{
					Server.Items.Kindling.RaiseCamping( from );
					ConsumeCharge( from );

					PlayerMobile pc = (PlayerMobile)from;
					string sX = from.X.ToString();
					string sY = from.Y.ToString();
					string sZ = from.Z.ToString();
					string sMap = Worlds.GetMyMapString( from.Map );
					string sZone = "the Camping Tent";
						if ( from.Region.IsPartOf( typeof( DungeonRegion ) ) || from.Region.IsPartOf( typeof( BardDungeonRegion ) ) ){ sZone = "the Dungeon Room"; }

					string doors = sX + "#" + sY + "#" + sZ + "#" + sMap + "#" + sZone;

					((PlayerMobile)from).CharacterPublicDoor = doors;

					Point3D loc = new Point3D( 3710, 3971, 0 );
						if ( from.Region.IsPartOf( typeof( DungeonRegion ) ) ){ loc = new Point3D( 3687, 3333, 0 ); }
						else if ( from.Region.IsPartOf( typeof( BardDungeonRegion ) ) ){ loc = new Point3D( 3687, 3333, 0 ); }
						else if ( from.Skills[SkillName.Camping].Value > 66 ){ loc = new Point3D( 3792, 3967, 0 ); }

					InternalItem builtTent = new InternalItem();
					builtTent.Name = "camping tent";
					ThruDoor publicTent = (ThruDoor)builtTent;
					publicTent.m_PointDest = loc;
					publicTent.m_MapDest = Map.Sosaria;
					builtTent.MoveToWorld( this.Location, this.Map );
					from.AddToBackpack( this );

					TentTeleport( from, loc, Map.Sosaria, 0x057, sZone, "enter" );
					return;
				}
				else if ( !from.InRange( this.GetWorldLocation(), 3 ) && Charges > 0 )
				{
					from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
					return;
				}
				else
				{
					from.SendMessage( "Esta barraca está muito desgastada pelo uso excessivo e não serve mais para nada." );
					this.Delete();
					return;
				}
			}
			else if ( CanUseTent > 0 )
			{
				from.SendMessage( "Sua barraca está um pouco mais desgastada por você falhar ao montá-la corretamente." );
				Server.Items.Kindling.RaiseCamping( from );
				ConsumeCharge( from );

				if ( Charges < 1 )
				{
					from.SendMessage( "Esta barraca está muito desgastada pelo uso excessivo e não serve mais para nada." );
					this.Delete();
					return;
				}

				return;
			}
		}

		public static void TentTeleport( Mobile m, Point3D loc, Map map, int sound, string zone, string direction )
		{
			BaseCreature.TeleportPets( m, loc, map, false );
			m.MoveToWorld ( loc, map );
			m.PlaySound( sound );
			LoggingFunctions.LogRegions( m, zone, direction );
		}

		public void ConsumeCharge( Mobile from )
		{
			--Charges;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060584, "{0}\t{1}", m_Charges.ToString(), "Uses" );
		}

		private class InternalItem : ThruDoor
		{
			public InternalItem()
			{
				ItemID = 0x2795;
				InternalTimer t = new InternalTimer( this );
				t.Start();
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				Delete();
			}

			private class InternalTimer : Timer
			{
				private Item m_Item;

				public InternalTimer( Item item ) : base( TimeSpan.FromSeconds( 30.0 ) )
				{
					Priority = TimerPriority.OneSecond;
					m_Item = item;
				}

				protected override void OnTick()
				{
					m_Item.Delete();
				}
			}
		}

		public CampersTent( Serial serial ) : base( serial )
		{ 
		} 
		
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 );
			writer.Write( (int) m_CamperTentEffect );
			writer.Write( (int) m_Charges );
		} 
		
		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
					m_CamperTentEffect = (CamperTentEffect)reader.ReadInt();
					m_Charges = (int)reader.ReadInt();
					break;
				}
			}
		}
	}
}
