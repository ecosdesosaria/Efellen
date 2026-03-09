using System;
using Server;
using Server.ContextMenus;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Mobiles
{
	public class TrainingSpirits : Citizens
	{
		[Constructable]
		public TrainingSpirits()
		{
			Blessed = true;
			CantWalk = true;
			AI = AIType.AI_Melee;
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextTalk )
			{
				foreach ( Item shrine in this.GetItemsInRange( 6 ) )
				{
					if ( 	shrine is AnkhWest || shrine is AnkhNorth || shrine is AltarEvil || shrine is AltarDurama || shrine is AltarWizard || shrine is AltarGargoyle || 
							shrine is AltarDaemon || shrine is AltarSea || shrine is AltarStatue || shrine is AltarShrineSouth || shrine is AltarShrineEast || 
							shrine is AltarDryad || shrine is AltarGodsSouth || shrine is AltarGodsEast )
					{
						Point3D goal = shrine.Location;
						Direction d = this.GetDirectionTo( goal );
						this.Direction = d;

						int action = Utility.RandomMinMax(1,4);

						string name = "Spirits";
							if ( shrine is AltarDurama ){ name = "Durama"; }
							else if ( shrine is AltarGargoyle ){ name = "Sin'Vraal"; }
							else if ( shrine is AltarWizard ){ name = "Archmage"; }
							else if ( shrine is AltarDaemon )
							{
								name = "Azrael";
								if ( shrine.ItemID == 0x6393 || shrine.ItemID == 0x6394 ){ name = "Ktulu"; }
							}
							else if ( shrine is AltarSea )
							{
								name = "Amphitrite";
								if ( shrine.ItemID == 0x4FB1 || shrine.ItemID == 0x4FB2 ){ name = "Poseidon"; }
								else if ( shrine.ItemID == 0x6395 ){ name = "Neptune"; }
							}
							else if ( shrine is AltarStatue ){ name = "Goddess"; }
							else if ( shrine is AltarEvil ){ name = "Deathly Reaper"; }
							else if ( shrine is AltarDryad ){ name = "Ancient Dryad"; }

						if ( action == 1 )
						{
							this.Say( "*meditando*" );
							this.PlaySound( 0xF9 );
							this.Animate( 269, 5, 1, true, false, 0 );
						}
						else if ( action == 2 )
						{
							this.Animate( 269, 5, 1, true, false, 0 );
							string plead = "Oh Grandes Espíritos";
							string resurrect = ", por favor ressuscitem";
							string who = NameList.RandomName( "male" ); 
								if ( Utility.RandomBool() ){ who = NameList.RandomName( "female" ); }
								if ( Utility.RandomBool() ){ who = who + " " + TavernPatrons.GetTitle(); }

							string dungeon = QuestCharacters.SomePlace( "tavern" );	
								if ( Utility.RandomBool() ){ dungeon = RandomThings.MadeUpDungeon(); }

							string died = "foi morto";
							switch( Utility.RandomMinMax( 0, 5 ) )
							{
								case 1: died = "foi abatido";				break;
								case 2: died = "foi derrotado";			break;
								case 3: died = "foi assassinado";			break;
								case 4: died = "pereceu";			break;
								case 5: died = "encontrou seu fim";		break;
							}

							switch ( Utility.Random( 8 ) )
							{
								case 0: resurrect = ", por favor ressuscitem";				break;
								case 1: resurrect = ", por favor tragam de volta";				break;
								case 2: resurrect = ", humildemente peço que ressuscitem";	break;
								case 3: resurrect = ", imploro que ressuscitem";			break;
								case 4: resurrect = ", humildemente peço que tragam de volta";	break;
								case 5: resurrect = ", imploro que tragam de volta";		break;
								case 6: resurrect = ", por favor devolvam a vida a";		break;
								case 7: resurrect = ", devolvam a vida a";				break;
							}

							switch ( Utility.Random( 7 ) )
							{
								case 0: plead = "Oh " + name + "";				break;
								case 1: plead = "Oh Grande " + name + "";		break;
								case 2: plead = "Por favor " + name + "";			break;
								case 3: plead = "Por favor Grande " + name + "";	break;
								case 4: plead = "" + name + "";					break;
								case 5: plead = "Grande " + name + "";			break;
								case 6: plead = "Oh Grande " + name + "";		break;
							}

							switch ( Utility.Random( 3 ) )
							{
								case 0: plead = plead + resurrect + " " + who + ".";											break;
								case 1: plead = plead + resurrect + " " + who + ", que " + died + ".";							break;
								case 2: plead = plead + resurrect + " " + who + ", que " + died + " em " + dungeon + ".";		break;
							}

							this.Say( plead );
						}
						else if ( action == 3 )
						{
							this.Animate( 230, 5, 1, true, false, 0 );
							this.PlaySound( 0x2E6 );
							string praise = "Oh Grandes Espíritos";
							string gold = ", aceitem este ouro como meu";
							string give = " tributo.";

							switch ( Utility.Random( 6 ) )
							{
								case 0: give = " tributo.";		break;
								case 1: give = " presente.";		break;
								case 2: give = " louvor.";		break;
								case 3: give = " devoção.";	break;
								case 4: give = " honra.";		break;
								case 5: give = " respeito.";		break;
							}

							switch ( Utility.Random( 8 ) )
							{
								case 0: gold = ", aceitem este ouro como meu";				break;
								case 1: gold = ", eu dou este ouro como meu";				break;
								case 2: gold = ", humildemente ofereço este ouro como meu";		break;
								case 3: gold = ", ofereço este ouro como meu";				break;
								case 4: gold = ", tomem este ouro como meu";				break;
								case 5: gold = ", eu me desfaço deste ouro como meu";			break;
								case 6: gold = ", humildemente sacrifico este ouro como meu";	break;
								case 7: gold = ", sacrifico este ouro como meu";			break;
							}

							switch ( Utility.Random( 7 ) )
							{
								case 0: praise = "Oh " + name + "";		break;
								case 1: praise = "Oh Grande " + name + "";	break;
								case 2: praise = "Por favor " + name + "";	break;
								case 3: praise = "Por favor Grande " + name + "";	break;
								case 4: praise = "" + name + "";	break;
								case 5: praise = "Grande " + name + "";	break;
								case 6: praise = "Oh Grande " + name + "";	break;
							}

							praise = praise + gold + give;
							this.Say( praise );
						}
						else
						{
							if ( this.Karma < 0 )
							{
								this.Say( "Xtee Mee Glau" );
								this.PlaySound( 0x481 );
							}
							else
							{
								this.Say( "Anh Mi Sah Ko" );
								this.PlaySound( 0x24A );
							}
							if ( Utility.RandomBool() )
							{
								if ( this.Karma < 0 )
								{
									this.FixedParticles( 0x3400, 1, 15, 9501, 2100, 4, EffectLayer.Waist );
								}
								else
								{
									this.FixedParticles( 0x376A, 9, 32, 5005, 0xB70, 0, EffectLayer.Waist );
								}
							}
							if ( Utility.RandomBool() )
							{
								this.Animate( 269, 5, 1, true, false, 0 );
							}
						}

						m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 12, 20 ) ));
					}
				}
			}
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			Server.Misc.MorphingTime.CheckNecromancer( this );
		}

		public TrainingSpirits( Serial serial ) : base( serial )
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