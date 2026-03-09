using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using System.Collections;

namespace Server.Spells.HolyMan
{
	public abstract class HolyManSpell : Spell
	{
		public abstract int RequiredTithing { get; }
		public abstract double RequiredSkill { get; }
		public abstract int RequiredMana{ get; }
		public override bool ClearHandsOnCast { get { return false; } }
		public override SkillName CastSkill { get { return SkillName.Spiritualism; } }
		public override SkillName DamageSkill { get { return SkillName.Healing; } }
		public override int CastRecoveryBase { get { return 7; } }

		public HolyManSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public static string SpellDescription( int spell )
		{
			string txt = "Este símbolo contém o conhecimento das bênçãos espirituais: ";
			string skl = "0";

			if ( spell == 770 ){ 		skl = "60";	txt += "Envia demônios e mortos de volta aos reinos do inferno."; }
			else if ( spell == 771 ){ 	skl = "70";	txt += "Absorve mana de outros e a concede ao sacerdote."; }
			else if ( spell == 772 ){ 	skl = "90";	txt += "Imbuí temporariamente uma arma com poderes sagrados."; }
			else if ( spell == 773 ){ 	skl = "50";	txt += "Convoca temporariamente um martelo dos deuses."; }
			else if ( spell == 774 ){ 	skl = "10";	txt += "Destrói a escuridão, permitindo enxergar melhor."; }
			else if ( spell == 775 ){ 	skl = "10";	txt += "O sacerdote pode ajudar aqueles que estão famintos ou sedentos."; }
			else if ( spell == 776 ){ 	skl = "40";	txt += "Remove maldições e outros efeitos debilitantes."; }
			else if ( spell == 777 ){ 	skl = "80";	txt += "Traz alguém de volta à vida, ou invoca um orbe para ressuscitar o sacerdote posteriormente."; }
			else if ( spell == 778 ){ 	skl = "20";	txt += "Circunda alguém com uma aura sagrada que cura ferimentos muito mais rápido."; }
			else if ( spell == 779 ){ 	skl = "30";	txt += "Os deuses concedem ao sacerdote maior força, velocidade e inteligência."; }
			else if ( spell == 780 ){ 	skl = "60";	txt += "Permite ao sacerdote adentrar o reino dos mortos, evitando qualquer dano."; }
			else if ( spell == 781 ){ 	skl = "40";	txt += "Invoca um raio dos céus, causando dano duplo a demônios e mortos-vivos."; }
			else if ( spell == 782 ){ 	skl = "20";	txt += "Restaura saúde e vigor aos cansados."; }
			else if ( spell == 783 ){ 	skl = "30";	txt += "Envolve o sacerdote em chamas sagradas, refletindo magia de volta ao conjurador."; }

			if ( skl == "0" )
				return txt;

			return txt + " Requer que um Sacerdote tenha pelo menos " + skl + " em Espiritualismo.";
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Caster.Karma < 2500 )
			{
				Caster.SendMessage( "Você tem Karma insuficiente para invocar esta prece." );
				return false;
			}
			else if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + RequiredSkill + " em Espiritualismo para invocar esta prece." );
				return false;
			}
			else if ( GetSoulsInSymbol( Caster ) < RequiredTithing )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + RequiredTithing + " de piedade para invocar esta prece." );
				return false;
			}
			else if ( Caster.Mana < GetMana() )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + GetMana() + " de Mana para invocar esta prece." );
				return false;
			}

			return true;
		}

		public override bool CheckFizzle()
		{
			int requiredTithing = GetTithing( Caster, this );
			int mana = GetMana();

			if ( Caster.Karma < 2500 )
			{
				Caster.SendMessage( "Você tem Karma insuficiente para invocar esta prece." );
				return false;
			}
			else if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + RequiredSkill + " em Espiritualismo para invocar esta prece." );
				return false;
			}
			else if ( GetSoulsInSymbol( Caster ) < requiredTithing )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + requiredTithing + " de piedade para invocar esta prece." );
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendMessage( "Você precisa ter pelo menos " + mana + " de Mana para invocar esta prece." );
				return false;
			}

			DrainSoulsInSymbol( Caster, RequiredTithing );

			return base.CheckFizzle();
		}

		public override void DoFizzle()
		{
			Caster.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "Você falha em invocar o poder.", Caster.NetState);
			Caster.FixedParticles( 0x3735, 1, 30, 9503, EffectLayer.Waist );
			Caster.PlaySound( 0x1D6 );
			Caster.NextSpellTime = DateTime.Now;
		}

		public override int GetMana()
		{
			return RequiredMana;
		}

		public static int GetTithing( Mobile Caster, HolyManSpell spell )
		{
			if ( AosAttributes.GetValue( Caster, AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				return 0;

			return spell.RequiredTithing;
		}

		public override void SayMantra()
		{
			Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, Info.Mantra );
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 0x1D6 );
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

			int v = (int) Math.Sqrt( ( from.Karma * -1 ) + 20000 + ( from.Skills.Spiritualism.Fixed * 10 ) );

			return v / div;
		}

		public static void DrainSoulsInSymbol( Mobile from, int tithing )
		{
			if ( AosAttributes.GetValue( from, AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				tithing = 0;

			if ( tithing > 0 )
			{
				ArrayList targets = new ArrayList();
				foreach ( Item item in World.Items.Values )
				{
					if ( item is HolySymbol )
					{
						HolySymbol symbol = (HolySymbol)item;
						if ( symbol.owner == from )
						{
							symbol.BanishedEvil = symbol.BanishedEvil - tithing;
							if ( symbol.BanishedEvil < 1 ){ symbol.BanishedEvil = 0; }
							symbol.InvalidateProperties();
						}
					}
				}
			}
		}

		public static int GetSoulsInSymbol( Mobile from )
		{
			int souls = 0;

			ArrayList targets = new ArrayList();
			foreach ( Item item in World.Items.Values )
			{
				if ( item is HolySymbol )
				{
					HolySymbol symbol = (HolySymbol)item;
					if ( symbol.owner == from )
					{
						souls = symbol.BanishedEvil;
					}
				}
			}

			return souls;
		}
	}
}
