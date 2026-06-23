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
using Server.Spells;
using Server.EffectsUtil;
using Server.Custom;
using Server.Custom.DailyBosses.System;
using Server.Custom.BossSystems;
using Server.Custom.Ascensions;

namespace Server.Mobiles
{
	[CorpseName("Cadáver de Lolth")]
	public class LolthQueenOfTheDemonweb : BaseCreature
	{
		private static Hashtable m_Table = new Hashtable();

		private class CaressEffect
		{
			public ResistanceMod EnergyMod;
			public ResistanceMod PoisonMod;
			public CaressTimer Timer;

			public CaressEffect(ResistanceMod energy, ResistanceMod poison, CaressTimer timer)
			{
				EnergyMod = energy;
				PoisonMod = poison;
				Timer = timer;
			}
		}

		private static readonly Type[] SummonTypes = new Type[]
		{
			typeof(LolthsIngenuity),
			typeof(LolthsJealousy),
			typeof(LolthsMercy),
			typeof(LolthsPenitence),
			typeof(LolthsChosen)
		};

		private static readonly string[] SummonWarcries = new string[]
		{
			"Filhos dos poços, vinde a mim!",
            "A tecedora anfitriã atenderá meu chamado!",
            "Avante, prole de seda do abismo!",
            "A ruína sem fim recairá sobre vós!"
		};

		private static readonly List<Type> BossDrops = new List<Type>
		{
			typeof(Artifact_DemonwebAuthority),
			typeof(Artifact_DemonwebGrasp),
			typeof(Artifact_DemonwebTyrant),
			typeof(Artifact_DemonwebFang)
		};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private List<BaseCreature> m_Summons = new List<BaseCreature>();

		private bool m_Rage1Applied = false;
		private bool m_Rage2Applied = false;
		private bool m_Rage3Applied = false;

		[Constructable]
		public LolthQueenOfTheDemonweb() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Lolth";
			Title = "Rainha de Demonweb Pits";
			Body = 193;
			BaseSoundID = 0x24D;
			Hue = 0x922;

			SetStr(996);
			SetDex(225);
			SetInt(1021);

			SetHits(102000);
			SetDamage(14, 18);

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Poison, 80);

			SetResistance(ResistanceType.Physical, 70);
			SetResistance(ResistanceType.Fire, 70);
			SetResistance(ResistanceType.Cold, 70);
			SetResistance(ResistanceType.Poison, 100);
			SetResistance(ResistanceType.Energy, 60);

			SetSkill(SkillName.Meditation, 125.0);
			SetSkill(SkillName.MagicResist, 150.0);
			SetSkill(SkillName.Tactics, 125.0);
			SetSkill(SkillName.FistFighting, 125.0);
			SetSkill(SkillName.Magery, 125.0);

			Fame = 50000;
			Karma = -50000;
			VirtualArmor = 70;

