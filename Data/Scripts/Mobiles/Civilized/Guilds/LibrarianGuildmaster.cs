using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Misc;
using Server.Mobiles;
using System.Collections;
using Server.Gumps;

namespace Server.Mobiles
{
	public class LibrarianGuildmaster : BaseGuildmaster
	{
		public override NpcGuild NpcGuild{ get{ return NpcGuild.LibrariansGuild; } }

		public override string TalkGumpTitle{ get{ return "The Writing On The Wall"; } }
		public override string TalkGumpSubject{ get{ return "Sage"; } }

		[Constructable]
		public LibrarianGuildmaster() : base( "librarian" )
		{
			SetSkill( SkillName.Inscribe, 60.0, 83.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.FistFighting, 60.0, 80.0 );
			SetSkill( SkillName.Mercantile, 64.0, 100.0 );
		}

		public override void InitSBInfo( Mobile m )
		{
			m_Merchant = m;
			SBInfos.Add( new MyStock() );
		}

		public class MyStock: SBInfo
		{
			private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
			private IShopSellInfo m_SellInfo = new InternalSellInfo();

			public MyStock()
			{
			}

			public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
			public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

			public class InternalBuyInfo : List<GenericBuyInfo>
			{
				public InternalBuyInfo()
				{
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.All,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Scribe,		ItemSalesInfo.World.None,	null	 );
				}
			}

			public class InternalSellInfo : GenericSellInfo
			{
				public InternalSellInfo()
				{
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.All,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Scribe,		ItemSalesInfo.World.None,	null	 );
				}
			}
		}

		///////////////////////////////////////////////////////////////////////////

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is Gold )
			{
				string sMessage = "";

				if ( dropped.Amount == 500 )
				{
					if ( from.Skills[SkillName.Inscribe].Value >= 30 )
					{
						if ( Server.Misc.Research.AlreadyHasBag( from ) )
						{
							this.PublicOverheadMessage( MessageType.Regular, 0, false, string.Format ( "Boa sorte com sua pesquisa." ) ); 
						}
						else
						{
							ResearchBag bag = new ResearchBag();
							from.PlaySound( 0x2E6 );
							Server.Misc.Research.SetupBag( from, bag );
							from.AddToBackpack( bag );
							this.PublicOverheadMessage( MessageType.Regular, 0, false, string.Format ( "Boa sorte com sua pesquisa." ) ); 
						}
						dropped.Delete();
					}
					else
					{
						sMessage = "Você precisa ser um escriba neófito antes que eu venda isso para você.";
						from.AddToBackpack ( dropped );
					}
				}
				else
				{
					sMessage = "Parece que você precisa disso mais do que eu.";
					from.AddToBackpack ( dropped );
				}

				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
			}
			else if ( dropped is SmallHollowBook )
			{
				dropped.ItemID = 0x56F9;
				from.PlaySound( 0x249 );
				from.AddToBackpack ( dropped );
				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Eu reli seu livro.", from.NetState);
			}
			else if ( dropped is LargeHollowBook )
			{
				dropped.ItemID = 0x5703;
				from.PlaySound( 0x249 );
				from.AddToBackpack ( dropped );
				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Eu reli seu livro.", from.NetState);
			}
			else if ( dropped is Runebook )
			{
				if ( dropped.ItemID == 0x22C5 ){ dropped.ItemID = 0x0F3D; }
				else if ( dropped.ItemID == 0x0F3D ){ dropped.ItemID = 0x5687; }
				else if ( dropped.ItemID == 0x5687 ){ dropped.ItemID = 0x4F50; }
				else if ( dropped.ItemID == 0x4F50 ){ dropped.ItemID = 0x4F51; }
				else if ( dropped.ItemID == 0x4F51 ){ dropped.ItemID = 0x5463; }
				else if ( dropped.ItemID == 0x5463 ){ dropped.ItemID = 0x5464; }
				else { dropped.ItemID = 0x22C5; }

				from.PlaySound( 0x249 );
				from.AddToBackpack ( dropped );
				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Eu troquei a capa do seu livro.", from.NetState);
			}

			return base.OnDragDrop( from, dropped );
		}

		private class FixEntry : ContextMenuEntry
		{
			private LibrarianGuildmaster m_Sage;
			private Mobile m_From;

			public FixEntry( LibrarianGuildmaster LibrarianGuildmaster, Mobile from ) : base( 6120, 12 )
			{
				m_Sage = LibrarianGuildmaster;
				m_From = from;
				Enabled = m_Sage.CheckVendorAccess( from );
			}

			public override void OnClick()
			{
				m_Sage.BeginServices( m_From );
			}
		}

		public override void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( CheckChattingAccess( from ) )
				list.Add( new FixEntry( this, from ) );

			base.AddCustomContextEntries( from, list );
		}

        public void BeginServices(Mobile from)
        {
            if ( Deleted || !from.Alive )
                return;

			int nCost = 500;

			if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
			{
				nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost );
				SayTo(from, "Já que você está implorando, você quer que eu decifre uma nota por " + nCost.ToString() + " de ouro?");
			}
			else { SayTo(from, "Você quer que eu decifre uma nota por " + nCost.ToString() + " de ouro?"); }

            from.Target = new RepairTarget(this);
        }

        private class RepairTarget : Target
        {
            private LibrarianGuildmaster m_Sage;

            public RepairTarget(LibrarianGuildmaster mage) : base(12, false, TargetFlags.None)
            {
                m_Sage = mage;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
				if ( targeted is ScrollClue )
				{
					Container packs = from.Backpack;
					int nCost = 100;
					ScrollClue WhatIsIt = (ScrollClue)targeted;

					if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
					{
						nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }
					}
					int toConsume = nCost;

                    if ( WhatIsIt.ScrollIntelligence == 0 )
					{
						m_Sage.SayTo( from, "Isso já foi decifrado por alguém." );
					}
					else if (packs.ConsumeTotal(typeof(Gold), toConsume))
					{
						if ( WhatIsIt.ScrollIntelligence >= 80 ){ WhatIsIt.Name = "pergamino diabolicamente codificado"; }
						else if ( WhatIsIt.ScrollIntelligence >= 70 ){ WhatIsIt.Name = "pergamino engenhosamente codificado"; }
						else if ( WhatIsIt.ScrollIntelligence >= 60 ){ WhatIsIt.Name = "pergamino astutamente codificado"; }
						else if ( WhatIsIt.ScrollIntelligence >= 50 ){ WhatIsIt.Name = "pergamino habilmente codificado"; }
						else if ( WhatIsIt.ScrollIntelligence >= 40 ){ WhatIsIt.Name = "pergamino competentemente codificado"; }
						else if ( WhatIsIt.ScrollIntelligence >= 30 ){ WhatIsIt.Name = "pergamino expertemente codificado"; }
						else { WhatIsIt.Name = "pergamino simplesmente codificado"; }

						WhatIsIt.ScrollIntelligence = 0;
						WhatIsIt.InvalidateProperties();
						from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
						m_Sage.SayTo(from, "Deixe-me mostrar-lhe o que isso diz...");
						WhatIsIt.ScrollSolved = "Decifrado por " + m_Sage.Name + " o Bibliotecário";
						from.PlaySound( 0x249 );
						WhatIsIt.InvalidateProperties();
					}
					else
					{
						m_Sage.SayTo(from, "Custaria {0} de ouro para ter isso decifrado.", toConsume);
						from.SendMessage("Você não tem ouro suficiente.");
					}
					}
					else
						m_Sage.SayTo(from, "Isso não precisa dos meus serviços.");
            }
        }

		public LibrarianGuildmaster( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
