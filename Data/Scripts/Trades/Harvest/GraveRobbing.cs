using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Misc;
using Server.Network;

namespace Server.Engines.Harvest
{
	public class GraveRobbing : HarvestSystem
	{
		private static GraveRobbing m_System;

		public static GraveRobbing System
		{
			get
			{
				if ( m_System == null )
					m_System = new GraveRobbing();

				return m_System;
			}
		}

		private HarvestDefinition m_Definition;

		public HarvestDefinition Definition
		{
			get{ return m_Definition; }
		}

		private GraveRobbing()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region GraveRobbing
			HarvestDefinition grave = new HarvestDefinition();
			grave.BankWidth = 1;
			grave.BankHeight = 1;
			grave.MinTotal = 1;
			grave.MaxTotal = 3;
			grave.MinRespawn = TimeSpan.FromMinutes( 50.0 );
			grave.MaxRespawn = TimeSpan.FromMinutes( 70.0 );
			grave.Skill = SkillName.Forensics;
			grave.Tiles = m_GraveTiles;
			grave.MaxRange = 1;
			grave.ConsumedPerHarvest = 1 * MyServerSettings.Resources();
			grave.ConsumedPerIslesDreadHarvest = grave.ConsumedPerHarvest + (int)(grave.ConsumedPerHarvest/2) + 2;
			grave.EffectActions = new int[]{ 14 };
			grave.EffectSounds = new int[]{ 0x125, 0x126 };
			grave.EffectCounts = new int[]{ 1 };
			grave.EffectDelay = TimeSpan.FromSeconds( 1.6 );
			grave.EffectSoundDelay = TimeSpan.FromSeconds( 0.9 );
			grave.NoResourcesMessage = 501756; // Nothing worth taking..
			grave.FailMessage = 501756; // Nothing worth taking
			grave.OutOfRangeMessage = 500446; // That is too far away.
			grave.PackFullMessage = 500720; // You don't have enough room in your backpack!
			grave.ToolBrokeMessage = 1044038; // You broke your tool.

			res = new HarvestResource[]
			{
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca alguns ossos na sua mochila", typeof( Bones ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca um braço podre na sua mochila", typeof( LeftArm ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca um braço podre na sua mochila", typeof( RightArm ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca um torso podre na sua mochila", typeof( Torso ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca uma perna podre na sua mochila", typeof( LeftLeg ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca uma perna podre na sua mochila", typeof( RightLeg ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca um osso na sua mochila", typeof( Bone ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca uma caixa torácica na sua mochila", typeof( RibCage ) ),
				new HarvestResource( 000.0, 000.0, 150.0, "Você coloca uma pilha de ossos na sua mochila", typeof( BonePile ) ),
				new HarvestResource( 020.0, 010.0, 150.0, "Você coloca um pouco de pó de sepultura na sua mochila", typeof( GraveDust ) ),
				new HarvestResource( 020.0, 010.0, 150.0, "Você coloca um pouco de terra na sua mochila", typeof( FertileDirt ) ),
				new HarvestResource( 050.0, 040.0, 150.0, "Você coloca uma poção na sua mochila", typeof( LesserCurePotion ) ),
				new HarvestResource( 050.0, 040.0, 150.0, "Você coloca alguns reagentes na sua mochila", typeof( Brimstone ) ),
				new HarvestResource( 050.0, 040.0, 150.0, "Você coloca um pergaminho na sua mochila", typeof( HealScroll ) )
			};

			veins = new HarvestVein[]
			{
				new HarvestVein( 05.0, 0.0, res[0], res[0] ),
				new HarvestVein( 05.0, 0.0, res[1], res[0] ),
				new HarvestVein( 05.0, 0.0, res[2], res[0] ),
				new HarvestVein( 05.0, 0.0, res[3], res[0] ),
				new HarvestVein( 05.0, 0.0, res[4], res[0] ),
				new HarvestVein( 05.0, 0.0, res[5], res[0] ),
				new HarvestVein( 05.0, 0.0, res[6], res[0] ),
				new HarvestVein( 05.0, 0.5, res[7], res[0] ),
				new HarvestVein( 05.0, 0.5, res[8], res[0] ),
				new HarvestVein( 13.0, 0.5, res[9], res[0] ),
				new HarvestVein( 12.0, 0.5, res[10], res[0] ),
				new HarvestVein( 10.0, 0.5, res[11], res[0] ),
				new HarvestVein( 10.0, 0.5, res[12], res[0] ),
				new HarvestVein( 10.0, 0.5, res[13], res[0] )
			};

			if ( Core.ML )
			{
				grave.BonusResources = new BonusHarvestResource[] // cos this is mining after all
				{
					new BonusHarvestResource( 0, 78.0, null, null ),	//Nothing
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicVase ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicArts ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicGrave ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicWeapon ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicArmor ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicJewels ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicInstrument ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicScrolls ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicReagent ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicOrbs ) ),
					new BonusHarvestResource( 60, 2.0, 1074542, typeof( DDRelicBook ) )
				};
			}

