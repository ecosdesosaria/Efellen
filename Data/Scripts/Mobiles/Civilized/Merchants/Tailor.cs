using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BulkOrders;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class Tailor : BaseVendor
	{
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } }
		public override bool IsBlackMarket { get { return true; } }

		public override string TalkGumpTitle{ get{ return "Altering Cloaks And Robes"; } }
		public override string TalkGumpSubject{ get{ return "Tailor"; } }

		public override NpcGuild NpcGuild{ get{ return NpcGuild.TailorsGuild; } }

		[Constructable]
		public Tailor() : base( "the tailor" )
		{
			SetSkill( SkillName.Tailoring, 64.0, 100.0 );
		}

		public override void InitSBInfo( Mobile m )
		{
			m_Merchant = m;
			m_SBInfos.Add( new MyStock() );
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
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.None,		ItemSalesInfo.Material.None,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.None,		ItemSalesInfo.Material.Cloth,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.Rare,		ItemSalesInfo.Material.None,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.Resource,	ItemSalesInfo.Material.None,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
				}
			}

			public class InternalSellInfo : GenericSellInfo
			{
				public InternalSellInfo()
				{
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.None,		ItemSalesInfo.Material.None,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.None,		ItemSalesInfo.Material.Cloth,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.Rare,		ItemSalesInfo.Material.None,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.Resource,	ItemSalesInfo.Material.None,		ItemSalesInfo.Market.Tailor,		ItemSalesInfo.World.None,	null	 );
				}
			}
		}

		public override void UpdateBlackMarket()
		{
			base.UpdateBlackMarket();

			if ( IsBlackMarket && MyServerSettings.BlackMarket() )
			{
				int v=2; while ( v > 0 ){ v--;
				ItemInformation.BlackMarketList( this, ItemSalesInfo.Category.None,		ItemSalesInfo.Material.Cloth,		ItemSalesInfo.Market.All,		ItemSalesInfo.World.None	 );
				}
			}
		}

		private class FixEntry : ContextMenuEntry
		{
			private Tailor m_Tailor;
			private Mobile m_From;

			public FixEntry( Tailor Tailor, Mobile from ) : base( 6120, 12 )
			{
				m_Tailor = Tailor;
				m_From = from;
				Enabled = Tailor.CheckVendorAccess( from );
			}

			public override void OnClick()
			{
				m_Tailor.BeginServices( m_From );
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

			int nCost = 5;
			int nCostH = 10;

			if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
			{
				nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }
				nCostH = nCostH - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCostH ); if ( nCostH < 1 ){ nCostH = 1; }
				SayTo(from, "Já que está implorando, ainda quer que eu ajuste sua vestimenta ou capa para parecer normal, custará apenas " + nCost.ToString() + " moedas de ouro? Ou talvez reparar um chapéu por pelo menos " + nCostH.ToString() + " moedas de ouro por durabilidade?");
			}
			else { SayTo(from, "Se quer que eu ajuste sua vestimenta ou capa para parecer normal, custará " + nCost.ToString() + " moedas de ouro. Ou talvez reparar um chapéu por " + nCostH.ToString() + " moedas de ouro por durabilidade?"); }

            from.Target = new RepairTarget(this);
        }

        private class RepairTarget : Target
        {
            private Tailor m_Tailor;

            public RepairTarget(Tailor tailor) : base(12, false, TargetFlags.None)
            {
                m_Tailor = tailor;
            }

            protected override void OnTarget(Mobile from, object targeted)
			{
				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if ( targeted is BaseOuterTorso && from.Backpack != null )
                {
                    Item ba = targeted as Item;
                    Container pack = from.Backpack;
                    int toConsume = 0;

                    if ( ba.ItemID != 0x1F03 && ba.ItemID != 0x1F04 )
                    {
						int nCost = 5;

						if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
						{
							nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }
						}
						toConsume = nCost;
                    }
                    else
                    {
						m_Tailor.SayTo(from, "Isso não precisa dos meus serviços.");
                    }

                    if (toConsume == 0)
                        return;

                    if (pack.ConsumeTotal(typeof(Gold), toConsume))
                    {
						if ( BeggingPose(from) > 0 ){ Titles.AwardKarma( from, -BeggingKarma( from ), true ); } // DO ANY KARMA LOSS
                        m_Tailor.SayTo(from, "Aqui está sua vestimenta.");
                        from.SendMessage(String.Format("Você paga {0} moedas de ouro.", toConsume));
                        Effects.PlaySound(from.Location, from.Map, 0x248);
						ba.ItemID = 0x1F03;
						ba.Name = "robe";
                    }
                    else
                    {
                        m_Tailor.SayTo(from, "Custaria {0} moedas de ouro para fazer isso.", toConsume);
						from.SendMessage("Você não tem ouro suficiente.");
                    }
                }
				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                else if ( targeted is BaseCloak && from.Backpack != null )
                {
                    Item ba = targeted as Item;
                    Container pack = from.Backpack;
                    int toConsume = 0;

                    if ( ba.ItemID != 0x1515 && ba.ItemID != 0x1530 )
                    {
						int nCost = 5;

						if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
						{
							nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }
						}
						toConsume = nCost;
                    }
                    else
                    {
						m_Tailor.SayTo(from, "Isso não precisa dos meus serviços.");
                    }

                    if (toConsume == 0)
                        return;

                    if (pack.ConsumeTotal(typeof(Gold), toConsume))
                    {
						if ( BeggingPose(from) > 0 ){ Titles.AwardKarma( from, -BeggingKarma( from ), true ); } // DO ANY KARMA LOSS
                        m_Tailor.SayTo(from, "Aqui está sua capa.");
                        from.SendMessage(String.Format("Você paga {0} moedas de ouro.", toConsume));
                        Effects.PlaySound(from.Location, from.Map, 0x248);
						ba.ItemID = 0x1515;
						ba.Name = "cloak";
                    }
                    else
                    {
                        m_Tailor.SayTo(from, "Custaria {0} moedas de ouro para fazer isso.", toConsume);
                        from.SendMessage("Você não tem ouro suficiente.");
                    }
                }
				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				else if ( ( targeted is BaseHat || targeted is BaseLevelHat || targeted is BaseGiftHat ) && from.Backpack != null && ((Item)targeted).ItemID != 0x1545 && ((Item)targeted).ItemID != 0x1546 && ((Item)targeted).ItemID != 0x1547 && ((Item)targeted).ItemID != 0x1548 && ((Item)targeted).ItemID != 0x2B6D && ((Item)targeted).ItemID != 0x3164 )
				{
                    BaseClothing ba = targeted as BaseClothing;
                    Container pack = from.Backpack;
                    int toConsume = 0;

                    if (ba.HitPoints < ba.MaxHitPoints)
                    {
						int nCost = 10;

						if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
						{
							nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }
							toConsume = (ba.MaxHitPoints - ba.HitPoints) * nCost;
						}
						else { toConsume = (ba.MaxHitPoints - ba.HitPoints) * nCost; }
                    }
                    else if (ba.HitPoints >= ba.MaxHitPoints)
                    {
						m_Tailor.SayTo(from, "Isso não precisa ser reparado.");
                    }
					else
					{
						m_Tailor.SayTo(from, "Eu não posso reparar isso.");
					}

                    if (toConsume == 0)
                        return;

                    if (pack.ConsumeTotal(typeof(Gold), toConsume))
                    {
						if ( BeggingPose(from) > 0 ){ Titles.AwardKarma( from, -BeggingKarma( from ), true ); } // DO ANY KARMA LOSS
                        m_Tailor.SayTo(from, "Aqui está seu chapéu.");
                        from.SendMessage(String.Format("Você paga {0} moedas de ouro.", toConsume));
                        Effects.PlaySound(from.Location, from.Map, 0x248);
                        ba.MaxHitPoints -= 1;
                        ba.HitPoints = ba.MaxHitPoints;
                    }
                    else
                    {
                        m_Tailor.SayTo(from, "Custaria {0} moedas de ouro para fazer isso.", toConsume);
                        from.SendMessage("Você não tem ouro suficiente.");
                    }
                }
				else
					m_Tailor.SayTo(from, "Isso não precisa dos meus serviços.");
            }
        }

		#region Bulk Orders
		public override Item CreateBulkOrder( Mobile from, bool fromContextMenu )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm != null && pm.NextTailorBulkOrder == TimeSpan.Zero && (fromContextMenu || 0.2 > Utility.RandomDouble()) )
			{
				double theirSkill = pm.Skills[SkillName.Tailoring].Base;

				if ( theirSkill >= 70.1 )
					pm.NextTailorBulkOrder = TimeSpan.FromMinutes( 0.01 );
				else if ( theirSkill >= 50.1 )
					pm.NextTailorBulkOrder = TimeSpan.FromMinutes( 0.01 );
				else
					pm.NextTailorBulkOrder = TimeSpan.FromMinutes( 0.01 );

				if ( theirSkill >= 70.1 && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble() )
					return new LargeTailorBOD();

				return SmallTailorBOD.CreateRandomFor( from );
			}

			return null;
		}

		public override bool IsValidBulkOrder( Item item )
		{
			return ( item is SmallTailorBOD || item is LargeTailorBOD );
		}

		public override bool SupportsBulkOrders( Mobile from )
		{
			return ( from is PlayerMobile && from.Skills[SkillName.Tailoring].Base > 0 );
		}

		public override TimeSpan GetNextBulkOrder( Mobile from )
		{
			if ( from is PlayerMobile )
				return ((PlayerMobile)from).NextTailorBulkOrder;

			return TimeSpan.Zero;
		}

		public override void OnSuccessfulBulkOrderReceive( Mobile from )
		{
			if( Core.SE && from is PlayerMobile )
				((PlayerMobile)from).NextTailorBulkOrder = TimeSpan.Zero;
		}
		#endregion

		public Tailor( Serial serial ) : base( serial )
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
