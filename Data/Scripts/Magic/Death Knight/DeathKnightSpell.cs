using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using System.Collections;

namespace Server.Spells.DeathKnight
{
	public abstract class DeathKnightSpell : Spell
	{
		public abstract int RequiredTithing { get; }
		public abstract double RequiredSkill { get; }
		public abstract int RequiredMana{ get; }
		public override bool ClearHandsOnCast { get { return false; } }
		public override SkillName CastSkill { get { return SkillName.Knightship; } }
		public override SkillName DamageSkill { get { return SkillName.Knightship; } }
		public override int CastRecoveryBase { get { return 7; } }

		public DeathKnightSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public static string SpellDescription( int spell )
		{
			string txt = "Esta caveira contém o conhecimento da magia dos Cavaleiros da Morte: ";
			string skl = "0";

			if ( spell == 750 ){         skl = "40"; txt += "Bane criaturas invocadas de volta ao seu reino, demônios de volta ao inferno, ou elementais de volta ao seu plano de existência."; }
			else if ( spell == 751 ){  skl = "15"; txt += "O alvo do cavaleiro da morte é curado por forças demoníacas por uma quantidade significativa."; }
			else if ( spell == 752 ){  skl = "90"; txt += "Invoca o diabo para lutar com o cavaleiro da morte."; }
			else if ( spell == 753 ){  skl = "30"; txt += "O próximo alvo atingido é marcado pelo ceifador. Todo dano causado a ele é aumentado, mas o cavaleiro da morte recebe dano extra de outros tipos de criaturas."; }
			else if ( spell == 754 ){  skl = "5";  txt += "Sua mão possui os poderes de uma bruxa, onde pode remover maldições de itens e outros."; }
			else if ( spell == 755 ){  skl = "70"; txt += "O inimigo do cavaleiro da morte é chamuscado por um fogo do inferno que continua a queimar o inimigo por uma curta duração."; }
			else if ( spell == 756 ){  skl = "25"; txt += "Invoca um raio de energia do próprio Lúcifer e atordoa temporariamente o inimigo."; }
			else if ( spell == 757 ){  skl = "80"; txt += "As forças de Orcus cercam o cavaleiro e refletem uma certa quantidade de efeitos mágicos de volta ao conjurador."; }
			else if ( spell == 758 ){  skl = "60"; txt += "Canaliza o ódio para formar uma barreira ao redor do alvo, protegendo-o de dano físico."; }
			else if ( spell == 759 ){  skl = "45"; txt += "Drena a alma do inimigo, reduzindo sua mana por um curto período de tempo."; }
			else if ( spell == 760 ){  skl = "20"; txt += "Aumenta grandemente a força do alvo por um curto período."; }
			else if ( spell == 761 ){  skl = "10"; txt += "O inimigo do cavaleiro da morte é danificado por uma energia demoníaca dos nove infernos."; }
			else if ( spell == 762 ){  skl = "35"; txt += "O alvo do cavaleiro da morte tem sua pele regenerando saúde ao longo do tempo."; }
			else if ( spell == 763 ){  skl = "50"; txt += "O cavaleiro da morte libera as forças do inferno sobre seus inimigos próximos, causando muito dano."; }

			if ( skl == "0" )
				return txt;

			return txt + " Requer que um Cavaleiro da Morte tenha pelo menos " + skl + " em Cavalaria.";
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Caster.Stam < 10 )
			{
				Caster.SendMessage( "Você está muito fatigado para fazer isso agora." );
				return false;
			}
			else if ( Caster.Karma > 0 )
			{
				Caster.SendMessage( "Você tem Karma demais para lançar este feitiço." );
				return false;
			}
			else if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + RequiredSkill + " em Cavalaria para lançar este feitiço." );
				return false;
			}
			else if ( GetSoulsInLantern( Caster ) < RequiredTithing )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + RequiredTithing + " Almas para lançar este feitiço." );
				return false;
			}
			else if ( Caster.Mana < GetMana() )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + GetMana() + " de Mana para lançar este feitiço." );
				return false;
			}

			return true;
		}

		public override bool CheckFizzle()
		{
			int requiredTithing = GetTithing( Caster, this );
			int mana = GetMana();

			if ( Caster.Stam < 10 )
			{
				Caster.SendMessage( "Você está muito fatigado para fazer isso agora." );
				return false;
			}
			else if ( Caster.Karma > 0 )
			{
				Caster.SendMessage( "Você tem Karma demais para lançar este feitiço." );
				return false;
			}
			else if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + RequiredSkill + " em Cavalaria para lançar este feitiço." );
				return false;
			}
			else if ( GetSoulsInLantern( Caster ) < requiredTithing )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + requiredTithing + " Almas para lançar este feitiço." );
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + mana + " de Mana para lançar este feitiço." );
				return false;
			}

			if ( !base.CheckFizzle() )
				return false;

			return true;
		}

		public override void DoFizzle()
		{
			Caster.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "Você falha em invocar o poder.", Caster.NetState);
			Caster.FixedParticles( 0x3735, 1, 30, 9503, EffectLayer.Waist );
			Caster.PlaySound( 0x19D );
			Caster.NextSpellTime = DateTime.Now;
		}

		public override int ComputeKarmaAward()
		{
			int circle = (int)(RequiredSkill / 10);
				if ( circle < 1 ){ circle = 1; }
			return -( 40 + ( 10 * circle ) );
		}

		public override int GetMana()
		{
			return RequiredMana;
		}

		public static int GetTithing( Mobile Caster, DeathKnightSpell spell )
		{
			if ( AosAttributes.GetValue( Caster, AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				return 0;

			return spell.RequiredTithing;
		}

		public override void SayMantra()
		{
			Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, Info.Mantra );
			Caster.PlaySound( 0x19E );
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 0x19D );
		}

		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
				Caster.PlaySound( 0x19D );
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill;
			max = RequiredSkill + 40.0;
		}

		public int ComputePowerValue( int div )
		{
			return ComputePowerValue( Caster, div );
		}

		public static int ComputePowerValue( Mobile from, int div )
		{
			if ( from == null )
				return 0;

			int v = (int) Math.Sqrt( ( from.Karma * -1 ) + 20000 + ( from.Skills.Knightship.Fixed * 10 ) );

			return v / div;
		}

		public static void DrainSoulsInLantern( Mobile from, int tithing )
		{
			if ( AosAttributes.GetValue( from, AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				tithing = 0;

			if ( tithing > 0 )
			{
				ArrayList targets = new ArrayList();
				foreach ( Item item in World.Items.Values )
				{
					if ( item is SoulLantern )
					{
						SoulLantern lantern = (SoulLantern)item;
						if ( lantern.owner == from )
						{
							lantern.TrappedSouls = lantern.TrappedSouls - tithing;
							if ( lantern.TrappedSouls < 1 ){ lantern.TrappedSouls = 0; }
							lantern.InvalidateProperties();
						}
					}
				}
			}
		}

		public static int GetSoulsInLantern( Mobile from )
		{
			int souls = 0;

			ArrayList targets = new ArrayList();
			foreach ( Item item in World.Items.Values )
			{
				if ( item is SoulLantern )
				{
					SoulLantern lantern = (SoulLantern)item;
					if ( lantern.owner == from )
					{
						souls = lantern.TrappedSouls;
					}
				}
			}

			return souls;
		}

		public static double GetKarmaPower( Mobile from )
		{
			int karma = ( from.Karma * -1 );
				if ( karma < 1 ){ karma = 0; }
				if ( karma > 15000 ){ karma = 15000; }

			double hate = karma / 125;

			return hate;
		}
	}
}