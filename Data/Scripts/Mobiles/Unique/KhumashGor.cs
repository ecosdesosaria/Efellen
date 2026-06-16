using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal corpse" )]
	public class KhumashGor : BaseCreature
	{
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		[Constructable]
		public KhumashGor () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Khumash-Gor";
			Title = "the Zealan Warrior";
			Body = 327;
			Hue = 0x92B;
			BaseSoundID = 451;

			SetStr( 1096, 1185 );
			SetDex( 686, 775 );
			SetInt( 86, 175 );

			SetHits( 658, 711 );

			SetDamage( 25, 31 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Poison, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 80, 90 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.FistFighting, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;
			AddItem( new LighterSource() );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 30 );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 2 );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // energy burst
				{
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						3,
						"Os segredos dos elementos são meus para carregar!",
						0x779,  // hue
						0,     // physical
						25,   // fire
						25,     // cold
						25,     // poison
						25      // energy
					);
					break;
				}
				case 2: // energy nova
				{
					BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "Vinde, sábios da minha casa! Ajudai vosso Senhor!",
                        amount: 4,
                        creatureType: typeof(AncientLich),
                        hue: 0x779
                    );
                    break;
			    }
			}
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );
   			c.DropItem( new ObeliskOnCorpse() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 6; } }
		public override bool BardImmune { get { return true; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public KhumashGor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
			writer.Write( m_NextSpecialAttack );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			if ( version >= 1 )
			{
				m_NextSpecialAttack = reader.ReadDateTime();
			}
		}
	}
}