using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class ChucklesJester : BasePerson
	{
		private DateTime m_NextTalk;
		public DateTime NextTalk{ get{ return m_NextTalk; } set{ m_NextTalk = value; } }
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( m is PlayerMobile )
			{
				if ( DateTime.Now >= m_NextTalk && InRange( m, 4 ) && InLOS( m ) )
				{
					DoJokes( this );
					m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( 30 ));
				}
			}
		}

		public override string TalkGumpTitle{ get{ return "Você Certamente Brinca"; } }
		public override string TalkGumpSubject{ get{ return "Bobo da Corte"; } }

		public static void DoJokes( Mobile m )
		{
			int act = Utility.Random( 28 );
			if ( m is PlayerMobile ){ act = Utility.Random( 22 ); }
			switch ( act )
			{
				case 0: m.Say("Por que o rei foi ao dentista? Para coroar seus dentes."); break;
				case 1: m.Say("Quando um cavaleiro de armadura foi morto em batalha, que placa colocaram em seu túmulo? Ferrugem em paz!"); break;
				case 2: m.Say("Como você chama um mosquito em uma armadura de lata? Uma mordida em armadura brilhante."); break;
				case 3: m.Say("Há muitos castelos no mundo, mas quem é forte o suficiente para mover um? Qualquer jogador de xadrez"); break;
				case 4: m.Say("Que rei era famoso porque passou tantas noites em sua Mesa Redonda escrevendo livros? Rei Artur!"); break;
				case 5: m.Say("Como você encontra uma princesa? Você segue o príncipe pé."); break;
				case 6: m.Say("Por que os primeiros dias eram chamados de idade das trevas? Porque havia tantos cavaleiros!"); break;
				case 7: m.Say("Por que Artur tinha uma mesa redonda? Para que ninguém pudesse encurralá-lo!"); break;
				case 8: m.Say("Quem inventou a mesa redonda do Rei Artur? Sir Cumferência!"); break;
				case 9: m.Say("Por que o cavaleiro correu gritando por um abridor de latas? Ele tinha uma abelha em sua armadura!"); break;
				case 10: m.Say("Pelo que Camelot era famosa? Pela sua vida de cavaleiro!"); break;
				case 11: m.Say("O que o sapo disse quando a princesa não quis beijá-lo? O que há de errado com você?"); break;
				case 12: m.Say("Como você chama o jovem real que não para de cair? Príncipe Agalopante!"); break;
				case 13: m.Say("Como você chama um gato que voa sobre o muro do castelo? Um gato-pulta!"); break;
				case 14: m.Say("Que jogo os peixes jogam no fosso? Verdade ou truta!"); break;
				case 15: m.Say("O que o peixe disse para o outro quando o cavalo caiu no fosso? Cavalo marinho!"); break;
				case 16: m.Say("Como você chama uma princesa brava recém-acordada de um longo sono? Bela adormecida de porrete!"); break;
				case 17: m.Say("Como o príncipe entrou no castelo quando a ponte levadiça quebrou? Ele usou um remosso!"); break;
				case 18: m.Say("Como a dragão fêmea venceu o concurso de beleza? Ela foi a fera do show!"); break;
				case 19: m.Say("Por que o dinossauro viveu mais que o dragão? Porque não fumava!"); break;
				case 20: m.Say("O que o dragão disse quando viu o Cavaleiro? 'Não mais comida enlatada!'"); break;
				case 21: m.Say("O que você faz com um dragão verde? Espera até que ele amadureça!"); break;
				case 22: m.PlaySound( m.Female ? 780 : 1051 ); m.Say( "*aplaude*" ); break;
				case 23: m.Say( "*faz uma reverência*" ); m.Animate( 32, 5, 1, true, false, 0 ); break;
				case 24: m.PlaySound( m.Female ? 794 : 1066 ); m.Say( "*risadinha*" ); break;
				case 25: m.PlaySound( m.Female ? 801 : 1073 ); m.Say( "*risos*" ); break;
				case 26: m.PlaySound( 792 ); m.Say( "*mostra a língua*" ); break;
				case 27: m.PlaySound( m.Female ? 783 : 1054 ); m.Say( "*uuhuul!*" ); break;
			};

			if ( act < 22 && Utility.RandomBool() )
			{
				switch ( Utility.Random( 6 ))
				{
					case 0: m.PlaySound( m.Female ? 780 : 1051 ); break;
					case 1: m.Animate( 32, 5, 1, true, false, 0 ); break;
					case 2: m.PlaySound( m.Female ? 794 : 1066 ); break;
					case 3: m.PlaySound( m.Female ? 801 : 1073 ); break;
					case 4: m.PlaySound( 792 ); break;
					case 5: m.PlaySound( m.Female ? 783 : 1054 ); break;
				};
			}

		}

		[Constructable]
		public ChucklesJester() : base( )
		{
			SpeechHue = Utility.RandomTalkHue();
			NameHue = 1154;

			Body = 0x190;

			Name = "Chuckles";
			Title = "o Bobo da Corte";
			Hue = Utility.RandomSkinColor();

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetDamage( 15, 20 );
			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.FistFighting, 100 );
			Karma = 1000;
			VirtualArmor = 30;

			AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			AddItem( new Shoes( Utility.RandomNeutralHue() ) );
			AddItem( new JesterSuit( Utility.RandomNeutralHue() ) );
			AddItem( new JesterHat( Utility.RandomNeutralHue() ) );

			Utility.AssignRandomHair( this );
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is JokeBook )
			{
				if ( from.Blessed )
				{
					string sSay = "Não posso lidar com você enquanto você está nesse estado.";
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sSay, from.NetState);
					return false;
				}
				else if ( IntelligentAction.GetMyEnemies( from, this, false ) )
				{
					string sSay = "Acho que não devo aceitar isso de você.";
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sSay, from.NetState);
					return false;
				}
				else
				{
					if ( Utility.RandomBool() )
					{
						GiftJesterHat hat = new GiftJesterHat();
						hat.Name = "Chapéu de Bobo da Corte Mágico";
						hat.Hue = 0;
						hat.ItemID = Utility.RandomList( 0x171C, 0x4C15 );
						hat.m_Owner = from;
						hat.m_Gifter = "Chuckles o Bobo da Corte";
						hat.m_How = "Dado para";
						hat.m_Points = Utility.RandomMinMax( 80, 100 );

						from.AddToBackpack ( hat );
						from.SendMessage( "Chuckles lhe deu um de seus chapéus!" );
					}
					else
					{
						GiftFancyDress coat = new GiftFancyDress();
						coat.Name = "Traje de Bobo da Corte Mágico";
						coat.Hue = 0;
						coat.ItemID = Utility.RandomList( 0x1f9f, 0x1fa0, 0x4C16, 0x4C17, 0x2B6B );
						coat.m_Owner = from;
						coat.m_Gifter = "Chuckles o Bobo da Corte";
						coat.m_How = "Dado para";
						coat.m_Points = Utility.RandomMinMax( 80, 100 );

						from.AddToBackpack ( coat );
						from.SendMessage( "Chuckles lhe deu um de seus trajes!" );
					}
					this.Say( "Obrigado, " + from.Name + "! Estou sempre procurando por algumas piadas novas." );
					from.SendSound( 0x3D );
					dropped.Delete();
					from.SendMessage( "Clique uma vez nele para encantá-lo." );
					return true;
				}
			}
			else if ( dropped is Artifact_JesterHatofChuckles )
			{
				this.Say( "Obrigado, " + from.Name + "! Perdi esse chapéu anos atrás." );
				from.SendSound( 0x5B4 );
				dropped.Delete();
				int gold = Utility.RandomMinMax(5,10) * 1000;
				from.AddToBackpack ( new BankCheck( gold ) );
				from.SendMessage( "Chuckles lhe deu um cheque de " + gold + " de ouro!" );
				return true;
			}

			return base.OnDragDrop( from, dropped );
		}

		public ChucklesJester( Serial serial ) : base( serial )
		{
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