using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Misc;
using Server.Mobiles;
using System.Collections;
using Server.Gumps;

namespace Server.Mobiles
{
	public class Courier : BasePerson
	{
		public override bool InitialInnocent{ get{ return true; } }

		[Constructable]
		public Courier() : base( )
		{
			SpeechHue = Utility.RandomTalkHue();
			NameHue = 0xB0C;
			Hue = Utility.RandomSkinColor();
			NameHue = 0xB0C;
			AI = AIType.AI_Citizen;
			FightMode = FightMode.None;

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomColor(0) ) );
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomColor(0) ) );
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt( Utility.RandomColor(0) ));
			
			switch ( Utility.Random( 5 ))
			{
				case 0: AddItem( new FeatheredHat( Utility.RandomColor(0) ) ); break;
				case 1: AddItem( new FloppyHat( Utility.RandomColor(0) ) ); break;
				case 2: AddItem( new StrawHat( Utility.RandomColor(0) ) ); break;
				case 3: AddItem( new WideBrimHat( Utility.RandomColor(0) ) ); break;
				case 4: AddItem( new TallStrawHat( Utility.RandomColor(0) ) ); break;
			}

			Title = "o mensageiro";

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

			VirtualArmor = 30;
		}

		public override bool OnBeforeDeath()
		{
			Say("In Vas Mani");
			this.Hits = this.HitsMax;
			this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
			this.PlaySound( 0x202 );
			return false;
		}

		public override bool IsEnemy( Mobile m )
		{
			return false;
		}

		public override void OnAfterSpawn()
		{
			this.WhisperHue = 999;

			///////////////// PUT THEM IN RANDOM CITIES /////////////////
			if ( this.X >= 0 && this.Y >= 0 && this.X <= 6 && this.Y <= 6 && this.Map == Map.Lodor )
			{
				switch( Utility.RandomMinMax( 0, 9 ) )
				{
					case 0: this.X=879; this.Y=963; this.Z=2; this.Map = Map.Lodor; break;
					case 1: this.X=3636; this.Y=413; this.Z=1; this.Map = Map.Lodor; break;
					case 2: this.X=4203; this.Y=1464; this.Z=0; this.Map = Map.Lodor; break;
					case 3: this.X=2903; this.Y=1308; this.Z=7; this.Map = Map.Lodor; break;
					case 4: this.X=865; this.Y=2032; this.Z=0; this.Map = Map.Lodor; break;
					case 5: this.X=1871; this.Y=2210; this.Z=0; this.Map = Map.Lodor; break;
					case 6: this.X=2838; this.Y=2252; this.Z=0; this.Map = Map.Lodor; break;
					case 7: this.X=4242; this.Y=2977; this.Z=0; this.Map = Map.Lodor; break;
					case 8: this.X=2676; this.Y=3198; this.Z=0; this.Map = Map.Lodor; break;
					case 9: this.X=2335; this.Y=3160; this.Z=0; this.Map = Map.Lodor; break;
				}
			}
			else if ( this.X >= 0 && this.Y >= 0 && this.X <= 6 && this.Y <= 6 && this.Map == Map.Sosaria )
			{
				switch( Utility.RandomMinMax( 0, 8 ) )
				{
					case 0: this.X=2126; this.Y=270; this.Z=0; this.Map = Map.Sosaria; break;
					case 1: this.X=813; this.Y=755; this.Z=0; this.Map = Map.Sosaria; break;
					case 2: this.X=2413; this.Y=870; this.Z=2; this.Map = Map.Sosaria; break;
					case 3: this.X=2999; this.Y=1039; this.Z=0; this.Map = Map.Sosaria; break;
					case 4: this.X=4513; this.Y=1274; this.Z=2; this.Map = Map.Sosaria; break;
					case 5: this.X=1605; this.Y=1554; this.Z=2; this.Map = Map.Sosaria; break;
					case 6: this.X=901; this.Y=2075; this.Z=0; this.Map = Map.Sosaria; break;
					case 7: this.X=3290; this.Y=2610; this.Z=0; this.Map = Map.Sosaria; break;
					case 8: this.X=2660; this.Y=3301; this.Z=0; this.Map = Map.Sosaria; break;
				}
			}
			else
			{
				switch( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: this.X=1452; this.Y=3759; this.Z=0; this.Map = Map.Sosaria; break;
					case 1: this.X=6776; this.Y=1749; this.Z=20; this.Map = Map.Sosaria; break;
					case 2: this.X=861; this.Y=1060; this.Z=60; this.Map = Map.SerpentIsland; break;
					case 3: this.X=358; this.Y=1123; this.Z=15; this.Map = Map.IslesDread; break;
					case 4: this.X=797; this.Y=902; this.Z=-4; this.Map = Map.SavagedEmpire; break;
					case 5: this.X=250; this.Y=1681; this.Z=37; this.Map = Map.SavagedEmpire; break;
				}
			}

			this.Home = this.Location;

			Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
			this.PlaySound( 0x1FE );

			base.OnAfterSpawn();

			foreach ( Mobile m in this.GetMobilesInRange( 5 ) )
			{
				if ( m != null && m is BaseCreature && m != this && m is Courier )
					this.Delete();
			}
		}

		protected override void OnMapChange( Map oldMap )
		{
			// DO NOTHING
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list ); 
			if ( !from.Blessed )
				list.Add( new SpeechGumpEntry( from, this ) ); 
		} 

		public class SpeechGumpEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;
			
			public SpeechGumpEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;
				
				PlayerMobile mobile = (PlayerMobile) m_Mobile;
				{
					if ( ! mobile.HasGump( typeof( SpeechGump ) ) )
					{
						Server.Misc.IntelligentAction.SayHey( m_Giver );
						mobile.SendGump(new SpeechGump( mobile, "Message Deliveries", SpeechFunctions.SpeechText( m_Giver, m_Mobile, "Courier" ) ));
					}
				}
            }
        }

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		private class CourierEntry : ContextMenuEntry
		{
			private Courier m_Courier;
			private Mobile m_From;

			public CourierEntry( Courier Courier, Mobile from ) : base( 2141, 3 )
			{
				m_Courier = Courier;
				m_From = from;
			}

			public override void OnClick()
			{
				m_Courier.FindMessage( m_From );
			}
		}

        public void FindMessage( Mobile m )
        {
            if ( Deleted || !m.Alive )
                return;

			string msgQuest = ((PlayerMobile)m).MessageQuest;

			string myHomeWorld = "the Land of Sosaria";

			bool GiveMail = true;

            if ( msgQuest != "" && msgQuest != null )
            {
				ArrayList targets = new ArrayList();
				foreach ( Item item in World.Items.Values )
				if ( item is CourierMail )
				{
					if ( ((CourierMail)item).owner == m )
					{
						GiveMail = false;
						m.AddToBackpack( item );
						m.PlaySound( 0x249 );
						SayTo(m, "Hmmm... já te dei uma mensagem de " + msgQuest + ". Aqui está outra, caso a tenha perdido.");
					}
				}
            }

            if ( GiveMail )
            {
				CourierMail envelope = new CourierMail( m );
				envelope.owner = m;
				string alignment = "good";

				int c = 0;

				ArrayList npcs = new ArrayList();
				foreach ( Mobile msg in World.Mobiles.Values )
				if ( msg is EpicCharacter && msg.Name != "the Great Earth Serpent" )
				{
					string tWorld = Server.Lands.LandName( Server.Lands.GetLand( msg.Map, msg.Location, msg.X, msg.Y ) );

					if ( ((EpicCharacter)msg).MyAlignment == "evil" && PlayerSettings.GetDiscovered( m, tWorld ) && ( m.Karma < 0 || ((PlayerMobile)m).KarmaLocked == true ) )
					{
						npcs.Add( msg ); c++;
					}
					else if ( ((EpicCharacter)msg).MyAlignment == "good" && PlayerSettings.GetDiscovered( m, tWorld ) && m.Karma >= 0 )
					{
						npcs.Add( msg ); c++;
					}
					else if ( ((EpicCharacter)msg).MyAlignment == "neutral" && PlayerSettings.GetDiscovered( m, tWorld ) )
					{
						npcs.Add( msg ); c++;
					}
				}

				if ( c < 1 )
				{
					foreach ( Mobile msg in World.Mobiles.Values )
					if ( msg is EpicCharacter && msg.Name != "the Great Earth Serpent" )
					{
						string tWorld = Server.Lands.LandName( Server.Lands.GetLand( msg.Map, msg.Location, msg.X, msg.Y ) );

						if ( ((EpicCharacter)msg).MyAlignment == "evil" && ( m.Karma < 0 || ((PlayerMobile)m).KarmaLocked == true ) )
						{
							npcs.Add( msg ); c++;
						}
						else if ( ((EpicCharacter)msg).MyAlignment == "good" && m.Karma >= 0 )
						{
							npcs.Add( msg ); c++;
						}
						else if ( ((EpicCharacter)msg).MyAlignment == "neutral" )
						{
							npcs.Add( msg ); c++;
						}
					}
				}

				int o = 1;

				bool foundNPC = false;

				while ( !foundNPC )
				{
					o = Utility.RandomMinMax( 1, c );

					for ( int i = 0; i < npcs.Count; ++i )
					{
						EpicCharacter dude = ( EpicCharacter )npcs[ i ];

						if ( i == o )
						{
							Point3D WhoLoc = new Point3D(dude.MyX, dude.MyY, 0);
							Map WhoMap = dude.MyWorld;

							envelope.mapB = WhoMap;
							envelope.xB = WhoLoc.X;
							envelope.yB = WhoLoc.Y;

							string my_location = "";

							int xLong = 0, yLat = 0;
							int xMins = 0, yMins = 0;
							bool xEast = false, ySouth = false;

							if ( Sextant.Format( WhoLoc, WhoMap, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
							{
								my_location = String.Format( "{0}° {1}'{2}, {3}° {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
							}

							myHomeWorld = Server.Lands.LandName( Server.Lands.GetLand( WhoMap, WhoLoc, dude.MyX, dude.MyY ) );
							envelope.ForWho = dude.Name + " " + dude.Title;
							envelope.ForWhere = my_location;
							envelope.ForAlignment = dude.MyAlignment;
							alignment = dude.MyAlignment;
							((PlayerMobile)m).MessageQuest = dude.Name;

							if ( dude.MyX > 0 )
								foundNPC = true;
						}
					}
				}

				PickSearchLocation( envelope, "No Dungeon Yet", m, alignment, myHomeWorld );

				m.AddToBackpack ( envelope );
				m.PlaySound( 0x249 );
				SayTo(m, "Hmmm... tenho uma mensagem para você. Aqui está.");
            }
        }

		public static void PickSearchLocation( CourierMail scroll, string DungeonNow, Mobile from, string alignment, string homeworld )
		{
			string QuestItem = Server.Misc.QuestCharacters.QuestItems( true );

			scroll.SearchItem = QuestItem;

			string QuestStory = Server.Misc.QuestCharacters.EpicQuestStory( QuestItem, alignment );

			string thisWorld = "the Land of Sosaria";
			string thisPlace = "the Dungeon of Doom";
			Map realMap = Map.Sosaria;
			Map thisMap = Map.Sosaria;

			int aCount = 0;
			ArrayList targets = new ArrayList();
			foreach ( Item target in World.Items.Values )
			if ( target is SearchBase && ( Server.Difficult.GetDifficulty( target.Location, target.Map ) <= GetPlayerInfo.GetPlayerDifficulty( from ) ) )
			{
				string tWorld = Server.Lands.LandName( Server.Lands.GetLand( target.Map, target.Location, target.X, target.Y ) );
				if ( tWorld == "the Land of Sosaria" ){ targets.Add( target ); aCount++; }
				else if ( PlayerSettings.GetDiscovered( from, tWorld ) ){ targets.Add( target ); aCount++; }
			}

			aCount = Utility.RandomMinMax( 1, aCount );

			int xCount = 0;
			for ( int i = 0; i < targets.Count; ++i )
			{
				xCount++;

				if ( xCount == aCount )
				{
					Item finding = ( Item )targets[ i ];
					realMap = finding.Map;
					thisMap = Server.Misc.Worlds.GetMyDefaultMap( finding.Land );
					thisPlace = Server.Misc.Worlds.GetRegionName( finding.Map, finding.Location );
					thisWorld = Lands.LandName( finding.Land );
					scroll.MsgComplete = 0;
					scroll.MsgReward = Server.Difficult.GetDifficulty( finding.Location, finding.Map ) + 2;
						if ( scroll.MsgReward < 2 ){ scroll.MsgReward = 2; }
				}
			}

			string Word1 = "Lendas";
			switch ( Utility.RandomMinMax( 1, 4 ) )
			{
				case 1: Word1 = "Rumores"; break;
				case 2: Word1 = "Mitos"; break;
				case 3: Word1 = "Contos"; break;
				case 4: Word1 = "Histórias"; break;
			}
			string Word2 = "perdido";
			switch ( Utility.RandomMinMax( 1, 4 ) )
			{
				case 1: Word2 = "guardado"; break;
				case 2: Word2 = "visto"; break;
				case 3: Word2 = "levado"; break;
				case 4: Word2 = "escondido"; break;
			}
			string Word3 = "nas profundezas de";
			switch ( Utility.RandomMinMax( 1, 4 ) )
			{
				case 1: Word3 = "dentro de"; break;
				case 2: Word3 = "em algum lugar de"; break;
				case 3: Word3 = "de alguma forma em"; break;
				case 4: Word3 = "longe em"; break;
			}
			string Word4 = "séculos atrás";
			switch ( Utility.RandomMinMax( 1, 4 ) )
			{
				case 1: Word4 = "milhares de anos atrás"; break;
				case 2: Word4 = "décadas atrás"; break;
				case 3: Word4 = "milhões de anos atrás"; break;
				case 4: Word4 = "muitos anos atrás"; break;
			}

            scroll.SearchDungeon = thisPlace;
            scroll.SearchWorld = thisWorld;
			scroll.DungeonMap = thisMap;

			string gold = (scroll.MsgReward * 1000).ToString();
				if ( alignment == "neutral" ){ gold = (scroll.MsgReward * 1500).ToString(); }
			string heard = "Ouvi dizer que você talvez pudesse me ajudar com algo da mais alta importância.";
			string reward = "Faça isso por mim, e poderei recompensá-lo com " + gold + " moedas de ouro.";

			if ( alignment != "evil" )
			{
				switch( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: heard = "Ouvi dizer que você talvez pudesse me ajudar com algo da mais alta importância.";     break;
					case 1: heard = RandomThings.GetRandomName() + " falou de você para mim, e que talvez você possa ajudar.";         break;
					case 2: heard = "Depois de conversar com meu amigo, " + RandomThings.GetRandomName() + ", ele mencionou que talvez você possa me ajudar com algo.";     break;
					case 3: heard = "Ouço dizer que você é alguém em quem posso confiar para esta importante tarefa que se avizinha.";     break;
					case 4: heard = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " mencionou que você talvez pudesse me ajudar com algo.";     break;
					case 5: heard = "Há uma situação grave que acho que você pode ajudar a resolver.";     break;
				}
			}
			else if ( alignment == "evil" )
			{
				reward = "Acho que " + gold + " moedas de ouro farão isso valer o seu tempo.";
				switch( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: heard = "Ouvi dizer que você é alguém que pode me servir em meus propósitos.";     break;
					case 1: heard = RandomThings.GetRandomName() + " falou de você para mim, e que você me serviria bem.";         break;
					case 2: heard = "Depois de conversar com meu servo, " + RandomThings.GetRandomName() + ", ele mencionou que talvez você fizesse o que eu ordeno.";     break;
					case 3: heard = "Ouço sussurros sobre suas ambições, e que talvez possamos nos beneficiar mutuamente do que estou prestes a pedir.";     break;
					case 4: heard = "Aqueles em " + RandomThings.GetRandomCity() + " às vezes mencionam seu nome em maldições sussurradas, e é por isso que lhe enviei esta mensagem.";     break;
					case 5: heard = "Há um item que preciso para meus planos, e acho que você é alguém que pode obtê-lo com pouca atenção de outros.";     break;
				}
			}

			string intro = from.Name + ",<br><br>" + heard;

			Map place;
			int xc;
			int yc;

			string EntranceLocation = Worlds.GetAreaEntrance( 0, scroll.SearchDungeon, realMap, out place, out xc, out yc  );

			scroll.mapA = place;
			scroll.xA = xc;
			scroll.yA = yc;

			scroll.SearchMessage = intro + " " + reward + " " + QuestStory + " " + Word1 + " contam que " + QuestItem + " foi " + Word2 + " " + Word3;

			scroll.SearchMessage = scroll.SearchMessage + " " + scroll.SearchDungeon + " " + Word4 + " em " + scroll.SearchWorld + " nas coordenadas de sextante abaixo.<br><br>" + EntranceLocation;

			scroll.SearchMessage = scroll.SearchMessage + "<br><br>Quando o encontrar, traga esta mensagem de volta para mim. Estou em " + homeworld + " nas coordenadas de sextante abaixo.<br><br>" + scroll.ForWhere;

			scroll.SearchMessage = scroll.SearchMessage + "<br><br>- " + scroll.ForWho;

			scroll.InvalidateProperties();
		}

		public override void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( from.Alive && !from.Blessed )
			{
				list.Add( new CourierEntry( this, from ) );
			}

			base.AddCustomContextEntries( from, list );
		}

		public Courier( Serial serial ) : base( serial )
		{
		}

		public override bool CanTeach { get { return true; } }

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