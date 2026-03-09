using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using System.Collections;
using Server.Misc;
using System.Text;

namespace Server.Gumps
{
    public class ShantyGump : Gump
    {
        int m_SelectedID;
        int m_ItemPrice = 0;
        string m_ItemTitle = "";
        int m_PlayerGold = 0;
        ShantyTools m_ShantyTools;
        string[] m_Categories;
        ShantyGumpCategory m_CurrentCategory;
        int m_CurrentPage;

        public ShantyGump(Mobile owner, ShantyTools tools, string currentCategory, int currentPage, int itemID, int price, string title): base(-40, 50)
        {
			string color = "#7ebfe1";
            m_SelectedID = itemID;
            m_ItemPrice = price;
            m_ItemTitle = title;
            m_ShantyTools = tools;
            m_CurrentPage = currentPage;
			int locMod = 90;
            if (currentCategory != null && ShantyRegistry.Categories.ContainsKey(currentCategory))
            {
                m_CurrentCategory = ShantyRegistry.Categories[currentCategory];
            }
            m_ShantyTools.Category = currentCategory;
            m_ShantyTools.Page = currentPage;

            ComputeGold(owner);

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);

			if ( currentPage == 999998 )
			{
				string text = "";
				text += "Estas ferramentas, com o uso adicional das ferramentas de proprietário, podem ser usadas para reformar sua casa. Cada item colocado custará uma quantia específica de ouro que será indicada no canto inferior esquerdo. A quantia de ouro em seu banco também será indicada na parte inferior. Para começar, certifique-se de estar sempre em sua casa, pois este é o único local onde você pode adicionar itens ou movê-los usando as ferramentas de proprietário. Selecione uma categoria no lado direito usando as caixas pequenas, e as opções aparecerão à esquerda. Você pode navegar pela lista com os botões de seta fornecidos. Selecione um item usando as caixas pequenas à esquerda do nome, e uma imagem do item aparecerá no centro para que você saiba como ele se parece.";
				text += "<br><br>";
				text += "Uma vez que um item é selecionado, uma opção COMPRAR estará disponível. Se você pressionar o botão OK, um cursor de mira aparecerá e você poderá selecionar onde o item será colocado. Se você colocar o item com sucesso, o cursor de mira permanecerá para que você possa colocar rapidamente outro item similar. Se não quiser colocar outro item similar, pressione a tecla ESC para limpar o cursor de mira. Você também pode selecionar um item diferente para colocar, onde então selecionar o botão COMPRAR armará seu cursor de mira para colocar aquele item em vez disso. Você não pode colocar estes itens fora de sua casa.";
				text += "<br><br>";
				text += "Uma vez que os itens são colocados, você pode movê-los usando as ferramentas de proprietário. A única coisa que as ferramentas de proprietário podem fazer com itens de reforma é movê-los para cima, para baixo, norte, sul, leste ou oeste. Alguns itens podem ser difíceis de selecionar, como as faíscas mágicas. Pressionar as teclas CONTROL e SHIFT ao mesmo tempo exibirá o nome do item, e então você poderá selecioná-lo. Os itens podem ser colocados diretamente na superfície do mundo e não aparecerão em cima de outros itens que você possa selecionar para colocar sobre. Em vez disso, use as ferramentas de proprietário para elevar o item à altura desejada.";
				text += "<br><br>";
				text += "Para vender um item, a maneira comum é clicar duas vezes nele e você será reembolsado pelo valor total, que será colocado em sua caixa bancária. Há dois tipos de itens que não podem ser vendidos clicando duas vezes neles, que são as portas e escadas. Clique uma vez nesses itens e escolha a opção VENDER quando o menu aparecer. A razão para esta diferença é que esses dois itens têm comportamentos especiais quando clicados duas vezes. Clicar duas vezes em uma peça de escada irá girá-la, enquanto clicar duas vezes em uma porta irá abri-la. Se você clicar uma vez em uma porta, pode definir a segurança para trancá-la ou destrancá-la. Somente amigos, proprietários, co-proprietários e membros da guilda podem abrir uma porta trancada. Somente proprietários e co-proprietários podem vender os itens, assim como girar peças de escada ou definir segurança em portas.";
				text += "<br><br>";
				text += "Quando você demolir uma casa, os itens de reforma serão automaticamente removidos e um cheque pelo valor total será colocado em sua caixa bancária. Se sua casa deteriorar por qualquer outro meio, os itens serão removidos logo em seguida e o valor total será colocado na caixa bancária do proprietário. Estas ferramentas de reforma são uma ótima maneira de fazer a atmosfera em sua casa refletir seu estilo pessoal.";
				text += "<br><br>";

				AddImage(0+locMod-2, -2, 9589, Server.Misc.PlayerSettings.GetGumpHue( owner ));
				AddHtml( 62+locMod, 13, 300, 20, @"<BODY><BASEFONT Color=" + color + ">REMODELING TOOLS - Help</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(595+locMod, 14, 4017, 4017, 999997, GumpButtonType.Reply, 0);
				AddHtml( 18+locMod, 84, 605, 309, @"<BODY><BASEFONT Color=" + color + ">" + text + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}
			else if ( currentPage == 999995 )
			{
				AddImage(0+locMod-2, -2, 9589, Server.Misc.PlayerSettings.GetGumpHue( owner ));
				AddHtml( 62+locMod, 13, 300, 20, @"<BODY><BASEFONT Color=" + color + ">REMODELING TOOLS - Remove</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(595+locMod, 14, 4017, 4017, 999997, GumpButtonType.Reply, 0);
				AddHtml( 18+locMod, 84, 605, 75, @"<BODY><BASEFONT Color=" + color + ">Se você quiser remover todas as decorações de reforma, pressione o botão abaixo. O ouro será reembolsado para sua caixa bancária. Se quiser cancelar esta solicitação, pressione o botão no canto superior direito.</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(18+locMod, 160, 4023, 4023, 999994, GumpButtonType.Reply, 0);
			}
			else
			{
				AddImage(0+locMod-2, -2, 9589, Server.Misc.PlayerSettings.GetGumpHue( owner ));

				if (m_SelectedID > 0)
				{
					Remodeling.ItemLayout( m_SelectedID, m_ItemTitle, this );
				}

				//Title & Help
				string header = "REMODELING TOOLS";
				if ( currentCategory != null && currentCategory != "" ){ header = header + " - " + currentCategory; }
				AddHtml( 62+locMod, 13, 300, 20, @"<BODY><BASEFONT Color=" + color + ">" + header + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(63+locMod, 40, 3610, 3610, 999999, GumpButtonType.Reply, 0);
				AddButton(128+locMod, 40, 4029, 4029, 999996, GumpButtonType.Reply, 0);

				//Item Cost
				AddItem(-1+locMod, 376, 3823);
				if ( m_ItemPrice > 0 )
				{
					AddHtml( 39+locMod, 378, 82, 20, @"<BODY><BASEFONT Color=" + color + ">" + String.Format("{0:0,0}", m_ItemPrice) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				//Bank Gold
				AddItem(248+locMod, 371, 5150);
				if ( m_PlayerGold > 0 )
				{
					AddHtml( 300+locMod, 378, 134, 20, @"<BODY><BASEFONT Color=" + color + ">" + String.Format("{0:0,0} Gold", m_PlayerGold) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				if (m_SelectedID > 0)
				{
					//Buy Button
					AddButton(379+locMod, 13, 4023, 4023, (int)Buttons.Place, GumpButtonType.Reply, 0);
					AddHtml( 415+locMod, 13, 60, 20, @"<BODY><BASEFONT Color=" + color + ">Buy</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				//Categories
				int catMod = 50;
				AddHtml( 490+locMod+catMod, 13, 119, 20, @"<BODY><BASEFONT Color=" + color + ">Categories</BASEFONT></BODY>", (bool)false, (bool)false);
				int categoryID = 0;
				m_Categories = new string[ShantyRegistry.Categories.Keys.Count];
				foreach (string categoryName in ShantyRegistry.Categories.Keys)
				{
					int hue = 1477;
					if (categoryName == currentCategory)
					{
						hue = 1671;
						AddButton(577+catMod, 52 + (25 * categoryID), 2448, 2448, 80851 + categoryID, GumpButtonType.Reply, 0);
					}
					else
					{
						hue = 1477;
						AddButton(577+catMod, 52 + (25 * categoryID), 2447, 2447, 80851 + categoryID, GumpButtonType.Reply, 0);
					}
					AddLabel(590+catMod, 49 + (25 * categoryID), hue, categoryName);
					m_Categories[categoryID] = categoryName;
					categoryID++;
				}

				if (m_CurrentCategory != null)
				{
					int i = 0;
					foreach (ShantyGumpEntry entry in m_CurrentCategory.Pages[m_CurrentPage].Values)
					{
						entry.AppendToGump(this, 107 + (i >= 12 ? 143 : 0), 95 + (i >= 12 ? 20 * (i - 12) : 20 * i), m_SelectedID);
						i++;
					}
				}
				else
				{
					AddHtml(105, 80, 510, 290, "<BODY><BASEFONT Color=" + color + ">Com estas ferramentas de reforma você pode adicionar certos terrenos e itens dentro de sua casa. Escolha uma categoria à direita para começar a navegar na lista de coisas com as quais pode decorar. Cada uma delas tem um preço que será deduzido de sua caixa bancária. Para informações adicionais, você pode acessar a tela de AJUDA acima.</BASEFONT></BODY>", false, false);
				}

				if (m_CurrentCategory != null && m_CurrentCategory.Pages.Count > m_CurrentPage + 1)
				{
					AddButton(172, 74, 4005, 4005, (int)Buttons.Next, GumpButtonType.Reply, 0);
				}

				if (m_CurrentCategory != null && m_CurrentPage > 0)
				{
					AddButton(112, 74, 4014, 4014, (int)Buttons.Prev, GumpButtonType.Reply, 0);
				}
			}
			AddItem(10+locMod, 10, 25576);
        }

        public enum Buttons
        {
            Exit,
            Place = -2,
            Next = -3,
            Prev = -4,
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
			from.SendSound( 0x4A ); 
			from.CloseGump( typeof( ShantyGump ) );
            if (info.ButtonID == 0)
            {
                return;
            }
			else if (info.ButtonID == 999999)
			{
				from.SendGump(new ShantyGump(from, m_ShantyTools, "", 999998, m_SelectedID, m_ItemPrice, m_ItemTitle));
			}
			else if (info.ButtonID == 999997)
			{
                ShantyTarget yt = new ShantyTarget(m_ShantyTools, from, 0, 0, "", "", 0);
				yt.GumpUp();
			}
			else if (info.ButtonID == 999996)
			{
				from.SendGump(new ShantyGump(from, m_ShantyTools, "", 999995, m_SelectedID, m_ItemPrice, m_ItemTitle));
			}
			else if (info.ButtonID == 999994)
			{
				BaseHouse house = BaseHouse.FindHouseAt( from );
				if ( house != null )
				{
					if ( house.IsOwner(from) )
						ShantySystem.RemoveShantys( house, from );
					else
						from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
				}
				else
				{
					from.SendLocalizedMessage( 502092 ); // You must be in your house to do this.
				}
                ShantyTarget yt = new ShantyTarget(m_ShantyTools, from, 0, 0, "", "", 0);
				yt.GumpUp();
			}
            else if (info.ButtonID == (int)Buttons.Next)
            {
                if (m_CurrentCategory != null && ShantyRegistry.Categories[m_CurrentCategory.Name].Pages.Count > m_CurrentPage + 1)
                {
                    from.SendGump(new ShantyGump(from, m_ShantyTools, m_CurrentCategory.Name, m_CurrentPage + 1, m_SelectedID, m_ItemPrice, m_ItemTitle));
                }
                else
                {
                    from.SendGump(new ShantyGump(from, m_ShantyTools, "", 0, m_SelectedID, m_ItemPrice, m_ItemTitle));
                }
            }
            else if (info.ButtonID == (int)Buttons.Prev)
            {
                if (m_CurrentCategory != null && m_CurrentPage > 0)
                {
                    from.SendGump(new ShantyGump(from, m_ShantyTools, m_CurrentCategory.Name, m_CurrentPage - 1, m_SelectedID, m_ItemPrice, m_ItemTitle));
                }
                else
                {
                    from.SendGump(new ShantyGump(from, m_ShantyTools, "", 0, m_SelectedID, m_ItemPrice, m_ItemTitle));
                }
            }
            else if (info.ButtonID == (int)Buttons.Place)
            {
                if (m_SelectedID > 0)
                {
                    from.SendMessage("Please choose where to place the item");
                    from.Target = new ShantyTarget(m_ShantyTools, from, m_SelectedID, m_ItemPrice, m_ItemTitle, m_CurrentCategory.Name, m_CurrentPage);
                }
            }
            else if (info.ButtonID >= 80851 && info.ButtonID <= 80865)
            {
                //Change categories
                if (m_Categories != null && m_Categories.Length > info.ButtonID - 80851)
                {
                    if (m_CurrentCategory != null)
                    {
                        from.SendGump(new ShantyGump(from, m_ShantyTools,
                                                   m_Categories[info.ButtonID - 80851] == m_CurrentCategory.Name ? "" : m_Categories[info.ButtonID - 80851], 
                                                   0, m_SelectedID, m_ItemPrice, m_ItemTitle));                        
                    }
                    else
                    {
                        from.SendGump(new ShantyGump(from, m_ShantyTools, m_Categories[info.ButtonID - 80851], 0, m_SelectedID, m_ItemPrice, m_ItemTitle));
                    }
                }
                else
                {
                    from.SendGump(new ShantyGump(from, m_ShantyTools, "", 0, m_SelectedID, m_ItemPrice, m_ItemTitle));
                }
            }
            else
            {
                m_SelectedID = info.ButtonID;
                if (m_CurrentCategory != null)
                {
                    ShantyGumpEntry entry = m_CurrentCategory.GetEntry(m_SelectedID);
                    if (entry != null)
                    {
                        m_ItemPrice = entry.Price;
                        m_ItemTitle = entry.Title;
                    }

                    from.SendGump(new ShantyGump(from, m_ShantyTools, m_CurrentCategory.Name, m_CurrentPage, m_SelectedID, m_ItemPrice, m_ItemTitle));
                }
            }
        }

        public void ComputeGold(Mobile from)
        {
            int goldInPack = 0;
            int goldInBank = 0;
            foreach (Gold gold in from.Backpack.FindItemsByType<Gold>(true))
            {
                goldInPack += gold.Amount;
            }

            foreach (Gold gold in from.BankBox.FindItemsByType<Gold>(true))
            {
                goldInBank += gold.Amount;
            }

            m_PlayerGold = goldInPack + goldInBank;
        }
    }
}