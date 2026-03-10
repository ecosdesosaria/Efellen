using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Custom.DefenderOfTheRealm.Vow;

namespace Server.Items
{
	[Flipable( 0x14EF, 0x14F0 )]
	public class TamingBOD : Item
	{
		private int reward;
		private int m_amount;
		private int m_tamed;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Reward
		{
			get{ return reward; }
			set{ reward = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int AmountToTame
		{
			get{ return m_amount; }
			set{ m_amount = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int AmountTamed
		{
			get{ return m_tamed; }
			set{ m_tamed = value; }
		}
		
		[Constructable]
		public TamingBOD() : base( 0x14EF )
		{
			Weight = 1;
			Movable = true;
			double pimple = Utility.RandomDouble();
			if (pimple >= 0.98)
				AmountToTame = Utility.RandomMinMax( 20, 30 );
			else if (pimple >= 0.94 )
				AmountToTame = Utility.RandomMinMax( 10, 20 );
			else if (pimple >= 0.70 )
				AmountToTame = Utility.RandomMinMax( 5, 10 );
			else 
				AmountToTame = Utility.RandomMinMax( 1, 5 );

			Reward = 0;
			Name = "Contract: " + AmountToTame + " creatures";
			AmountTamed = 0;
		}
		
		[Constructable]
		public TamingBOD(  int atk ) : base( 0x14F0 )
		{
			Weight = 1;
			Movable = true;
			AmountToTame = atk;
			Reward = 0;
			Name = "Contract: " + AmountToTame + " creatures";
			AmountTamed = 0;
		}
		
		[Constructable]
		public TamingBOD( int ak, int atk, int gpreward) : this( atk )
		{
			AmountTamed = ak;
			Reward = gpreward;
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			
			base.AddNameProperties( list );	
			
			list.Add( "This deed is currently worth " + Reward + " gold." ); 
			list.Add( "Add more creatures to increase the payout." );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) )
			{
				from.SendGump( new TamingBODGump( from, this ) );
			}
			else
			{
				from.SendLocalizedMessage( 1047012 ); // This contract must be in your backpack to use it
			}
		}

		private static Item TryGetItem(int goldAmount)
		{
			int reward = 0;
			if (goldAmount < 1000)
			{
				reward = Utility.RandomMinMax(0, 60);
			}
			else if (goldAmount < 5000)
			{
				reward = Utility.RandomMinMax(50, 250);
			}
			else if (goldAmount < 10000)
			{
				reward = Utility.RandomMinMax(100, 350);
			}
			else if (goldAmount < 30000)
			{
				reward = Utility.RandomMinMax(200, 400);
			}
			else if (goldAmount < 60000)
			{
				reward = Utility.RandomMinMax(300, 500);
			}
			else if (goldAmount < 100000)
			{
				reward = Utility.RandomMinMax(400, 510);
			}
			else if (goldAmount < 120000)
			{
				reward = Utility.RandomMinMax(450, 525);
			}
			else if (goldAmount >= 160000)
			{
				reward = Utility.RandomMinMax(475, 600);
			}

			if (500 < reward)  // impossible finds
			{
				if(Utility.RandomDouble() < 0.05)
				{
					return new ParagonPetDeed();
				} 
				else if (Utility.RandomDouble() < 0.15)
				{
					return Construct( m_MegaRareMorph ) as Item;
				}
			}

			if (470 < reward)  // impossible finds
			{
				if(Utility.RandomDouble() < 0.01)
				{
					return new ParagonPetDeed();
				} 
				else if (Utility.RandomDouble() < 0.05)
				{
					return new PetEasingDeed();
				}
				else if (Utility.RandomDouble() < 0.15)
				{
					return new PetDyeTub();
				}
				else if (Utility.RandomDouble() < 0.20)
				{
					return new PetGrowthDeedStrong();
				}
				else if (Utility.RandomDouble() < 0.25)
				{
					return Construct( m_RareMorph ) as Item;
				}
			}

			if (375 < reward) // rare finds
			{
				if(Utility.RandomDouble() < 0.05)
				{
					return new PetEasingDeed();
				} 
				else if (Utility.RandomDouble() < 0.10)
				{
					return new BallOfSummoning();
				}
				else if (Utility.RandomDouble() < 0.20)
				{
					return new BraceletOfBinding();
				}
				else if (Utility.RandomDouble() < 0.30)
				{
					return new PetGrowthDeedMid();
				}
				else if (Utility.RandomDouble() < 0.35)
				{
					return Construct( m_MidMorph ) as Item;
				}
			}

			if (250 < reward) // medium finds
			{
				if(Utility.RandomDouble() < 0.05)
				{
					return new PetControlDeed();
				} 
				else if (Utility.RandomDouble() < 0.10)
				{
					return new PetTrainer();
				}
				else if (Utility.RandomDouble() < 0.30)
				{
					return new PetGrowthDeedWeak();
				}
				else if (Utility.RandomDouble() < 0.35)
				{
					return Construct( m_LowMorph ) as Item;
				}
			}
			
			if (100 < reward)// easy finds
			{
				if(Utility.RandomDouble() < 0.05)
				{
					return new PowderOfTranslocation(10);
				} 
				else if (Utility.RandomDouble() < 0.30)
				{
					return new PetGrowthDeedWeak();
				}
				else if (Utility.RandomDouble() < 0.35)
				{
					return Construct( m_LowMorph ) as Item;
				}
			}
			return null;
		}

		public static bool PayRewardTo ( Mobile m_from, TamingBOD MCparent )
		{
			if ( MCparent.AmountTamed >= MCparent.AmountToTame)
			{
				Container backpack = m_from.Backpack;
				
				Container rewardBag = new Bag();
				rewardBag.Name = "Druidic satchel";
				rewardBag.Hue = 669;
				
				Item specialItem = TryGetItem(MCparent.Reward);
				if (specialItem != null)
				{
					rewardBag.DropItem(specialItem);
					m_from.SendMessage("Você recebeu um drop especial!");
				}
				
				int goldAmount = MCparent.Reward;
				PlayerMobile druid = m_from as PlayerMobile;
				
				if(druid.NpcGuild == NpcGuild.DruidsGuild)
				{
					if (goldAmount < 1000)
					{
						VowRewardHelper.GenerateRewards(m_from, 5, rewardBag, VowType.Wilds);
						rewardBag.DropItem(new MarksOfTheWilds(Utility.RandomMinMax(10, 25)));
					}
					else if (goldAmount < 10000)
					{
						VowRewardHelper.GenerateRewards(m_from, 10, rewardBag, VowType.Wilds);
						rewardBag.DropItem(new MarksOfTheWilds(Utility.RandomMinMax(35, 75)));
					}
					else if (goldAmount < 30000)
					{
						VowRewardHelper.GenerateRewards(m_from, 20, rewardBag, VowType.Wilds);
						rewardBag.DropItem(new MarksOfTheWilds(Utility.RandomMinMax(105, 145)));
					}
					else if (goldAmount < 60000)
					{
						VowRewardHelper.GenerateRewards(m_from, 30, rewardBag, VowType.Wilds);
						rewardBag.DropItem(new MarksOfTheWilds(Utility.RandomMinMax(185, 225)));
					}
					else if (goldAmount < 100000)
					{
						VowRewardHelper.GenerateRewards(m_from, 40, rewardBag, VowType.Wilds);
						rewardBag.DropItem(new MarksOfTheWilds(Utility.RandomMinMax(285, 345)));
					}
					else if (goldAmount >= 160000)
					{
						VowRewardHelper.GenerateRewards(m_from, 50, rewardBag, VowType.Wilds);
						rewardBag.DropItem(new MarksOfTheWilds(Utility.RandomMinMax(425, 500)));
					}
				}
				rewardBag.DropItem(new BankCheck(MCparent.Reward));
				
				backpack.DropItem(rewardBag);
				m_from.SendMessage("Sua recompensa foi colocada na sua mochila.");
			
				return true;
			}
			else
			{
				m_from.SendMessage("Há algo errado com este documento.");		
			}	

			return false;				
		}		

		private static Type[] m_LowMorph = new Type[]
			{

				typeof( BodyChangeBlackBearStatue),
				typeof( BodyChangeBrownBearStatue),
				typeof( BodyChangeCatStatue),
				typeof( BodyChangeChickenStatue),
				typeof( BodyChangeCowStatue),
				typeof( BodyChangeDogStatue),
				typeof( BodyChangeEagleStatue),
				typeof( BodyChangeFoxStatue),
				typeof( BodyChangeGiantRatStatue),
				typeof( BodyChangeGoatStatue),
				typeof( BodyChangeGorillaStatue),
				typeof( BodyChangeHindStatue),
				typeof( BodyChangeLlamaStatue),
				typeof( BodyChangeOstardStatue),
				typeof( BodyChangePantherStatue),
				typeof( BodyChangePigStatue),
				typeof( BodyChangeRabbitStatue),
				typeof( BodyChangeRatStatue),
				typeof( BodyChangeSheepStatue),
				typeof( BodyChangeSquirrelStatue),
				typeof( BodyChangeStagStatue),
				typeof( BodyChangeLizardStatue)
			};

		private static Type[] m_MidMorph = new Type[]
			{

				typeof( BodyChangeCraneStatue),
				typeof( BodyChangeFerretStatue),
				typeof( BodyChangeGiantSnakeStatue),
				typeof( BodyChangeGiantToadStatue),
				typeof( BodyChangeHellHoundStatue),
				typeof( BodyChangeKirinStatue),
				typeof( BodyChangeLionStatue),
				typeof( BodyChangePolarBearStatue),
				typeof( BodyChangeScorpionStatue),
				typeof( BodyChangeSlimeStatue),
				typeof( BodyChangeSnakeStatue),
				typeof( BodyChangeSpiderStatue),
				typeof( BodyChangeToadStatue),
                typeof( BodyChangeKongStatue),
                typeof( BodyChangeTurtleStatue)
			};

		private static Type[] m_RareMorph = new Type[]
			{

				typeof( BodyChangeFrenziedOstardStatue),
				typeof( BodyChangeFrostSpiderStatue),
				typeof( BodyChangeGremlinStatue),
				typeof( BodyChangeMysticalFoxStatue),
				typeof( BodyChangePlainsBeastStatue),
				typeof( BodyChangeRockLobsterStatue),
				typeof( BodyChangeShadowLionStatue),
				typeof( BodyChangeTigerBeetleStatue),
				typeof( BodyChangeWidowSpiderStatue),
				typeof( BodyChangeScorpoidStatue),
                typeof( BodyChangeFlyingFangsStatue),
                typeof(BodyChangeGryphonStatue)
			};

		private static Type[] m_MegaRareMorph = new Type[]
			{

				typeof( BodyChangeCerberusStatue),
				typeof( BodyChangeDepthsBeastStatue),
                typeof( BodyChangeGazerHoundStatue),
                typeof( BodyChangeGlassSpiderStatue),
				typeof( BodyChangeHornedBeetleStatue),
				typeof( BodyChangeMagmaHoundStatue),
				typeof( BodyChangeRaptorStatue),
				typeof( BodyChangeRuneBearStatue),
				typeof( BodyChangeStalkerStatue),
				typeof( BodyChangeVerminBeastStatue),
				typeof( BodyChangeWeaver )
			};

		public static Item Construct( Type[] types )
		{
			if ( types.Length > 0 )
				return Construct( types, Utility.Random( types.Length ) );

			return null;
		}

		public static Item Construct( Type[] types, int index )
		{
			if ( index >= 0 && index < types.Length )
				return Construct( types[index] );

			return null;
		}

		public static Item Construct( Type type )
		{
			try
			{
				return Activator.CreateInstance( type ) as Item;
			}
			catch
			{
				return null;
			}
		}


		public TamingBOD( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write( reward );
			writer.Write( m_amount );
			writer.Write( m_tamed );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			reward = reader.ReadInt();
			m_amount = reader.ReadInt();
			m_tamed = reader.ReadInt();
		}
	}
}