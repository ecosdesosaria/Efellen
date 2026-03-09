using System;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Items;
using Server.Mobiles;
using System.Collections;

namespace Server.Spells.Jester
{
	public class Insult : JesterSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Insult", "You know what?",
				-1,
				0
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }
		public override int RequiredTithing{ get{ return 120; } }
		public override int RequiredMana{ get{ return 40; } }

		public Insult( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		private static Hashtable m_Table = new Hashtable();

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public static bool HasEffect( Mobile m )
		{
			return ( m_Table[m] != null );
		}

		public static void RemoveEffect( Mobile m )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
			{
				t.Stop();
				m_Table.Remove( m );
			}
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
				int TotalLoss = Server.Spells.Jester.JesterSpell.Buff( Caster, "range" )+1;
				int TotalTime = (int)(Server.Spells.Jester.JesterSpell.Buff( Caster, "time" )/2)+1;

				Timer t = new InternalTimer( m, TotalLoss, TotalTime );

				m_Table[m] = t;

				t.Start();

				Caster.Say( GetInsult() );

				switch ( Utility.Random( 3 ))
				{
					case 0: Caster.PlaySound( Caster.Female ? 794 : 1066 ); break;
					case 1: Caster.PlaySound( Caster.Female ? 801 : 1073 ); break;
					case 2: Caster.PlaySound( 792 ); break;
				};

				if ( Utility.RandomBool() ){ Effects.PlaySound( m.Location, m.Map, m.Female ? 0x31B : 0x42B ); m.Say( "*groans*" ); }
				else { Effects.PlaySound( m.Location, m.Map, m.Female ? 0x338 : 0x44A ); m.Say( "*growls*" ); }

				m.SendMessage( "You have been quite insulted!" );
			}

			FinishSequence();
		}

		public static string GetInsult()
		{
			string str = "Um goblin com uma mão pregada numa árvore seria mais ameaçador que você!";
			switch( Utility.RandomMinMax( 1, 100 ) )
			{
				case 1: str = "Um goblin com uma mão pregada numa árvore seria mais ameaçador que você!"; break;
				case 2: str = "Um gato molhado é mais durão que você!"; break;
				case 3: str = "A magia de amizade animal foi a única forma que seus pais conseguiram para fazer filhotes brincarem com você!"; break;
				case 4: str = "Você é um orc cruzado com um porco? Ah sim, há coisas que um porco não faria!"; break;
				case 5: str = "Você é sempre estúpido ou está fazendo um esforço especial hoje!"; break;
				case 6: str = "Olhando para você, agora sei o que se obtém quando se raspa o fundo do barril!"; break;
				case 7: str = "Pelos deuses, você é feio! Aposto que seu pai se arrepende de ter conhecido sua mãe!"; break;
				case 8: str = "Você poderia chamar seu marido? Não gosto de lutar com mulheres feias!"; break;
				case 9: str = "Sua mãe lançou uma magia de escuridão para alimentar você!"; break;
				case 10: str = "Não ouvi dizer que você era mais durão que isso?"; break;
				case 11: str = "Você tem uma caneta? Bem, é melhor voltar para ela antes que o fazendeiro perceba que você sumiu!"; break;
				case 12: str = "Você parece com algo que vi no chão do estábulo!"; break;
				case 13: str = "Já viu uma pilha de esterco? Então talvez deva olhar no espelho!"; break;
				case 14: str = "Até ghouls não tocariam em algo tão nojento quanto você!"; break;
				case 15: str = "Ei, você já foi confundido com um verme de carniça?"; break;
				case 16: str = "Ei, seu monte de esterco cheio de varíola, aposto que nem um vampiro faminto chegaria perto de você!"; break;
				case 17: str = "Como é saber que você não é digno de ninguém lançar um feitiço decente em você!"; break;
				case 18: str = "Posso ver que seu reservatório de coragem é alimentado pelo tributário escorrendo pela sua perna!"; break;
				case 19: str = "Eu poderia dizer que você é tão feio quanto um ogro, mas isso seria um insulto aos ogros!"; break;
				case 20: str = "Não sei se uso um feitiço de encantar pessoa ou encantar monstro!"; break;
				case 21: str = "Ouvi o que aconteceu com sua mãe, não é todo dia que seu reflexo te mata!"; break;
				case 22: str = "Juro, se você fosse pior nisso, estaria fazendo meu trabalho por mim!"; break;
				case 23: str = "Ia lançar ler mentes, mas acho que não vou encontrar nada lá em cima!"; break;
				case 24: str = "Estava pensando em lançar mente fraca, mas duvido que funcione em você!"; break;
				case 25: str = "Estava me perguntando o que você é, você é gordo o suficiente para ser um ogro, mas nunca vi um ogro tão feio antes!"; break;
				case 26: str = "Queria ainda ter aquele feitiço de cegueira, então não teria que aguentar mais essa cara!"; break;
				case 27: str = "Entraria em contato com sua mãe sobre sua morte, mas não falo goblin!"; break;
				case 28: str = "Tentaria insultar seu pai, mas você provavelmente foi confundido com um orc e deserdado!"; break;
				case 29: str = "Puxaria minha espada, mas não gostaria de deixá-lo com ciúmes!"; break;
				case 30: str = "Insultaria seus pais, mas você provavelmente não sabe quem eles são!"; break;
				case 31: str = "Gostaria de deixar você com um pensamento... mas não tenho certeza se você tem onde colocá-lo!"; break;
				case 32: str = "Gostaria de ver as coisas do seu ponto de vista, mas não consigo enfiar minha cabeça tão fundo na minha bunda!"; break;
				case 33: str = "Eu diria que você é um oponente digno, mas uma vez lutei contra um coelho empunhando um dente-de-leão!"; break;
				case 34: str = "Se eu fosse você, pegaria meu ouro de volta por aquele feitiço de remover maldição!"; break;
				case 35: str = "Se ignorância é felicidade, você deve ser o mais feliz vivo!"; break;
				case 36: str = "Se esta luta ficar mais difícil, terei que realmente me esforçar!"; break;
				case 37: str = "Se sua mente explodisse, nem mesmo bagunçaria seu cabelo!"; break;
				case 38: str = "Fico feliz que você seja alto... Significa que há mais de você para eu desprezar!"; break;
				case 39: str = "Me dá dor de cabeça só de tentar pensar no seu nível!"; break;
				case 40: str = "Já ouvi falar de cabras com melhores habilidades de luta que você!"; break;
				case 41: str = "Já vi pássaros mais ameaçadores!"; break;
				case 42: str = "Nenhum tesouro vale a pena ter que olhar para você!"; break;
				case 43: str = "Não é à toa que você está se escondendo atrás de cobertura, eu também me esconderia com uma cara dessas!"; break;
				case 44: str = "Olha só, você está realmente tentando lutar!"; break;
				case 45: str = "Achei que trogloditas cheirassem mal!"; break;
				case 46: str = "Por que não me dá sua arma para eu me bater com ela, porque seria mais eficaz do que você tentando!"; break;
				case 47: str = "Foi um ogro que respirou em mim ou é você?"; break;
				case 48: str = "Um dia vou fazer uma história desta luta. Diga seu nome, espero que rime com horrivelmente abatido!"; break;
				case 49: str = "Ufa! Você acabou de lançar nuvem fétida ou sempre cheira assim!"; break;
				case 50: str = "Sua mãe nunca te ensinou a lutar?"; break;
				case 51: str = "Gosto de como você finge lutar!"; break;
				case 52: str = "Algum dia você irá longe e espero que fique lá!"; break;
				case 53: str = "Algum dia você encontrará um doppelganger de si mesmo e ficará desapontado!"; break;
				case 54: str = "Em algum lugar, você está privando uma vila de seu idiota!"; break;
				case 55: str = "Você parece fofinho, ou está tentando ser ameaçador?"; break;
				case 56: str = "Diga-me, você fugiu de seus pais ou eles fugiram de você!"; break;
				case 57: str = "Não há olho de beholder no qual você seja bonito!"; break;
				case 58: str = "Dizem que toda rosa tem seu espinho, não é mesmo, querida!"; break;
				case 59: str = "Ugh. O que é isso no seu rosto? Ah... é só sua cara!"; break;
				case 60: str = "Muito impressionante, acho que vou contratá-lo para um show de marionetes!"; break;
				case 61: str = "Quando os deuses estavam distribuindo caras feias, você foi o primeiro da fila?"; break;
				case 62: str = "Espere, espere, só preciso perguntar, o que você quer que eu ponha na sua lápide!"; break;
				case 63: str = "Bem, meu tempo de não levá-lo a sério está chegando ao meio!"; break;
				case 64: str = "Bem... Já conheci pães mais afiados!"; break;
				case 65: str = "Você foi atingido por um elemental de ácido ou sempre pareceu com um bife meio comido?"; break;
				case 66: str = "O que cheira pior que um goblin? Ah sim, você!"; break;
				case 67: str = "O que é esse cheiro? Pensei que armas de sopro saíssem da sua boca!"; break;
				case 68: str = "Qual a diferença entre você e um coelho doente? O coelho provavelmente me daria um desafio!"; break;
				case 69: str = "Qual a diferença entre você e uma árvore? Uma árvore provavelmente desviaria melhor de mim!"; break;
				case 70: str = "Quando seu deus o criou, esqueceu de adicionar um cérebro?"; break;
				case 71: str = "Conheci alguém que lutou tão bem quanto você! Foi a galinha mais saborosa de todas!"; break;
				case 72: str = "Gostaria que eu removesse essa maldição? Oh, meu engano, você nasceu assim!"; break;
				case 73: str = "Uau, você é tão gordo que acho que quem está atrás de você está ganhando cobertura nesta luta!"; break;
				case 74: str = "Você é uma torta de vermes servida da codpiece de um anão!"; break;
				case 75: str = "Você é as fezes criadas quando a vergonha come estupidez demais!"; break;
				case 76: str = "Você é o pior exemplo de sua espécie que já encontrei!"; break;
				case 77: str = "Chama isso de ataque? Já vi gatinhos mortos acertarem mais forte!"; break;
				case 78: str = "Você sabe que estou bem aqui se quiser tentar me acertar!"; break;
				case 79: str = "Você parece uma casca na verruga de um troll!"; break;
				case 80: str = "Você parece a axila de uma bruxa do pântano não depilada!"; break;
				case 81: str = "Você se parece com sua mãe, e sua mãe se parece com seu pai!"; break;
				case 82: str = "Você entediaria as pernas de um idiota da vila!"; break;
				case 83: str = "Seu hálito faria um elemental de estrume correr!"; break;
				case 84: str = "Talvez tenha que ressuscitá-lo depois disso para tentarmos de novo!"; break;
				case 85: str = "Sua mãe é tão feia que sacerdotes tentam lançar banir mortos-vivos nela!"; break;
				case 86: str = "Sua mãe é tão gorda que fazer uma piada aqui desviaria da seriedade da condição dela!"; break;
				case 87: str = "Sua mãe ocupa mais terreno que o castelo do Lord British!"; break;
				case 88: str = "Sua mãe era um kobold e seu pai cheirava a sabugueiro!"; break;
				case 89: str = "Sua mãe era tão estúpida que zumbis fizeram um chapéu de burro para ela!"; break;
				case 90: str = "Sua mãe é tão feia que as pessoas viram pedra caso aconteça de avistarem seu rosto!"; break;
				case 91: str = "Sua cara feia é um bom argumento contra levantar os mortos!"; break;
				case 92: str = "Sua própria existência é um insulto a todos!"; break;
				case 93: str = "Você vai fazer um cinto excelente!"; break;
				case 94: str = "Você é como um dragão, só que não tão forte ou feroz... ou nada!"; break;
				case 95: str = "Você é como um gnomo em palafitas, bem fofo, mas não está funcionando!"; break;
				case 96: str = "Você é como um macaco treinado, só que sem o treinamento!"; break;
				case 97: str = "Você tem sorte de ter nascido bonito, ao contrário de mim, que nasci para ser um grande mentiroso!"; break;
				case 98: str = "Você não é um idiota completo... Algumas partes obviamente faltam!"; break;
				case 99: str = "Você é tão estúpido que se um mind flayer tentasse comer seu cérebro, morreria de fome!"; break;
				case 100: str = "Você é o motivo de gnomes bebês chorarem!"; break;
			}

			return str;
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private DateTime m_Expire;
			private double m_Time;
			private int m_Loss;

			public InternalTimer( Mobile owner, int loss, int time ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.5 ) )
			{
				m_Time = (double)time;
				m_Loss = loss;
				m_Owner = owner;
				m_Expire = DateTime.Now + TimeSpan.FromSeconds( m_Time );

				BuffInfo.RemoveBuff( owner, BuffIcon.Insult );
				BuffInfo.AddBuff( owner, new BuffInfo( BuffIcon.Insult, 1063518, TimeSpan.FromSeconds( m_Time ), owner ) );
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CheckAlive() || DateTime.Now >= m_Expire )
				{
					Stop();
					m_Table.Remove( m_Owner );
					m_Owner.SendMessage( "The insult is wearing off." );
				}
				else if ( m_Owner.Mana < m_Loss )
				{
					m_Owner.Mana = 0;
				}
				else
				{
					m_Owner.Mana -= m_Loss;
				}
			}
		}

		private class InternalTarget : Target
		{
			private Insult m_Owner;

			public InternalTarget( Insult owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
