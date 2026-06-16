using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using System.Net;
using Server.CustomSpells;
using Server.Custom;
using Server.EffectsUtil;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Custom.DailyBosses.System;
using Server.Custom.BossSystems;
using Server.Custom.Ascensions;

namespace Server.Mobiles
{
	[CorpseName( "Orthan's corpse" )]
	public class OrthanLolthsConsumingTenderness : BaseCreature
	{
        private DateTime m_NextSpecialAttack;


		
		[Constructable]
		public OrthanLolthsConsumingTenderness() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Orthan";
			Title = "Ternura Devoradora de Lolth";
            Body = 222;
			BaseSoundID = 959;
			Hue = 0x0b32;

			SetStr( 596, 670 );
			SetDex( 221, 360 );
			SetInt( 356, 490 );
			SetHits( 7358, 7422 );
			SetDamage( 16, 25 );
			SetDamageType( ResistanceType.Energy, 100 );
			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 40 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 40 );
		
			SetSkill( SkillName.MagicResist, 110.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.FistFighting, 100.0 );
			SetSkill( SkillName.Magery, 111.0 );
			SetSkill( SkillName.Meditation, 121.0 );
            SetSkill( SkillName.Necromancy, 111.0 );

			Fame = 19500;
			Karma = -19500;
			VirtualArmor = 60;

			m_NextSpecialAttack = DateTime.MinValue;
        }

		
		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
		
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 20 );
			}
			
			base.OnDamage( amount, from, willKill );
		}
		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 2 );

			switch ( attackChoice )
			{
				case 1:
					BossSpecialAttack.PerformTargettedAoE( this, target, 3, "Que o toque de Lolth esteja sobre vós!", 0x0b32, 20, 20, 20, 20, 20 );
					break;
				case 2:
					BossSpecialAttack.PerformDelayedExplosion(
				        this,
				        "Carregue a marca de nossa rainha!",
				        0x0b32,   // hue
				        10,     // radius
				        3,
				        0,      // physical
				        0,      // fire
				        0,      // cold
				        50,    // poison
				        50       // energy
				    );
                    break;
			}
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			Mobile killer = this.LastKiller;
			if ( killer != null )
			{

				if ( killer is BaseCreature )
					killer = ((BaseCreature)killer).GetMaster();
                
				BossLootSystem.BossEnchant(this, c, 350, 100, 3, "DrowPriestess");
				for ( int i = 0; i < 2; i++ )
				{
					c.DropItem( Loot.RandomArty() );
					c.DropItem( new EtherealPowerScroll() );
					c.DropItem( AscensionScrollFactory.CreateRandom());
				}

				if ( killer is PlayerMobile && Utility.RandomDouble() <= 0.10 )
				{
					c.DropItem( new OrbOfTheDemonwebPits() );
				}
			}
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			this.MobileMagics(9, SpellType.Cleric, 0x0b32);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}

		public override bool CanRummageCorpses{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		
		public OrthanLolthsConsumingTenderness( Serial serial ) : base( serial )
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
			this.MobileMagics(9, SpellType.Cleric, 0x0b32);
		}
	}
}