			grave.RandomizeVeins = Core.ML;

			grave.Resources = res;
			grave.Veins = veins;

			m_Definition = grave;
			Definitions.Add( grave );
			#endregion
		}

		public override bool CheckHarvest( Mobile from, Item tool )
		{
			if ( !base.CheckHarvest( from, tool ) )
				return false;

			if ( from.Mounted )
			{
				from.SendMessage("Você não pode cavar sepulturas enquanto estiver montado.");
				return false;
			}
			else if ( from.IsBodyMod && !from.Body.IsHuman && from.RaceID < 1 )
			{
				from.SendMessage("Você não pode cavar sepulturas enquanto estiver polimorfado.");
				return false;
			}

			return true;
		}

		public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
				return false;

			else if ( from.Mounted )
			{
				from.SendMessage("Você não pode cavar sepulturas enquanto estiver montado.");
				return false;
			}
			else if ( from.IsBodyMod && !from.Body.IsHuman && from.RaceID < 1 )
			{
				from.SendMessage("Você não pode cavar sepulturas enquanto estiver polimorfado.");
				return false;
			}

			return true;
		}

		public override bool BeginHarvesting( Mobile from, Item tool )
		{
			if ( !base.BeginHarvesting( from, tool ) )
				return false;

			from.SendMessage("Qual sepultura você quer escavar?");
			return true;
		}

		public override void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
		{
			from.SendMessage( "Você não pode cavar ali." );
		}

		public override void OnHarvestStarted( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			base.OnHarvestStarted( from, tool, def, toHarvest );

			if ( Core.ML )
				from.RevealingAction();
		}

		private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				 0, -1,
				 0,  1,
				 1, -1,
				 1,  0,
				 1,  1
			};

		public override void OnHarvestFinished( Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
		{
			Map map = from.Map;
			Point3D loc = from.Location;

			HarvestResource res = vein.PrimaryResource;
			
			if  ( res == resource )
			{
				try
				{
					if ( from.Karma > -2459 ){ Titles.AwardKarma( from, -50, true ); }

					if ( Utility.RandomMinMax( 1, 100 ) < 3 ) // CHECK TO SEE IF THEY WERE WITNESSED DIGGING UP A GRAVE 2%
					{
						int caught = 1;
						if ( from.Skills[SkillName.Hiding].Value >= 30 )
						{
							from.SendMessage( "Alguém passou por aqui, mas sua furtividade o protegeu de ser visto." );
							if ( from.CheckSkill( SkillName.Stealth, 0, 100 ) ){ caught = 0; }
						}
						if ( caught > 0 )
						{
							from.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Alguém te viu à distância!", from.NetState);
							from.SendMessage( "Você foi denunciado como criminoso!" );
							from.Criminal = true;
							Server.Items.DisguiseTimers.RemoveDisguise( from );
						}
					}

					map = from.Map;

					if ( map == null )
						return;

					BaseCreature spawned = new Zombie();

					switch ( Utility.Random( 20 ))
					{
						case 0: spawned = new Zombie(); break;
						case 1: spawned = new Skeleton(); break;
						case 2: spawned = new Ghoul(); break;
						case 3: spawned = new Shade(); break;
						case 4: spawned = new Spectre(); break;
						case 5: spawned = new Wraith(); break;
						case 6: spawned = new BoneKnight(); break; 
						case 7: spawned = new BoneMagi(); break;
						case 8: spawned = new Ghostly(); break;
						case 9: spawned = new Lich(); break;
						case 10: spawned = new LichLord(); break;
						case 11: spawned = new Mummy(); break;
						case 12: spawned = new RottingCorpse(); break;
						case 13: spawned = new Shade(); break;
						case 14: spawned = new SkeletalKnight(); break;
						case 15: spawned = new SkeletalWizard(); break;
						case 16: spawned = new SkeletalMage(); break;
						case 17: spawned = new Phantom(); break;
						case 18: spawned = new Vampire(); break;
						case 19: spawned = new Bodak(); break;
					}

					int nSpiritualism = (int)(from.Skills[SkillName.Spiritualism].Value / 10);

					string sSaying = "";
					switch ( Utility.Random( 9 ))
					{
						case 0: sSaying = "Quem me perturbou!"; break;
						case 1: sSaying = "Você ousa roubar de minha sepultura?"; break;
						case 2: sSaying = "Aqueles que tomam de mim se juntarão a mim!"; break;
						case 3: sSaying = "Sua alma agora é minha para ser tomada!"; break;
						case 4: sSaying = "Quem ousa me despertar?"; break;
						case 5: sSaying = "Sua vida será extinguida!"; break;
						case 6: sSaying = "Você não tem respeito pelos mortos?"; break;
						case 7: sSaying = "Estive esperando para me banquetear com os vivos!"; break;
						case 8: sSaying = "Logo você se juntará à minha legião dos mortos!"; break;
					}

					if ( ( spawned != null ) && ( Utility.Random( 100 ) > (nSpiritualism + 85) ) ) // 10% chance you will get a grave raiser
					{
						from.CheckSkill( SkillName.Spiritualism, 0, 100 );
						int offset = Utility.Random( 8 ) * 2;

						for ( int i = 0; i < m_Offsets.Length; i += 2 )
						{
							int x = from.X + m_Offsets[(offset + i) % m_Offsets.Length];
							int y = from.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

							if ( map.CanSpawnMobile( x, y, from.Z ) )
							{
								spawned.OnBeforeSpawn( new Point3D( x, y, from.Z ), map );
								spawned.Home = new Point3D( x, y, from.Z );
								spawned.RangeHome = 5;
								spawned.Title += " [Awakened]";
								spawned.MoveToWorld( new Point3D( x, y, from.Z ), map );
								spawned.Say(sSaying);
								spawned.IsTempEnemy = true;
								spawned.Combatant = from;
								return;
							}
							else
							{
								int z = map.GetAverageZ( x, y );

								if ( map.CanSpawnMobile( x, y, z ) )
								{
									spawned.OnBeforeSpawn( new Point3D( x, y, z ), map );
									spawned.Home = new Point3D( x, y, z );
									spawned.RangeHome = 5;
									spawned.Title += " [Awakened]";
									spawned.MoveToWorld( new Point3D( x, y, z ), map );
									spawned.Say(sSaying);
									spawned.IsTempEnemy = true;
									spawned.Combatant = from;
									return;
								}
							}
						}
						spawned.OnBeforeSpawn( from.Location, from.Map );
						spawned.Home = from.Location;
						spawned.RangeHome = 5;
						spawned.Title += " [Awakened]";
						spawned.MoveToWorld( from.Location, from.Map );
						spawned.Say(sSaying);
						spawned.IsTempEnemy = true;
						spawned.Combatant = from;
					}

					int digger = (int)(from.Skills[SkillName.Forensics].Value / 10);
					if ( (2+digger) > Utility.Random( 100 ) ) // chance to dig up a box
					{
						if ( from.CheckSkill( SkillName.Forensics, 0, 125 ) )
						{
							Item chest = new GraveChest(6, from);
							switch ( Utility.Random( 10+digger ))
							{
								case 0: chest = new GraveChest(1, from); break;
								case 1: chest = new GraveChest(1, from); break;
								case 2: chest = new GraveChest(1, from); break;
								case 3: chest = new GraveChest(1, from); break;
								case 4: chest = new GraveChest(1, from); break;
								case 5: chest = new GraveChest(1, from); break;
								case 6: chest = new GraveChest(2, from); break;
								case 7: chest = new GraveChest(2, from); break;
								case 8: chest = new GraveChest(2, from); break;
								case 9: chest = new GraveChest(2, from); break;
								case 10: chest = new GraveChest(2, from); break;
								case 11: chest = new GraveChest(3, from); break;
								case 12: chest = new GraveChest(3, from); break;
								case 13: chest = new GraveChest(3, from); break;
								case 14: chest = new GraveChest(3, from); break;
								case 15: chest = new GraveChest(4, from); break;
								case 16: chest = new GraveChest(4, from); break;
								case 17: chest = new GraveChest(4, from); break;
								case 18: chest = new GraveChest(5, from); break;
								case 19: chest = new GraveChest(5, from); break;
							}
							if ( chest != null )
							{
								chest.MoveToWorld( loc, map );
								from.SendMessage( "Você escava um baú de cemitério." );
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		public static void Initialize()
		{
			Array.Sort( m_GraveTiles );
		}

		#region Tile lists
		private static int[] m_GraveTiles = new int[]
		{
			0x4ED2, 0x4ED3, 0x4ED4, 0x4ED5, 0x4ED6, 0x4ED7, 0x4ED8, 0x4ED9, 0x4EDA, 0x4EDB, 0x4EDC,
			0x4EDD, 0x4EDE, 0x4EDF, 0x4EE0, 0x4EE1, 0x4EE7, 
			0x5164, 0x5165, 0x5166, 0x5167, 0x5168, 0x5169, 0x516A, 0x516B, 0x516C, 0x516D, 0x516E, 0x516F,
			0x5170, 0x5171, 0x5172, 0x5173, 0x5174, 0x5175, 0x5176, 0x5177, 0x5178, 0x5170, 0x517A,
			0x517B, 0x517C, 0x517D, 0x517E, 0x517F, 0x5180, 0x5181, 0x5182, 0x5183, 
			0x2BEA, 0x2BEB, 0x2BEC, 0x2BED, 0x2BEE, 0x2BEF, 0x2BF0, 0x2BF1, 
			0x137A, 0x137B, 0x137C, 0x137D, 0x137F, 
			0x1381, 0x1387, 0x1388, 0x1389, 0x138A, 0x138B, 0x138C, 0x138D, 0x138E

		};
		#endregion
	}
}