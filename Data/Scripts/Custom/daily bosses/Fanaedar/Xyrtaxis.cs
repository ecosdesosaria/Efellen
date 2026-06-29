using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;
using Server.Spells.Necromancy;
using Server.Spells;
using Server.EffectsUtil;
using Server.Custom;
using Server.Custom.DailyBosses.System;
using Server.Custom.BossSystems;
using Server.CustomSpells;
using Server.Custom.Ascensions;

namespace Server.Mobiles
{
	[CorpseName( "Xyrtaxis's Corpse" )]
	public class Xyrtaxis : BaseSpellCaster
	{		
		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_GrimoireOfTheDemonweb),
            typeof(Artifact_LolthsUnendingFlow),
            typeof(Artifact_XyrtaxisBlackReach)
    	};

		private int m_Rage;
		private Mobile m_LastTarget;
		private DateTime m_NextSpecialAttack;

		private bool m_Rage1Applied = false;
		private bool m_Rage2Applied = false;
		private bool m_Rage3Applied = false;
		
		[Constructable]
		public Xyrtaxis () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomTalkHue();
			Hue = 1316;
			Body = 605;
			Name = "Xyrtaxis";
			Utility.AssignRandomHair( this );
			HairHue = 1150;
			NameHue = 0x22;
			Title = "O Decano das Artes Negras";
			
			SetStr( 596, 785 );
			SetDex( 165, 225 );
			SetInt( 556, 655 );
			SetHits( 30000 );
			SetDamage( 11, 15 );
			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Energy, 20 );
			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 70 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 70 );
			SetSkill( SkillName.Meditation, 102.5, 125.0 );
			SetSkill( SkillName.MagicResist, 125.5, 145.0 );
			SetSkill( SkillName.Tactics, 101.0, 110.0 );
			SetSkill( SkillName.FistFighting, 91.0 );
			SetSkill( SkillName.Bludgeoning, 101.0, 111.0 );
			SetSkill( SkillName.Magery, 101.0, 110.0 );
			SetSkill( SkillName.Necromancy, 101.0, 110.0 );
			SetSkill( SkillName.Spiritualism, 101.0, 110.0 );
			SetSkill( SkillName.Psychology, 101.0, 110.0 );

			Fame = 30000;
			Karma = -30000;
			VirtualArmor = 50;
			IsBoss = true;
		    AddItem( new ScholarRobe{ Hue = 0x0213 } );
            AddItem( new Sandals{ Hue = 0x0213 } );
            AddItem( new BlackStaff{ Hue = 0x0213 } );

        	m_NextSpecialAttack = DateTime.MinValue;
		  }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 6 );
		}

		public override bool AlwaysAttackable { get { return true; } }
		public override int TreasureMapLevel { get { return 4; } }
		public override bool CanRummageCorpses { get { return false; } }
		public override bool ReacquireOnMovement { get { return !Controlled; } }
		public override bool BleedImmune { get { return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public override void OnThink()
		{
		    base.OnThink();

		    Mobile combatant = this.Combatant;

		    if (combatant == null || combatant.Deleted || !combatant.Alive)
		        return;

		    if (DateTime.UtcNow >= m_NextSpecialAttack)
		    {
		        PerformRageAttack(combatant);
		        m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds(15 - (m_Rage * 2));
		    }
		    m_LastTarget = combatant;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;
			if (Utility.RandomDouble() < 0.35 )
				TryWeaveStep();

			base.OnDamage( amount, from, willKill );

			CheckRageThresholds();
		}

		private void CheckRageThresholds()
		{
			if (this.HitsMax <= 0)
				return;

			double hpPercent = (double)this.Hits / (double)this.HitsMax;

			if (!m_Rage1Applied && hpPercent <= 0.75)
			{
				m_Rage1Applied = true;
				m_Rage = 1;
				ApplyRage1();
			}
			else if (!m_Rage2Applied && hpPercent <= 0.50)
			{
				m_Rage2Applied = true;
				m_Rage = 2;
				ApplyRage2();
			}
			else if (!m_Rage3Applied && hpPercent <= 0.25)
			{
				m_Rage3Applied = true;
				m_Rage = 3;
				ApplyRage3();
			}
		}

		private void ApplyRage1()
		{
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "Must I be interrupted at every time?!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(16, 20);
			VirtualArmor += 5;
		}

		private void ApplyRage2()
		{
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "Your optimism is so touching!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(21, 25);
			VirtualArmor += 10;
		}

		private void ApplyRage3()
		{
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "This is beyond my attention!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(26, 30);
			VirtualArmor += 15;
		}

        private static Point3D[] m_WeaveLocations = new Point3D[]
		{
		    new Point3D( 733, 2864, 40 ),
			new Point3D( 741, 2864, 40 ),
			new Point3D( 741, 2872, 40 ),
			new Point3D( 732, 2872, 40 ),
			new Point3D( 736, 2868, 40 )
		};

		private void TryWeaveStep()
		{
			Map map = Map;

			if (map == null)
				return;

			Point3D current = Location;

			Point3D[] possible = new Point3D[m_WeaveLocations.Length];
			int count = 0;

			for (int i = 0; i < m_WeaveLocations.Length; i++)
			{
				if (m_WeaveLocations[i] != current)
				{
					possible[count] = m_WeaveLocations[i];
					count++;
				}
			}

			if (count == 0)
				return;

			Point3D dest = possible[Utility.Random(count)];

			if (map.CanSpawnMobile(dest))
			{
				Location = dest;

				PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*Steps into the weave*");

				Effects.SendLocationEffect(dest, map, 0x3728, 13, 10, 0, 0);
				Effects.PlaySound(dest, map, 0x1FE);
			}
		}

		private void PerformRageAttack(Mobile target)
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 3 );

			switch ( attackChoice )
			{
				case 1:
                    BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						m_Rage+1,
						"Você vai se arrepender desta interrupção!",
						1316,  // hue
						20,     // physical
						20,   // fire
						20,     // cold
						20,     // poison
						20      // energy
					);
					break;
				case 2:
					BossSpecialAttack.PerformDegenAura( this, "Eu vou te desfazer!", 8, m_Rage+1, 16, 29, "mana", 0x0213 );
					break;
				case 3:
                    BossSpecialAttack.PerformEntangle(
        			    boss: this,
        			    warcry: "Sangre por Lolth!",
        			    hue: 1316,
        			    rage: m_Rage+1,
        			    range: 8,
        			    bleedLevel: 10  // 20-30 damage per tick
        			);
					break;
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = ( Utility.Random( 100 ) < m_Rage * 16 );
		}

		public override bool OnBeforeDeath()
		{
			BossLootSystem.AwardBossMarks(this, this.LastKiller, 156, 223, "Você...Você me...matou?");	
			return base.OnBeforeDeath();
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );
            BossLootSystem.BossEnchant(this, c, 550, 100, 3, "DrowMage");
			BossLootSystem.AwardBossSpecial( this, BossDrops, 30 );
			for ( int i = 0; i < 4; i++ )
			{
				c.DropItem( Loot.RandomArty() );
				c.DropItem( new EtherealPowerScroll() );
				c.DropItem( AscensionScrollFactory.CreateRandom());
			}
			int amount = Utility.Random(3,6);
			c.DropItem(new EssenceOfLolthsHatred(amount));
			RichesSystem.SpawnRiches( m_LastTarget, 4 );
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			this.MobileMagics(8, SpellType.Wizard, 0x0213);
			LeechImmune = true;
		}

		public Xyrtaxis( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 3 );
			writer.Write( m_Rage );
			writer.Write( m_NextSpecialAttack );
			writer.Write(m_Rage1Applied);
			writer.Write(m_Rage2Applied);
			writer.Write(m_Rage3Applied);
       }

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            
			if ( version >= 1 )
			{
				m_Rage = reader.ReadInt();
				m_NextSpecialAttack = reader.ReadDateTime();
			}
			
			if(version>=2)
			{
				this.MobileMagics(8, SpellType.Wizard, 0x0213);
			}

			if (version >= 3)
			{
				m_Rage1Applied = reader.ReadBool();
				m_Rage2Applied = reader.ReadBool();
				m_Rage3Applied = reader.ReadBool();
			}
			else
			{
				m_Rage1Applied = m_Rage >= 1;
				m_Rage2Applied = m_Rage >= 2;
				m_Rage3Applied = m_Rage >= 3;
			}
			
			LeechImmune = true;
		}
	}
}