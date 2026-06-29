using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.CustomSpells;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "cadáver de Arun" )]
	public class Arun : BaseCreature
	{
        private DateTime m_NextSpecialAttack;
		[Constructable]
		public Arun() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Arun";
			Title = "o sacerdote de Myrkul";
            Hue = 0x0455;
			Body = 305;
			BaseSoundID = 471;

			SetStr( 426, 400 );
			SetDex( 121, 160 );
			SetInt( 56, 90 );
			SetHits( 358, 422 );
			SetDamage( 13, 21 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 50 );
			SetResistance( ResistanceType.Cold, 50 );
			SetResistance( ResistanceType.Poison, 65 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.MagicResist, 80.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.FistFighting, 100.0 );
			SetSkill( SkillName.Anatomy, 80.0);

			Fame = 9500;
			Karma = -9500;
			VirtualArmor = 60;

			m_NextSpecialAttack = DateTime.MinValue;
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			this.MobileMagics(4, SpellType.Cleric, 0x0455);
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

			int attackChoice = Utility.RandomMinMax( 1, 4 );

			switch ( attackChoice )
			{
				case 1:
					BossSpecialAttack.PerformTargettedAoE( this, target, 1, "seus ossos serão transformados em oferenda!", 0x0455, 20, 20, 20, 20, 20 );
					break;
				case 2:
					BossSpecialAttack.PerformCrossExplosion( this, target, "Por deuses sombrios e magia vil, lanço-te à morte!", 0x0455, 1, 20, 20, 20, 20, 20 );
                	break;
				case 3:
					BossSpecialAttack.PerformSlam( this, "Senhores deste mundo, abatei este intruso!", 0x0455, 2, 6, 0, 0, 0, 100, 0 );
                	break;
				case 4:
					BossSpecialAttack.SummonHonorGuard(boss: this, target: target, warcry: "Vinde a mim, irmãos das trevas!", amount: 2, creatureType: typeof(BoneGolem), hue: 0x0455);
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
                
				if ( killer is PlayerMobile )
				{
					if ( Utility.RandomMinMax( 1, 10 ) == 1 )
                    {
                        EtherealUnicorn mount = new EtherealUnicorn();
                        mount.Hue = 0x0455;
                        c.DropItem( mount );
                    }
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 3 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int Cloths{ get{ return 14; } }
		public override ClothType ClothType{ get{ return ClothType.Haunted; } }

		public Arun( Serial serial ) : base( serial )
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
				this.MobileMagics(4, SpellType.Cleric, 0x0455);
			}
		}
	}
}