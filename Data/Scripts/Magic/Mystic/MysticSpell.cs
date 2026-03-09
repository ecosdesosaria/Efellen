using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Items;

namespace Server.Spells.Mystic
{
	public abstract class MysticSpell : Spell
	{
		public abstract int RequiredTithing { get; }
		public abstract double RequiredSkill { get; }
		public abstract int RequiredMana{ get; }
		public override bool ClearHandsOnCast { get { return false; } }
		public override SkillName CastSkill { get { return SkillName.FistFighting; } }
		public override SkillName DamageSkill { get { return SkillName.FistFighting; } }
		public override int CastRecoveryBase { get { return 2; } }
		public abstract int MysticSpellCircle{ get; }

		public MysticSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public static string SpellDescription( int spell )
		{
			string txt = "Este pergaminho contém o conhecimento dos místicos: ";
			string skl = "0";

			if ( spell == 250 ){             skl = "80"; txt += "Entre no plano astral onde sua alma é imune a danos. Enquanto estiver nesse estado, você pode viajar livremente, mas sua interação com o mundo é mínima. Quanto maior sua habilidade, maior a duração. Monges usam essa habilidade para viajar com segurança por áreas perigosas."; }
			else if ( spell == 251 ){        skl = "50"; txt += "Viaje pelo plano astral para outro local usando uma runa de recall mágica. A runa deve ser marcada por outros meios mágicos antes que você possa viajar para esse local. Se desejar viajar usando um livro de runas, defina a localização padrão do seu livro de runas e então você poderá mirar no livro ao usar esta habilidade."; }
			else if ( spell == 252 ){        skl = "25"; txt += "Cria uma vestimenta que você precisará para usar as outras habilidades deste tomo. A vestimenta terá poder baseado na sua habilidade geral como monge, e ninguém mais poderá usá-la. Você só pode ter uma vestimenta dessas por vez, então criar uma nova fará com que quaisquer outras que você possua retornem ao plano astral. Após a criação, clique uma vez na vestimenta e selecione a opção 'Encantar' para gastar os pontos nos atributos que deseja que a vestimenta tenha."; }
			else if ( spell == 253 ){        skl = "30"; txt += "Execute um toque suave, curando danos sofridos. Quanto maior sua habilidade, mais dano você curará com seu toque."; }
			else if ( spell == 254 ){        skl = "35"; txt += "Permite que você salte uma longa distância. Esta é uma ação rápida e pode permitir que um monge salte em direção a um oponente, salte para um lugar seguro ou salte sobre alguns obstáculos como rios e riachos."; }
			else if ( spell == 255 ){        skl = "30"; txt += "Invoca seu Ki para realizar um ataque mental que causa uma quantidade de dano energético baseada em seus valores de luta com punhos e inteligência. Resistências Elementais podem reduzir o dano causado por este ataque."; }
			else if ( spell == 256 ){        skl = "60"; txt += "Sua pura força de vontade cria uma barreira ao seu redor, desviando ataques mágicos. Isto não funciona contra magias incomuns como a necromancia. Feitiços afetados frequentemente ricocheteiam de volta para o conjurador."; }
			else if ( spell == 257 ){        skl = "40"; txt += "Você pode purificar seu corpo de venenos com esta habilidade devido à sua disciplina física e, como tal, não pode ser usada para ajudar qualquer outra pessoa."; }
			else if ( spell == 258 ){        skl = "20"; txt += "Você deve estar usando algum tipo de luvas de pugilista para esta habilidade funcionar. Ela aprimora temporariamente o tipo de dano que as luvas causam. O tipo de dano infligido ao acertar um alvo será convertido para o tipo de resistência mais baixa do alvo. A duração do efeito é afetada por sua habilidade de luta com punhos."; }
			else if ( spell == 259 )
			{
				skl = "70"; txt += "Esta habilidade permite ao monge correr tão rápido quanto uma montaria. Esta habilidade deve ser evitada se você já estiver montado em uma montaria, ou talvez tenha botas mágicas que permitam correr nessa velocidade. Usar esta habilidade nessas condições pode causar velocidades de deslocamento incomuns, então tome cuidado.";
				if ( MySettings.S_NoMountsInCertainRegions )
					txt += " Esteja ciente, ao explorar a terra, de que há algumas áreas onde você não pode usar esta habilidade. Estas são áreas como masmorras, cavernas e alguns locais internos. Se você entrar em tal área, esta habilidade será prejudicada.";
			}

			if ( skl == "0" )
				return txt;

			return txt + " Requer que um Místico tenha pelo menos " + skl + " de habilidade para usá-la.";
		}

