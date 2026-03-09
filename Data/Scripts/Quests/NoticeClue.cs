using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class NoticeClue : Item
	{
		public override bool HandlesOnMovement{ get{ return true; } }

		private DateTime m_NextTalk;
		public DateTime NextTalk{ get{ return m_NextTalk; } set{ m_NextTalk = value; } }

		public override void OnMovement( Mobile from, Point3D oldLocation )
		{
			if( from is PlayerMobile )
			{
				if ( DateTime.Now >= m_NextTalk && Utility.InRange( from.Location, this.Location, 5 ) )
				{
					from.PrivateOverheadMessage(MessageType.Regular, 1150, false, this.Name, from.NetState);

					if ( this.X == 5764 && this.Y == 2215 )
					{
						from.CloseGump( typeof(Server.Gumps.ClueGump) );
						from.SendGump(new Server.Gumps.ClueGump( from, "Parece uma frase estranha. Talvez eu deva lembrar o nome que alguns dão a um rubi.", "A Pedra de Sangue" ) );
					}
					else if ( this.X == 6268 && this.Y == 2661 )
					{
						from.CloseGump( typeof(Server.Gumps.ClueGump) );
						from.SendGump(new Server.Gumps.ClueGump( from, "Que altares Harkyn estabeleceu? Que nome deve ser pronunciado?", "Os Altares de Harkyn" ) );
					}
					else if ( this.X == 6293 && this.Y == 1649 )
					{
						from.CloseGump( typeof(Server.Gumps.ClueGump) );
						from.SendGump(new Server.Gumps.ClueGump( from, "O portão de esmeralda? Talvez um portão mágico verde? Se eu pronunciar o nome do rubi perto dele, talvez eu consiga entrar.", "O Portão de Esmeralda" ) );
					}
					else if ( this.X == 6497 && this.Y == 1440 )
					{
						from.CloseGump( typeof(Server.Gumps.ClueGump) );
						from.SendGump(new Server.Gumps.ClueGump( from, "As três formas, de prata elas são, podem fazer a caveira dourada falar? Talvez eu deva encontrar essas coisas, mas onde?", "As Formas de Prata" ) );
					}
					else if ( this.X == 6501 && this.Y == 1773 )
					{
						from.CloseGump( typeof(Server.Gumps.ClueGump) );
						from.SendGump(new Server.Gumps.ClueGump( from, "Saiba disso, que um homem chamado Tarjan, considerado louco por muitos, através de poderes mágicos se proclamou um deus em Skara Brae há cem anos. Talvez aquele culto na cidade saiba disso.", "O Deus Louco" ) );
					}
					else if ( this.X == 6988 && this.Y == 164 )
					{
						from.CloseGump( typeof(Server.Gumps.ClueGump) );
						from.SendGump(new Server.Gumps.ClueGump( from, "Você já pode sentir a energia mágica que está selando esta porta. Talvez haja outra maneira de entrar neste lugar vil.", "A Porta da Torre de Mangar" ) );
					}

					m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( 30 ));
				}
			}
		}

		[Constructable]
		public NoticeClue( ) : base( 0x181E )
		{
			Movable = false;
			Visible = false;
			Name = "clue";
		}

		public NoticeClue( Serial serial ) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}	
}