using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Spells.Elementalism
{
	public abstract class ElementalSpell : Spell
	{
		public override SkillName CastSkill{ get{ return SkillName.Elementalism; } }
		public override SkillName DamageSkill{ get{ return SkillName.Elementalism; } }

		public ElementalSpell( Mobile caster, Item scroll, SpellInfo info ): base( caster, scroll, info )
		{
		}

		public abstract SpellCircle Circle { get; }

		public override bool ConsumeReagents()
		{
			return true;
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Caster.Stam < GetStam() )
			{
				Caster.SendMessage( "Stamina insuficiente para este feitiço." );
				Server.Misc.IntelligentAction.FizzleSpell( Caster );
				return false;
			}
			else if ( ArmorFizzle( Caster ) >= Utility.RandomMinMax(1,100) )
			{
				Caster.SendMessage( "Você está usando armadura demais para lançar isso (" + ArmorFizzle( Caster ) + "%)." );
				Server.Misc.IntelligentAction.FizzleSpell( Caster );
				return false;
			}

			return true;
		}

		private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;

		public override void GetCastSkills( out double min, out double max )
		{
			int circle = (int)Circle;

			if( Scroll != null )
				circle -= 2;

			double avg = ChanceLength * circle;

			min = avg - ChanceOffset;
			max = avg + ChanceOffset;
		}

		private static int[] m_ManaTable = new int[] { 5, 7, 10, 14, 19, 24, 40, 50 };

		public override int GetMana()
		{
			return m_ManaTable[(int)Circle];
		}

		public static int GetPower( int circle )
		{
			return m_ManaTable[circle];
		}

		public static string GetElement( Mobile m )
		{
			int element = ((PlayerMobile)m).CharacterElement;

			string elm = "air";

			if ( element == 0 )
				elm = "air";

			else if ( element == 1 )
				elm = "earth";

			else if ( element == 2 )
				elm = "fire";

			else if ( element == 3 )
				elm = "water";

			return elm;
		}

		public int GetStam()
		{
			int stam = GetMana();

			double reduce = AosAttributes.GetValue( Caster, AosAttribute.LowerRegCost ) * 0.01;

			stam = stam - ( (int)(stam * reduce) );

			if ( stam < 0 )
				stam = 0;

			return stam;
		}

		public override double GetResistSkill( Mobile m )
		{
			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if( m.Skills[SkillName.MagicResist].Value < maxSkill )
				m.CheckSkill( SkillName.MagicResist, 0.0, m.Skills[SkillName.MagicResist].Cap );

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

			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if( target.Skills[SkillName.MagicResist].Value < maxSkill )
				target.CheckSkill( SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap );

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercentForCircle( Mobile target, SpellCircle circle )
		{
			double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}

		public virtual double GetResistPercent( Mobile target )
		{
			return GetResistPercentForCircle( target, Circle );
		}

		public override TimeSpan GetCastDelay()
		{
			return base.GetCastDelay();
		}

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}

		public static void AddWater( Mobile m )
		{
			Item water = new ElementalEffect( Utility.RandomMinMax( 0x5691, 0x569A ), 5.0, null );
			water.Name = "water";
			water.Hue = 0xB3F;
			water.MoveToWorld( m.Location, m.Map );

			int extraWater = Utility.RandomMinMax( 2, 4 );

			for( int i = 0; i < extraWater; i++ )
			{
				Item wet = new ElementalEffect( Utility.RandomMinMax( 0x5691, 0x569A ), 5.0, null );
				wet.Name = "water";
				wet.Hue = 0xB3F;
				wet.MoveToWorld( new Point3D(
				m.X + Utility.RandomMinMax( -1, 1 ),
				m.Y + Utility.RandomMinMax( -1, 1 ),
				m.Z ), m.Map );
			}
		}

		public static int ElementalHue( string element )
		{
			int hue = 0;

			int val = Utility.RandomMinMax( 0, 3 );
				if ( element == "earth" ){ val = 0; }
				else if ( element == "water" ){ val = 3; }
				else if ( element == "air" ){ val = 2; }
				else if ( element == "fire" ){ val = 1; }

			switch ( val )
			{
				case 0:	hue = Utility.RandomList( 0xB79, 0xB51, 0x85D, 0x82E, 0xB61, 0xABE, 0xABF, 0xAC0 ); break; // EARTH
				case 1:	hue = Utility.RandomList( 0xB17, 0x981, 0x86C, 0x775 ); break; // FIRE
				case 2:	hue = Utility.RandomList( 0x8C1, 0xB2B, 0x613, 0xB4D, 0xAFE, 0xAF8, 0x8E4 ); break; // AIR
				case 3:	hue = Utility.RandomList( 0x97F, 0xB3D, 0xB0A, 0x5CE ); break; // WATER
			}

			return hue;
		}

		public static string CommonInfo( int id, int cat )
		{
			string info = "";

			string shortName = "";
			string longName = "";
			string mantra = "";
			string sphere = "First";
			string skill = "0";

			if ( id == 300 ){ 	   shortName = "Armor"; longName = "Elemental Armor"; mantra = "Armura"; }
			else if ( id == 301 ){ shortName = "Bolt"; longName = "Elemental Bolt"; mantra = "Sulita"; }
			else if ( id == 302 ){ shortName = "Mend"; longName = "Elemental Mend"; mantra = "Vindeca"; }
			else if ( id == 303 ){ shortName = "Sanctuary"; longName = "Elemental Sanctuary"; mantra = "Invata"; }
			else if ( id == 304 ){ shortName = "Pain"; longName = "Elemental Pain"; mantra = "Durere"; sphere = "Second"; }
			else if ( id == 305 ){ shortName = "Protection"; longName = "Elemental Protection"; mantra = "Proteja"; sphere = "Second"; }
			else if ( id == 306 ){ shortName = "Purge"; longName = "Elemental Purge"; mantra = "Vindeca"; sphere = "Second"; }
			else if ( id == 307 ){ shortName = "Steed"; longName = "Elemental Steed"; mantra = "Faptura"; sphere = "Second"; }
			else if ( id == 308 ){ shortName = "Call"; longName = "Elemental Call"; mantra = "Striga"; sphere = "Third"; skill = "9"; }
			else if ( id == 309 ){ shortName = "Force"; longName = "Elemental Force"; mantra = "Forta"; sphere = "Third"; skill = "9"; }
			else if ( id == 310 ){ shortName = "Wall"; longName = "Elemental Wall"; mantra = "Perete"; sphere = "Third"; skill = "9"; }
			else if ( id == 311 ){ shortName = "Warp"; longName = "Elemental Warp"; mantra = "Urzeala"; sphere = "Third"; skill = "9"; }
			else if ( id == 312 ){ shortName = "Field"; longName = "Elemental Field"; mantra = "Limite"; sphere = "Fourth"; skill = "23"; }
			else if ( id == 313 ){ shortName = "Restoration"; longName = "Elemental Restoration"; mantra = "Restabili"; sphere = "Fourth"; skill = "23"; }
			else if ( id == 314 ){ shortName = "Strike"; longName = "Elemental Strike"; mantra = "Lovitura"; sphere = "Fourth"; skill = "23"; }
			else if ( id == 315 ){ shortName = "Void"; longName = "Elemental Void"; mantra = "Mutare"; sphere = "Fourth"; skill = "23"; }
			else if ( id == 316 ){ shortName = "Blast"; longName = "Elemental Blast"; mantra = "Deteriora"; sphere = "Fifth"; skill = "38"; }
			else if ( id == 317 ){ shortName = "Echo"; longName = "Elemental Echo"; mantra = "Oglinda"; sphere = "Fifth"; skill = "38"; }
			else if ( id == 318 ){ shortName = "Fiend"; longName = "Elemental Fiend"; mantra = "Diavol"; sphere = "Fifth"; skill = "38"; }
			else if ( id == 319 ){ shortName = "Hold"; longName = "Elemental Hold"; mantra = "Temnita"; sphere = "Fifth"; skill = "38"; }
			else if ( id == 320 ){ shortName = "Barrage"; longName = "Elemental Barrage"; mantra = "Baraj"; sphere = "Sixth"; skill = "52"; }
			else if ( id == 321 ){ shortName = "Rune"; longName = "Elemental Rune"; mantra = "Marca"; sphere = "Sixth"; skill = "52"; }
			else if ( id == 322 ){ shortName = "Storm"; longName = "Elemental Storm"; mantra = "Furtuna"; sphere = "Sixth"; skill = "52"; }
			else if ( id == 323 ){ shortName = "Summon"; longName = "Elemental Summon"; mantra = "Convoca"; sphere = "Sixth"; skill = "52"; }
			else if ( id == 324 ){ shortName = "Devastation"; longName = "Elemental Devastation"; mantra = "Devasta"; sphere = "Seventh"; skill = "66"; }
			else if ( id == 325 ){ shortName = "Fall"; longName = "Elemental Fall"; mantra = "Toamna"; sphere = "Seventh"; skill = "66"; }
			else if ( id == 326 ){ shortName = "Gate"; longName = "Elemental Gate"; mantra = "Poarta"; sphere = "Seventh"; skill = "66"; }
			else if ( id == 327 ){ shortName = "Havoc"; longName = "Elemental Havoc"; mantra = "Haotic"; sphere = "Seventh"; skill = "66"; }
			else if ( id == 328 ){ shortName = "Apocalypse"; longName = "Elemental Apocalypse"; mantra = "Moarte"; sphere = "Eighth"; skill = "80"; }
			else if ( id == 329 ){ shortName = "Lord"; longName = "Elemental Lord"; mantra = "Dumnezeu"; sphere = "Eighth"; skill = "80"; }
			else if ( id == 330 ){ shortName = "Soul"; longName = "Elemental Soul"; mantra = "Viata"; sphere = "Eighth"; skill = "80"; }
			else if ( id == 331 ){ shortName = "Spirit"; longName = "Elemental Spirit"; mantra = "Fantoma"; sphere = "Eighth"; skill = "80"; }

			if ( cat == 1 ){ info = shortName; }
			else if ( cat == 2 ){ info = longName; }
			else if ( cat == 3 ){ info = mantra; }
			else if ( cat == 4 ){ }
			else if ( cat == 5 ){ info = longName.ToLower(); }
			else if ( cat == 6 ){ info = sphere; }
			else if ( cat == 7 ){ info = skill; }

			return info;
		}

		public static string SpellDescription( int spell )
		{
			string txt = "Este pergaminho contém um feitiço elemental, onde um exemplo do que ele faz é fornecido dentro da Esfera do Elementalismo do Ar: " + DescriptionInfo( spell, 0x6717 ) + "";

			return txt + " Ele requer pelo menos um " + CommonInfo( spell, 7 ) + " em Elementalismo.";
		}

		public static string DescriptionInfo( int id, int item )
		{
			string description = "";

			string elm = "air";

			if ( item == 0x6717 ){ elm = "air"; }
			else if ( item == 0x6713 ){ elm = "earth"; }
			else if ( item == 0x6719 ){ elm = "fire"; }
			else if ( item == 0x6715 ){ elm = "water"; }

			if ( id == 300 )
			{
				description = "Aumenta sua resistência física enquanto reduz suas outras resistências. Ativo até que o feitiço seja desativado ao lançá-lo novamente.";
				if ( elm == "air" ){ description = "Aumenta sua resistência de energia enquanto reduz suas outras resistências. Ativo até que o feitiço seja desativado ao lançá-lo novamente."; }
				else if ( elm == "fire" ){ description = "Aumenta sua resistência de fogo enquanto reduz suas outras resistências. Ativo até que o feitiço seja desativado ao lançá-lo novamente."; }
				else if ( elm == "water" ){ description = "Aumenta sua resistência de frio enquanto reduz suas outras resistências. Ativo até que o feitiço seja desativado ao lançá-lo novamente."; }
			}
			else if ( id == 301 )
			{
				description = "Dispara um raio mágico em um alvo, que causa dano de fogo e físico.";
				if ( elm == "air" ){ description = "Dispara um raio mágico em um alvo, que causa dano de energia e físico."; }
				else if ( elm == "earth" ){ description = "Dispara um raio mágico em um alvo, que causa dano de veneno e físico."; }
				else if ( elm == "water" ){ description = "Dispara um raio mágico em um alvo, que causa dano de gelo e físico."; }
			}
			else if ( id == 302 ){ description = "Restaura uma pequena quantidade de pontos de vida perdidos do alvo."; }
			else if ( id == 303 ){ description = "Transporta o elementalista para a segurança do Liceu. Pode ser conjurado em masmorras em níveis mais altos."; }
			else if ( id == 304 )
			{
				description = "Afasta o alvo com chamas, causando dano de fogo. Quanto mais próximo o alvo estiver do conjurador, mais dano é causado.";
				if ( elm == "air" ){ description = "Afasta o alvo com um ventoinho, causando dano físico e de energia. Quanto mais próximo o alvo estiver do conjurador, mais dano é causado."; }
				else if ( elm == "earth" ){ description = "Afasta o alvo com rochas caindo, causando dano físico. Quanto mais próximo o alvo estiver do conjurador, mais dano é causado."; }
				else if ( elm == "water" ){ description = "Afasta o alvo com água congelante, causando dano de gelo. Quanto mais próximo o alvo estiver do conjurador, mais dano é causado."; }
			}
			else if ( id == 305 ){ description = "Impede que os feitiços do conjurador sejam interrompidos, mas diminui sua resistência física e resistência mágica. Ativo até que o feitiço seja desativado ao ser relançado."; }
			else if ( id == 306 )
			{
				description = "Tenta queimar venenos que afetam o alvo.";
				if ( elm == "air" ){ description = "Tenta soprar para longe venenos que afetam o alvo."; }
				else if ( elm == "earth" ){ description = "Tenta limpar venenos que afetam o alvo."; }
				else if ( elm == "water" ){ description = "Tenta lavar venenos que afetam o alvo."; }
			}
			else if ( id == 307 )
			{
				description = "Invoca uma fênix flamejante que não luta, mas você pode montar por toda a terra. A criatura desaparece após um tempo determinado e requer um slot de controle.";
				if ( elm == "air" ){ description = "Invoca um dragão do ar que não luta, mas você pode montar por toda a terra. A criatura desaparece após um tempo determinado e requer um slot de controle."; }
				else if ( elm == "earth" ){ description = "Invoca um grande urso que não luta, mas você pode montar por toda a terra. A criatura desaparece após um tempo determinado e requer um slot de controle."; }
				else if ( elm == "water" ){ description = "Invoca um besouro d'água que não luta, mas você pode montar por toda a terra. A criatura desaparece após um tempo determinado e requer um slot de controle."; }
			}
			else if ( id == 308 )
			{
				description = "Um elemental do fogo menor é invocado para servir o conjurador. O elemental desaparece após um tempo determinado e requer um slot de controle.";
				if ( elm == "air" ){ description = "Um elemental do ar menor é invocado para servir o conjurador. O elemental desaparece após um tempo determinado e requer um slot de controle."; }
				else if ( elm == "earth" ){ description = "Um elemental da terra menor é invocado para servir o conjurador. O elemental desaparece após um tempo determinado e requer um slot de controle."; }
				else if ( elm == "water" ){ description = "Um elemental da água menor é invocado para servir o conjurador. O elemental desaparece após um tempo determinado e requer um slot de controle."; }
			}
			else if ( id == 309 )
			{
				description = "Dispara uma bola de fogo em um alvo, causando algum dano físico, mas principalmente dano de fogo.";
				if ( elm == "air" ){ description = "Dispara um raio em um alvo, causando algum dano físico, mas principalmente dano de energia."; }
				else if ( elm == "earth" ){ description = "Arremessa uma rocha mágica em um alvo, causando dano físico."; }
				else if ( elm == "water" ){ description = "Força água dolorosa em um alvo, causando algum dano físico, mas principalmente dano de gelo."; }
			}
			else if ( id == 310 )
			{
				description = "Cria uma parede temporária de chamas que bloqueia o movimento.";
				if ( elm == "air" ){ description = "Cria uma parede temporária de ar que bloqueia o movimento."; }
				else if ( elm == "earth" ){ description = "Cria uma parede temporária de lama que bloqueia o movimento."; }
				else if ( elm == "water" ){ description = "Cria uma parede temporária de algas que bloqueia o movimento."; }
			}
			else if ( id == 311 ){ description = "O Conjurador é transportado para o local alvo."; }
			else if ( id == 312 )
			{
				description = "Cria uma parede de chamas que causa dano de fogo a todos que passarem por ela.";
				if ( elm == "air" ){ description = "Cria uma parede de eletricidade que causa dano de energia a todos que passarem por ela."; }
				else if ( elm == "earth" ){ description = "Cria uma parede de vinhas que causa dano físico e de veneno a todos que passarem por ela."; }
				else if ( elm == "water" ){ description = "Cria uma parede de água que causa dano de gelo a todos que passarem por ela."; }
			}
			else if ( id == 313 ){ description = "Restaura uma quantidade média de pontos de vida perdidos do alvo."; }
			else if ( id == 314 )
			{
				description = "Ataca o alvo com lava caindo, que causa dano físico e de fogo.";
				if ( elm == "air" ){ description = "Ataca o alvo com cometas do céu, que causam dano físico e de energia."; }
				else if ( elm == "earth" ){ description = "Ataca o alvo com rochas caindo, que causam dano físico."; }
				else if ( elm == "water" ){ description = "Ataca o alvo com estilhaços de gelo de cima, que causam dano físico e de gelo."; }
			}
			else if ( id == 315 ){ description = "O Conjurador é transportado para a localização marcada numa runa, junto com seus seguidores. Se uma chave de navio for alvo, o conjurador é transportado para o barco que a chave abre."; }
			else if ( id == 316 )
			{
				description = "Faz uma explosão flamejante atingir seu alvo com dano de fogo, dependente de seu elementalismo e inteligência. Tem um pequeno atraso.";
				if ( elm == "air" ){ description = "Faz uma explosão elétrica atingir seu alvo com dano de energia, dependente de seu elementalismo e inteligência. Tem um pequeno atraso."; }
				else if ( elm == "earth" ){ description = "Faz uma explosão de pedra atingir seu alvo com dano físico, dependente de seu elementalismo e inteligência. Tem um pequeno atraso."; }
				else if ( elm == "water" ){ description = "Faz uma explosão aquática atingir seu alvo com dano de gelo, dependente de seu elementalismo e inteligência. Tem um pequeno atraso."; }
			}
			else if ( id == 317 )
			{
				string rock = "a ruby";
				if ( elm == "air" ){ rock = "an amethyst"; }
				else if ( elm == "earth" ){ rock = "an emerald"; }
				else if ( elm == "water" ){ rock = "a sapphire"; }
				description = "Feitiços de magia prejudiciais lançados em você serão refletidos de volta ao conjurador com base em seu elementalismo. Você precisará de " + rock + " para fazer este feitiço funcionar.";
			}
			else if ( id == 318 )
			{
				description = "Conjura uma criatura de lava viscosa que ataca um alvo com base em sua força de combate e proximidade. Ela desaparece após um tempo determinado e requer 2 slots de controle.";
				if ( elm == "air" ){ description = "Conjura uma criatura de luz estelar que ataca um alvo com base em sua força de combate e proximidade. Ela desaparece após um tempo determinado e requer 2 slots de controle."; }
				else if ( elm == "earth" ){ description = "Conjura uma criatura vegetal que ataca um alvo com base em sua força de combate e proximidade. Ela desaparece após um tempo determinado e requer 2 slots de controle."; }
				else if ( elm == "water" ){ description = "Conjura uma criatura aquática viscosa que ataca um alvo com base em sua força de combate e proximidade. Ela desaparece após um tempo determinado e requer 2 slots de controle."; }
			}
			else if ( id == 319 )
			{
				description = "Fios de chama emergem para imobilizar o alvo por um breve período de tempo. A habilidade de resistência mágica do alvo afeta a duração da imobilização.";
				if ( elm == "air" ){ description = "Estrelas aparecem para imobilizar o alvo por um breve período de tempo. A habilidade de resistência mágica do alvo afeta a duração da imobilização."; }
				else if ( elm == "earth" ){ description = "Vinhas emergem para imobilizar o alvo por um breve período de tempo. A habilidade de resistência mágica do alvo afeta a duração da imobilização."; }
				else if ( elm == "water" ){ description = "Tentáculos de lula emergem para imobilizar o alvo por um breve período de tempo. A habilidade de resistência mágica do alvo afeta a duração da imobilização."; }
			}
			else if ( id == 320 )
			{
				description = "Lança uma gota de plasma ardente no alvo, causando dano significativo de fogo.";
				if ( elm == "air" ){ description = "Lança uma onda de eletricidade mágica no alvo, causando dano significativo de energia."; }
				else if ( elm == "earth" ){ description = "Lança um orbe de gás de pântano vil no alvo, causando dano significativo de veneno."; }
				else if ( elm == "water" ){ description = "Lança uma esfera de água mística no alvo, causando dano significativo de gelo."; }
			}
			else if ( id == 321 ){ description = "Marca uma runa na localização atual do elementalista. Existem feitiços e habilidades mágicas que podem ser usados na runa para teleportar alguém para a localização com a qual ela está marcada."; }
			else if ( id == 322 )
			{
				description = "Cria uma tempestade vulcânica de magma derretido, causando dano físico e de fogo.";
				if ( elm == "air" ){ description = "Cria tempestades de ventoinho ao redor do alvo, causando dano físico e de energia."; }
				else if ( elm == "earth" ){ description = "Causa uma tempestade de hera venenosa ao redor do alvo, causando dano de veneno e físico."; }
				else if ( elm == "water" ){ description = "Invoca um tufão de vento e água, causando dano físico e de gelo."; }
			}
			else if ( id == 323 )
			{
				description = "Um elemental de magma é invocado para servir o conjurador.";
				if ( elm == "air" ){ description = "Um elemental de relâmpago é invocado para servir o conjurador."; }
				else if ( elm == "earth" ){ description = "Um ent é invocado para servir o conjurador."; }
				else if ( elm == "water" ){ description = "Um elemental de gelo é invocado para servir o conjurador."; }
			}
			else if ( id == 324 )
			{
				description = "Invoca um malestrom flamejante, danificando inimigos próximos com dano de fogo.";
				if ( elm == "air" ){ description = "Traz poderosas tempestades de relâmpagos, danificando inimigos próximos com dano de energia."; }
				else if ( elm == "earth" ){ description = "Invoca montes de terra e lama para cair sobre inimigos próximos, causando dano físico."; }
				else if ( elm == "water" ){ description = "Conjura uma tempestade de estilhaços de gelo sobre inimigos próximos, causando dano de gelo."; }
			}
			else if ( id == 325 )
			{
				description = "Traz uma tempestade de fogo que afeta todos os alvos dentro de um raio ao redor do local alvo. O dano total físico e de fogo é dividido entre todos os alvos.";
				if ( elm == "air" ){ description = "Invoca estrelas que explodem e afetam todos os alvos dentro de um raio ao redor do local alvo. O dano total físico e de energia é dividido entre todos os alvos."; }
				else if ( elm == "earth" ){ description = "Traz uma bola de gás de pântano que afeta todos os alvos dentro de um raio ao redor do local alvo. O dano físico e de veneno é dividido entre todos os alvos."; }
				else if ( elm == "water" ){ description = "Traz um furacão que afeta todos os alvos dentro de um raio ao redor do local alvo. O dano total físico e de gelo é dividido entre todos os alvos."; }
			}
			else if ( id == 326 ){ description = "Mirar uma runa marcada abre um portal temporário para a localização marcada da runa. O portal pode ser usado por qualquer um para viajar para aquela localização."; }
			else if ( id == 327 )
			{
				description = "Envolve o alvo em chamas, causando uma quantidade massiva de dano físico e de fogo.";
				if ( elm == "air" ){ description = "Conjura ventos de força de furacão ao redor do alvo, causando uma quantidade massiva de dano físico e de energia."; }
				else if ( elm == "earth" ){ description = "Envolve o alvo num enxame de abelhas mortais, causando uma quantidade massiva de dano físico e de veneno."; }
				else if ( elm == "water" ){ description = "Cria uma onda de água para explodir do alvo, causando uma quantidade massiva de dano físico e de gelo."; }
			}
			else if ( id == 328 )
			{
				description = "Invoca uma tempestade de fogo sobre os inimigos próximos ao conjurador, causando algum dano físico, mas principalmente dano de fogo.";
				if ( elm == "air" ){ description = "Envia estilhaços de uma estrela próxima sobre inimigos próximos, causando algum dano físico, mas principalmente dano de energia."; }
				else if ( elm == "earth" ){ description = "Erupciona uma quantidade massiva de lama e terra do chão, causando dano físico horripilante aos inimigos próximos."; }
				else if ( elm == "water" ){ description = "Traz uma chuva devastadora de gelo aquoso sobre inimigos próximos, causando algum dano físico, mas principalmente dano de gelo."; }
			}
			else if ( id == 329 )
			{
				description = "Um Senhor da Chama é invocado para ajudar o conjurador.";
				if ( elm == "air" ){ description = "Um Senhor dos Céus é invocado para ajudar o conjurador."; }
				else if ( elm == "earth" ){ description = "Um Senhor da Terra é invocado para ajudar o conjurador."; }
				else if ( elm == "water" ){ description = "Um Senhor do Mar é invocado para ajudar o conjurador."; }
			}
			else if ( id == 330 ){ description = "Ressuscita outro ou invoca um item mágico para se ressuscitar mais tarde."; }
			else if ( id == 331 )
			{
				description = "Invoca um espírito de lava que ataca um alvo com base em sua inteligência e proximidade. Ele desaparece após um tempo determinado e requer 2 slots de controle.";
				if ( elm == "air" ){ description = "Invoca um espírito de nuvem que ataca um alvo com base em sua inteligência e proximidade. Ele desaparece após um tempo determinado e requer 2 slots de controle."; }
				else if ( elm == "earth" ){ description = "Invoca um espírito da terra que ataca um alvo com base em sua inteligência e proximidade. Ele desaparece após um tempo determinado e requer 2 slots de controle."; }
				else if ( elm == "water" ){ description = "Invoca um espírito da água que ataca um alvo com base em sua inteligência e proximidade. Ele desaparece após um tempo determinado e requer 2 slots de controle."; }
			}

			return description;
		}

		public static int ArmorCheck( Item item, int val )
		{
			int num = 0;

			if ( item is BaseArmor && ((BaseArmor)item).ArmorAttributes.MageArmor == 0 && ((BaseArmor)item).Attributes.SpellChanneling == 0 )
			{
				if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Leather ){ num = 2 * val; }
				else if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Skin ){ num = 2 * val; }
				else if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Wood ){ num = 4 * val; }
				else if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Scales ){ num = 4 * val; }
				else if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Skeletal ){ num = 4 * val; }
				else if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Metal ){ num = 6 * val; }
				else if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Block ){ num = 6 * val; }
				else if ( CraftResources.GetType( item.Resource ) == CraftResourceType.Fabric ){ /* ADD NOTHING */ }
			}
			return num;
		}

		public static int ArmorFizzle( Mobile m )
		{
			int penalty = 0;

			if ( m.FindItemOnLayer( Layer.OuterTorso ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.OuterTorso ), 5 ); }
			if ( m.FindItemOnLayer( Layer.TwoHanded ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.TwoHanded ), 3 ); }
			if ( m.FindItemOnLayer( Layer.Helm ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Helm ), 2 ); }
			if ( m.FindItemOnLayer( Layer.Arms ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Arms ), 3 ); }
			if ( m.FindItemOnLayer( Layer.OuterLegs ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.OuterLegs ), 4 ); }
			if ( m.FindItemOnLayer( Layer.Neck ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Neck ), 1 ); }
			if ( m.FindItemOnLayer( Layer.Gloves ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Gloves ), 1 ); }
			if ( m.FindItemOnLayer( Layer.Shoes ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Shoes ), 1 ); }
			if ( m.FindItemOnLayer( Layer.Cloak ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Cloak ), 3 ); }
			if ( m.FindItemOnLayer( Layer.Waist ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Waist ), 1 ); }
			if ( m.FindItemOnLayer( Layer.InnerLegs ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.InnerLegs ), 4 ); }
			if ( m.FindItemOnLayer( Layer.InnerTorso ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.InnerTorso ), 4 ); }
			if ( m.FindItemOnLayer( Layer.Pants ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Pants ), 4 ); }
			if ( m.FindItemOnLayer( Layer.Shirt ) != null ){ penalty = penalty + ArmorCheck( m.FindItemOnLayer( Layer.Shirt ), 4 ); }

			return penalty;
		}

		public static int ScrollLook( int id, int cat )
		{
			int info = 0;
			int item = 0;
			int hue = 0;

			if ( id == 300 ){ item = 25676; hue = 0xB94; }
			else if ( id == 301 ){ item = 25677; hue = 0xB64; }
			else if ( id == 302 ){ item = 25678; hue = 0xB40; }
			else if ( id == 303 ){ item = 25679; hue = 0xAE2; }
			else if ( id == 304 ){ item = 25680; hue = 0x983; }
			else if ( id == 305 ){ item = 25677; hue = 0xB94; }
			else if ( id == 306 ){ item = 25678; hue = 0xB64; }
			else if ( id == 307 ){ item = 25679; hue = 0xB40; }
			else if ( id == 308 ){ item = 25680; hue = 0xAE2; }
			else if ( id == 309 ){ item = 25681; hue = 0x983; }
			else if ( id == 310 ){ item = 25676; hue = 0xB72; }
			else if ( id == 311 ){ item = 25678; hue = 0xB94; }
			else if ( id == 312 ){ item = 25679; hue = 0xB64; }
			else if ( id == 313 ){ item = 25680; hue = 0xB40; }
			else if ( id == 314 ){ item = 25681; hue = 0xAE2; }
			else if ( id == 315 ){ item = 25679; hue = 0xB94; }
			else if ( id == 316 ){ item = 25680; hue = 0xB64; }
			else if ( id == 317 ){ item = 25681; hue = 0xB40; }
			else if ( id == 318 ){ item = 25676; hue = 0x983; }
			else if ( id == 319 ){ item = 25677; hue = 0xB72; }
			else if ( id == 320 ){ item = 25680; hue = 0xB94; }
			else if ( id == 321 ){ item = 25681; hue = 0xB64; }
			else if ( id == 322 ){ item = 25676; hue = 0xAE2; }
			else if ( id == 323 ){ item = 25677; hue = 0x983; }
			else if ( id == 324 ){ item = 25681; hue = 0xB94; }
			else if ( id == 325 ){ item = 25676; hue = 0xB40; }
			else if ( id == 326 ){ item = 25677; hue = 0xAE2; }
			else if ( id == 327 ){ item = 25678; hue = 0x983; }
			else if ( id == 328 ){ item = 25676; hue = 0xB64; }
			else if ( id == 329 ){ item = 25677; hue = 0xB40; }
			else if ( id == 330 ){ item = 25678; hue = 0xAE2; }
			else if ( id == 331 ){ item = 25679; hue = 0x983; }

			if ( cat == 1 ){ info = item; }
			else if ( cat == 2 ){ info = hue; }

			return info;
		}

		public static int SpellIcon( int item, int spell )
		{
			if ( item == 0x6717 ){ spell = 11477 + spell - 300; }
			else if ( item == 0x6713 ){ spell = 11509 + spell - 300; }
			else if ( item == 0x6719 ){ spell = 11541 + spell - 300; }
			else if ( item == 0x6715 ){ spell = 11573 + spell - 300; }

			return spell;
		}

		public static string FontColor( int item )
		{
			string color = "";

			if ( item == 0x6717 ){ color = "#9484DE"; }			// AIR
			else if ( item == 0x6713 ){ color = "#ADE76b"; }	// EARTH
			else if ( item == 0x6719 ){ color = "#FFAD52"; }	// FIRE
			else if ( item == 0x6715 ){ color = "#189CE7"; }	// WATER

			return color;
		}

		public static void BookCover( Item item, int element )
		{
			if ( item.ArtifactLevel == 0 )
			{
				if ( element == 0 ){ item.ItemID = 0x6717; }		// AIR
				else if ( element == 1 ){ item.ItemID = 0x6713; }	// EARTH
				else if ( element == 2 ){ item.ItemID = 0x6719; }	// FIRE
				else if ( element == 3 ){ item.ItemID = 0x6715; }	// WATER
			}
		}

		public static bool CanUseBook( Item item, Mobile from, bool msg )
		{
			if ( item != null && from != null )
			{
				if ( item is ElementalSpellbook && from is PlayerMobile )
				{
					int element = ((PlayerMobile)from).CharacterElement;

					if ( element != 0 && item.ItemID == 0x6717 )
					{
						if ( msg ){ from.SendMessage("Você precisa estar focado na magia elemental do ar para usar isso!"); }
						return false;
					}
					if ( element != 1 && item.ItemID == 0x6713 )
					{
						if ( msg ){ from.SendMessage("Você precisa estar focado na magia elemental da terra para usar isso!"); }
						return false;
					}
					if ( element != 2 && item.ItemID == 0x6719 )
					{
						if ( msg ){ from.SendMessage("Você precisa estar focado na magia elemental do fogo para usar isso!"); }
						return false;
					}
					if ( element != 3 && item.ItemID == 0x6715 )
					{
						if ( msg ){ from.SendMessage("Você precisa estar focado na magia elemental da água para usar isso!"); }
						return false;
					}
				}
			}
			return true;
		}

		public static void UnequipBook( Mobile from )
		{
			if ( from.FindItemOnLayer( Layer.Trinket ) != null )
			{
				if ( from.FindItemOnLayer( Layer.Trinket ) is ElementalSpellbook )
				{
					if ( !CanUseBook( from.FindItemOnLayer( Layer.Trinket ), from, false ) )
					{
						Container pack = from.Backpack;
						from.AddToBackpack( from.FindItemOnLayer( Layer.Trinket ) );
					}
				}
			}
		}

		public static void ChangeBooks( Mobile m, int element )
		{
			ArrayList targets = new ArrayList();
			foreach ( Item item in World.Items.Values )
			{
				if ( item is ElementalSpellbook && item.ArtifactLevel == 0 )
				{
					if ( ((ElementalSpellbook)item).EllyOwner == m )
					{
						targets.Add( item );
					}
				}
			}
			for ( int i = 0; i < targets.Count; ++i )
			{
				Item item = ( Item )targets[ i ];
				BookCover( item, element );
			}
		}
	}
}
