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
	[CorpseName("Cadáver de Kamina")]
	public class BalTsareth : BaseSpellCaster
	{
		private static readonly Type[] SummonTypes = new Type[]
		{
			typeof(Imp),
			typeof(AnyElemental),
			typeof(AnyGemElemental)
		};

		private static readonly string[] SummonWarcries = new string[]
		{
			"A trama responde a mim!",
			"Minha magia sobreviveu a civilizações, prove-a!",
			"Esta é MINHA CASA! MINHA!",
			"Esqueci magias que sua civilização ainda vai aprender!"
		};

		private static readonly List<Type> BossDrops = new List<Type>
		{
			typeof(Artifact_PrismaticRobeOfBalTsareth),
			typeof(Artifact_PrismaticGlassesOfBalTsareth),
			typeof(Artifact_PrismaticCapeOfBalTsareth),
			typeof(Artifact_PrismaticBootsOfBalTsareth),
			typeof(Artifact_PrismaticRingOfBalTsareth)
		};

		private int m_Rage;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime;
		private DateTime m_NextSpecialAttack;
		private List<BaseCreature> m_Summons;

		private bool m_Rage1Applied = false;
		private bool m_Rage2Applied = false;
		private bool m_Rage3Applied = false;

		[Constructable]
		public BalTsareth() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Bal Tsareth";
			Body = 401;
			Utility.AssignRandomHair(this);
			HairHue = Utility.RandomHairHue();
			Hue = Utility.RandomSkinHue();
			NameHue = 0x22;
			Title = "O Guardião do Conhecimento Ancestral";

			SetStr(596, 785);
			SetDex(165, 225);
			SetInt(556, 655);
			SetHits(30000);
			SetDamage(11, 15);
			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Poison, 20);
			SetDamageType(ResistanceType.Cold, 20);
			SetDamageType(ResistanceType.Fire, 20);
			SetDamageType(ResistanceType.Energy, 20);
			SetResistance(ResistanceType.Physical, 45);
			SetResistance(ResistanceType.Fire, 70);
			SetResistance(ResistanceType.Cold, 70);
			SetResistance(ResistanceType.Poison, 70);
			SetResistance(ResistanceType.Energy, 70);
			SetSkill(SkillName.Meditation, 102.5, 125.0);
			SetSkill(SkillName.MagicResist, 125.5, 145.0);
			SetSkill(SkillName.Tactics, 101.0, 110.0);
			SetSkill(SkillName.FistFighting, 91.0);
			SetSkill(SkillName.Bludgeoning, 101.0, 111.0);
			SetSkill(SkillName.Magery, 101.0, 110.0);
			SetSkill(SkillName.Psychology, 101.0, 110.0);

			Fame = 30000;
			Karma = -30000;
			VirtualArmor = 50;

			PackItem(new EerieIdol(Utility.Random(12, 26)));
			AddItem(new ScholarRobe { Hue = 0x0213 });
			AddItem(new Sandals { Hue = 0x0213 });
			AddItem(new BlackStaff { Hue = 0x0213 });

			m_NextSummonTime = DateTime.MinValue;
			m_NextSpecialAttack = DateTime.MinValue;
			m_Summons = new List<BaseCreature>();
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich, 6);
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

			BossSummonSystem.TrySummonCreature(
				this,
				combatant,
				SummonTypes,
				m_Rage,
				ref m_NextSummonTime,
				SummonWarcries,
				m_Summons,
				1316,
				GetMaxSummons(),
				35
			);

			if (DateTime.UtcNow >= m_NextSpecialAttack)
			{
				PerformRageAttack(combatant);
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds(25 - (m_Rage * 2));
			}

			m_LastTarget = combatant;
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			m_LastTarget = from;

			if (Utility.RandomDouble() < 0.25 && !willKill)
				TryWeaveStep();

			base.OnDamage(amount, from, willKill);

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
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "Você guardará minha tumba para sempre!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(16, 20);
			VirtualArmor += 5;
		}

		private void ApplyRage2()
		{
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "Renda sua mente a mim!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(21, 25);
			VirtualArmor += 10;
		}

		private void ApplyRage3()
		{
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "EU VOU DESTRUIR VOCÊ!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(26, 30);
			VirtualArmor += 15;
		}

		private static Point3D[] m_WeaveLocations = new Point3D[]
		{
			new Point3D( 5880, 2168, 0 ),
			new Point3D( 5890, 2167, 0 ),
			new Point3D( 5885, 2170, 0 ),
			new Point3D( 5886, 2177, 0 ),
			new Point3D( 5893, 2173, 0 )
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

				PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*Adentra a trama*");

				Effects.SendLocationEffect(dest, map, 0x3728, 13, 10, 0, 0);
				Effects.PlaySound(dest, map, 0x1FE);
			}
		}

		private void PerformRageAttack(Mobile target)
		{
			if (target == null || target.Deleted || !target.Alive)
				return;

			int attackChoice = Utility.RandomMinMax(1, 3);

			switch (attackChoice)
			{
				case 1:
					BossSpecialAttack.PerformSlam(this, "MEUS! TODOS ESTES SEGREDOS SÃO MEUS!", 0x0213, m_Rage + 1, 6, 20, 20, 20, 20, 20);
					break;
				case 2:
					BossSpecialAttack.PerformDegenAura(this, "Você ousa me atacar? A MIM? NA MINHA CASA?", 8, m_Rage + 1, 16, 29, "mana", 0x0213);
					break;
				case 3:
				{
					Type summonType;
					switch (Utility.Random(3))
					{
						case 0:  summonType = typeof(IceGiant); break;
						case 1:  summonType = typeof(StormGiant); break;
						default: summonType = typeof(LavaGiant); break;
					}
					BossSpecialAttack.SummonHonorGuard(this, target, "Os elementos são meus! Desvendei seus segredos antes mesmo do seu reino nascer!", m_Rage + 1, summonType, 0x0213);
					break;
				}
			}
		}

		public override void CheckReflect(Mobile caster, ref bool reflect)
		{
			reflect = (Utility.Random(100) < m_Rage * 16);
		}

		private int GetMaxSummons()
		{
			switch (m_Rage)
			{
				case 1: return 8;
				case 2: return 7;
				case 3: return 6;
				default: return 6;
			}
		}

		public override bool OnBeforeDeath()
		{
			BossLootSystem.AwardBossMarks(this, this.LastKiller, 156, 223, "Eu me levantarei novamente... em outros mil anos...");
			return base.OnBeforeDeath();
		}

		public override void OnDelete()
		{
			if (m_Summons != null)
			{
				BossSummonSystem.CleanupSummons(m_Summons);
				m_Summons.Clear();
				m_Summons = null;
			}

			base.OnDelete();
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			BossLootSystem.AwardBossSpecial(this, BossDrops, 30);
			for (int i = 0; i < 4; i++)
			{
				c.DropItem(Loot.RandomArty());
				c.DropItem(new EtherealPowerScroll());
				c.DropItem(AscensionScrollFactory.CreateRandom());
			}

			RichesSystem.SpawnRiches(m_LastTarget, 4);
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			this.MobileMagics(7, SpellType.Wizard, 0x0213);
			LeechImmune = true;
		}

		public BalTsareth(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)3);
			writer.Write(m_Rage);
			writer.Write(m_NextSummonTime);
			writer.Write(m_NextSpecialAttack);
			writer.Write(m_Rage1Applied);
			writer.Write(m_Rage2Applied);
			writer.Write(m_Rage3Applied);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			if (version >= 1)
			{
				m_Rage = reader.ReadInt();
				m_NextSummonTime = reader.ReadDateTime();
				m_NextSpecialAttack = reader.ReadDateTime();
			}

			if (version >= 2)
			{
				this.MobileMagics(7, SpellType.Wizard, 0x0213);
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

			if (m_Summons == null)
				m_Summons = new List<BaseCreature>();
		}
	}
}