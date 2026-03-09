using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Prompts;
using System.Net;
using Server.Accounting;
using Server.Mobiles;
using Server.Commands;
using Server.Regions;
using Server.Spells;
using Server.Gumps;
using Server.Targeting;

namespace Server.Items
{
	[Flipable(0x577B, 0x577C)]
	public class SearchBoard : Item
	{
		[Constructable]
		public SearchBoard( ) : base( 0x577B )
		{
			Weight = 1.0;
			Name = "Sage Advice";
			Hue = 0x986;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "The Search for Artifacts" );
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( e.InRange( this.GetWorldLocation(), 4 ) )
			{
				e.CloseGump( typeof( BoardGump ) );
				e.SendGump( new BoardGump( e, "CONSELHO DO SÁBIO", "Se você tem uma grande missão para desenterrar um artefato, pode buscar o conselho dos sábios em sua jornada. O conselho deles não é barato, e você pode gastar 10.000 moedas de ouro pelo melhor conselho que eles podem dar. Para começar sua missão, visite um dos muitos sábios da terra e dê a eles ouro suficiente para seu conselho. Eles lhe darão uma enciclopédia de artefatos, a partir da qual você pode procurar as primeiras pistas sobre o paradeiro do seu artefato. Essas enciclopédias vêm em graus variados de precisão, dependendo de quanto ouro você está disposto a gastar.<br><br>Lenda e Sabedoria<br><br>  Nível 1 - 5.000 Moedas de Ouro<br>  Nível 2 - 6.000 Moedas de Ouro<br>  Nível 3 - 7.000 Moedas de Ouro<br>  Nível 4 - 8.000 Moedas de Ouro<br>  Nível 5 - 9.000 Moedas de Ouro<br>  Nível 6 - 10.000 Moedas de Ouro<br><br>Os sábios nunca são capazes de dar a você informações absolutamente precisas sobre a localização de um artefato, mas quanto maior o nível de sabedoria da enciclopédia, melhores são as chances de você encontrá-lo. Ao receber sua enciclopédia, abra-a e escolha um artefato de suas muitas páginas. Se não tiver certeza de qual artefato procura, basta olhar os produtos à venda do Sábio. No final de seu inventário, você verá réplicas de pesquisa desses artefatos com preço zero. Pode passar o cursor sobre esses artefatos para ver o que eles podem oferecer, mas não pode comprá-los. Artefatos como livros, aljavas e instrumentos serão mostrados com algumas qualidades comuns e aleatórias, onde encontrar o artefato real terá propriedades um pouco diferentes. Os itens restantes têm qualidades definidas, bem como um número de Pontos de Encantamento que você pode gastar para tornar o artefato mais personalizado para si mesmo. Ao encontrar esses artefatos, clique uma vez neles e selecione a opção 'Encantar' para gastar os pontos nos atributos adicionais que desejar. Após selecionar um artefato do livro, você rasgará a página apropriada e jogará fora o restante do livro. Esta página lhe dará a primeira pista de onde procurar. As áreas onde o artefato pode estar podem abranger muitas terras ou mundos diferentes, alguns dos quais você pode nunca ter estado ou ouvido falar. Você receberá as coordenadas do lugar que procura, então certifique-se de ter um sextante com você.<br><br><br><br>Ao longo da história, muitas pessoas guardaram esses artefatos armazenados em blocos de pedra trabalhada. Essas pedras trabalhadas são frequentemente decoradas com um símbolo na superfície, onde um baú de metal repousa e o item pode estar dentro. Alguns caçadores de tesouros encontram os baús vazios, percebendo que as lendas eram falsas. Quanto melhor o nível de sabedoria do livro do qual você arrancou a página, melhor a chance de encontrar o artefato. Se nada mais, você pode encontrar uma grande soma de ouro para cobrir algumas de suas despesas nessa jornada. Alguns podem fornecer uma nova pista sobre onde o artefato está, e você atualizará suas anotações quando essas pistas forem encontradas. A busca mais decepcionante pode resultar em um artefato falso. Esses acabam sendo itens inúteis que simplesmente se parecem com o artefato que você estava procurando.<br><br><br><br>Essas missões são bastante complexas e você só pode participar de uma dessas missões por vez. Se você não terminou uma missão e tenta procurar um sábio para outra, descobrirá que a página de sua missão anterior terá desaparecido. Certamente teria sido perdida em algum lugar. Se você terminar uma missão, seja com sucesso ou fracasso, um sábio não terá nenhum conselho novo para você por um bom tempo, então você terá que esperar até lá para começar uma nova missão. Portanto, boa sorte caçador de tesouros, e que os deuses o auxiliem em sua jornada.", "#d3d307", true ) );
			}
			else
			{
				e.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public SearchBoard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}