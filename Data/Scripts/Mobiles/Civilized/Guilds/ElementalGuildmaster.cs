using System;
using System.Collections.Generic;
using Server;
using System.Collections;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class ElementalGuildmaster : BaseGuildmaster
	{
		public override NpcGuild NpcGuild{ get{ return NpcGuild.ElementalGuild; } }

		[Constructable]
		public ElementalGuildmaster() : base( "elemental" )
		{
			SetSkill( SkillName.Focus, 85.0, 100.0 );
			SetSkill( SkillName.Inscribe, 65.0, 88.0 );
			SetSkill( SkillName.MagicResist, 64.0, 100.0 );
			SetSkill( SkillName.Elementalism, 90.0, 100.0 );
			SetSkill( SkillName.FistFighting, 60.0, 83.0 );
			SetSkill( SkillName.Meditation, 85.0, 100.0 );
			SetSkill( SkillName.Bludgeoning, 36.0, 68.0 );
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
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.Weapon,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Mage,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.Weapon,		ItemSalesInfo.Material.Wood,	ItemSalesInfo.Market.Wizard,	ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.Book,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Elemental,	ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.Scroll,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Elemental,	ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetSellList( m_Merchant, this, 	ItemSalesInfo.Category.None,		ItemSalesInfo.Material.Cloth,	ItemSalesInfo.Market.Wizard,	ItemSalesInfo.World.None,	null	 );
				}
			}

			public class InternalSellInfo : GenericSellInfo
			{
				public InternalSellInfo()
				{
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.Weapon,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Mage,		ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.Weapon,		ItemSalesInfo.Material.Wood,	ItemSalesInfo.Market.Wizard,	ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.Book,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Elemental,	ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.Scroll,		ItemSalesInfo.Material.All,		ItemSalesInfo.Market.Elemental,	ItemSalesInfo.World.None,	null	 );
					ItemInformation.GetBuysList( m_Merchant, this, 	ItemSalesInfo.Category.None,		ItemSalesInfo.Material.Cloth,	ItemSalesInfo.Market.Wizard,	ItemSalesInfo.World.None,	null	 );
				}
			}
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			switch ( Utility.RandomMinMax( 0, 4 ) )
			{
				case 1: AddItem( new Server.Items.GnarledStaff() ); break;
				case 2: AddItem( new Server.Items.BlackStaff() ); break;
				case 3: AddItem( new Server.Items.WildStaff() ); break;
				case 4: AddItem( new Server.Items.QuarterStaff() ); break;
			}
		}

		private class FixEntry : ContextMenuEntry
		{
			private ElementalGuildmaster m_Elementalist;
			private Mobile m_From;

			public FixEntry( ElementalGuildmaster ElementalGuildmaster, Mobile from ) : base( 6120, 12 )
			{
				m_Elementalist = ElementalGuildmaster;
				m_From = from;
				Enabled = m_Elementalist.CheckVendorAccess( from );
			}

			public override void OnClick()
			{
				m_Elementalist.BeginServices( m_From );
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
				nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }
				SayTo(from, "Já que você está implorando, você ainda quer que eu carregue uma bola de cristal de invocação com 5 cargas, custará apenas " + nCost.ToString() + " de ouro?");
			}
			else { SayTo(from, "Se você quiser que eu carregue uma bola de cristal de invocação com 5 cargas, custará " + nCost.ToString() + " de ouro."); }

            from.Target = new RepairTarget(this);
        }

        private class RepairTarget : Target
        {
            private ElementalGuildmaster m_Elementalist;

            public RepairTarget(ElementalGuildmaster elementalist) : base(12, false, TargetFlags.None)
            {
                m_Elementalist = elementalist;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is HenchmanFamiliarItem && from.Backpack != null)
                {
                    HenchmanFamiliarItem ball = targeted as HenchmanFamiliarItem;
                    Container pack = from.Backpack;

                    int toConsume = 0;

					if ( ball.Charges < 50 )
                    {
						toConsume = 500;

						if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
						{
							toConsume = toConsume - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * toConsume );
						}
                    }
                    else
					{
						m_Elementalist.SayTo(from, "Essa bola de cristal já tem cargas demais.");
					}

                    if (toConsume == 0)
                        return;

					if (pack.ConsumeTotal(typeof(Gold), toConsume))
					{
						if ( BeggingPose(from) > 0 ){ Titles.AwardKarma( from, -BeggingKarma( from ), true ); } // DO ANY KARMA LOSS
						m_Elementalist.SayTo(from, "Sua bola de cristal está carregada.");
						from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
                        Effects.PlaySound(from.Location, from.Map, 0x5C1);
						ball.Charges = ball.Charges + 5;
                    }
                    else
                    {
						m_Elementalist.SayTo(from, "Custaria {0} de ouro para ter isso carregado.", toConsume);
						from.SendMessage("Você não tem ouro suficiente.");
                    }
                }
				else
				{
						m_Elementalist.SayTo(from, "Isso não precisa dos meus serviços.");
				}
            }
        }

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is Ruby )
			{
				int Rubies = dropped.Amount;
				string sMessage = "";

				if ( ( Rubies > 19 ) && ( from.Skills[SkillName.Elementalism].Base >= 50 || from.Skills[SkillName.Elementalism].Base >= 50 || from.Skills[SkillName.Magery].Base >= 50 || from.Skills[SkillName.Necromancy].Base >= 50 ) )
				{
					sMessage = "Ahhh... isso é generoso de sua parte. Aqui... tome isto como um símbolo da gratidão da guilda.";
					HenchmanFamiliarItem ball = new HenchmanFamiliarItem();
					ball.FamiliarOwner = from.Serial;
					from.AddToBackpack ( ball );
				}
				else
				{
					sMessage = "Obrigado por estes. Rubis são algo que frequentemente procuramos.";
				}

				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
				dropped.Delete();
			}
			else if ( dropped is HenchmanFamiliarItem )
			{
				string sMessage = "";

				int HighSpellCaster = 0;
				if ( from.Skills[SkillName.Elementalism].Base >= 50 || from.Skills[SkillName.Magery].Base >= 50 || from.Skills[SkillName.Necromancy].Base >= 50 ){ HighSpellCaster = 1; }
				if ( from.Skills[SkillName.Elementalism].Base >= 100 || from.Skills[SkillName.Magery].Base >= 100 || from.Skills[SkillName.Necromancy].Base >= 100 ){ HighSpellCaster = 2; }

				if ( HighSpellCaster > 0 )
				{
					HenchmanFamiliarItem ball = (HenchmanFamiliarItem)dropped;

					if ( ball.FamiliarType == 0x16 ){ ball.FamiliarType = 0xD9; sMessage = "Seu familiar está agora na forma de um cachorro." ; }
					else if ( ball.FamiliarType == 0xD9 ){ ball.FamiliarType = 238; sMessage = "Seu familiar está agora na forma de um rato." ; }
					else if ( ball.FamiliarType == 238 ){ ball.FamiliarType = 0xC9; sMessage = "Seu familiar está agora na forma de um gato." ; }
					else if ( ball.FamiliarType == 0xC9 ){ ball.FamiliarType = 0xD7; sMessage = "Seu familiar está agora na forma de um rato enorme." ; }
					else if ( ball.FamiliarType == 0xD7 ){ ball.FamiliarType = 80; sMessage = "Seu familiar está agora na forma de um sapo grande." ; }
					else if ( ball.FamiliarType == 80 ){ ball.FamiliarType = 81; sMessage = "Seu familiar está agora na forma de um sapo enorme." ; }
					else if ( ball.FamiliarType == 81 ){ ball.FamiliarType = 340; sMessage = "Seu familiar está agora na forma de um gato grande." ; }
					else if ( ball.FamiliarType == 340 ){ ball.FamiliarType = 277; sMessage = "Seu familiar está agora na forma de um lobo." ; }
					else if ( ball.FamiliarType == 277 ){ ball.FamiliarType = 0xCE; sMessage = "Seu familiar está agora na forma de uma lagartixa grande." ; }
					else if ( ball.FamiliarType == 0xCE && HighSpellCaster == 1 ){ ball.FamiliarType = 590; sMessage = "Seu familiar está agora na forma de um dragão pequeno." ; }
					else if ( ball.FamiliarType == 0xCE && HighSpellCaster == 2 ){ ball.FamiliarType = 0x3C; sMessage = "Seu familiar está agora na forma de um dragão." ; }
					else if ( ball.FamiliarType == 590 || ball.FamiliarType == 0x3C ){ ball.FamiliarType = 315; sMessage = "Seu familiar está agora na forma de um escorpião grande." ; }
					else if ( ball.FamiliarType == 315 ){ ball.FamiliarType = 120; sMessage = "Seu familiar está agora na forma de um besouro enorme." ; }
					else if ( ball.FamiliarType == 120 ){ ball.FamiliarType = 202; sMessage = "Seu familiar está agora na forma de um demônio menor." ; }
					else if ( ball.FamiliarType == 202 && HighSpellCaster == 1 ){ ball.FamiliarType = 140; sMessage = "Seu familiar está agora na forma de uma aranha." ; }
					else if ( ball.FamiliarType == 202 && HighSpellCaster == 2 ){ ball.FamiliarType = 173; sMessage = "Seu familiar está agora na forma de uma aranha gigante." ; }
					else if ( ball.FamiliarType == 140 || ball.FamiliarType == 173 ){ ball.FamiliarType = 317; sMessage = "Seu familiar está agora na forma de um morcego." ; }
					else if ( ball.FamiliarType == 317 ){ ball.FamiliarType = 242; sMessage = "Seu familiar está agora na forma de um inseto gigante." ; }
					else if ( ball.FamiliarType == 242 ){ ball.FamiliarType = 0x15; sMessage = "Seu familiar está agora na forma de uma serpente." ; }
					else if ( ball.FamiliarType == 0x15 && HighSpellCaster == 1 ){ ball.FamiliarType = 0x4; sMessage = "Seu familiar está agora na forma de um demônio." ; }
					else if ( ball.FamiliarType == 0x15 && HighSpellCaster == 2 ){ ball.FamiliarType = 0x9; sMessage = "Seu familiar está agora na forma de um demônio maior." ; }
					else if ( ball.FamiliarType == 0x4 || ball.FamiliarType == 0x9 ){ ball.FamiliarType = 0x16; sMessage = "Seu familiar está agora na forma de um observador." ; }

					sMessage = "Você talvez gostaria de um familiar diferente? " + sMessage;
					from.AddToBackpack ( ball );
				}
				else
				{
					sMessage = "Obrigado por isto. Só posso assumir que um conjurador aprendiz perdeu isso.";
					dropped.Delete();
				}

				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
			}
			return base.OnDragDrop( from, dropped );
		}

		public ElementalGuildmaster( Serial serial ) : base( serial )
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
