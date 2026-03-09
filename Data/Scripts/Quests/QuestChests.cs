using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Network;
using Server.Mobiles;
using Server.Misc;
using Server.Commands;
using Server.Commands.Generic;

namespace Server.Items
{
	public class BardHarkynBox : Item
	{
		[Constructable]
		public BardHarkynBox() : base( 0x9AB )
		{
			Name = "Harkyn's Treasure Chest";
			Movable = false;
			Hue = 0x556;
		}

		public BardHarkynBox( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				PlayerMobile pm = (PlayerMobile)from;

				if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleSilverSquare" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você não encontra nada de interesse.", from.NetState);
				}
				else
				{
					PlayerSettings.SetBardsTaleQuest( from, "BardsTaleSilverSquare", true );
					PlayerSettings.SetBardsTaleQuest( from, "BardsTaleCrystalSword", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você encontrou um quadrado de prata e uma espada de cristal.", from.NetState);
					from.CloseGump( typeof(Server.Gumps.ClueGump) );
					from.SendGump(new Server.Gumps.ClueGump( from, "Você encontrou um quadrado de prata e uma espada de cristal. Você precisará das três formas de prata para obter a chave de prata para a porta da câmara de Mangar. Quando tiver as três, procure a caveira dourada e use-a para posicionar as formas sobre ela. A espada parece forte o suficiente para quebrar esculturas de cristal.", "O Quadrado de Prata" ) );
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class BardKylearanBox : Item
	{
		[Constructable]
		public BardKylearanBox() : base( 0xE40 )
		{
			Name = "Kylearan's Treasure Chest";
			Movable = false;
			Hue = 0x48E;
		}

		public BardKylearanBox( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleSilverTriangle" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você não encontra nada de interesse.", from.NetState);
				}
				else
				{
					PlayerSettings.SetBardsTaleQuest( from, "BardsTaleSilverTriangle", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você encontrou um triângulo de prata.", from.NetState);
					from.CloseGump( typeof(Server.Gumps.ClueGump) );
					from.SendGump(new Server.Gumps.ClueGump( from, "Você encontrou um triângulo de prata. Você precisará das três formas de prata para obter a chave de prata para a porta da câmara de Mangar. Quando tiver as três, procure a caveira dourada e use-a para posicionar as formas sobre ela.", "O Triângulo de Prata" ) );
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class BardMangarBox : Item
	{
		[Constructable]
		public BardMangarBox() : base( 0xE40 )
		{
			Name = "Mangar's Treasure Chest";
			Movable = false;
			Hue = 0x489;
		}

		public BardMangarBox( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				PlayerMobile pm = (PlayerMobile)from;

				if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleSilverCircle" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você não encontra nada de interesse.", from.NetState);
				}
				else
				{
					PlayerSettings.SetBardsTaleQuest( from, "BardsTaleSilverCircle", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você encontrou um círculo de prata.", from.NetState);
					from.CloseGump( typeof(Server.Gumps.ClueGump) );
					from.SendGump(new Server.Gumps.ClueGump( from, "Você encontrou um círculo de prata. Você precisará das três formas de prata para obter a chave de prata para a porta da câmara de Mangar. Quando tiver as três, procure a caveira dourada e use-a para posicionar as formas sobre ela.", "O Círculo de Prata" ) );
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class BardGoldSkull : Item
	{
		[Constructable]
		public BardGoldSkull() : base( 0x2203 )
		{
			Name = "a golden skull";
			Movable = false;
			Hue = 1281;
		}

		public BardGoldSkull( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				PlayerMobile pm = (PlayerMobile)from;

				if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleMangarKey" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Esta caveira dourada tem um brilho sinistro.", from.NetState);
				}
				else if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleSilverSquare" ) && 
					PlayerSettings.GetBardsTaleQuest( from, "BardsTaleSilverTriangle" ) && 
					PlayerSettings.GetBardsTaleQuest( from, "BardsTaleSilverCircle" ) )
				{
					PlayerSettings.SetBardsTaleQuest( from, "BardsTaleMangarKey", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Ao colocar as 3 formas de prata na caveira, a boca se abre revelando uma chave de prata.", from.NetState);
					from.CloseGump( typeof(Server.Gumps.ClueGump) );
					from.SendGump(new Server.Gumps.ClueGump( from, "Você obteve a chave de prata da caveira dourada. Talvez ela funcione naquela porta escura a oeste.", "A Chave de Prata" ) );
				}
				else
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Esta caveira dourada tem um brilho sinistro, e parece haver 3 formas diferentes entalhadas nela.", from.NetState);
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class CrystalStatueBoxKyl : Item
	{
		[Constructable]
		public CrystalStatueBoxKyl() : base( 0xE80 )
		{
			Name = "jade box";
			Movable = false;
			Hue = 0xB95;
			ItemRemovalTimer thisTimer = new ItemRemovalTimer( this ); 
			thisTimer.Start(); 
		}

		public CrystalStatueBoxKyl( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleBedroomKey" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você não encontra nada de interesse.", from.NetState);
				}
				else
				{
					PlayerSettings.SetBardsTaleQuest( from, "BardsTaleBedroomKey", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você encontrou uma chave com um símbolo de árvore.", from.NetState);
					from.CloseGump( typeof(Server.Gumps.ClueGump) );
					from.SendGump(new Server.Gumps.ClueGump( from, "Você encontrou uma chave com um símbolo de árvore.", "A Chave da Floresta" ) );
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			CrystalStatueKyl MyStatue = new CrystalStatueKyl();
			Point3D loc = new Point3D( 5840, 2337, 0 );
			MyStatue.MoveToWorld( loc, this.Map );
			this.Delete();
		}

		public class ItemRemovalTimer : Timer 
		{ 
			private Item i_item; 
			public ItemRemovalTimer( Item item ) : base( TimeSpan.FromMinutes( 10.0 ) ) 
			{ 
				Priority = TimerPriority.OneSecond; 
				i_item = item; 
			} 

			protected override void OnTick() 
			{ 
				if (( i_item != null ) && ( !i_item.Deleted ))
				{
					CrystalStatueKyl MyStatue = new CrystalStatueKyl();
					Point3D loc = new Point3D( 5840, 2337, 0 );
					MyStatue.MoveToWorld( loc, i_item.Map );
					i_item.Delete();
				}
			} 
		} 
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class CrystalStatueKyl : Item
	{
		[Constructable]
		public CrystalStatueKyl() : base( 0x40BC )
		{
			Name = "a crystal statue";
			Movable = false;
			Hue = 0x480;
			Weight = -2;
		}

		public CrystalStatueKyl( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleBedroomKey" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Esta estátua de cristal parece linda.", from.NetState);
				}
				else if ( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleCrystalSword" ) && !( PlayerSettings.GetBardsTaleQuest( from, "BardsTaleBedroomKey" ) ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você golpeia a estátua com a espada de cristal, estilhaçando ambas.", from.NetState);
					from.PlaySound( 0x040 );

					CrystalStatueBoxKyl MyChest = new CrystalStatueBoxKyl();

					Map map = this.Map;

					bool validLocation = false;
					Point3D loc = this.Location;

					for ( int j = 0; !validLocation && j < 10; ++j )
					{
						int x = from.X + Utility.Random( 3 ) - 1;
						int y = from.Y + Utility.Random( 3 ) - 1;
						int z = map.GetAverageZ( x, y );

						if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
							loc = new Point3D( x, y, Z );
						else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
							loc = new Point3D( x, y, z );
					}

					MyChest.MoveToWorld( loc, map );

					this.Delete();
				}
				else
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você consegue ver uma pequena caixa de jade dentro da estátua de cristal.", from.NetState);
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class DwarvenBox : Item
	{
		[Constructable]
		public DwarvenBox() : base( 0x2DF1 )
		{
			Name = "Ancient Dwarven Chest";
			Movable = false;
		}

		public DwarvenBox( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				if ( PlayerSettings.GetKeys( from, "UndermountainKey" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você não encontra nada de interesse.", from.NetState);
				}
				else
				{
					PlayerSettings.SetKeys( from, "UndermountainKey", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você encontrou uma chave anã.", from.NetState);
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class SkullGateBook : Item
	{
		[Constructable]
		public SkullGateBook() : base( 0x2B6F )
		{
			Name = "Manual of Skull Gate";
			Movable = false;
			Hue = 0x9C4;
		}

		public SkullGateBook( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				if ( from is PlayerMobile && Server.Items.BaseRace.IsEvilDeadCreature( from ) && !PlayerSettings.GetDiscovered( from, "the Land of Sosaria" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Este livro faria mais sentido se você encontrasse um caminho para Sosaria.", from.NetState);
				}
				else if ( PlayerSettings.GetKeys( from, "SkullGate" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você já aprendeu os segredos deste livro.", from.NetState);
				}
				else
				{
					PlayerSettings.SetKeys( from, "SkullGate", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você aprendeu os segredos do Portão da Caveira.", from.NetState);
				}
				from.SendMessage( "Verifique seu registro de missões para detalhes sobre as localizações." );
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class SerpentPillarBook : Item
	{
		[Constructable]
		public SerpentPillarBook() : base( 0x5689 )
		{
			Name = "The Serpent Pillars";
			Movable = false;
			Hue = 0xB20;
		}

		public SerpentPillarBook( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				if ( PlayerSettings.GetKeys( from, "SerpentPillars" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você já aprendeu os segredos deste livro.", from.NetState);
				}
				else
				{
					PlayerSettings.SetKeys( from, "SerpentPillars", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você aprendeu os segredos dos Pilares da Serpente.", from.NetState);
				}
				from.SendMessage( "Verifique seu registro de missões para detalhes sobre as localizações." );
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class DragonRidingScroll : Item
	{
		[Constructable]
		public DragonRidingScroll() : base( 0x02DD )
		{
			Name = "The Dragon Riders";
		}

		public DragonRidingScroll( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				if ( PlayerSettings.GetKeys( from, "DragonRiding" ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você já aprendeu estes segredos então você joga fora.", from.NetState);
					this.Delete();
				}
				else
				{
					PlayerSettings.SetKeys( from, "DragonRiding", true );
					from.SendSound( 0x3D );
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você aprendeu os segredos de como montar dragões.", from.NetState);
					this.Delete();
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}