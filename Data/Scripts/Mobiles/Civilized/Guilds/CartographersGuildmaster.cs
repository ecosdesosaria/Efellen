using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class CartographersGuildmaster : BaseGuildmaster
	{
		public override NpcGuild NpcGuild{ get{ return NpcGuild.CartographersGuild; } }

		public override string TalkGumpTitle{ get{ return "X Marks The Spot"; } }
		public override string TalkGumpSubject{ get{ return "Mapmaker"; } }

		[Constructable]
		public CartographersGuildmaster() : base( "cartographer" )
		{
			SetSkill( SkillName.Cartography, 90.0, 100.0 );
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
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.All,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Cartographer,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.All,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Cartographer,		ItemSalesInfo.World.None,	typeof( BlankScroll )	 );
				}
			}

			public class InternalSellInfo : GenericSellInfo
			{
				public InternalSellInfo()
				{
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.All,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Cartographer,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.All,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Cartographer,		ItemSalesInfo.World.None,	typeof( BlankScroll )	 );
				}
			}
		}

		private class FixEntry : ContextMenuEntry
		{
			private CartographersGuildmaster m_CartographersGuildmaster;
			private Mobile m_From;

			public FixEntry( CartographersGuildmaster CartographersGuildmaster, Mobile from ) : base( 6120, 12 )
			{
				m_CartographersGuildmaster = CartographersGuildmaster;
				m_From = from;
				Enabled = m_CartographersGuildmaster.CheckVendorAccess( from );
			}

			public override void OnClick()
			{
				m_CartographersGuildmaster.BeginServices( m_From );
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
			int money = 1000;

			double w = money * (MyServerSettings.GetGoldCutRate() * .01);
			money = (int)w;

            if ( Deleted || !from.Alive )
                return;

			int nCost = money;

			if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
			{
				nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }
				SayTo(from, "Já que você está implorando, você ainda quer que eu decifre um mapa do tesouro para você, custará apenas " + nCost.ToString() + " de ouro por nível do mapa?");
			}
			else { SayTo(from, "Se você quiser que eu decifre um mapa do tesouro para você, custará " + nCost.ToString() + " de ouro por nível do mapa"); }

            from.Target = new RepairTarget(this);
        }

        private class RepairTarget : Target
        {
            private CartographersGuildmaster m_CartographersGuildmaster;

            public RepairTarget(CartographersGuildmaster CartographersGuildmaster) : base(12, false, TargetFlags.None)
            {
                m_CartographersGuildmaster = CartographersGuildmaster;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
				int money = 1000;

				double w = money * (MyServerSettings.GetGoldCutRate() * .01);
				money = (int)w;

                if (targeted is TreasureMap && from.Backpack != null)
                {
                    TreasureMap tmap = targeted as TreasureMap;
                    Container pack = from.Backpack;
                    int toConsume = tmap.Level * money;

					if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
					{
						toConsume = toConsume - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * toConsume );
					}

                    if (toConsume == 0)
                        return;

					if ( tmap.Decoder != null )
					{
						m_CartographersGuildmaster.SayTo(from, "Esse mapa já foi decifrado.");
					}
                    else if (pack.ConsumeTotal(typeof(Gold), toConsume))
                    {
						if ( BeggingPose(from) > 0 ){ Titles.AwardKarma( from, -BeggingKarma( from ), true ); } // FAZER QUALQUER PERDA DE KARMA
						if ( tmap.Level == 1 ){ m_CartographersGuildmaster.SayTo(from, "Este mapa era realmente muito simples."); }
						else if ( tmap.Level == 2 ){ m_CartographersGuildmaster.SayTo(from, "Pareceu bem fácil... então aqui está."); }
						else if ( tmap.Level == 3 ){ m_CartographersGuildmaster.SayTo(from, "Este mapa foi um pouco desafiador."); }
						else if ( tmap.Level == 4 ){ m_CartographersGuildmaster.SayTo(from, "Quem quer que tenha desenhado este mapa não queria que fosse encontrado."); }
						else if ( tmap.Level == 5 ){ m_CartographersGuildmaster.SayTo(from, "Isso exigiu mais pesquisa do que o normal."); }
						else { m_CartographersGuildmaster.SayTo(from, "Com os escritos antigos e enigmas, este mapa agora deve levar você até lá."); }

                        from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
                        Effects.PlaySound(from.Location, from.Map, 0x249);
						tmap.Decoder = from;
                    }
                    else
                    {
			m_CartographersGuildmaster.SayTo(from, "Custaria {0} de ouro para eu decifrar esse mapa.", toConsume);
			from.SendMessage("Você não tem ouro suficiente.");
                    }
                }
				else
				{
					m_CartographersGuildmaster.SayTo(from, "Isso não precisa dos meus serviços.");
				}
            }
        }

		public CartographersGuildmaster( Serial serial ) : base( serial )
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
