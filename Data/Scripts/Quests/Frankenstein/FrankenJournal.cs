using System;
using Server;
using Server.Network;
using Server.Multis;
using Server.Gumps;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Items
{
	public class FrankenJournal : Item
	{
		public Mobile JournalOwner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Journal_Owner { get{ return JournalOwner; } set{ JournalOwner = value; } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasHead;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Head { get { return HasHead; } set { HasHead = value; InvalidateProperties(); } }

		public int HasTorso;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Torso { get { return HasTorso; } set { HasTorso = value; InvalidateProperties(); } }

		public int HasBrain;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Brain { get { return HasBrain; } set { HasBrain = value; InvalidateProperties(); } }

		public int HasArmLeft;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_ArmLeft { get { return HasArmLeft; } set { HasArmLeft = value; InvalidateProperties(); } }

		public int HasArmRight;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_ArmRight { get { return HasArmRight; } set { HasArmRight = value; InvalidateProperties(); } }

		public int HasLegLeft;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_LegLeft { get { return HasLegLeft; } set { HasLegLeft = value; InvalidateProperties(); } }

		public int HasLegRight;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_LegRight { get { return HasLegRight; } set { HasLegRight = value; InvalidateProperties(); } }

		public string BrainFrom;
		[CommandProperty(AccessLevel.Owner)]
		public string Brain_From { get { return BrainFrom; } set { BrainFrom = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		[Constructable]
		public FrankenJournal() : base( 0x1A97 )
		{
			Weight = 1.0;
			Hue = 0xB51;
			Name = "Frankenstein's Journal";
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( JournalOwner != null ){ list.Add( 1049644, "Now Belongs to " + JournalOwner.Name + "" ); }
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else if ( JournalOwner != from )
			{
				from.SendMessage( "This journal does not belong to you toss it out!" );
				bool remove = true;
				foreach ( Account a in Accounts.GetAccounts() )
				{
					if (a == null)
						break;

					int index = 0;

					for (int i = 0; i < a.Length; ++i)
					{
						Mobile m = a[i];

						if (m == null)
							continue;

						if ( m == JournalOwner )
						{
							m.AddToBackpack( this );
							remove = false;
						}

						++index;
					}
				}
				if ( remove )
				{
					this.Delete();
				}
			}
			else
			{
				from.SendSound( 0x55 );
				from.CloseGump( typeof( FrankenGump ) );
				from.SendGump( new FrankenGump( this, from ) );
			}
		}

		public FrankenJournal(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
			writer.Write( (Mobile)JournalOwner);
			writer.Write( HasHead );
			writer.Write( HasTorso );
			writer.Write( HasBrain );
			writer.Write( HasArmLeft );
			writer.Write( HasArmRight );
			writer.Write( HasLegLeft );
			writer.Write( HasLegRight );
			writer.Write( BrainFrom );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			JournalOwner = reader.ReadMobile();
			HasHead = reader.ReadInt();
			HasTorso = reader.ReadInt();
			HasBrain = reader.ReadInt();
			HasArmLeft = reader.ReadInt();
			HasArmRight = reader.ReadInt();
			HasLegLeft = reader.ReadInt();
			HasLegRight = reader.ReadInt();
			BrainFrom = reader.ReadString();
		}

		private class FrankenGump : Gump
		{
			private FrankenJournal m_Journal;
			private Mobile m_From;

			public FrankenGump( FrankenJournal book, Mobile from ) : base( 50, 50 )
			{
				string color = "#edad9c";
				m_Journal = book;
				m_From = from;

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 7017, Server.Misc.PlayerSettings.GetGumpHue( from ));

				AddHtml( 12, 12, 420, 20, @"<BODY><BASEFONT Color=" + color + ">DIÁRIO DE FRANKENSTEIN</BASEFONT></BODY>", (bool)false, (bool)false);

				AddButton(563, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

				AddHtml( 14, 44, 575, 360, @"<BODY><BASEFONT Color=" + color + ">Este livro contém os escritos do Doutor Victor Frankenstein, um notável alquimista e especialista forense. Dentro destas páginas, estão os segredos para reanimar uma criatura que pode servir aos seus propósitos. Onde a maioria só conseguiu criar criaturas de tamanho humano, este tomo explica como criar uma criatura de grande poder. Para isso, seria necessário ser pelo menos um necrotério novato. Enquanto carrega este livro e usa um item cortante para esfolar criaturas, você deve encontrar cadáveres de gigantes para obter as partes do corpo necessárias para a construção de tal criatura. Gigantes são criaturas como ogros, ettins e ciclopes. Essas partes do corpo podem ser difíceis de separar da criatura, então você pode ter que matar muitos para coletar o que precisa. Se você conseguir partes do corpo que não precisa, talvez o agente funerário na Guilda da Magia Negra as adquira de você.<br><br>À medida que coletar partes individuais separadas, clique duas vezes nelas e direcione este diário para adicioná-las ao seu experimento. Você só pode ter uma de cada parte do corpo para este experimento: um torso, cabeça, braço esquerdo, braço direito, perna esquerda e perna direita. Você também precisará de um cérebro de um gigante, e quanto mais poderoso, melhor. Um cérebro de um gigante da tempestade dará à sua criação mais poder do que o cérebro de um ogro estúpido. Uma vez que você tenha um cérebro, adicione-o ao seu experimento da mesma maneira. Diferente de outras partes do corpo, você pode adicionar um cérebro diferente mais tarde, antes de executar o experimento final. Sempre que você adicionar um cérebro diferente, jogará o antigo fora.<br><br>Uma vez que você tenha tudo o que precisa, então precisa encontrar uma bobina de energia que possa gerar energia elétrica suficiente para reanimar o cadáver. O agente funerário de quem escrevi anteriormente tem uma em seu laboratório, mas ele também venderá para você uma bem ajustada para colocar em sua casa. Quando você estiver perto o suficiente de uma bobina de energia, então selecione o tipo de criatura que deseja reanimar. Você tem a escolha de um guerreiro reanimado ou um escravo para carregar seus itens para você. O guerreiro lutará sob seu comando, enquanto o outro carregará seus itens e outras criaturas parecem deixá-lo em paz.<br><br>Um item aparecerá em sua mochila que permitirá que você invoque a criatura. Uma vez invocada, o item desaparecerá até que você liberte a criatura e então o item reaparecerá em sua mochila. Se a criatura guerreira morrer em batalha, o item também aparecerá em sua mochila. Para invocar sua reanimação, você precisará de fluido de embalsamamento para evitar que ela apodreça. Agentes funerários os vendem a preços altos, mas alguém bom em forense pode às vezes encontrá-los nos cadáveres de outras reanimações, zumbis ou múmias. Se você conseguir algum fluido de embalsamamento, simplesmente use-o no item da reanimação em sua mochila para adicionar o preservativo.</BASEFONT></BODY>", (bool)false, (bool)true);

				int bodyParts = 0;

				if ( book.HasBrain > 0 ){ bodyParts = 1; }

				bodyParts = bodyParts + book.HasTorso + book.HasHead + book.HasArmLeft + book.HasArmRight + book.HasLegLeft + book.HasLegRight;

				int v = 35;

				if ( book.HasBrain > 0 )
				{
					AddItem(12, 430, 9698);
					AddHtml( 55, 430, 261, 20, @"<BODY><BASEFONT Color=" + color + ">De " + book.BrainFrom + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 55, 460, 261, 20, @"<BODY><BASEFONT Color=" + color + ">Cérebro Nível " + book.HasBrain + "</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				if ( book.HasArmRight > 0 ){ AddItem(449, 417+v, 14988); } // BRAÇO DIREITO
				if ( book.HasArmLeft > 0 ){ AddItem(547, 417+v, 14991); } // BRAÇO ESQUERDO
				if ( book.HasLegRight > 0 ){ AddItem(471, 467+v, 16025); } // PERNA DIREITA
				if ( book.HasLegLeft > 0 ){ AddItem(522, 466+v, 16002); } // PERNA ESQUERDA
				if ( book.HasTorso > 0 ){ AddItem(491, 415+v, 15003); } // TORSO
				if ( book.HasHead > 0 ){ AddItem(504, 399+v, 15873); } // CABEÇA

				if ( bodyParts > 6 )
				{
					AddButton(12, 535, 4005, 4005, 1, GumpButtonType.Reply, 0);
					AddHtml( 55, 535, 261, 20, @"<BODY><BASEFONT Color=" + color + ">Reanimar um Escravo</BASEFONT></BODY>", (bool)false, (bool)false);

					AddButton(12, 565, 4005, 4005, 2, GumpButtonType.Reply, 0);
					AddHtml( 55, 565, 261, 20, @"<BODY><BASEFONT Color=" + color + ">Reanimar um Protetor</BASEFONT></BODY>", (bool)false, (bool)false);
				}
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( info.ButtonID > 0 )
				{
					Point3D loc = m_From.Location;
					Map map = m_From.Map;

					bool nearCoil = false;
					foreach ( Item coil in m_From.GetItemsInRange( 10 ) )
					{
						if ( coil is PowerCoil )
						{
							nearCoil = true;
							loc = new Point3D(coil.X, coil.Y, (coil.Z+20));
						}
					}

					if ( nearCoil )
					{
						int Fighter = info.ButtonID-1;

						FrankenPorterItem flesh = new FrankenPorterItem();

						string QuestLog = "has reanimated a flesh golem";

						flesh.PorterOwner = m_From.Serial;
						flesh.PorterLevel = m_Journal.HasBrain;
						flesh.PorterType = Fighter;

						m_From.AddToBackpack ( flesh );

						Server.Misc.LoggingFunctions.LogGenericQuest( m_From, QuestLog );

						m_From.PrivateOverheadMessage(MessageType.Regular, 1153, false, "My experiment is a success.", m_From.NetState);

						int sound = Utility.RandomList( 0x028, 0x029 );
						Effects.SendLocationEffect( loc, map, 0x2A4E, 30, 10, 0, 0 );
						m_From.PlaySound( sound );

						m_Journal.Delete();
					}
					else
					{
						m_From.SendMessage("You need to be near a power coil to do that.");
						m_From.SendSound( 0x55 );
					}
				}
				else
				{
					m_From.SendSound( 0x55 );
					m_From.CloseGump( typeof( FrankenGump ) );
				}
			}
		}
	}
}