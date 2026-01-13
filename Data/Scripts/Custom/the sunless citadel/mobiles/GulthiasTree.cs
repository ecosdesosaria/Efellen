using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Plants;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "Gulthias Tree's corpse" )]
	public class GulthiasTree : BaseCreature
	{
		private DateTime m_NextSpawn;
		private static readonly TimeSpan SpawnInterval = TimeSpan.FromSeconds(60);
        private Map m_DeathMap;
        private Point3D m_DeathLoc;

		[Constructable]
		public GulthiasTree() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Gulthias Tree";
			Body = 47;
			BaseSoundID = 442;
            SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 65 );
			SetResistance( ResistanceType.Fire, 25 );
			SetResistance( ResistanceType.Cold, 60 );
			SetResistance( ResistanceType.Poison, 80 );
			SetResistance( ResistanceType.Energy, 70 );

			SetSkill( SkillName.MagicResist, 115.0 );
			SetSkill( SkillName.Tactics, 1.0 );
			SetSkill( SkillName.FistFighting, 1.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 90;

			PackItem( new MandrakeRoot( 5 ) );

			int modifySta = 0;
			int modifyHit = 0;
			int modifyDmg = 0;

			switch ( Utility.Random( 14 ) )
			{
				case 0: Resource = CraftResource.None;																	break;
				case 1: Resource = CraftResource.AshTree;			modifySta = 5;	modifyHit = 10;		modifyDmg = 1;	break;
				case 2: Resource = CraftResource.CherryTree;		modifySta = 10;	modifyHit = 20;		modifyDmg = 2;	break;
				case 3: Resource = CraftResource.EbonyTree;			modifySta = 15;	modifyHit = 30;		modifyDmg = 3;	break;
				case 4: Resource = CraftResource.GoldenOakTree;		modifySta = 20;	modifyHit = 40;		modifyDmg = 4;	break;
				case 5: Resource = CraftResource.HickoryTree;		modifySta = 25;	modifyHit = 50;		modifyDmg = 5;	break;
				case 6: Resource = CraftResource.MahoganyTree;		modifySta = 30;	modifyHit = 60;		modifyDmg = 6;	break;
				case 7: Resource = CraftResource.OakTree;			modifySta = 35;	modifyHit = 70;		modifyDmg = 7;	break;
				case 8: Resource = CraftResource.PineTree;			modifySta = 40;	modifyHit = 80;		modifyDmg = 8;	break;
				case 9: Resource = CraftResource.RosewoodTree;		modifySta = 45;	modifyHit = 90;		modifyDmg = 9;	break;
				case 10: Resource = CraftResource.WalnutTree;		modifySta = 50;	modifyHit = 100;	modifyDmg = 10;	break;
				case 11: Resource = CraftResource.PetrifiedTree;	modifySta = 55;	modifyHit = 110;	modifyDmg = 11;	break;
				case 12: Resource = CraftResource.DriftwoodTree;	modifySta = 60;	modifyHit = 120;	modifyDmg = 12;	break;
				case 13: Resource = CraftResource.ElvenTree;		modifySta = 70;	modifyHit = 130;	modifyDmg = 13;	break;
			}

			if ( Resource != CraftResource.None )
				Hue = CraftResources.GetClr(Resource);

			SetStr( 1 );
			SetDex( 75 );
			SetInt( 115 );

			SetHits( 450 );
			SetStam( 0 );

			SetDamage( 1,2 );

			m_NextSpawn = DateTime.UtcNow + SpawnInterval;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( DateTime.UtcNow >= m_NextSpawn && Hits < HitsMax )
			{
				SpawnTwigBlights();
				m_NextSpawn = DateTime.UtcNow + SpawnInterval;
			}
		}

		private void SpawnTwigBlights()
		{
			for ( int i = 0; i < 2; i++ )
			{
				Point3D spawnLoc = GetSpawnLocation();
				
				if ( spawnLoc != Point3D.Zero )
				{
					TwigBlight blight = new TwigBlight();
					blight.MoveToWorld( spawnLoc, Map );
					
					if ( Combatant != null )
						blight.Combatant = Combatant;
				}
			}
		}

		private Point3D GetSpawnLocation()
		{
			for ( int i = 0; i < 20; i++ )
			{
				int distance = Utility.RandomMinMax( 3, 6 );
				int x = X + Utility.RandomMinMax( -distance, distance );
				int y = Y + Utility.RandomMinMax( -distance, distance );
				int z = Map.GetAverageZ( x, y );

				Point3D p = new Point3D( x, y, z );

				if ( Map.CanSpawnMobile( p ) && InRange( p, 6 ) && GetDistanceToSqrt( p ) >= 3 )
					return p;
			}

			return Point3D.Zero;
		}

        
        public override bool OnBeforeDeath()
        {
            m_DeathMap = this.Map;
            m_DeathLoc = this.Location;

            return base.OnBeforeDeath();
        }

		public override void OnDeath( Container c )
        {
        	base.OnDeath( c );

        	if ( 1 == Utility.RandomMinMax( 1, 3 ) )
        	{
        		SummerFruitOfGulthias apple = new SummerFruitOfGulthias();
        		apple.Amount = Utility.RandomMinMax( 2, 4 );
        		c.DropItem(apple);
        	}
        	else
        	{
        		WinterFruitOfGulthias badapple = new WinterFruitOfGulthias();
        		badapple.Amount = Utility.RandomMinMax( 2, 4 );
        		c.DropItem(badapple);
        	}

        	 Timer.DelayCall(
                TimeSpan.FromSeconds( 0.5 ),
                delegate()
                {
                    HandleDeathEffects( m_DeathMap, m_DeathLoc );
                }
           );
        }

        private void HandleDeathEffects( Map map, Point3D loc )
        {
            if ( map == null )
                return;
        
            BelakTheOutcast belak = null;
            List<TwigBlight> twigBlights = new List<TwigBlight>();
            List<Mobile> thralls = new List<Mobile>();

            IPooledEnumerable eable = map.GetMobilesInRange( loc, 12 );

            foreach ( Mobile m in eable )
            {
                if ( m is BelakTheOutcast )
                    belak = (BelakTheOutcast)m;
                else if ( m is TwigBlight )
                    twigBlights.Add( (TwigBlight)m );
                else if ( m is SirBradford || m is Sharwyn )
                    thralls.Add( m );
            }

            eable.Free();

            if ( belak != null && twigBlights.Count > 0 )
            {
                foreach ( TwigBlight blight in twigBlights )
                {
                    blight.Team = 0;
                    blight.Combatant = belak;
                    blight.FocusMob = belak;

                    if ( blight.AIObject != null )
                        blight.AIObject.Action = ActionType.Combat;
                }
            }

            foreach ( Mobile thrall in thralls )
            {
                Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), delegate()
                {
                    if ( thrall != null && !thrall.Deleted && thrall.Alive )
                    {
                        thrall.Say("I...I am...Free...");
                        thrall.Kill();
                    }    
                });
            }
        }


		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Average );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool DisallowAllMoves{ get{ return true; } }
		public override bool BleedImmune{ get{ return true; } }
		public override int Wood{ get{ return Utility.RandomMinMax( 10, 20 ); } }
		public override WoodType WoodType{ get{ return ResourceWood(); } }

		public GulthiasTree( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( m_NextSpawn );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_NextSpawn = reader.ReadDateTime();
		}
	}
}