			if (Backpack == null)
				AddItem(new Backpack());
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich, 12);
		}

		public override int TreasureMapLevel { get { return 5; } }
		public override bool CanRummageCorpses { get { return false; } }
		public override bool ReacquireOnMovement { get { return !Controlled; } }
		public override bool BleedImmune { get { return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public override void CheckReflect(Mobile caster, ref bool reflect)
		{
			reflect = (Utility.Random(100) < m_Rage * 25);
		}

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
				30
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

			if (Utility.RandomDouble() < 0.75)
				Server.Misc.IntelligentAction.LeapToAttacker(this, from);

			if (Utility.RandomDouble() < 0.10)
			{
				switch (Utility.RandomMinMax(1, 4))
				{
					case 1:
						BossSpecialAttack.PerformTargettedAoE(this, from, m_Rage, "Aproxime-se, receba meu beijo!", 0x922, 0, 0, 0, 100, 0);
						break;
					case 2:
						BossSpecialAttack.PerformCrossExplosion(this, from, "Aventure-se em meu coração, abrace minha escuridão!", 0x922, m_Rage, 0, 0, 0, 100, 0);
						break;
					case 3:
						BossSpecialAttack.PerformSlam(this, "Meu amor vos devorará!", 0x922, m_Rage, 6, 0, 0, 0, 100, 0);
						break;
                    case 4:
						BossSpecialAttack.SummonHonorGuard(this, from, "Anfitriões de Demonweb, atendei-me!", 4, typeof(Archfiend), 0x922);
						break;
				}
			}

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
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "Meu toque eterno vos devastará!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(16, 20);
		}

		private void ApplyRage2()
		{
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "O casulo será alimentado com suas entranhas!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(21, 25);
			VirtualArmor += 10;
		}

		private void ApplyRage3()
		{
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "O abismo atende ao meu comando!");
			this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
			this.PlaySound(0x202);
			SetDamage(26, 30);
			VirtualArmor += 10;
		}

		private void PerformRageAttack(Mobile target)
		{
			if (target == null || target.Deleted || !target.Alive)
				return;

			int attackChoice = Utility.RandomMinMax(1, 3);

			switch (attackChoice)
			{
				case 1:
				{
					BossSpecialAttack.PerformPull(
						this,
						"Vinde a mim as criancinhas da vida e da luz!",
						0x922,
						m_Rage,
						true
					);
					break;
				}
				case 2:
				{
					BossSpecialAttack.PerformDegenAura(
						this,
						"VOCÊ SERÁ MEU!",
						8,
						m_Rage + 1,
						22,
						44,
						"health",
						0x922
					);
					break;
				}
				case 3:
				{
					BossSpecialAttack.PerformDelayedExplosion(
						this,
						"Trago-vos o dom da companhia eterna!",
						0x922,
						16,
						m_Rage + 1,
						0,
						0,
						0,
						100,
						0
					);
					break;
				}
			}
		}

		private int GetMaxSummons()
		{
			switch (m_Rage)
			{
				case 0: return 14;
				case 1: return 12;
				case 2: return 10;
				case 3: return 8;
				default: return 14;
			}
		}

		public override bool OnBeforeDeath()
		{
			BossLootSystem.AwardBossMarks(this, this.LastKiller, 431, 647, "Não ouse pensar que isso acabou, mortal!");
			return base.OnBeforeDeath();
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			BossLootSystem.AwardBossSpecial(this, BossDrops, 100);
			for (int i = 0; i < 8; i++)
			{
				c.DropItem(Loot.RandomArty());
				c.DropItem(new EtherealPowerScroll());
				c.DropItem(AscensionScrollFactory.CreateRandom());
				if (Utility.RandomDouble() < 0.55)
				{
					c.DropItem(new OrbOfTheDemonwebPits());
				}
			}
			if (Utility.RandomDouble() < 0.55)
			{
				c.DropItem(new EternalPowerScroll());
			}
			RichesSystem.SpawnRiches(m_LastTarget, 6);
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

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (defender == null || !defender.Player || !defender.Alive)
				return;

			if (Utility.RandomDouble() <= 0.55)
				ApplyLolthsCaress(defender);
		}

		private class CaressTimer : Timer
		{
			private Mobile m_Mobile;
			private DateTime m_End;

			public CaressTimer(Mobile m, DateTime end) : base(TimeSpan.Zero, TimeSpan.FromSeconds(4.0))
			{
				m_Mobile = m;
				m_End = end;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if (m_Mobile == null || m_Mobile.Deleted || !m_Mobile.Alive)
				{
					StopEffect(m_Mobile);
					return;
				}

				if (DateTime.UtcNow >= m_End)
				{
					StopEffect(m_Mobile);
					return;
				}

				int stamLoss = Math.Max(1, (int)(m_Mobile.StamMax * 0.035));
				int manLoss = Math.Max(1, (int)(m_Mobile.ManaMax * 0.035));

				m_Mobile.Stam = Math.Max(0, m_Mobile.Stam - stamLoss);
				m_Mobile.Mana = Math.Max(0, m_Mobile.Mana - manLoss);
			}
		}

		public static bool HasLolthsCaress(Mobile m)
		{
			return m_Table.ContainsKey(m);
		}

		public static void ApplyLolthsCaress(Mobile m)
		{
			if (m == null || HasLolthsCaress(m))
				return;

			m.SendMessage(38, "O toque de Lolth penetra fundo em sua alma!");

			ResistanceMod energyMod = new ResistanceMod(ResistanceType.Energy, -24);
			ResistanceMod poisonMod = new ResistanceMod(ResistanceType.Poison, -24);

			m.AddResistanceMod(energyMod);
			m.AddResistanceMod(poisonMod);

			DateTime end = DateTime.UtcNow + TimeSpan.FromSeconds(32.0);

			CaressTimer timer = new CaressTimer(m, end);
			CaressEffect effect = new CaressEffect(energyMod, poisonMod, timer);

			m_Table[m] = effect;

			timer.Start();
		}

		public static void StopEffect(Mobile m)
		{
			if (m == null)
				return;

			CaressEffect effect = m_Table[m] as CaressEffect;

			if (effect == null)
				return;

			if (effect.Timer != null)
				effect.Timer.Stop();

			if (effect.EnergyMod != null)
				m.RemoveResistanceMod(effect.EnergyMod);

			if (effect.PoisonMod != null)
				m.RemoveResistanceMod(effect.PoisonMod);

			m_Table.Remove(m);

			m.SendMessage(68, "A influência de Lolth sobre sua alma desaparece.");
		}

		public LolthQueenOfTheDemonweb(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)2);

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