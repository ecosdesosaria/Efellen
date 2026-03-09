using System;
using Server;
using System.Collections;
using Server.Misc;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;
using Server.Targeting;
using System.Collections.Generic;
using Server.Items;

namespace Server.Misc
{
    class HenchmanFunctions
    {
		public static bool IsInRestRegion(Mobile from)
		{
			bool house = false;
			if ( from.Region is HouseRegion )
			    if (((HouseRegion)from.Region).House.IsOwner(from))
					house = true;
			
			if ( from.Region.GetLogoutDelay( from ) == TimeSpan.Zero || house ) 
				return true;

			return false;
		}

		public static void ForceSlow( Mobile m )
		{
			if ( m is HenchmanMonster || m is HenchmanArcher || m is HenchmanFighter || m is HenchmanWizard )
			{
				BaseCreature bc = (BaseCreature)m;
				IMount mount = m.Mount;
				if ( mount != null )
				{
					mount.Rider = null;
					BaseCreature horse = (BaseCreature)mount;
					horse.Delete();
				}
				bc.ActiveSpeed = 0.2;
				bc.PassiveSpeed = 0.4;
			}
		}

        public static void ReportStatus( Mobile henchman )
        {
            string time = ((int)(henchman.Fame/5)).ToString();
            string bandages = henchman.Hunger.ToString();
            if (henchman is HenchmanMonster)
		henchman.Say("Tenho " + bandages + " bandagens. Ficarei por " + time + " minutos.");
            else
		henchman.Say("Tenho " + bandages + " bandagens. Viajarei contigo por " + time + " minutos.");
        }

		public static void DismountHenchman( Mobile from )
		{
			if ( from is PlayerMobile )
			{
				PlayerMobile master = (PlayerMobile)from;
				List<Mobile> pets = master.AllFollowers;

				if ( pets.Count > 0 )
				{
					for ( int i = 0; i < pets.Count; ++i )
					{
						Mobile m = (Mobile)pets[i];

						if ( m is HenchmanMonster || m is HenchmanArcher || m is HenchmanFighter || m is HenchmanWizard || MyServerSettings.FastFriends( m ) )
						{
							BaseCreature bc = (BaseCreature)m;
							IMount mount = m.Mount;
							if ( mount != null )
							{
								mount.Rider = null;
								BaseCreature horse = (BaseCreature)mount;
								horse.Delete();
							}
							bc.ActiveSpeed = 0.2;
							bc.PassiveSpeed = 0.4;
						}
					}
				}
			}
		}

		public static void MountHenchman( Mobile from )
		{
			if ( from is PlayerMobile )
			{
				PlayerMobile master = (PlayerMobile)from;
				List<Mobile> pets = master.AllFollowers;

				if ( pets.Count > 0 )
				{
					for ( int i = 0; i < pets.Count; ++i )
					{
						Mobile m = (Mobile)pets[i];

						if ( m is HenchmanMonster || m is HenchmanArcher || m is HenchmanFighter || m is HenchmanWizard || MyServerSettings.FastFriends( m ) )
						{
							BaseCreature bc = (BaseCreature)m;
							IMount mount = m.Mount;
							if ( mount == null && ( m is HenchmanArcher || m is HenchmanFighter || m is HenchmanWizard ) )
							{
								new HenchHorse().Rider = m;
							}
							bc.ActiveSpeed = 0.1;
							bc.PassiveSpeed = 0.2;
						}
					}
				}
			}
		}

		public static bool OnMoving( Mobile m, Point3D oldLocation, Mobile from, DateTime m_NextMorale )
		{
			bool GoAway = false;
			bool monster = false;

			if ( DateTime.Now >= m_NextMorale && from.InRange( m, 20 ) )
			{
				HenchmanFunctions.BandageMySelf( from );
				if ( HenchmanFunctions.IsInRestRegion( from ) == true ){} else
				{
					from.Fame = from.Fame - 5;
					if ( from.Fame < 0 )
					{
						from.Fame = 0;
						ArrayList targets = new ArrayList();
						foreach ( Item item in World.Items.Values )
						if ( item is HenchmanItem )
						{
							HenchmanItem henchItem = (HenchmanItem)item;
							if ( henchItem.HenchSerial == from.Serial )
							{
								targets.Add( item );
							}
						}
						for ( int i = 0; i < targets.Count; ++i )
						{
							Item item = ( Item )targets[ i ];
							HenchmanItem henchThing = (HenchmanItem)item;
							henchThing.HenchTimer = from.Fame;
							henchThing.HenchBandages = from.Hunger;
							henchThing.LootType = LootType.Regular;

							if ( item is HenchmanArcherItem ){ henchThing.Name = "archer henchman"; }
							else if ( item is HenchmanFighterItem ){ henchThing.Name = "fighter henchman"; }
							else if ( item is HenchmanMonsterItem ){ henchThing.Name = "creature henchman"; monster = true; }
							else { henchThing.Name = "wizard henchman"; }

							henchThing.Visible = true;
						}

						if ( monster )
						{
							switch ( Utility.Random( 2 ) )		   
							{
								case 0: from.Say("Não há recompensa suficiente nisso para mim."); break;
								case 1: from.Say("Se você ouvir histórias de riquezas, venha me buscar."); break;
							}
						}
						else
						{
							switch ( Utility.Random( 5 ) )		   
							{
								case 0: from.Say("Desculpe, mas não há recompensa suficiente nesta jornada para mim."); break;
								case 1: from.Say("Acho que vou voltar para a cidade e tomar uma bebida."); break;
								case 2: from.Say("O risco não vale a pequena recompensa que estou recebendo."); break;
								case 3: from.Say("Venha me encontrar mais tarde quando tiver uma missão por riquezas."); break;
								case 4: from.Say("Se você ouvir qualquer rumor de ouro, venha me buscar."); break;
							}
						}
						GoAway = true;
					}
					else if ( from.Fame < 26 )
					{
						if ( monster )
						{
							switch ( Utility.Random( 2 ) )		   
							{
								case 0: from.Say("Vou embora em breve se não encontrarmos tesouro."); break;
								case 1: from.Say("Você disse que havia riquezas, mas não estou vendo."); break;
							}
						}
						else
						{
							switch ( Utility.Random( 5 ))		   
							{
									case 0: from.Say("Vou ter que ir embora em breve se não encontrarmos algum tesouro."); break;
									case 1: from.Say("Sinto que esta missão é um beco sem saída e posso ir embora em breve."); break;
									case 2: from.Say("Esta falta de tesouro não é pelo que eu vim."); break;
									case 3: from.Say("Você prometeu riquezas, mas receio que não haja nenhuma."); break;
									case 4: from.Say("O que estamos procurando? Obviamente não é tesouro."); break;
							}
						}
					}
				}

				((BaseCreature)from).Loyalty = 100;
			}

			return GoAway;
		}