		public override bool CheckCast()
		{
			int mana = ScaleMana( RequiredMana );

			if ( !base.CheckCast() )
				return false;

			if ( Caster.TithingPoints < RequiredTithing )
			{
				Caster.SendLocalizedMessage( 1060173, RequiredTithing.ToString() ); // You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
				return false;
			}
			else if ( !MonkNotIllegal( Caster ) && !( this is CreateRobe ) )
			{
				Caster.SendMessage( "Seu equipamento ou habilidades não são compatíveis com as de um verdadeiro monge." );
				return false;
			}
			else if ( this is WindRunner && MySettings.S_NoMountsInCertainRegions && Server.Mobiles.AnimalTrainer.IsNoMountRegion( Caster, Region.Find( Caster.Location, Caster.Map ) ) )
			{
				Caster.SendMessage( "Esta habilidade não parece funcionar neste lugar." );
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage( 1060174, mana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			return true;
		}

		public static bool MonkNotIllegal( Mobile from )
		{
			if ( from.FindItemOnLayer( Layer.OneHanded ) != null )
			{
				Item oneHand = from.FindItemOnLayer( Layer.OneHanded );

				if ( oneHand is Artifact_GlovesOfThePugilist || oneHand is GiftPugilistGloves || oneHand is LevelPugilistGloves || oneHand is PugilistGloves || oneHand is PugilistGlove ){}
				else if ( oneHand is BaseWeapon )
					return false;
			}

			if ( from.FindItemOnLayer( Layer.TwoHanded ) != null )
			{
				Item twoHand = from.FindItemOnLayer( Layer.TwoHanded );

				if ( twoHand is BaseWeapon )
					return false;

				if ( twoHand is BaseArmor )
				{
					if ( ((BaseArmor)twoHand).Attributes.SpellChanneling == 0 )
						return false;
				}
			}

			if ( Server.Misc.RegenRates.GetArmorOffset( from ) > 0 )
			{
				return false;
			}

			if ( from.FindItemOnLayer( Layer.OuterTorso ) != null )
			{
				Item robe = from.FindItemOnLayer( Layer.OuterTorso );
				if ( !(robe is MysticMonkRobe) )
					return false;
			}
			else { return false; }

			if ( from.Skills[SkillName.Focus].Base < 100 || from.Skills[SkillName.Meditation].Base < 100 )
			{
				return false;
			}

			return true;
		}

		public override bool CheckFizzle()
		{
			int requiredTithing = this.RequiredTithing;

			if ( AosAttributes.GetValue( Caster, AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				requiredTithing = 0;

			int mana = ScaleMana( RequiredMana );

			if ( Caster.TithingPoints < requiredTithing )
			{
				Caster.SendLocalizedMessage( 1060173, RequiredTithing.ToString() ); // You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
				return false;
			}
			else if ( !MonkNotIllegal( Caster ) && !( this is CreateRobe ) )
			{
				Caster.SendMessage( "Seu equipamento ou habilidades não são compatíveis com as de um verdadeiro monge." );
				return false;
			}
			else if ( this is WindRunner && MySettings.S_NoMountsInCertainRegions && Server.Mobiles.AnimalTrainer.IsNoMountRegion( Caster, Region.Find( Caster.Location, Caster.Map ) ) )
			{
				Caster.SendMessage( "Esta habilidade não parece funcionar neste lugar." );
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage( 1060174, mana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			Caster.TithingPoints -= requiredTithing;

			return base.CheckFizzle();
		}

		public override void SayMantra()
		{
			Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, Info.Mantra );
		}

		public override void DoFizzle()
		{
			Caster.PlaySound( 0x1D6 );
			Caster.NextSpellTime = DateTime.Now;
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 0x1D6 );
		}

		public override double GetResistSkill( Mobile m )
		{
			int maxSkill = (1 + (int)MysticSpellCircle) * 10;
			maxSkill += (1 + ((int)MysticSpellCircle / 6)) * 25;

			if( m.Skills[SkillName.MagicResist].Value < maxSkill )
				m.CheckSkill( SkillName.MagicResist, 0.0, 120.0 );

			return m.Skills[SkillName.MagicResist].Value;
		}

		public virtual bool CheckResisted( Mobile target )
		{
			double n = GetResistPercent( target );

			n /= 100.0;

			if( n <= 0.0 )
				return false;

			if( n >= 1.0 )
				return true;

			int maxSkill = (1 + (int)MysticSpellCircle) * 10;
			maxSkill += (1 + ((int)MysticSpellCircle / 6)) * 25;

			if( target.Skills[SkillName.MagicResist].Value < maxSkill )
				target.CheckSkill( SkillName.MagicResist, 0.0, 120.0 );

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercentForCircle( Mobile target )
		{
			double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)MysticSpellCircle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}

		public virtual double GetResistPercent( Mobile target )
		{
			return GetResistPercentForCircle( target );
		}

		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
				Caster.PlaySound( 0x1D6 );
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			SendCastEffect();
		}

		public virtual void SendCastEffect()
		{
			Caster.FixedEffect( 0x37C4, 10, (int)( GetCastDelay().TotalSeconds * 28 ), 4, 3 );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill;
			max = RequiredSkill + 50.0;
		}

		public override int GetMana()
		{
			return RequiredMana;
		}
	}
}