		public static void BandageMySelf( Mobile m )
		{
			DateTime healing = (DateTime.Now + TimeSpan.FromSeconds( 0 ) );

			if ( m is HenchmanFighter ){ healing = ((HenchmanFighter)m).Healing; }
			else if ( m is HenchmanArcher ){ healing = ((HenchmanArcher)m).Healing; }
			else if ( m is HenchmanMonster ){ healing = ((HenchmanMonster)m).Healing; }
			else if ( m is HenchmanWizard ){ healing = ((HenchmanWizard)m).Healing; }

			if ( DateTime.Now >= healing )
			{
				int nHealing = (int)(m.Skills[SkillName.Healing].Value);

				if ( (m.Hunger > 0) && (m.Hits < m.HitsMax) && (nHealing >= Utility.Random( 105 )) )
				{
					if ( m.Poisoned && nHealing >= 90 ){ m.CurePoison( m ); }
					else if ( m.Poisoned && nHealing >= 80 && Utility.Random( 4 ) != 1 ){ m.CurePoison( m ); }
					else if ( m.Poisoned && nHealing >= 70 && Utility.RandomBool() ){ m.CurePoison( m ); }
					else if ( m.Poisoned && nHealing >= 60 && Utility.Random( 4 ) == 1 ){ m.CurePoison( m ); }
					else if ( !m.Poisoned )
					{
						int minHeal = (int)( m.Skills[SkillName.Anatomy].Value / 4 ) + (int)( m.Skills[SkillName.Healing].Value / 4 ) + 6;
						int maxHeal = (int)( m.Skills[SkillName.Anatomy].Value / 4 ) + (int)( m.Skills[SkillName.Healing].Value / 2 ) + 20;
						m.Hits = m.Hits + Utility.RandomMinMax( minHeal, maxHeal );
					}
					m.Hunger = m.Hunger - 1;
					m.RevealingAction();
					m.PlaySound( 0x57 );

					healing = (DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 4, 8 ) ) );

					if ( m is HenchmanFighter ){ ((HenchmanFighter)m).Healing = healing; }
					else if ( m is HenchmanArcher ){ ((HenchmanArcher)m).Healing = healing; }
					else if ( m is HenchmanMonster ){ ((HenchmanMonster)m).Healing = healing; }
					else if ( m is HenchmanWizard ){ ((HenchmanWizard)m).Healing = healing; }
				}
				if ( m.Stam < m.StamMax ){ m.Stam = m.Stam + (int)(m.StamMax / 40); }
				if ( m.Mana < m.ManaMax ){ m.Mana = m.Mana + (int)(m.StamMax / 20); }
			}
		}

		public static void OnGaveAttack( Mobile from )
		{
			if ( !(from is HenchmanMonster) )
			{
				switch ( Utility.Random( 74 ))		   
				{
					case 0: from.Say("Hora de morrer!"); break;
					case 1: from.Say("Vou te mandar para o inferno!"); break;
					case 2: from.Say("Sua vida termina aqui!"); break;
					case 3: from.Say("Você não é páreo para mim!"); break;
					case 4: from.Say("Prepare-se para morrer, tolo!"); break;
					case 5: from.Say("Sinta minha ira e minha lâmina!"); break;
					case 6: from.Say("Renda-se a mim!"); break;
					case 7: from.Say("Eu te sentencio à morte!"); break;
					case 8: from.PlaySound( from.Female ? 793 : 1065 ); from.Say( "*suspiro*" ); break;
					case 9: from.PlaySound( from.Female ? 0x338 : 0x44A ); from.Say( "*rosna*" ); break;
					case 10: from.PlaySound( from.Female ? 797 : 1069 ); from.Say( "Ei!" ); break;
					case 11: from.PlaySound( from.Female ? 821 : 1095 ); from.Say( "*assobia*" ); break;
					case 12: from.PlaySound( from.Female ? 783 : 1054 ); from.Say( "Uhuu!" ); break;
					case 13: from.PlaySound( from.Female ? 823 : 1097 ); from.Say( "Isso!" ); break;
					case 14: from.PlaySound( from.Female ? 0x31C : 0x42C ); from.Say( "*grita*" ); break;
					case 15: from.Say("Vou te esfolar vivo!"); break;
					case 16: from.Say("Você não sairá daqui vivo!"); break;
					case 17: from.Say("Você cometeu seu último erro!"); break;
					case 18: from.Say("Aqui é onde sua jornada termina!"); break;
					case 19: from.Say("Eu nasci para este momento!"); break;
					case 20: from.Say("Espero que esteja em paz com seus deuses!"); break;
					case 21: from.Say("Seus gritos ecoarão no vazio!"); break;
					case 22: from.Say("Não vacilarei!"); break;
					case 23: from.Say("Não serei eu quem vai recuar."); break;
					case 24: from.Say("Você deveria ter fugido quando teve chance!"); break;
					case 28: from.Say("Você é só mais um cadáver para mim!"); break;
					case 29: from.Say("Vou me banhar em seu sangue!"); break;
					case 30: from.Say("Isso é tudo que você tem?"); break;
					case 31: from.Say("Para o Mundo Inferior com você!"); break;
					case 32: from.Say("Venha então, vamos dançar!"); break;
					case 33: from.Say("Um golpe, um cadáver."); break;
					case 34: from.Say("Você escolheu a briga errada!"); break;
					case 35: from.Say("Renda-se... Ou caia!"); break;
					case 36: from.Say("Morra com alguma dignidade, pode ser?"); break;
					case 37: from.Say("O aço só responde à coragem!"); break;
				}
			}
			((BaseCreature)from).Loyalty = 100;
			HenchmanFunctions.BandageMySelf( from );
		}

        public static void OnSpellAttack( Mobile from )
        {
			if ( !(from is HenchmanMonster) )
			{
				switch ( Utility.Random( 68 ))		   
				{
					case 0: from.Say("Hora de morrer!"); break;
					case 1: from.Say("Vou te mandar para o inferno!"); break;
					case 2: from.Say("Sua vida termina aqui!"); break;
					case 3: from.Say("Você não é páreo para mim!"); break;
					case 4: from.Say("Prepare-se para morrer, tolo!"); break;
					case 5: from.Say("Sinta minha ira e minha lâmina!"); break;
					case 6: from.Say("Renda-se a mim!"); break;
					case 7: from.Say("Eu te sentencio à morte!"); break;
					case 8: from.PlaySound( from.Female ? 793 : 1065 ); from.Say( "*suspiro*" ); break;
					case 9: from.PlaySound( from.Female ? 0x338 : 0x44A ); from.Say( "*rosna*" ); break;
					case 10: from.PlaySound( from.Female ? 797 : 1069 ); from.Say( "Ei!" ); break;
					case 11: from.PlaySound( from.Female ? 821 : 1095 ); from.Say( "*assobia*" ); break;
					case 12: from.PlaySound( from.Female ? 783 : 1054 ); from.Say( "Uhuu!" ); break;
					case 13: from.PlaySound( from.Female ? 823 : 1097 ); from.Say( "Isso!" ); break;
					case 14: from.PlaySound( from.Female ? 0x31C : 0x42C ); from.Say( "*grita*" ); break;
					case 15: from.Say("Belo truque. Tem outro?"); break;
					case 16: from.Say("Já levei feitiços maiores."); break;
					case 17: from.Say("Isso é o melhor que sua magia pode fazer?"); break;
					case 18: from.Say("Truques de salão não vencerão esta luta."); break;
					case 19: from.Say("Você se arrependerá de lançar feitiços em mim!"); break;
					case 20: from.Say("Você ousa conjurar em mim?"); break;
					case 21: from.Say("Aquele feitiço... quase impressionante."); break;
					case 22: from.Say("Um feitiço? Você precisará de mais que isso."); break;
					case 23: from.Say("Isso fez mais cócegas do que doeu."); break;
					case 24: from.Say("Tente novamente, conjurador."); break;
					case 25: from.Say("Vou atravessar suas barreiras e glifos."); break;
					case 26: from.Say("Belo truque. Tem outro?"); break;
					case 27: from.Say("Feitiços não te salvarão da minha ira."); break;
					case 28: from.Say("Magia... Eu odeio magia."); break;
					case 29: from.Say("Continue conjurando, eu continuarei atacando."); break;
					case 30: from.Say("Você precisará de um feitiço mais forte que esse."); break;
					case 31: from.Say("Isso é o melhor que sua magia pode fazer?"); break;
					case 32: from.Say("Uma faísca e uma baforada de fumaça. Impressionante."); break;
					case 33: from.Say("Sinto cheiro de cabelo queimado. É melhor rezar para não ser o meu."); break;
					case 34: from.Say("Um pequeno choque não vai me parar."); break;
				}
			}
			((BaseCreature)from).Loyalty = 100;
			HenchmanFunctions.BandageMySelf( from );
        }

		public static void OnGotAttack( Mobile from )
		{
			if ( !(from is HenchmanMonster) )
			{
				Server.Misc.IntelligentAction.CryOut( from );

				switch ( Utility.Random( 82 ))		   
				{
					case 0: from.Say("Isso é tudo que você tem?"); break;
					case 1: from.Say("É só um arranhão!"); break;
					case 2: from.Say("Já tive piores!"); break;
					case 3: from.Say("Você terá que fazer melhor que isso!"); break;
					case 4: from.Say("Você vai pagar por isso!"); break;
					case 5: from.Say("Ninguém faz isso e vive!"); break;
					case 6: from.Say("Agora é sua vez!"); break;
					case 7: from.Say("Não é suficiente para me derrubar!"); break;
					case 8: from.PlaySound( from.Female ? 793 : 1065 ); from.Say( "*suspiro*" ); break;
					case 9: from.PlaySound( from.Female ? 0x338 : 0x44A ); from.Say( "*rosna*" ); break;
					case 10: from.PlaySound( from.Female ? 797 : 1069 ); from.Say( "Ei!" ); break;
					case 11: from.PlaySound( from.Female ? 0x31C : 0x42C ); from.Say( "*grita*" ); break;
					case 12: from.Say("Você ousa me golpear?"); break;
					case 13: from.Say("Você se arrependerá disso!"); break;
					case 14: from.Say("Isso é tudo que você tem?"); break;
					case 15: from.Say("Você vai pagar por isso, covarde!"); break;
					case 16: from.Say("Você derramou sangue - agora eu derramo vingança."); break;
					case 17: from.Say("Você não me pegará tão facilmente da próxima vez."); break;
					case 18: from.Say("Você arranha como um gatinho."); break;
					case 19: from.Say("Isso foi um erro."); break;
					case 20: from.Say("Agora você tornou isso pessoal!"); break;
					case 21: from.Say("Este ferimento só me fortalece."); break;
					case 22: from.Say("Você terá que fazer melhor que isso."); break;
					case 23: from.Say("Hah! Eu estava começando a ficar entediado."); break;
					case 24: from.Say("Você vai desejar ter errado."); break;
					case 25: from.Say("Golpeie novamente e veja o que acontece."); break;
					case 26: from.Say("Não vim até aqui para cair para alguém como você."); break;
					case 27: from.Say("Você escolheu o guerreiro errado para irritar."); break;
					case 28: from.Say("Isso fez cócegas."); break;
					case 29: from.Say("Você terá que acertar mais forte para me parar."); break;
					case 30: from.Say("Ah, então é uma luta que você quer!"); break;
					case 31: from.Say("Finalmente, alguma emoção!"); break;
					case 32: from.Say("Seu golpe falta convicção."); break;
					case 33: from.Say("Esse é seu melhor esforço?"); break;
					case 34: from.Say("Senti pior com um vento frio."); break;
					case 35: from.Say("Você selou seu destino."); break;
					case 36: from.Say("Uma pobre escolha de alvo."); break;
					case 37: from.Say("Você acabou de ganhar minha atenção total."); break;
					case 38: from.Say("Vou fazer você se arrepender desse golpe."); break;
					case 39: from.Say("Atacando das sombras? Covarde!"); break;
					case 40: from.Say("Você terá que terminar o que começou."); break;
					case 41: from.Say("Você esquece que não estou sozinho aqui!"); break;
				}
			}
			((BaseCreature)from).Loyalty = 100;
			HenchmanFunctions.BandageMySelf( from );
		}

		public static void OnDead( Mobile from )
		{
			ArrayList targets = new ArrayList();
			foreach ( Item item in World.Items.Values )
			if ( item is HenchmanItem )
			{
				HenchmanItem henchItem = (HenchmanItem)item;
				if ( henchItem.HenchSerial == from.Serial )
				{
					targets.Add( item );
				}
			}
			for ( int i = 0; i < targets.Count; ++i )
			{
				Item item = ( Item )targets[ i ];
				HenchmanItem henchThing = (HenchmanItem)item;
				henchThing.HenchDead = ( from.RawStr + from.RawInt + from.RawDex ) * 2;
				henchThing.HenchTimer = from.Fame;
				henchThing.HenchBandages = from.Hunger;
				henchThing.LootType = LootType.Regular;

				if ( item is HenchmanArcherItem ){ henchThing.Name = "dead archer henchman"; }
				else if ( item is HenchmanFighterItem ){ henchThing.Name = "dead fighter henchman"; }
				else if ( item is HenchmanMonsterItem ){ henchThing.Name = "dead creature henchman"; }
				else { henchThing.Name = "dead wizard henchman"; }

				henchThing.Visible = true;
			}

			IMount mount = from.Mount;
			if ( mount != null )
			{
				BaseCreature Horsey = (BaseCreature)mount;
				Horsey.Delete();
			}

			Mobile killer = from.FindMostRecentDamager(true);

			if ( ( killer is PlayerMobile ) && ( ((BaseCreature)from).ControlMaster != killer ) )
			{
				killer.Criminal = true;
				killer.Kills = killer.Kills + 1;
			}
		}

		public static void OnGive( Mobile from, Item dropped, Mobile henchman )
		{
			if ( dropped is Bandage )
			{
				henchman.Hunger = henchman.Hunger + dropped.Amount;
				if ( henchman is HenchmanMonster ){ henchman.Say("Posso usar estas bandagens. Tenho " + henchman.Hunger.ToString() + " delas agora."); }
				else { henchman.Say("Ahhh... bandagens podem ser muito úteis. Tenho " + henchman.Hunger.ToString() + " delas agora."); }
				dropped.Delete();
			}
			else if ( dropped is LesserCurePotion || dropped is CurePotion || dropped is GreaterCurePotion )
			{
				if ( henchman is HenchmanMonster ){ henchman.Say("Bom, " + from.Name + "."); }
				else { henchman.Say("Obrigado, " + from.Name + "."); }

				henchman.CurePoison( henchman );
				henchman.RevealingAction();
				henchman.PlaySound( 0x2D6 );
				from.AddToBackpack( new Bottle() );
				if ( henchman.Body.IsHuman && !henchman.Mounted )
					henchman.Animate( 34, 5, 1, true, false, 0 );
				dropped.Delete();
			}
			else if ( dropped is RefreshPotion || dropped is TotalRefreshPotion )
			{
				if ( henchman is HenchmanMonster ){ henchman.Say("Bom, " + from.Name + "."); }
				else { henchman.Say("Obrigado, " + from.Name + "."); }

				henchman.Stam = henchman.StamMax;
				henchman.RevealingAction();
				henchman.PlaySound( 0x2D6 );
				from.AddToBackpack( new Bottle() );
				if ( henchman.Body.IsHuman && !henchman.Mounted )
					henchman.Animate( 34, 5, 1, true, false, 0 );
				dropped.Delete();
			}
			else if ( dropped is LesserHealPotion || dropped is HealPotion || dropped is GreaterHealPotion )
			{
				if ( henchman is HenchmanMonster ){ henchman.Say("Bom, " + from.Name + "."); }
				else { henchman.Say("Obrigado, " + from.Name + "."); }

				henchman.Hits = henchman.HitsMax;
				henchman.RevealingAction();
				henchman.PlaySound( 0x2D6 );
				from.AddToBackpack( new Bottle() );
				if ( henchman.Body.IsHuman && !henchman.Mounted )
					henchman.Animate( 34, 5, 1, true, false, 0 );
				dropped.Delete();
			}
			else if ( dropped is LesserRejuvenatePotion || dropped is RejuvenatePotion || dropped is GreaterRejuvenatePotion )
			{
				if ( henchman is HenchmanMonster ){ henchman.Say("Bom, " + from.Name + "."); }
				else { henchman.Say("Obrigado, " + from.Name + "."); }

				henchman.Hits = henchman.HitsMax;
				henchman.Stam = henchman.StamMax;
				henchman.Mana = henchman.ManaMax;
				henchman.RevealingAction();
				henchman.PlaySound( 0x2D6 );
				from.AddToBackpack( new Bottle() );
				if ( henchman.Body.IsHuman && !henchman.Mounted )
					henchman.Animate( 34, 5, 1, true, false, 0 );
				dropped.Delete();
			}
			else if ( dropped is LesserManaPotion || dropped is ManaPotion || dropped is GreaterManaPotion )
			{
				if ( henchman is HenchmanMonster ){ henchman.Say("Bom, " + from.Name + "."); }
				else { henchman.Say("Obrigado, " + from.Name + "."); }

				henchman.Mana = henchman.ManaMax;
				henchman.RevealingAction();
				henchman.PlaySound( 0x2D6 );
				from.AddToBackpack( new Bottle() );
				if ( henchman.Body.IsHuman && !henchman.Mounted )
					henchman.Animate( 34, 5, 1, true, false, 0 );
				dropped.Delete();
			}
			else 
			{
				int nAmount = dropped.Amount;
				int nGold = 0;

				if ( dropped is DDSilver ){ double dGold = (nAmount / 5); nGold = (int)(Math.Floor(dGold)); }
				else if ( dropped is DDCopper ){ double dGold = (nAmount / 10); nGold = (int)(Math.Floor(dGold)); }
				else if ( dropped is DDJewels ){ nGold = nAmount * 2; }
				else if ( dropped is DDXormite ){ nGold = nAmount * 3; }
				else if ( dropped is Crystals ){ nGold = nAmount * 5; }
				else if ( dropped is Gold ){ nGold = nAmount; }
				else if ( dropped is DDGemstones ){ nGold = nAmount * 2; }
				else if ( dropped is DDGoldNuggets ){ nGold = nAmount; }

				else if ( dropped is JewelryRing ){ nGold = Utility.Random( 50,300 ); }
				else if ( dropped is JewelryNecklace ){ nGold = Utility.Random( 50,300 ); }
				else if ( dropped is JewelryEarrings ){ nGold = Utility.Random( 50,300 ); }
				else if ( dropped is JewelryBracelet ){ nGold = Utility.Random( 50,300 ); }
				else if ( dropped is JewelryCirclet ){ nGold = Utility.Random( 50,300 ); }

				else if ( dropped is Amber ){ nGold = nAmount*12; }
				else if ( dropped is Amethyst ){ nGold = nAmount*25; }
				else if ( dropped is Citrine ){ nGold = nAmount*12; }
				else if ( dropped is Diamond ){ nGold = nAmount*50; }
				else if ( dropped is Emerald ){ nGold = nAmount*25; }
				else if ( dropped is Ruby ){ nGold = nAmount*19; }
				else if ( dropped is Sapphire ){ nGold = nAmount*25; }
				else if ( dropped is StarSapphire ){ nGold = nAmount*31; }
				else if ( dropped is Tourmaline ){ nGold = nAmount*23; }

				if ( nGold > 0 )
				{
					if ( henchman.Fame >= 1800 )
					{
						if ( henchman is HenchmanMonster ){ henchman.Say("Desculpe, " + from.Name + "... mas minha bolsa está cheia."); }
						else { henchman.Say("Obrigado, " + from.Name + "... mas minha bolsa de tesouro está cheia."); }
					}
					else
					{
						switch ( Utility.Random( 15 ) )
						{
							case 0: henchman.Say("Bom... mais tesouro para mim."); break;
							case 1: henchman.Say("Ahh... uma parte do tesouro. Esta jornada vale o risco."); break;
							case 2: henchman.Say("Ouro compra lealdade."); break;
							case 3: henchman.Say("Ora, ora... parece que sou apreciado."); break;
							case 4: henchman.Say("Bom. Eu estava precisando urgentemente de um novo par de sapatos"); break;
							case 5: henchman.Say("Um presente generoso. Vamos manter as riquezas vindo."); break;
							case 6: henchman.Say("Agora *isso* é o tipo de liderança que eu sigo."); break;
							case 7: henchman.Say("Parece que este empreendimento valeu a pena, afinal."); break;
							case 8: henchman.Say("Hmm... nada mal. Talvez eu fique por aqui mesmo."); break;
							case 9: henchman.Say("Tesouro compartilhado é tesouro ganho."); break;
							case 10: henchman.Say("Minha bolsa fica mais pesada. Gosto deste acordo."); break;
							case 11: henchman.Say("Moeda na mão, e sangue ainda para derramar. Um bom dia."); break;
							case 12: henchman.Say("Continue trazendo o ouro, e eu continuarei mantendo os inimigos longe."); break;
							case 13: henchman.Say("Uma parte justa por uma luta justa."); break;
							case 14: henchman.Say("As crianças que tenho em casa vão apreciar isso, "+from.Name+"."); break;
						}


						if ( (henchman.Fame + nGold) > 1800 ){ henchman.Fame = 1800; }
						else { henchman.Fame = henchman.Fame + nGold; }
						int nTime = (int)(henchman.Fame/5);
						from.SendMessage("" + henchman.Name + " provavelmente aventurará com você por mais " + nTime.ToString() + " minutos.");
						dropped.Delete();
					}
				}
				else
				{
					switch ( Utility.Random( 15 ) )
					{
						case 0: henchman.Say("Desculpe, " + from.Name + "... mas não jurei carregar seus fardos."); break;
						case 1: henchman.Say("Não, " + from.Name + "... isso é inútil para mim."); break;
						case 2: henchman.Say("O que eu devo fazer com *isso*?"); break;
						case 3: henchman.Say("Estou aqui para lutar, não para organizar suas tralhas."); break;
						case 4: henchman.Say("A menos que seja ouro ou glória, guarde."); break;
						case 5: henchman.Say("Meus bolsos não são para sua bagunça, " + from.Name + "."); break;
						case 6: henchman.Say("Isso é uma piada? Não sou um animal de carga."); break;
						case 7: henchman.Say("Tente a mula, não eu."); break;
						case 8: henchman.Say("A menos que brilhe ou sangre, não estou interessado."); break;
						case 9: henchman.Say("Não aceito coisas de segunda mão, " + from.Name + "."); break;
						case 10: henchman.Say("Ouro foi o que combinamos."); break;
						case 11: henchman.Say("Isso está amaldiçoado? Parece amaldiçoado."); break;
						case 12: henchman.Say("Não, obrigado. Já tenho peso suficiente para carregar."); break;
						case 13: henchman.Say("Não sou uma mula de carga, " + from.Name + "."); break;
						case 14: henchman.Say("Não posso pagar a pensão alimentícia com isso, " + from.Name + "."); break;
					}
				}
			}

			((BaseCreature)henchman).Loyalty = 100;
		}

		public static void NormalizeArmor( BaseCreature friend )
		{
			if ( friend.ColdResistance > 70 ){ friend.ColdResistSeed = friend.ColdResistSeed - (friend.ColdResistance - friend.ColdResistSeed); }
			if ( friend.FireResistance > 70 ){ friend.FireResistSeed = friend.FireResistSeed - (friend.FireResistance - friend.FireResistSeed); }
			if ( friend.PoisonResistance > 70 ){ friend.PoisonResistSeed = friend.PoisonResistSeed - (friend.PoisonResistance - friend.PoisonResistSeed); }
			if ( friend.EnergyResistance > 70 ){ friend.EnergyResistSeed = friend.EnergyResistSeed - (friend.EnergyResistance - friend.EnergyResistSeed); }
			if ( friend.PhysicalResistance > 70 ){ friend.PhysicalResistanceSeed = friend.PhysicalResistanceSeed - (friend.PhysicalResistance - friend.PhysicalResistanceSeed); }
		}

		public static void DressUp( HenchmanItem henchman, BaseCreature friend, Mobile from )
		{
			bool isOriental = Server.Misc.GetPlayerInfo.OrientalPlay( from );
			bool isEvil = Server.Misc.GetPlayerInfo.EvilPlay( from );

			if ( henchman is HenchmanWizardItem )
			{
				if ( isEvil == true && henchman.HenchGearColor != 0x485 && henchman.HenchGearColor != 0x497 && henchman.HenchGearColor != 0x4E9 )
				{
					henchman.HenchTitle = TavernPatrons.GetEvilTitle();
					friend.Title = henchman.HenchTitle;
					henchman.HenchGearColor = Utility.RandomList( 0x485, 0x497, 0x4E9 );
					henchman.Hue = henchman.HenchGearColor;
					henchman.HenchCloakColor = Utility.RandomList( 0x485, 0x497, 0x4E9 );
					if ( Utility.Random( 2 ) == 1 ){ henchman.HenchHatColor = henchman.HenchGearColor; } else { henchman.HenchHatColor = henchman.HenchCloakColor; }
				}

				if ( ( henchman.HenchBody == 401 ) && ( henchman.HenchRobe == 1 ) )
				{
					Item Armor4 = new GildedDress();
						Armor4.Hue = henchman.HenchGearColor;
						Armor4.Movable = false;
						BaseClothing Barmor4 = (BaseClothing)Armor4; Barmor4.StrRequirement = 1;
						Armor4.Name = "Robe";
						Armor4.LootType = LootType.Blessed;
							friend.AddItem( Armor4 );
				}
				else 
				{
					Item Armor4 = new Robe();
						Armor4.Hue = henchman.HenchGearColor;
						Armor4.Movable = false;
						BaseClothing Barmor4 = (BaseClothing)Armor4; Barmor4.StrRequirement = 1;
						Armor4.Name = "Robe";
						Armor4.LootType = LootType.Blessed;
							friend.AddItem( Armor4 );
				}

				Item Gear1 = new WizardsHat();
					Gear1.ItemID = henchman.HenchHelmID; if ( isOriental == true ){ Gear1.ItemID = 0x2798; }
					Gear1.Hue = henchman.HenchHatColor;
					Gear1.Movable = false;
					BaseClothing BarmorH = (BaseClothing)Gear1; BarmorH.StrRequirement = 1;
					Gear1.LootType = LootType.Blessed;
					Gear1.Name = "Hat";
						friend.AddItem( Gear1 );

				Item Gear3 = new WizardStaff();
					Gear3.ItemID = henchman.HenchWeaponID;
					Gear3.Movable = false;
					BaseWeapon Bwep = (BaseWeapon)Gear3; Bwep.StrRequirement = 1;
					Gear3.LootType = LootType.Blessed;
					Gear3.Name = "Weapon";
						friend.AddItem( Gear3 );

				if ( henchman.HenchCloak == 1 )
				{
					Item Capes = new Cloak();
						Capes.Hue = henchman.HenchCloakColor;
						Capes.Movable = false;
						BaseClothing Caper = (BaseClothing)Capes; Caper.StrRequirement = 1;
						Capes.LootType = LootType.Blessed;
							friend.AddItem( Capes );
				}

				Item Bootsy = new Boots();
					Bootsy.Hue = 0x967;
					Bootsy.Movable = false;
					BaseClothing Booty = (BaseClothing)Bootsy; Booty.StrRequirement = 1;
					Bootsy.LootType = LootType.Blessed;
						friend.AddItem( Bootsy );

				if ( henchman.HenchGloves > 1 )
				{
					Item Gloves = new LeatherGloves();
						Gloves.Hue = henchman.HenchCloakColor;
						Gloves.Movable = false;
						BaseArmor Glove = (BaseArmor)Gloves; Glove.StrRequirement = 1;
						Gloves.LootType = LootType.Blessed;
						Gloves.Name = "Gloves";
							friend.AddItem( Gloves );
				}
			}
			else if ( henchman is HenchmanFighterItem )
			{
				if ( isEvil == true && henchman.HenchGearColor != 0x485 && henchman.HenchGearColor != 0x497 && henchman.HenchGearColor != 0x4E9 )
				{
					henchman.HenchTitle = TavernPatrons.GetEvilTitle();
					friend.Title = henchman.HenchTitle;
					henchman.HenchGearColor = Utility.RandomList( 0x485, 0x497, 0x4E9 );
					henchman.Hue = henchman.HenchGearColor;
					henchman.HenchCloakColor = Utility.RandomList( 0x485, 0x497, 0x4E9 );
					if ( henchman.HenchHelmID > 0 ){ henchman.HenchHelmID = 0x2FBB; }
					switch ( Utility.Random( 2 ))		   
					{
						case 0: henchman.HenchShieldID = 0x2FC8; break;
						case 1: henchman.HenchShieldID = 0x1BC3; break;
					}
				}

				if ( henchman.HenchArmorType != 1 )
				{
					Item Armor0 = new PlateArms(); if ( isOriental == true ){ Armor0.ItemID = 0x2780; }
						Armor0.Hue = henchman.HenchGearColor;
						Armor0.Movable = false;
						BaseArmor Barmor0 = (BaseArmor)Armor0; Barmor0.StrRequirement = 1;
						Armor0.Name = "Armor";
						Armor0.LootType = LootType.Blessed;
							friend.AddItem( Armor0 );

					Item Armor1 = new PlateLegs(); if ( isOriental == true ){ Armor1.ItemID = 0x2788; }
						Armor1.Hue = henchman.HenchGearColor;
						Armor1.Movable = false;
						BaseArmor Barmor1 = (BaseArmor)Armor1; Barmor1.StrRequirement = 1;
						Armor1.Name = "Armor";
						Armor1.LootType = LootType.Blessed;
							friend.AddItem( Armor1 );

						if ( isOriental == true )
						{ 
							Item Bootsy = new Boots();
								Bootsy.Hue = 0x967;
								Bootsy.ItemID = 0x2796;
								Bootsy.Movable = false;
								BaseClothing Booty = (BaseClothing)Bootsy; Booty.StrRequirement = 1;
								Bootsy.LootType = LootType.Blessed;
									friend.AddItem( Bootsy );
						}

					Item Armor2 = new PlateGloves();
						Armor2.Hue = henchman.HenchGearColor;
						Armor2.Movable = false;
						BaseArmor Barmor2 = (BaseArmor)Armor2; Barmor2.StrRequirement = 1;
						Armor2.Name = "Armor";
						Armor2.LootType = LootType.Blessed;
							friend.AddItem( Armor2 );

					Item Armor3 = new PlateGorget(); if ( isOriental == true ){ Armor3.ItemID = 0x2779; }
						Armor3.Hue = henchman.HenchGearColor;
						Armor3.Movable = false;
						BaseArmor Barmor3 = (BaseArmor)Armor3; Barmor3.StrRequirement = 1;
						Armor3.Name = "Armor";
						Armor3.LootType = LootType.Blessed;
							friend.AddItem( Armor3 );

					if ( henchman.HenchBody == 401 )
					{
						Item Armor4 = new FemalePlateChest(); if ( isOriental == true ){ Armor4.ItemID = 0x277D; }
							Armor4.Hue = henchman.HenchGearColor;
							Armor4.Movable = false;
							BaseArmor Barmor4 = (BaseArmor)Armor4; Barmor4.StrRequirement = 1;
							Armor4.Name = "Armor";
							Armor4.LootType = LootType.Blessed;
								friend.AddItem( Armor4 );
					}
					else 
					{
						Item Armor4 = new PlateChest(); if ( isOriental == true ){ Armor4.ItemID = 0x277D; }
							Armor4.Hue = henchman.HenchGearColor;
							Armor4.Movable = false;
							BaseArmor Barmor4 = (BaseArmor)Armor4; Barmor4.StrRequirement = 1;
							Armor4.Name = "Armor";
							Armor4.LootType = LootType.Blessed;
								friend.AddItem( Armor4 );
					}
				}
				else
				{
					Item Armor0 = new RingmailArms(); if ( isOriental == true ){ Armor0.ItemID = 0x277F; }
						Armor0.Hue = henchman.HenchGearColor;
						Armor0.Movable = false;
						BaseArmor Barmor0 = (BaseArmor)Armor0; Barmor0.StrRequirement = 1;
						Armor0.Name = "Armor";
						Armor0.LootType = LootType.Blessed;
							friend.AddItem( Armor0 );

					Item Armor1 = new RingmailLegs(); if ( isOriental == true ){ Armor1.ItemID = 0x278D; }
						Armor1.Hue = henchman.HenchGearColor;
						Armor1.Movable = false;
						BaseArmor Barmor1 = (BaseArmor)Armor1; Barmor1.StrRequirement = 1;
						Armor1.Name = "Armor";
						Armor1.LootType = LootType.Blessed;
							friend.AddItem( Armor1 );

					Item Armor2 = new RingmailGloves();
						Armor2.Hue = henchman.HenchGearColor;
						Armor2.Movable = false;
						BaseArmor Barmor2 = (BaseArmor)Armor2; Barmor2.StrRequirement = 1;
						Armor2.Name = "Armor";
						Armor2.LootType = LootType.Blessed;
							friend.AddItem( Armor2 );

					Item Armor3 = new PlateGorget(); if ( isOriental == true ){ Armor3.ItemID = 0x2779; }
						Armor3.Hue = henchman.HenchGearColor;
						Armor3.Movable = false;
						BaseArmor Barmor3 = (BaseArmor)Armor3; Barmor3.StrRequirement = 1;
						Armor3.Name = "Armor";
						Armor3.LootType = LootType.Blessed;
							friend.AddItem( Armor3 );

					Item Armor4 = new ChainChest(); if ( isOriental == true ){ Armor4.ItemID = 0x277D; }
						Armor4.Hue = henchman.HenchGearColor;
						Armor4.Movable = false;
						BaseArmor Barmor4 = (BaseArmor)Armor4; Barmor1.StrRequirement = 1;
						Armor4.Name = "Armor";
						Armor4.LootType = LootType.Blessed;
							friend.AddItem( Armor4 );

					Item Bootsy = new Boots();
						Bootsy.Hue = 0x967;
						if ( isOriental == true ){ Bootsy.ItemID = 0x2796; }
						Bootsy.Movable = false;
						BaseClothing Booty = (BaseClothing)Bootsy; Booty.StrRequirement = 1;
						Bootsy.LootType = LootType.Blessed;
							friend.AddItem( Bootsy );
				}

				if ( henchman.HenchHelmID > 0 )
				{
					Item Gear1 = new PlateHelm();
						Gear1.ItemID = henchman.HenchHelmID; if ( isOriental == true ){ Gear1.ItemID = 0x2785; }
						Gear1.Hue = henchman.HenchGearColor;
						Gear1.Movable = false;
						BaseArmor BarmorH = (BaseArmor)Gear1; BarmorH.StrRequirement = 1;
						Gear1.LootType = LootType.Blessed;
						Gear1.Name = "Helm";
							friend.AddItem( Gear1 );
				}

				Item Gear2 = new BronzeShield();
					Gear2.ItemID = henchman.HenchShieldID;
					Gear2.Movable = false;
					BaseArmor BarmorS = (BaseArmor)Gear2; BarmorS.StrRequirement = 1;
					Gear2.LootType = LootType.Blessed;
					Gear2.Name = "Shield";
						friend.AddItem( Gear2 );

				if ( henchman.HenchWeaponType != 1 )
				{
					Item Gear3 = new Longsword();
						Gear3.ItemID = henchman.HenchWeaponID;
						Gear3.Movable = false;
						BaseWeapon Bwep = (BaseWeapon)Gear3; Bwep.StrRequirement = 1;
						Gear3.LootType = LootType.Blessed;
						Gear3.Name = "Weapon";
							friend.AddItem( Gear3 );
				}
				else
				{
					Item Gear3 = new Mace();
						Gear3.ItemID = henchman.HenchWeaponID;
						Gear3.Movable = false;
						BaseWeapon Bwep = (BaseWeapon)Gear3; Bwep.StrRequirement = 1;
						Gear3.LootType = LootType.Blessed;
						Gear3.Name = "Weapon";
							friend.AddItem( Gear3 );
				}

				if ( henchman.HenchCloak == 1 )
				{
					Item Capes = new Cloak();
						Capes.Hue = henchman.HenchCloakColor;
						Capes.Movable = false;
						BaseClothing Caper = (BaseClothing)Capes; Caper.StrRequirement = 1;
						Capes.LootType = LootType.Blessed;
							friend.AddItem( Capes );
				}
			}
			else
			{
				if ( isEvil == true && henchman.HenchGearColor != 0x485 && henchman.HenchGearColor != 0x497 && henchman.HenchGearColor != 0x4E9 )
				{
					henchman.HenchTitle = TavernPatrons.GetEvilTitle();
					friend.Title = henchman.HenchTitle;
					henchman.HenchGearColor = Utility.RandomList( 0x485, 0x497, 0x4E9 );
					henchman.Hue = henchman.HenchGearColor;
					henchman.HenchCloakColor = Utility.RandomList( 0x485, 0x497, 0x4E9 );
					if ( henchman.HenchHelmID > 0 && Utility.RandomBool() ){ henchman.HenchHelmID = 0x278E; }
				}

				if ( henchman.HenchArmorType != 1 )
				{
					Item Armor0 = new LeatherArms(); if ( isOriental == true ){ Armor0.ItemID = 0x277E; }
						Armor0.Hue = henchman.HenchGearColor;
						Armor0.Movable = false;
						BaseArmor Barmor0 = (BaseArmor)Armor0; Barmor0.StrRequirement = 1;
						Armor0.Name = "Armor";
						Armor0.LootType = LootType.Blessed;
							friend.AddItem( Armor0 );

					Item Armor1 = new LeatherLegs(); if ( isOriental == true ){ Armor1.ItemID = 0x2791; }
						Armor1.Hue = henchman.HenchGearColor;
						Armor1.Movable = false;
						BaseArmor Barmor1 = (BaseArmor)Armor1; Barmor1.StrRequirement = 1;
						Armor1.Name = "Armor";
						Armor1.LootType = LootType.Blessed;
							friend.AddItem( Armor1 );

					Item Armor2 = new LeatherGloves();
						Armor2.Hue = henchman.HenchGearColor;
						Armor2.Movable = false;
						BaseArmor Barmor2 = (BaseArmor)Armor2; Barmor2.StrRequirement = 1;
						Armor2.Name = "Armor";
						Armor2.LootType = LootType.Blessed;
							friend.AddItem( Armor2 );

					Item Armor3 = new LeatherGorget(); if ( isOriental == true ){ Armor3.ItemID = 0x277A; }
						Armor3.Hue = henchman.HenchGearColor;
						Armor3.Movable = false;
						BaseArmor Barmor3 = (BaseArmor)Armor3; Barmor3.StrRequirement = 1;
						Armor3.Name = "Armor";
						Armor3.LootType = LootType.Blessed;
							friend.AddItem( Armor3 );

					if ( henchman.HenchBody == 401 )
					{
						Item Armor4 = new FemaleLeatherChest(); if ( isOriental == true ){ Armor4.ItemID = 0x2793; }
							Armor4.Hue = henchman.HenchGearColor;
							Armor4.Movable = false;
							BaseArmor Barmor4 = (BaseArmor)Armor4; Barmor4.StrRequirement = 1;
							Armor4.Name = "Armor";
							Armor4.LootType = LootType.Blessed;
								friend.AddItem( Armor4 );
					}
					else 
					{
						Item Armor4 = new LeatherChest(); if ( isOriental == true ){ Armor4.ItemID = 0x2793; }
							Armor4.Hue = henchman.HenchGearColor;
							Armor4.Movable = false;
							BaseArmor Barmor4 = (BaseArmor)Armor4; Barmor4.StrRequirement = 1;
							Armor4.Name = "Armor";
							Armor4.LootType = LootType.Blessed;
								friend.AddItem( Armor4 );
					}
				}
				else
				{
					Item Armor0 = new StuddedArms(); if ( isOriental == true ){ Armor0.ItemID = 0x277F; }
						Armor0.Hue = henchman.HenchGearColor;
						Armor0.Movable = false;
						BaseArmor Barmor0 = (BaseArmor)Armor0; Barmor0.StrRequirement = 1;
						Armor0.Name = "Armor";
						Armor0.LootType = LootType.Blessed;
							friend.AddItem( Armor0 );

					Item Armor1 = new StuddedLegs(); if ( isOriental == true ){ Armor1.ItemID = 0x2791; }
						Armor1.Hue = henchman.HenchGearColor;
						Armor1.Movable = false;
						BaseArmor Barmor1 = (BaseArmor)Armor1; Barmor1.StrRequirement = 1;
						Armor1.Name = "Armor";
						Armor1.LootType = LootType.Blessed;
							friend.AddItem( Armor1 );

					Item Armor2 = new StuddedGloves();
						Armor2.Hue = henchman.HenchGearColor;
						Armor2.Movable = false;
						BaseArmor Barmor2 = (BaseArmor)Armor2; Barmor2.StrRequirement = 1;
						Armor2.Name = "Armor";
						Armor2.LootType = LootType.Blessed;
							friend.AddItem( Armor2 );

					Item Armor3 = new StuddedGorget(); if ( isOriental == true ){ Armor3.ItemID = 0x277A; }
						Armor3.Hue = henchman.HenchGearColor;
						Armor3.Movable = false;
						BaseArmor Barmor3 = (BaseArmor)Armor3; Barmor3.StrRequirement = 1;
						Armor3.Name = "Armor";
						Armor3.LootType = LootType.Blessed;
							friend.AddItem( Armor3 );

					if ( henchman.HenchBody == 401 )
					{
						Item Armor4 = new FemaleStuddedChest(); if ( isOriental == true ){ Armor4.ItemID = 0x2793; }
							Armor4.Hue = henchman.HenchGearColor;
							Armor4.Movable = false;
							BaseArmor Barmor4 = (BaseArmor)Armor4; Barmor4.StrRequirement = 1;
							Armor4.Name = "Armor";
							Armor4.LootType = LootType.Blessed;
								friend.AddItem( Armor4 );
					}
					else 
					{
						Item Armor4 = new StuddedChest(); if ( isOriental == true ){ Armor4.ItemID = 0x2793; }
							Armor4.Hue = henchman.HenchGearColor;
							Armor4.Movable = false;
							BaseArmor Barmor4 = (BaseArmor)Armor4; Barmor4.StrRequirement = 1;
							Armor4.Name = "Armor";
							Armor4.LootType = LootType.Blessed;
								friend.AddItem( Armor4 );
					}
				}

				Item Gear1 = new LeatherCap();
					Gear1.ItemID = henchman.HenchHelmID; if ( isOriental == true ){ Gear1.ItemID = 0x2798; }
					Gear1.Hue = henchman.HenchGearColor;
					Gear1.Movable = false;
					BaseArmor BarmorH = (BaseArmor)Gear1; BarmorH.StrRequirement = 1;
					Gear1.LootType = LootType.Blessed;
					Gear1.Name = "Helm";
						friend.AddItem( Gear1 );

				if ( henchman.HenchWeaponType != 1 )
				{
					Item Gear3 = new Bow();
						Gear3.ItemID = henchman.HenchWeaponID; if ( isOriental == true ){ Gear3.ItemID = 0x27A5; }
						Gear3.Movable = false;
						BaseWeapon Bwep = (BaseWeapon)Gear3; Bwep.StrRequirement = 1;
						Gear3.LootType = LootType.Blessed;
						Gear3.Name = "Weapon";
							friend.AddItem( Gear3 );
				}
				else
				{
					Item Gear3 = new Crossbow();
						Gear3.ItemID = henchman.HenchWeaponID;
						Gear3.Movable = false;
						BaseWeapon Bwep = (BaseWeapon)Gear3; Bwep.StrRequirement = 1;
						Gear3.LootType = LootType.Blessed;
						Gear3.Name = "Weapon";
							friend.AddItem( Gear3 );
				}

				if ( henchman.HenchCloak == 1 )
				{
					Item Capes = new Cloak();
						Capes.Hue = henchman.HenchCloakColor;
						Capes.Movable = false;
						BaseClothing Caper = (BaseClothing)Capes; Caper.StrRequirement = 1;
						Capes.LootType = LootType.Blessed;
							friend.AddItem( Capes );
				}

				Item Bootsy = new Boots();
					Bootsy.Hue = 0x967;
					if ( isOriental == true ){ Bootsy.ItemID = 0x2796; }
					Bootsy.Movable = false;
					BaseClothing Booty = (BaseClothing)Bootsy; Booty.StrRequirement = 1;
					Bootsy.LootType = LootType.Blessed;
						friend.AddItem( Bootsy );
			}
		}

		public static int GetHue( int nValue )
		{
			int Hue = 0;
			switch( nValue )
			{
				case 0: Hue = Utility.RandomNeutralHue(); break;
				case 1: Hue = Utility.RandomRedHue(); break;
				case 2: Hue = Utility.RandomBlueHue(); break;
				case 3: Hue = Utility.RandomGreenHue(); break;
				case 4: Hue = Utility.RandomYellowHue(); break;
				case 5: Hue = Utility.RandomSnakeHue(); break;
				case 6: Hue = Utility.RandomMetalHue(); break;
				case 7: Hue = Utility.RandomAnimalHue(); break;
				case 8: Hue = Utility.RandomSlimeHue(); break;
				case 9: Hue = Utility.RandomOrangeHue(); break;
				case 10: Hue = Utility.RandomPinkHue(); break;
				case 11: Hue = Utility.RandomDyedHue(); break;
				case 12: Hue = Utility.RandomList( 0x467E, 0x481, 0x482, 0x47F ); break;
				case 13: Hue = Utility.RandomList( 0x54B, 0x54C, 0x54D, 0x54E, 0x54F, 0x550, 0x4E7, 0x4E8, 0x4E9, 0x4EA, 0x4EB, 0x4EC ); break;
				case 14: Hue = Utility.RandomList( 0x551, 0x552, 0x553, 0x554, 0x555, 0x556, 0x4ED, 0x4EE, 0x4EF, 0x4F0, 0x4F1, 0x4F2 ); break;
				case 15: Hue = Utility.RandomList( 0x557, 0x558, 0x559, 0x55A, 0x55B, 0x55C, 0x4F3, 0x4F4, 0x4F5, 0x4F6, 0x4F7, 0x4F8 ); break;
				case 16: Hue = Utility.RandomList( 0x55D, 0x55E, 0x55F, 0x560, 0x561, 0x562, 0x4F9, 0x4FA, 0x4FB, 0x4FC, 0x4FD, 0x4FE ); break;
				case 17: Hue = Utility.RandomList( 0xB93, 0xB94, 0xB95, 0xB96, 0xB83 ); break;
				case 18: Hue = Utility.RandomList( 0x1, 0x497, 0x965, 0x966, 0x96B, 0x96C ); break;
			}
			return Hue;
		}
	}
}
