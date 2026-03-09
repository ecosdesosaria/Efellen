using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Misc;
using Server.Engines.BulkOrders;
using Server.Regions;
using Server.Multis;
using Server.Targeting;

namespace Server.Mobiles
{
	public enum VendorShoeType
	{
		None,
		Shoes,
		Boots,
		Sandals,
		ThighBoots
	}

	public abstract class SBInfo
	{
		public SBInfo()
		{
		}

		public abstract IShopSellInfo SellInfo { get; }

        public abstract List<GenericBuyInfo> BuyInfo { get; }
	}

	public abstract class BaseVendor : BaseCreature, IVendor
	{
		public DateTime m_NextAction;
		public DateTime NextAction{ get{ return m_NextAction; } set{ m_NextAction = value; } }

		private const int MaxSell = 500;

		protected abstract List<SBInfo> SBInfos { get; }

		private ArrayList m_ArmorBuyInfo = new ArrayList();
		private ArrayList m_ArmorSellInfo = new ArrayList();

		private DateTime m_LastRestock;

		public override bool CanTeach { get { return true; } }

		public override bool BardImmune { get { return false; } }

		public override bool PlayerRangeSensitive { get { return true; } }

		public virtual bool IsBlackMarket { get { return false; } }
		public virtual bool IsActiveVendor { get { return true; } }
		public virtual bool IsActiveBuyer
		{
			get
			{
				if ( !MySettings.S_VendorsBuyStuff )
					return false;

				return IsActiveVendor;
			}
		} // response to vendor SELL
		public virtual bool IsActiveSeller { get { return IsActiveVendor; } } // repsonse to vendor BUY

		public virtual NpcGuild NpcGuild { get { return NpcGuild.None; } }

		public virtual bool IsInvulnerable { get { return false; } }
		public override bool Unprovokable { get { return false; } }
		public override bool Uncalmable{ get{ return false; } }

		public override bool ShowFameTitle { get { return false; } }

		public virtual bool IsValidBulkOrder( Item item )
		{
			return false;
		}

		public virtual Item CreateBulkOrder( Mobile from, bool fromContextMenu )
		{
			return null;
		}

		public virtual bool SupportsBulkOrders( Mobile from )
		{
			return false;
		}

		public virtual TimeSpan GetNextBulkOrder( Mobile from )
		{
			return TimeSpan.Zero;
		}

		public virtual void OnSuccessfulBulkOrderReceive( Mobile from )
		{
		}

		public virtual int GetPriceScalar()
		{
			return 100;
		}

		public void UpdateBuyInfo()
		{
			int priceScalar = GetPriceScalar();

			IBuyItemInfo[] buyinfo = (IBuyItemInfo[])m_ArmorBuyInfo.ToArray( typeof( IBuyItemInfo ) );

			if ( buyinfo != null )
			{
				foreach ( IBuyItemInfo info in buyinfo )
					info.PriceScalar = priceScalar;
			}
		}

		public virtual void UpdateBlackMarket()
		{
			if ( typeof( PlayerVendor ) == this.GetType() || typeof( PlayerBarkeeper ) == this.GetType() )
				return;

			Container bank = this.BankBox;
			if ( IsBlackMarket && MyServerSettings.BlackMarket() )
			{
				List<Item> belongings = new List<Item>();
				foreach( Item i in bank.Items )
				{
					belongings.Add(i);
				}
				foreach ( Item stuff in belongings )
				{
					stuff.Delete();
				}
			}
		}

		public virtual void UpdateCoins()
		{
			if ( typeof( PlayerVendor ) == this.GetType() || typeof( PlayerBarkeeper ) == this.GetType() )
				return;

			Container pack = this.Backpack;

			if ( pack == null )
			{
				pack = new Backpack();
				pack.Movable = false;
				AddItem( pack );
			}

			List<Item> belongings = new List<Item>();
			foreach( Item i in pack.Items )
			{
				belongings.Add(i);
			}
			foreach ( Item stuff in belongings )
			{
				stuff.Delete();
			}

			int money1 = 30;
			int money2 = 120;

			double w1 = money1 * (MyServerSettings.GetGoldCutRate() * .01);
			double w2 = money2 * (MyServerSettings.GetGoldCutRate() * .01);

			money1 = (int)w1;
			money2 = (int)w2;

			this.PackGold( money1, money2 );

			int loot = 10;

			while ( loot > 0 )
			{
				loot--;

				if ( Utility.RandomBool() )
					this.PackItem( Loot.RandomItem( this, -10 ) );
			}
		}

		private class BulkOrderInfoEntry : ContextMenuEntry
		{
			private Mobile m_From;
			private BaseVendor m_Vendor;

			public BulkOrderInfoEntry( Mobile from, BaseVendor vendor ): base( 6152 )
			{
				m_From = from;
				m_Vendor = vendor;
				Enabled = vendor.CheckVendorAccess( from );
			}

			public override void OnClick()
			{
				if ( m_Vendor.SupportsBulkOrders( m_From ) )
				{
					TimeSpan ts = m_Vendor.GetNextBulkOrder( m_From );

					int totalSeconds = (int)ts.TotalSeconds;
					int totalHours = ( totalSeconds + 3599 ) / 3600;
					int totalMinutes = ( totalSeconds + 59 ) / 60;

					if ( ( ( Core.SE ) ? totalMinutes == 0 : totalHours == 0 ) )
					{
						m_From.SendLocalizedMessage( 1049038 ); // You can get an order now.

						if ( Core.AOS )
						{
							Item bulkOrder = m_Vendor.CreateBulkOrder( m_From, true );

							if ( bulkOrder is LargeBOD )
								m_From.SendGump( new LargeBODAcceptGump( m_From, (LargeBOD)bulkOrder ) );
							else if ( bulkOrder is SmallBOD )
								m_From.SendGump( new SmallBODAcceptGump( m_From, (SmallBOD)bulkOrder ) );
						}
					}
					else
					{
						int oldSpeechHue = m_Vendor.SpeechHue;
						m_Vendor.SpeechHue = Utility.RandomTalkHue();

						m_Vendor.SayTo( m_From, 1072058, totalMinutes.ToString() ); // An offer may be available in about ~1_minutes~ minutes.

						m_Vendor.SpeechHue = oldSpeechHue;
					}
				}
			}
		}

		public BaseVendor( string title ) : base( AIType.AI_Melee, FightMode.Closest, 15, 1, 0.1, 0.2 )
		{
			this.Title = title;

			SpeechHue = Utility.RandomTalkHue();

			InitBody();
			InitOutfit();

			Container pack;
			//these packs MUST exist, or the client will crash when the packets are sent
			pack = new Backpack();
			pack.Layer = Layer.ShopBuy;
			pack.Movable = false;
			pack.Visible = false;
			AddItem( pack );

			pack = new Backpack();
			pack.Layer = Layer.ShopResale;
			pack.Movable = false;
			pack.Visible = false;
			AddItem( pack );

			LoadSBInfo( this );
			m_LastRestock = DateTime.Now;
			UpdateBlackMarket();
			DefaultCoinPurse();
			UpdateCoins();
		}

		public void DefaultCoinPurse()
		{
			if ( typeof( PlayerVendor ) == this.GetType() || typeof( PlayerBarkeeper ) == this.GetType() )
				return;

			this.CoinPurse = Utility.RandomMinMax( MySettings.S_MinMerchant, MySettings.S_MaxMerchant );

			if ( this is BaseGuildmaster )
				this.CoinPurse = this.CoinPurse * 3;

			this.InvalidateProperties();
		}

		public BaseVendor( Serial serial ): base( serial )
		{
		}

		public static int BeggingPose( Mobile from ) // LET US SEE IF THEY ARE BEGGING
		{
			int beggar = 0;
			if ( from is PlayerMobile )
			{
				if ( ((PlayerMobile)from).CharacterBegging > 0 )
				{
					beggar = (int)from.Skills[SkillName.Begging].Value;
				}
			}
			return beggar;
		}
		
		public static int BeggingKarma( Mobile from ) // LET US SEE IF THEY ARE BEGGING
		{
			int charisma = 0;
			if ( from.Karma > -2459 ){ charisma = 40; }
			from.CheckSkill( SkillName.Begging, 0, 125 );
			return charisma;
		}

		public static string BeggingWords() // LET US SEE IF THEY ARE BEGGING
		{
			string sSpeak = "Por favor, me dê um bom preço, pois sou muito pobre.";
			switch( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: sSpeak = "Por favor, me dê um bom preço, pois sou muito pobre."; break;
				case 1: sSpeak = "Tenho muito pouco ouro, então qualquer coisa que você possa dar..."; break;
				case 2: sSpeak = "Não como há dias, então seu ouro certamente ajudará."; break;
				case 3: sSpeak = "Dará a uma alma pobre mais por isto?"; break;
				case 4: sSpeak = "Passei por tempos difíceis, será gentil?"; break;
				case 5: sSpeak = "O que você puder dar por isto certamente ajudará."; break;
			}
			return sSpeak;
		}

		public DateTime LastRestock
		{
			get
			{
				return m_LastRestock;
			}
			set
			{
				m_LastRestock = value;
			}
		}

		public virtual TimeSpan RestockDelay
		{
			get
			{
				return TimeSpan.FromHours( 1 );
			}
		}

		public virtual TimeSpan RestockDelayFull
		{
			get
			{
				return TimeSpan.FromMinutes( 15 );
			}
		}

		public Container BuyPack
		{
			get
			{
				Container pack = FindItemOnLayer( Layer.ShopBuy ) as Container;

				if ( pack == null )
				{
					pack = new Backpack();
					pack.Layer = Layer.ShopBuy;
					pack.Visible = false;
					AddItem( pack );
				}

				return pack;
			}
		}

		public abstract void InitSBInfo( Mobile m );

		protected void LoadSBInfo( Mobile m )
		{
			m_LastRestock = DateTime.Now;

			for ( int i = 0; i < m_ArmorBuyInfo.Count; ++i )
			{
				GenericBuyInfo buy = m_ArmorBuyInfo[i] as GenericBuyInfo;

				if ( buy != null )
					buy.DeleteDisplayEntity();
			}

			SBInfos.Clear();

			InitSBInfo( m );

			m_ArmorBuyInfo.Clear();
			m_ArmorSellInfo.Clear();

			for ( int i = 0; i < SBInfos.Count; i++ )
			{
				SBInfo sbInfo = (SBInfo)SBInfos[i];
				m_ArmorBuyInfo.AddRange( sbInfo.BuyInfo );
				m_ArmorSellInfo.Add( sbInfo.SellInfo );
			}
		}

		public virtual bool GetGender()
		{
			return Utility.RandomBool();
		}

		public virtual void InitBody()
		{
			InitStats( 100, 100, 25 );

			SpeechHue = Utility.RandomTalkHue();
			Hue = Utility.RandomSkinColor();

			if ( IsInvulnerable && !Core.AOS )
				NameHue = 0x35;

			if ( this is Roscoe || this is Garth )
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
			}
			else if ( this is KungFu )
			{
				if ( Female = GetGender() )
				{
					Body = 0x191;
					Name = NameList.RandomName( "tokuno female" );
				}
				else
				{
					Body = 0x190;
					Name = NameList.RandomName( "tokuno male" );
				}
			}
			else if ( Female = GetGender() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
			}
		}

		public virtual int GetRandomHue()
		{
			switch ( Utility.Random( 5 ) )
			{
				default:
				case 0: return Utility.RandomBlueHue();
				case 1: return Utility.RandomGreenHue();
				case 2: return Utility.RandomRedHue();
				case 3: return Utility.RandomYellowHue();
				case 4: return Utility.RandomNeutralHue();
			}
		}

		public virtual int GetShoeHue()
		{
			if ( 0.1 > Utility.RandomDouble() )
				return 0;

			return Utility.RandomNeutralHue();
		}

		public virtual VendorShoeType ShoeType
		{
			get { return VendorShoeType.Shoes; }
		}

		public virtual int RandomBrightHue()
		{
			if ( 0.1 > Utility.RandomDouble() )
				return Utility.RandomList( 0x62, 0x71 );

			return Utility.RandomList( 0x03, 0x0D, 0x13, 0x1C, 0x21, 0x30, 0x37, 0x3A, 0x44, 0x59 );
		}

		public static void DoJokes( Mobile m )
		{
			int act = Utility.Random( 28 );
			if ( m is PlayerMobile ){ act = Utility.Random( 22 ); }
			switch ( act )
			{
				case 0: m.Say("Por que o rei foi ao dentista? Para coroar seus dentes."); break;
				case 1: m.Say("Quando um cavaleiro de armadura era morto em batalha, que sinal colocavam em seu túmulo? Ferre em paz!"); break;
				case 2: m.Say("Como você chama um mosquito em uma armadura? Uma picada em armadura brilhante."); break;
				case 3: m.Say("Há muitos castelos no mundo, mas quem é forte o suficiente para mover um? Qualquer jogador de xadrez"); break;
				case 4: m.Say("Qual rei era famoso porque passou tantas noites em sua Mesa Redonda escrevendo livros? Rei Artur!"); break;
				case 5: m.Say("Como você encontra uma princesa? Você segue o príncipe pé."); break;
				case 6: m.Say("Por que os primeiros dias eram chamados de idade das trevas? Porque havia tantos cavaleiros!"); break;
				case 7: m.Say("Por que Artur tinha uma mesa redonda? Para que ninguém pudesse encurralá-lo!"); break;
				case 8: m.Say("Quem inventou a mesa redonda do Rei Artur? Sir Cumferência!"); break;
				case 9: m.Say("Por que o cavaleiro correu gritando por um abridor de latas? Ele tinha uma abelha em sua armadura!"); break;
				case 10: m.Say("Pelo que Camelot era famosa? Por sua vida de cavaleiro!"); break;
				case 11: m.Say("O que o sapo disse quando a princesa não quis beijá-lo? O que há de verruga com você?"); break;
				case 12: m.Say("Como você chama o jovem nobre que não para de cair? Príncipe de Queda!"); break;
				case 13: m.Say("Como você chama um gato que voa sobre o muro do castelo? Um gato-apulto!"); break;
				case 14: m.Say("Que jogo os peixes jogam no fosso? Verdade ou truta!"); break;
				case 15: m.Say("O que o peixe disse para o outro quando o cavalo caiu no fosso? Cavalo marinho!"); break;
				case 16: m.Say("Como você chama uma princesa brava recém-acordada de um longo sono? Bela adormecida à tapas!"); break;
				case 17: m.Say("Como o príncipe entrou no castelo quando a ponte levadiça quebrou? Ele usou um remosso!"); break;
				case 18: m.Say("Como a dragão ganhou o concurso de beleza? Ela era a fera do show!"); break;
				case 19: m.Say("Por que o dinossauro viveu mais que o dragão? Porque não fumava!"); break;
				case 20: m.Say("O que o dragão disse quando viu o Cavaleiro? 'Não mais comida enlatada!'"); break;
				case 21: m.Say("O que você faz com um dragão verde? Espera até ele amadurecer!"); break;
				case 22: m.PlaySound( m.Female ? 780 : 1051 ); m.Say( "*bate palmas*" ); break;
				case 23: m.Say( "*faz uma reverência*" ); m.Animate( 32, 5, 1, true, false, 0 ); break;
				case 24: m.PlaySound( m.Female ? 794 : 1066 ); m.Say( "*risadinha*" ); break;
				case 25: m.PlaySound( m.Female ? 801 : 1073 ); m.Say( "*risos*" ); break;
				case 26: m.PlaySound( 792 ); m.Say( "*mostra a língua*" ); break;
				case 27: m.PlaySound( m.Female ? 783 : 1054 ); m.Say( "*uuhu!*" ); break;
			};

			if ( act < 22 && Utility.RandomBool() )
			{
				switch ( Utility.Random( 6 ))
				{
					case 0: m.PlaySound( m.Female ? 780 : 1051 ); break;
					case 1: m.Animate( 32, 5, 1, true, false, 0 ); break;
					case 2: m.PlaySound( m.Female ? 794 : 1066 ); break;
					case 3: m.PlaySound( m.Female ? 801 : 1073 ); break;
					case 4: m.PlaySound( 792 ); break;
					case 5: m.PlaySound( m.Female ? 783 : 1054 ); break;
				};
			}
		}

		public override void OnThink()
		{
			bool ran = false;
			if ( DateTime.Now >= m_NextAction && ( this is Jester || ( this.Location == this.Home && this.RangeHome == 0 ) ) )
			{
				int range = 1;
				if ( this is Fisherman || this is Ranger || this is Bowyer )
					range = 7;

				m_NextAction = (DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 5, 15 ) ) );

				if ( this is Jester )
				{
					DoJokes( this );
					m_NextAction = (DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 25, 35 ) ) );
					ran = true;
				}
				else
				{
					ArrayList acts = new ArrayList();
					foreach ( Item act in this.GetItemsInRange( range ) )
					{
						if ( act.Map == this.Map && act is ArcheryButte && ( this is Ranger || this is Bowyer ) && ( act.X == X || act.Y == Y ) )
						{
							if ( this is Bowyer ){ this.Title = "the archer"; }
								acts.Add( act );
						}
						else if ( act.Map == this.Map && act is TrainingDummy && this is Fighter )
						{
							acts.Add( act );
						}
						else if ( act.Map == this.Map && act is Actions )
						{
							int z = Z - act.Z;
								if ( z < 0 )
									z = act.Z - Z;

							if ( z < 15 )
							{
								acts.Add( act );
							}
						}
					}
					for ( int i = 0; i < acts.Count; ++i )
					{
						ran = true;
						Item it = ( Item )acts[ i ];
						it.OnDoubleClick( this );
					}
				}

				if ( !ran )
					m_NextAction = (DateTime.Now).AddYears(1);
			}

			base.OnThink();

			if ( PlayersInSight() )
				this.CantWalk = true;
			else
				this.CantWalk = false;
		}

		private bool PlayersInSight()
		{
			int players = 0;
			bool enemies = false;

			foreach ( Mobile player in GetMobilesInRange( 6 ) )
			{
				if ( player is PlayerMobile )
				{
					if ( CanSee( player ) && InLOS( player ) ){ ++players; }
				}

				if ( IsEnemy( player ) )
					enemies = true;
			}

			if ( players > 0 && !enemies )
				return true;

			return false;
		}

		public override void OnAfterSpawn()
		{
			ControlSlots = RangeHome;

			bool runActs = true;

			base.OnAfterSpawn();

			Region reg = Region.Find( this.Location, this.Map );

			if ( !Server.Items.EssenceBase.ColorCitizen( this ) )
				Server.Misc.MorphingTime.CheckMorph( this );

			if ( this is BaseVendor && ( reg.Name == "the Grey Archeological Dig" || this is Merchant ) )
			{
				Item candle = new HeldLight();
				candle.Name = "lantern";
				candle.ItemID = 0xA22;
				candle.Light = LightType.Circle300;
				AddItem( candle );
			}

			if ( ( reg.Name == "the Basement" || reg.Name == "the Dungeon Room" || reg.Name == "the Camping Tent" ) && this is Provisioner )
			{
				this.Title = "the merchant";
				runActs = false;
			}
			else if ( Server.Misc.Worlds.IsSeaTown( this.Location, this.Map ) )
			{
				if ( this is Provisioner && reg.Name != "the Port" ){ this.Title = "the dock worker"; if ( Utility.RandomBool() ){ this.Title = "the merchant"; } }
				else if ( this is Fisherman && reg.Name != "the Port" ){ runActs = false; this.Title = "the sailor"; }
				else if ( this is Carpenter && reg.Name != "the Port" ){ runActs = false; this.Title = "the cooper"; }
				else if ( this is Waiter ){ this.Title = "the cabin boy"; if ( this.Female ){ this.Title = "the serving wench"; } }
				else if ( this is Weaponsmith && reg.Name != "the Port" ){ runActs = false; this.Title = "the master-at-arms"; }
				else if ( this is Ranger )
				{
					this.Title = "the harpooner";
					if ( this.FindItemOnLayer( Layer.OneHanded ) != null ) { this.FindItemOnLayer( Layer.OneHanded ).Delete(); }
					if ( this.FindItemOnLayer( Layer.TwoHanded ) != null ) { this.FindItemOnLayer( Layer.TwoHanded ).Delete(); }
					if ( this.FindItemOnLayer( Layer.Helm ) != null ) { this.FindItemOnLayer( Layer.Helm ).Delete(); }
					if ( this.FindItemOnLayer( Layer.Helm ) != null ) { this.FindItemOnLayer( Layer.Helm ).Delete(); }
					if ( Utility.RandomBool() ){ this.AddItem( new SkullCap( Utility.RandomDyedHue() ) ); }
					this.AddItem( new Harpoon() );
					runActs = false;
				}
				else if ( this is Shipwright && reg.Name != "the Port" ){ runActs = false; this.Title = "the boatswain"; }

				if ( !(this is Shipwright && reg.Name == "the Port" ) )
				{
					if ( !(this is Jester) && !(this is Druid) && !(this is VarietyDealer) )
					{
						if ( this.FindItemOnLayer( Layer.Helm ) != null ) { this.FindItemOnLayer( Layer.Helm ).Delete(); }
						if ( Utility.RandomBool() ){ this.AddItem( new SkullCap() ); }
						else { this.AddItem( new Bandana() ); }
						MorphingTime.ColorMixClothes( this );
					}
				}
			}
			else if ( reg.Name == "the Thieves Guild" && this is Provisioner )
			{
				this.Title = "the fence";
			}
			else if ( reg.Name == "the Ship's Lower Deck" && !(this is Jester) )
			{
				if ( this is Provisioner  ){ this.Title = "the quartermaster"; }
				else if ( this is Waiter ){ this.Title = "the cabin boy"; if ( this.Female ){ this.Title = "the serving wench"; } }
				if ( this.FindItemOnLayer( Layer.Helm ) != null ) { this.FindItemOnLayer( Layer.Helm ).Delete(); }
				if ( Utility.RandomBool() ){ this.AddItem( new SkullCap() ); }
				else { this.AddItem( new Bandana() ); }
				MorphingTime.ColorOnlyClothes( this, Utility.RandomColor(0), 0 );
			}
			else if ( reg.Name == "the Wizards Guild" && this is Waiter && this.Body == 400 )
			{
				this.Title = "the butler";
			}
			else if ( reg.Name == "the Wizards Guild" && this is Waiter && this.Body == 401 )
			{
				this.Title = "the maid";
			}

			if ( Server.Items.ActionFunc.HasActs( this ) && runActs )
			{
				Server.Items.ActionFunc.RemoveActions( true, this.Home, this.Map );
				Server.Items.ActionFunc.MakeActs( (BaseCreature)this );
			}

			if ( Title.Contains("mistress") && !Female )
				Title = Title.Replace("mistress", "master");
			else if ( Title.Contains("master") && Female )
				Title = Title.Replace("master", "mistress");
		}

		protected override void OnMapChange( Map oldMap )
		{
			base.OnMapChange( oldMap );

			if ( !Server.Items.EssenceBase.ColorCitizen( this ) )
			{
				Server.Misc.MorphingTime.CheckMorph( this );
			}

			LoadSBInfo( this );
		}

		public virtual int GetHairHue()
		{
			return Utility.RandomHairHue();
		}

		public virtual void InitOutfit()
		{
			Server.Misc.IntelligentAction.DressUpMerchants( this );

			int hairHue = GetHairHue();

			Utility.AssignRandomHair( this, hairHue );

			if ( !Female )
			{
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				FacialHairHue = hairHue;
			}
		}

		public virtual void Restock()
		{
			m_LastRestock = DateTime.Now;
			LoadSBInfo( this );
		}

		public override bool OnBeforeDeath()
		{
			Server.Misc.MorphingTime.TurnToSomethingOnDeath( this );

			Mobile killer = this.LastKiller;

			if (killer is BaseCreature)
			{
				BaseCreature bc_killer = (BaseCreature)killer;
				if(bc_killer.Summoned)
				{
					if(bc_killer.SummonMaster != null)
						killer = bc_killer.SummonMaster;
				}
				else if(bc_killer.Controlled)
				{
					if(bc_killer.ControlMaster != null)
						killer=bc_killer.ControlMaster;
				}
				else if(bc_killer.BardProvoked)
				{
					if(bc_killer.BardMaster != null)
						killer=bc_killer.BardMaster;
				}
			}

			if ( killer is PlayerMobile )
			{
				killer.Criminal = true;
				killer.Kills = killer.Kills + 1;
				Server.Items.DisguiseTimers.RemoveDisguise( killer );
			}

			if ( !base.OnBeforeDeath() )
				return false;

			string bSay = "Help! Guards!";
			this.PublicOverheadMessage( MessageType.Regular, 0, false, string.Format ( bSay ) ); 

			return true;
		}

		public override bool IsEnemy( Mobile m )
		{
			if ( !IntelligentAction.GetMyEnemies( m, this, true ) )
				return false;

			if ( m.Region != this.Region && !(m is PlayerMobile) )
				return false;

			return true;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			switch ( Utility.Random( 4 ))		   
			{
				case 0: Say("Saia deste lugar!"); break;
				case 1: Say("" + defender.Name + ", nós ouvimos falar de você!"); break;
				case 2: Say("Nos disseram para ficar de olho em você, " + defender.Name + "!"); break;
				case 3: Say("Guardas, " + defender.Name + " está aqui!"); break;
			};
		}

		private static TimeSpan InventoryDecayTime = TimeSpan.FromHours( 2.0 );

		public virtual void VendorBuy( Mobile from )
		{
			if ( !IsActiveSeller )
				return;

			if ( !from.CheckAlive() )
				return;

			if ( !CheckVendorAccess( from ) )
			{
				this.Say( "Não tenho negócios com você." );
				return;
			}

			if ( DateTime.Now - m_LastRestock > RestockDelay )
			{
				UpdateBlackMarket();
				DefaultCoinPurse();
				UpdateCoins();
			}

			if ( DateTime.Now - m_LastRestock > RestockDelay || ( from.Region.IsPartOf( typeof( PublicRegion ) ) && DateTime.Now - m_LastRestock > RestockDelayFull ) || ( this is BaseGuildmaster && DateTime.Now - m_LastRestock > RestockDelayFull ) )
				Restock();

			UpdateBuyInfo();

			if ( this.RaceID == 0 && Utility.RandomBool() ){ this.PlaySound( this.Female ? 797 : 1069 ); }

			int count = 0;
			List<BuyItemState> list;
			IBuyItemInfo[] buyInfo = this.GetBuyInfo();
			IShopSellInfo[] sellInfo = this.GetSellInfo();

            list = new List<BuyItemState>( buyInfo.Length );
			Container cont = this.BuyPack;

			List<ObjectPropertyList> opls = null;

			for ( int idx = 0; idx < buyInfo.Length; idx++ )
			{
				IBuyItemInfo buyItem = (IBuyItemInfo)buyInfo[idx];

				int sales = 250;
					if ( this is Sage ){ sales = 500; }

				if ( buyItem.Amount <= 0 || list.Count >= sales )
					continue;

				// NOTE: Only GBI supported; if you use another implementation of IBuyItemInfo, this will crash
				GenericBuyInfo gbi = (GenericBuyInfo)buyItem;
				IEntity disp = gbi.GetDisplayEntity();

				list.Add( new BuyItemState( buyItem.Name, cont.Serial, disp == null ? (Serial)0x7FC0FFEE : disp.Serial, buyItem.Price, buyItem.Amount, buyItem.ItemID, buyItem.Hue ) );
				count++;

				if ( opls == null ) {
					opls = new List<ObjectPropertyList>();
				}

				if ( disp is Item ) {
					opls.Add( ( ( Item ) disp ).PropertyList );
				} else if ( disp is Mobile ) {
					opls.Add( ( ( Mobile ) disp ).PropertyList );
				}
			}

			List<Item> playerItems = cont.Items;

			for ( int i = playerItems.Count - 1; i >= 0; --i )
			{
				if ( i >= playerItems.Count )
					continue;

				Item item = playerItems[i];

				if ( ( item.LastMoved + InventoryDecayTime ) <= DateTime.Now )
					item.Delete();
			}

			for ( int i = 0; i < playerItems.Count; ++i )
			{
				Item item = playerItems[i];

				int price = 0;
				string name = null;

				foreach ( IShopSellInfo ssi in sellInfo )
				{
					if ( ssi.IsSellable( item ) )
					{
						price = ssi.GetBuyPriceFor( item );
						name = ssi.GetNameFor( item );
						break;
					}
				}

				if ( name != null && list.Count < 250 )
				{
					list.Add( new BuyItemState( name, cont.Serial, item.Serial, price, item.Amount, item.ItemID, item.Hue ) );
					count++;

					if ( opls == null ) {
						opls = new List<ObjectPropertyList>();
					}

					opls.Add( item.PropertyList );
				}
			}

			//one (not all) of the packets uses a byte to describe number of items in the list.  Osi = dumb.
			//if ( list.Count > 255 )
			//	Console.WriteLine( "Vendor Warning: Vendor {0} has more than 255 buy items, may cause client errors!", this );

			int inventory = 0;

			if ( list.Count > 0 )
			{
				inventory++;

				list.Sort( new BuyItemStateComparer() );

				SendPacksTo( from );

				NetState ns = from.NetState;

				if ( ns == null )
					return;

				if ( ns.ContainerGridLines )
					from.Send( new VendorBuyContent6017( list ) );
				else
					from.Send( new VendorBuyContent( list ) );

				from.Send( new VendorBuyList( this, list ) );

				if ( ns.HighSeas )
					from.Send( new DisplayBuyListHS( this ) );
				else
					from.Send( new DisplayBuyList( this ) );

				from.Send( new MobileStatusExtended( from ) );//make sure their gold amount is sent

				if ( opls != null ) {
					for ( int i = 0; i < opls.Count; ++i ) {
						from.Send( opls[i] );
					}
				}

				if ( this is Sage )
					SayTo( from, "Dê uma olhada, mas não pode comprar os itens de pesquisa inestimáveis!" );
				else
					SayTo( from, 500186 ); // Greetings.  Have a look around.
			}

			if ( inventory < 1 )
				SayTo( from, "Desculpe, mas não tenho nada à venda no momento." );
		}

		public virtual void SendPacksTo( Mobile from )
		{
			Item pack = FindItemOnLayer( Layer.ShopBuy );

			if ( pack == null )
			{
				pack = new Backpack();
				pack.Layer = Layer.ShopBuy;
				pack.Movable = false;
				pack.Visible = false;
				AddItem( pack );
			}

			from.Send( new EquipUpdate( pack ) );

			pack = FindItemOnLayer( Layer.ShopSell );

			if ( pack != null )
				from.Send( new EquipUpdate( pack ) );

			pack = FindItemOnLayer( Layer.ShopResale );

			if ( pack == null )
			{
				pack = new Backpack();
				pack.Layer = Layer.ShopResale;
				pack.Movable = false;
				pack.Visible = false;
				AddItem( pack );
			}

			from.Send( new EquipUpdate( pack ) );
		}

		public virtual void VendorSell( Mobile from )
		{
			if ( BeggingPose(from) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) ) // LET US SEE IF THEY ARE BEGGING
			{
				from.Say( BeggingWords() );
			}

			if ( !IsActiveBuyer )
				return;

			if ( !from.CheckAlive() )
				return;

			if ( !CheckVendorAccess( from ) )
			{
				//Say( 501522 ); // I shall not treat with scum like thee!
				this.Say( "Não tenho negócios com você." );
				return;
			}

			Container pack = from.Backpack;

			if ( pack != null )
			{
				IShopSellInfo[] info = GetSellInfo();

				Hashtable table = new Hashtable();

				foreach ( IShopSellInfo ssi in info )
				{
					Item[] items = pack.FindItemsByType( ssi.Types );

					foreach ( Item item in items )
					{
						LockableContainer parentcon = item.ParentEntity as LockableContainer;

						if ( item is Container && ( (Container)item ).Items.Count != 0 )
							continue;

						if ( parentcon != null && parentcon.Locked == true )
							continue;

						if ( item.IsStandardLoot() && item.Movable && ssi.IsSellable( item ) )
						{
							PlayerMobile pm = (PlayerMobile)from;

							int barter = (int)from.Skills[SkillName.Mercantile].Value;

							int GuildMember = 0;

							if ( barter < 100 && this.NpcGuild != NpcGuild.None && this.NpcGuild == pm.NpcGuild ){ barter = 100; GuildMember = 1; } // FOR GUILD MEMBERS

							if ( BeggingPose(from) > 0 && GuildMember == 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) ) // LET US SEE IF THEY ARE BEGGING
							{
								Titles.AwardKarma( from, -BeggingKarma( from ), true );
								barter = (int)from.Skills[SkillName.Begging].Value;
							}

							table[item] = new SellItemState( item, ssi.GetSellPriceFor( item, barter ), ssi.GetNameFor( item ) );
						}
					}
				}

				if ( table.Count > 0 )
				{
					SendPacksTo( from );

					if ( this.RaceID == 0 && Utility.RandomBool() ){ this.PlaySound( this.Female ? 797 : 1069 ); }

					from.Send( new VendorSellList( this, table ) );
				}
				else
				{
					Say( true, "Voce nao tem nada que eu esteja interessado." );
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( from.Blessed )
			{
				string sSay = "Não posso negociar com você enquanto estiver nesse estado.";
				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sSay, from.NetState);
				return false;
			}
			else if ( IntelligentAction.GetMyEnemies( from, this, false ) )
			{
				string sSay = "Não acho que deveria aceitar isso de você.";
				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sSay, from.NetState);
				return false;
			}
			else
			{
				PlayerMobile pm = (PlayerMobile)from;

				if ( dropped is BaseQuiver && ( this is Bowyer || this is ArcherGuildmaster ) )
				{
					if ( dropped.ItemID == 0x5770 )
					{
						dropped.ItemID = Utility.RandomList( 0x2B02, 0x2B03 );
					}
					else
					{
						dropped.ItemID = 0x5770;
					}

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Deixe-me consertar essa aljava para você.", from.NetState);
					Effects.PlaySound(from.Location, from.Map, 0x55);
					return false;
				}
				else if ( ( dropped is DrakkhenEggRed || dropped is DrakkhenEggBlack ) && ( this is Druid || this is DruidTree || this is DruidGuildmaster ) )
				{
					if ( dropped is DrakkhenEggRed )
					{
						Server.Items.DrakkhenEggRed.ProcessDrakkhenEgg( from, this, dropped );
					}
					else if ( dropped is DrakkhenEggBlack )
					{
						Server.Items.DrakkhenEggBlack.ProcessDrakkhenEgg( from, this, dropped );
					}
				}
				else if ( dropped is GolemManual && ( this is Tinker || this is TinkerGuildmaster ) )
				{
					Server.Items.GolemManual.ProcessGolemBook( from, this, dropped );
				}
				else if ( dropped is OrbOfTheAbyss && ( this is Tinker || this is TinkerGuildmaster ) )
				{
					Server.Items.OrbOfTheAbyss.ChangeOrb( from, this, dropped );
				}
				else if ( dropped is RobotSchematics && ( this is Tinker || this is TinkerGuildmaster ) )
				{
					Server.Items.RobotSchematics.ProcessRobotBook( from, this, dropped );
				}
				else if ( dropped is DragonEgg && ( this is AnimalTrainer || this is Veterinarian ) )
				{
					Server.Items.DragonEgg.ProcessDragonEgg( from, this, dropped );
				}
				else if ( dropped is DracolichSkull && ( this is NecromancerGuildmaster ) )
				{
					Server.Items.DracolichSkull.ProcessDracolichSkull( from, this, dropped );
				}
				else if ( dropped is DemonPrison && ( this is NecromancerGuildmaster || this is MageGuildmaster || this is Mage || this is Necromancer || this is Witches ) )
				{
					Server.Items.DemonPrison.ProcessDemonPrison( from, this, dropped );
				}

				if ( dropped is Cargo )
				{
					Server.Items.Cargo.GiveCargo( (Cargo)dropped, this, from );
				}
				else if ( dropped is Museums && this is VarietyDealer )
				{
					Server.Items.Museums.GiveAntique( (Museums)dropped, this, from );
				}
				else if ( Server.Multis.BaseBoat.isRolledCarpet( dropped ) && ( this is Tailor || this is TailorGuildmaster ) )
				{
					Item carpet = null;

					if ( dropped is MagicDockedCarpetA || dropped is MagicCarpetADeed ){ carpet = new MagicCarpetBDeed(); }
					else if ( dropped is MagicDockedCarpetB || dropped is MagicCarpetBDeed ){ carpet = new MagicCarpetCDeed(); }
					else if ( dropped is MagicDockedCarpetC || dropped is MagicCarpetCDeed ){ carpet = new MagicCarpetDDeed(); }
					else if ( dropped is MagicDockedCarpetD || dropped is MagicCarpetDDeed ){ carpet = new MagicCarpetEDeed(); }
					else if ( dropped is MagicDockedCarpetE || dropped is MagicCarpetEDeed ){ carpet = new MagicCarpetFDeed(); }
					else if ( dropped is MagicDockedCarpetF || dropped is MagicCarpetFDeed ){ carpet = new MagicCarpetGDeed(); }
					else if ( dropped is MagicDockedCarpetG || dropped is MagicCarpetGDeed ){ carpet = new MagicCarpetHDeed(); }
					else if ( dropped is MagicDockedCarpetH || dropped is MagicCarpetHDeed ){ carpet = new MagicCarpetIDeed(); }
					else if ( dropped is MagicDockedCarpetI || dropped is MagicCarpetIDeed ){ carpet = new MagicCarpetADeed(); }

					dropped.Delete();
					carpet.Hue = dropped.Hue;
					from.AddToBackpack( carpet );
					SayTo(from, "Eu alterei seu tapete mágico.");
					Effects.PlaySound(from.Location, from.Map, 0x248);
				}
				else if ( dropped is Gold && dropped.Amount == 1000 && ( this is Mapmaker || this is CartographersGuildmaster ) )
				{
					MapWorld wMap = new MapWorld();

					wMap.WorldMap = this.Land;

					if ( wMap.WorldMap == Land.Atlantis )
						wMap.WorldMap = Land.Sosaria;
					else if ( wMap.WorldMap == Land.Luna )
						wMap.WorldMap = Land.Sosaria;
					else if ( wMap.WorldMap == Land.SkaraBrae )
						wMap.WorldMap = Land.Sosaria;

					wMap.Name = "world map";
					wMap.ColorText3 = Lands.LandName( wMap.WorldMap );
					wMap.ColorHue3 = "63B95F";

					from.AddToBackpack ( wMap );

					this.CoinPurse += 1000;
					this.InvalidateProperties();
					string sMessage = "Obrigado. Aqui está seu mapa mundial.";
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
					dropped.Delete();
					return true;
				}
				else if (	( dropped is DDCopper || dropped is DDSilver ) && 
							( this is Minter || this is Banker )	)
				{
					int nRate = 5;
					string sCoin = "prata";
					if ( dropped is DDCopper ){ nRate = 10; sCoin = "cobre";}

					int nCoins = dropped.Amount;
					int nGold = (int)Math.Floor((decimal)(dropped.Amount / nRate));
					int nChange = dropped.Amount - ( nGold * nRate );

					string sMessage = "Desculpe, você não tem o suficiente aqui para trocar por uma única moeda de ouro.";

					if ( ( nGold > 0 ) && ( nChange > 0 ) )
					{
						sMessage = "Aqui estão " + nGold.ToString() + " de ouro para você, e " + nChange.ToString() + " " + sCoin + " de troco.";
						from.AddToBackpack ( new Gold( nGold ) );
					}
					else if ( nGold > 0 )
					{
						sMessage = "Aqui estão " + nGold.ToString() + " de ouro para você.";
						from.AddToBackpack ( new Gold( nGold ) );
					}

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);

					if ( ( nChange > 0 ) && ( dropped is DDCopper ) ){ from.AddToBackpack ( new DDCopper( nChange ) ); }
					else if ( ( nChange > 0 ) && ( dropped is DDSilver ) ){ from.AddToBackpack ( new DDSilver( nChange ) ); }

					dropped.Delete();
					return true;
				}
				else if ( ( dropped is DDXormite ) && ( this is Minter || this is Banker ) )
				{
					int nGold = dropped.Amount * 3;

					string sMessage = "Aqui estão " + nGold.ToString() + " de ouro para você.";
					from.AddToBackpack ( new Gold( nGold ) );

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);

					dropped.Delete();
					return true;
				}
				else if ( ( dropped is Crystals ) && ( this is Minter || this is Banker ) )
				{
					int nGold = dropped.Amount * 5;

					string sMessage = "Aqui estão " + nGold.ToString() + " de ouro para você.";
					from.AddToBackpack ( new Gold( nGold ) );

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);

					dropped.Delete();
					return true;
				}
				else if ( ( dropped is DDJewels ) && ( this is Minter || this is Banker ) )
				{
					int nGold = dropped.Amount * 2;

					string sMessage = "Aqui estão " + nGold.ToString() + " de ouro para você.";
					from.AddToBackpack ( new Gold( nGold ) );

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);

					dropped.Delete();
					return true;
				}
				else if ( ( dropped is DDGemstones ) && ( this is Minter || this is Banker ) )
				{
					int nGold = dropped.Amount * 2;

					string sMessage = "Aqui estão " + nGold.ToString() + " de ouro para você.";
					from.AddToBackpack ( new Gold( nGold ) );

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);

					dropped.Delete();
					return true;
				}
				else if ( ( dropped is DDGoldNuggets ) && ( this is Minter || this is Banker ) )
				{
					int nGold = dropped.Amount;

					string sMessage = "Aqui estão " + nGold.ToString() + " de ouro para você.";
					from.AddToBackpack ( new Gold( nGold ) );

					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);

					dropped.Delete();
					return true;
				}
				else if ( ( dropped is HenchmanFighterItem || dropped is HenchmanArcherItem || dropped is HenchmanWizardItem ) && ( this is TavernKeeper || this is Barkeeper ) )
				{
					int fairTrade = 1;
					string sMessage = "";
					switch ( Utility.RandomMinMax( 0, 7 ) )
					{
						case 0:	sMessage = "Então, este seguidor não está dando certo para você?"; break;
						case 1:	sMessage = "Procurando um ajudante substituto, hein?"; break;
						case 2:	sMessage = "Bem... este aqui está procurando fama e fortuna."; break;
						case 3:	sMessage = "Talvez este seja mais adequado para o seu grupo."; break;
						case 4:	sMessage = "Nem todos os relacionamentos dão certo."; break;
						case 5:	sMessage = "Pelo menos vocês se separaram amigavelmente."; break;
						case 6:	sMessage = "Este tem ficado por aqui."; break;
						case 7:	sMessage = "Este também busca grandes tesouros.";		break;
					}
					if ( dropped is HenchmanFighterItem )
					{
						HenchmanFighterItem myFollower = (HenchmanFighterItem)dropped;
						if ( myFollower.HenchDead > 0 ){ fairTrade = 0; } else
						{
							HenchmanFighterItem newFollower = new HenchmanFighterItem();
							newFollower.HenchTimer = myFollower.HenchTimer;
							newFollower.HenchBandages = myFollower.HenchBandages;
							from.AddToBackpack ( newFollower );
						}
					}
					else if ( dropped is HenchmanWizardItem )
					{
						HenchmanWizardItem myFollower = (HenchmanWizardItem)dropped;
						if ( myFollower.HenchDead > 0 ){ fairTrade = 0; } else
						{
							HenchmanWizardItem newFollower = new HenchmanWizardItem();
							newFollower.HenchTimer = myFollower.HenchTimer;
							newFollower.HenchBandages = myFollower.HenchBandages;
							from.AddToBackpack ( newFollower );
						}
					}
					else if ( dropped is HenchmanArcherItem )
					{
						HenchmanArcherItem myFollower = (HenchmanArcherItem)dropped;
						if ( myFollower.HenchDead > 0 ){ fairTrade = 0; } else
						{
							HenchmanArcherItem newFollower = new HenchmanArcherItem();
							newFollower.HenchTimer = myFollower.HenchTimer;
							newFollower.HenchBandages = myFollower.HenchBandages;
							from.AddToBackpack ( newFollower );
						}
					}
					if ( fairTrade == 1 )
					{
						this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
						dropped.Delete();
						return true;
					}
					else { this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Isto não é um cemitério! Enterre-os em outro lugar!", from.NetState); }
				}
				else if ( dropped is BookBox && ( this is Mage || this is KeeperOfChivalry || this is Witches || this is Necromancer || this is MageGuildmaster ) )
				{
					Container pack = (Container)dropped;
						List<Item> items = new List<Item>();
						foreach (Item item in pack.Items)
						{
							items.Add(item);
						}
						foreach (Item item in items)
						{
							from.AddToBackpack ( item );
						}
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "A maldição foi removida dos livros.", from.NetState);
					dropped.Delete();
					return true;
				}
				else if ( dropped is CurseItem && ( this is Mage || this is KeeperOfChivalry || this is Witches || this is Necromancer || this is MageGuildmaster ) )
				{
					Container pack = (Container)dropped;
						List<Item> items = new List<Item>();
						foreach (Item item in pack.Items)
						{
							items.Add(item);
						}
						foreach (Item item in items)
						{
							from.AddToBackpack ( item );
						}
					string curseName = dropped.Name;
						if ( curseName == ""){ curseName = "item"; }
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "A maldição foi removida do(a) " + curseName + ".", from.NetState);
					dropped.Delete();
					return true;
				}
				else if ( ( dropped is SewageItem || dropped is SlimeItem ) && ( this is TavernKeeper || this is Barkeeper || this is Waiter ) )
				{
					Container pack = (Container)dropped;
						List<Item> items = new List<Item>();
						foreach (Item item in pack.Items)
						{
							items.Add(item);
						}
						foreach (Item item in items)
						{
							from.AddToBackpack ( item );
						}
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "O item foi limpo.", from.NetState);
					dropped.Delete();
					return true;
				}
				else if ( dropped is WeededItem && ( this is Alchemist || this is Herbalist ) )
				{
					Container pack = (Container)dropped;
						List<Item> items = new List<Item>();
						foreach (Item item in pack.Items)
						{
							items.Add(item);
						}
						foreach (Item item in items)
						{
							from.AddToBackpack ( item );
						}
					this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "As ervas daninhas foram removidas.", from.NetState);
					dropped.Delete();
					return true;
				}

				//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

				/* TODO: Thou art giving me? and fame/karma for gold gifts */

				if ( dropped is SmallBOD || dropped is LargeBOD )
				{
					if( Core.ML )
					{
						if( ((PlayerMobile)from).NextBODTurnInTime > DateTime.Now )
						{
							SayTo( from, 1079976 );	//
							return false;
						}
					}

					if ( !IsValidBulkOrder( dropped ) || !SupportsBulkOrders( from ) )
					{
						SayTo( from, 1045130 ); // That order is for some other shopkeeper.
						return false;
					}
					else if ( ( dropped is SmallBOD && !( (SmallBOD)dropped ).Complete ) || ( dropped is LargeBOD && !( (LargeBOD)dropped ).Complete ) )
					{
						SayTo( from, 1045131 ); // You have not completed the order yet.
						return false;
					}

					Item reward;
					int gold, fame;

					if ( dropped is SmallBOD )
						( (SmallBOD)dropped ).GetRewards( out reward, out gold, out fame );
					else
						( (LargeBOD)dropped ).GetRewards( out reward, out gold, out fame );

					from.SendSound( 0x3D );

					SayTo( from, 1045132 ); // Thank you so much!  Here is a reward for your effort.

					if ( reward != null )
						from.AddToBackpack( reward );

					if ( gold > 1000 )
						from.AddToBackpack( new BankCheck( gold ) );
					else if ( gold > 0 )
						from.AddToBackpack( new Gold( gold ) );

					Titles.AwardFame( from, fame, true );

					OnSuccessfulBulkOrderReceive( from );

					if( Core.ML )
					{
						((PlayerMobile)from).NextBODTurnInTime = DateTime.Now + TimeSpan.FromSeconds( 10.0 );
					}

					dropped.Delete();
					return true;
				}
			}

			return base.OnDragDrop( from, dropped );
		}

		private GenericBuyInfo LookupDisplayObject( object obj )
		{
			IBuyItemInfo[] buyInfo = this.GetBuyInfo();

			for ( int i = 0; i < buyInfo.Length; ++i ) {
				GenericBuyInfo gbi = (GenericBuyInfo)buyInfo[i];

				if ( gbi.GetDisplayEntity() == obj )
					return gbi;
			}

			return null;
		}

        private void ProcessSinglePurchase( BuyItemResponse buy, IBuyItemInfo bii, List<BuyItemResponse> validBuy, ref int controlSlots, ref bool fullPurchase, ref int totalCost )
		{
			int amount = buy.Amount;

			if ( amount > bii.Amount )
				amount = bii.Amount;

			if ( amount <= 0 )
				return;

			int slots = bii.ControlSlots * amount;

			if ( controlSlots >= slots )
			{
				controlSlots -= slots;
			}
			else
			{
				fullPurchase = false;
				return;
			}

			totalCost += bii.Price * amount;
			validBuy.Add( buy );
		}

		private void ProcessValidPurchase( int amount, IBuyItemInfo bii, Mobile buyer, Container cont )
		{
			if ( amount > bii.Amount )
				amount = bii.Amount;

			if ( amount < 1 )
				return;

			bii.Amount -= amount;

			IEntity o = bii.GetEntity();

			if ( o is Item )
			{
				Item item = (Item)o;

				if ( item.Stackable )
				{
					item.Amount = amount;

					if ( cont == null || !cont.TryDropItem( buyer, item, false ) )
						item.MoveToWorld( buyer.Location, buyer.Map );
				}
				else
				{
					item.Amount = 1;

					if ( cont == null || !cont.TryDropItem( buyer, item, false ) )
						item.MoveToWorld( buyer.Location, buyer.Map );

					for ( int i = 1; i < amount; i++ )
					{
						item = bii.GetEntity() as Item;

						if ( item is Spear ){ item.ItemID = 0xF62; }
						else if ( item is Club ){ item.ItemID = 0x13B4; }
						else if ( item is Cleaver ){ item.ItemID = 0xEC3; }

						if ( item != null )
						{
							item.Amount = 1;

							if ( cont == null || !cont.TryDropItem( buyer, item, false ) )
								item.MoveToWorld( buyer.Location, buyer.Map );
						}
					}
				}
			}
			else if ( o is Mobile )
			{
				Mobile m = (Mobile)o;

				m.Direction = (Direction)Utility.Random( 8 );
				m.MoveToWorld( buyer.Location, buyer.Map );
				m.PlaySound( m.GetIdleSound() );

				if ( m is BaseCreature )
				{
					( (BaseCreature)m ).SetControlMaster( buyer );
					( (BaseCreature)m ).Tamable = true;
					( (BaseCreature)m ).MinTameSkill = 29.1;
				}

				for ( int i = 1; i < amount; ++i )
				{
					m = bii.GetEntity() as Mobile;

					if ( m != null )
					{
						m.Direction = (Direction)Utility.Random( 8 );
						m.MoveToWorld( buyer.Location, buyer.Map );

						if ( m is BaseCreature )
						{
							( (BaseCreature)m ).SetControlMaster( buyer );
							( (BaseCreature)m ).Tamable = true;
							( (BaseCreature)m ).MinTameSkill = 29.1;
						}
					}
				}
			}
		}

    public virtual bool OnBuyItems(Mobile buyer, List<BuyItemResponse> list) {
		
		if (!IsActiveSeller || !buyer.CheckAlive() || !CheckVendorAccess(buyer))
		{
			Say("Não tenho negócios com você.");
			return false;
		}

		UpdateBuyInfo();

		IBuyItemInfo[] buyInfo = GetBuyInfo();
		IShopSellInfo[] sellInfo = GetSellInfo();
		List<BuyItemResponse> validBuy = new List<BuyItemResponse>(list.Count);

		int totalCost = 0;
		int controlSlots = buyer.FollowersMax - buyer.Followers;
		bool fullPurchase = true;
		bool tryGettingArty = false;

		foreach (BuyItemResponse buy in list)
		{
			Serial ser = buy.Serial;
			int amount = buy.Amount;

			if (ser.IsItem)
			{
				Item item = World.FindItem(ser);

				if (item != null && item.ArtifactLevel > 0)
				{
					item = null;
					tryGettingArty = true;
				}

				if (item == null)
					continue;

				GenericBuyInfo gbi = LookupDisplayObject(item);

				if (gbi != null)
				{
					ProcessSinglePurchase(buy, gbi, validBuy, ref controlSlots, ref fullPurchase, ref totalCost);
				}
				else if (item != this.BuyPack && item.IsChildOf(this.BuyPack))
				{
					if (amount > item.Amount)
						amount = item.Amount;

					if (amount <= 0)
						continue;

					foreach (IShopSellInfo ssi in sellInfo)
					{
						if (ssi.IsSellable(item) && ssi.IsResellable(item))
						{
							totalCost += ssi.GetBuyPriceFor(item) * amount;
							validBuy.Add(buy);
							break;
						}
					}
				}
			}
			else if (ser.IsMobile)
			{
				Mobile mob = World.FindMobile(ser);

				if (mob == null)
					continue;

				GenericBuyInfo gbi = LookupDisplayObject(mob);

				if (gbi != null)
					ProcessSinglePurchase(buy, gbi, validBuy, ref controlSlots, ref fullPurchase, ref totalCost);
			}
		}

		if (tryGettingArty)
		{
			SayTo(buyer, true, "Não! Esses são itens de pesquisa e não artefatos reais!");
			PlaySound(Female ? 802 : 1074);
			return false;
		}

		if (validBuy.Count == 0)
		{
			SayTo(buyer, fullPurchase ? 500190 : 500187);
			return false;
		}

		bool bought = (buyer.AccessLevel >= AccessLevel.GameMaster);
		bool fromBank = false;
		int fromPack = 0, fromBankAmount = 0;

		if (!bought)
		{
			Container pack = buyer.Backpack;
			Container bank = buyer.BankBox;

			int packGold = pack != null ? pack.GetAmount(typeof(Gold)) : 0;
			int bankGold = bank != null ? bank.GetAmount(typeof(Gold)) : 0;
			int totalGold = packGold + bankGold;

			if (totalGold >= totalCost)
			{
				int toConsume = totalCost;

				if (packGold > 0)
				{
					fromPack = Math.Min(packGold, toConsume);
					pack.ConsumeTotal(typeof(Gold), fromPack);
					toConsume -= fromPack;
				}

				if (toConsume > 0 && bankGold > 0)
				{
					fromBankAmount = Math.Min(bankGold, toConsume);
					bank.ConsumeTotal(typeof(Gold), fromBankAmount);
					toConsume -= fromBankAmount;
					fromBank = true;
				}

				bought = true;
			}
			else
			{
				SayTo(buyer, 500191); // Bank lacks the funds
				return false;
			}
		}

		buyer.PlaySound(0x32);

		Container dropContainer = buyer.Backpack ?? buyer.BankBox;

		foreach (BuyItemResponse buy in validBuy)
		{
			Serial ser = buy.Serial;
			int amount = buy.Amount;

			if (amount < 1)
				continue;

			if (ser.IsItem)
			{
				Item item = World.FindItem(ser);
				if (item == null)
					continue;

				GenericBuyInfo gbi = LookupDisplayObject(item);

				if (gbi != null)
				{
					ProcessValidPurchase(amount, gbi, buyer, dropContainer);
				}
				else
				{
					if (amount > item.Amount)
						amount = item.Amount;

					foreach (IShopSellInfo ssi in sellInfo)
					{
						if (ssi.IsSellable(item) && ssi.IsResellable(item))
						{
							Item buyItem = (amount >= item.Amount)
								? item
								: Mobile.LiftItemDupe(item, item.Amount - amount) ?? item;

							if (buyItem is Spear) buyItem.ItemID = 0xF62;
							else if (buyItem is Club) buyItem.ItemID = 0x13B4;
							else if (buyItem is Cleaver) buyItem.ItemID = 0xEC3;

							if (dropContainer == null || !dropContainer.TryDropItem(buyer, buyItem, false))
								buyItem.MoveToWorld(buyer.Location, buyer.Map);

							break;
						}
					}
				}
			}
			else if (ser.IsMobile)
			{
				Mobile mob = World.FindMobile(ser);
				if (mob == null)
					continue;

				GenericBuyInfo gbi = LookupDisplayObject(mob);

				if (gbi != null)
					ProcessValidPurchase(amount, gbi, buyer, dropContainer);
			}
		}

		this.CoinPurse += totalCost;

		if (buyer.AccessLevel >= AccessLevel.GameMaster)
		{
			SayTo(buyer, true, fullPurchase
				? "Eu não me atreveria a cobrar nada de ti. Aqui estão os bens que solicitaste."
				: "Eu não me atreveria a cobrar nada de ti. Infelizmente, não pude vender todos os bens que solicitaste.");
		}
		else
		{
			if (fromPack > 0 && fromBankAmount > 0)
			{
				SayTo(buyer, true, fullPurchase
					? "O total de tua compra é {0} de ouro. {1} retirados da tua mochila e {2} do teu banco. Meus agradecimentos pelo patrocínio."
					: "O total de tua compra é {0} de ouro. {1} retirados da tua mochila e {2} do teu banco. Meus agradecimentos pelo patrocínio. Infelizmente, não pude vender todos os bens que solicitaste.",
					totalCost, fromPack, fromBankAmount);
			}
			else if (fromBankAmount > 0)
			{
				SayTo(buyer, true, fullPurchase
					? "O total de tua compra é {0} de ouro, retirados do teu banco. Meus agradecimentos pelo patrocínio."
					: "O total de tua compra é {0} de ouro, retirados do teu banco. Meus agradecimentos pelo patrocínio. Infelizmente, não pude vender todos os bens que solicitaste.",
					totalCost);
			}
			else
			{
				SayTo(buyer, true, fullPurchase
					? "O total de tua compra é {0} de ouro, retirados da tua mochila. Meus agradecimentos pelo patrocínio."
					: "O total de tua compra é {0} de ouro, retirados da tua mochila. Meus agradecimentos pelo patrocínio. Infelizmente, não pude vender todos os bens que solicitaste.",
					totalCost);
			}
		}

		this.InvalidateProperties();
		return true;
	}

		public virtual bool CheckVendorAccess( Mobile from )
		{
			PlayerMobile pm = (PlayerMobile)from;

			if ( this is BaseGuildmaster && this.NpcGuild != pm.NpcGuild ) // ONLY GUILD MEMBERS CAN BUY FROM GUILD MASTERS
				return false;

			if ( !from.Alive || from.Blessed )
				return false;

			if ( !(this.CanSee( from )) || !(this.InLOS( from )) )
				return false;

			bool publicRegion = false;

			if ( from.Region.IsPartOf( typeof( PublicRegion ) ) )
				publicRegion = true;

			if ( from.Region.IsPartOf( typeof( StartRegion ) ) )
				publicRegion = true;

			if ( from.Region.IsPartOf( typeof( SafeRegion ) ) )
				publicRegion = true;

			if ( from.Region.IsPartOf( typeof( ProtectedRegion ) ) )
				publicRegion = true;

			if ( from.Region.IsPartOf( typeof( NecromancerRegion ) ) && GetPlayerInfo.EvilPlayer( from ) )
				publicRegion = true;

			if ( !publicRegion && ( from.Criminal || from.Kills > 0 ) )
				return false;

			if ( !publicRegion && from is PlayerMobile && ((PlayerMobile)from).Fugitive == 1 )
				return false;

			if ( IntelligentAction.GetMyEnemies( from, this, false ) )
				return false;

			return true;
		}

        public virtual bool OnSellItems( Mobile seller, List<SellItemResponse> list )
		{
			if ( !IsActiveBuyer )
				return false;

			if ( !seller.CheckAlive() )
				return false;

			if ( !CheckVendorAccess( seller ) )
			{
				this.Say( "Não tenho negócios com você." );
				return false;
			}

			IShopSellInfo[] info = GetSellInfo();
			IBuyItemInfo[] buyInfo = this.GetBuyInfo();
			int GiveGold = 0;
			int Sold = 0;
			int SoldPrice = 0;
			PlayerMobile pm = (PlayerMobile)seller;
			int GuildMember = 0;
			Container cont;

			int SoldBarter = (int)seller.Skills[SkillName.Mercantile].Value;
			if ( SoldBarter < 100 && this.NpcGuild != NpcGuild.None && this.NpcGuild == pm.NpcGuild ){ SoldBarter = 100; GuildMember = 1; } // FOR GUILD MEMBERS
			if ( BeggingPose(seller) > 0 && GuildMember == 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) ) // LET US SEE IF THEY ARE BEGGING
				SoldBarter = (int)seller.Skills[SkillName.Begging].Value;

			foreach ( SellItemResponse resp in list )
			{
				if ( resp.Item.RootParent != seller || resp.Amount <= 0 || !resp.Item.IsStandardLoot() || !resp.Item.Movable || ( resp.Item is Container && ( (Container)resp.Item ).Items.Count != 0 ) )
					continue;

				foreach ( IShopSellInfo ssi in info )
				{
					if ( ssi.IsSellable( resp.Item ) )
					{
						Sold++;
						SoldPrice += ssi.GetSellPriceFor( resp.Item, SoldBarter ) * resp.Amount;
						break;
					}
				}
			}

			if ( Sold > MaxSell )
			{
				SayTo( seller, true, "Você pode vender apenas {0} itens por vez!", MaxSell );
				return false;
			}
			else if ( !MySettings.S_RichMerchants && SoldPrice > this.CoinPurse && !MySettings.S_UseRemainingGold )
			{
				SayTo( seller, true, "Desculpe, mas só tenho {0} de ouro para negociar.", this.CoinPurse );
				return false;
			}
			else if ( Sold == 0 )
			{
				return true;
			}

			seller.PlaySound( 0x32 );

			// Calculate maximum affordable amount if using remaining gold
			int maxAffordableGold = SoldPrice;
			if ( !MySettings.S_RichMerchants && SoldPrice > this.CoinPurse && MySettings.S_UseRemainingGold )
			{
				maxAffordableGold = this.CoinPurse;
				SayTo( seller, true, "Eu lhe dou meu {0} de ouro restante.", this.CoinPurse );
			}

			bool isBegging = BeggingPose(seller) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType());

			foreach ( SellItemResponse resp in list )
			{
				if ( resp.Item.RootParent != seller || resp.Amount <= 0 || !resp.Item.IsStandardLoot() || !resp.Item.Movable || ( resp.Item is Container && ( (Container)resp.Item ).Items.Count != 0 ) )
					continue;

				if ( isBegging )
				{
					Titles.AwardKarma( seller, -BeggingKarma( seller ), true );
				}

				foreach ( IShopSellInfo ssi in info )
				{
					if ( ssi.IsSellable( resp.Item ) )
					{
						int amount = resp.Amount;

						if ( amount > resp.Item.Amount )
							amount = resp.Item.Amount;

						// Calculate the price for this item
						int barter = (int)seller.Skills[SkillName.Mercantile].Value;
						if ( barter < 100 && this.NpcGuild != NpcGuild.None && this.NpcGuild == pm.NpcGuild ){ barter = 100; GuildMember = 1; } // FOR GUILD MEMBERS

						if ( isBegging && GuildMember == 0 )
						{
							barter = (int)seller.Skills[SkillName.Begging].Value;
						}

						int itemPrice = ssi.GetSellPriceFor( resp.Item, barter ) * amount;

						// Check if this item would exceed the affordable limit
						if ( GiveGold + itemPrice > maxAffordableGold )
						{
							// If we can't afford any of this item, skip it entirely
							if ( GiveGold >= maxAffordableGold )
								break;
								
							// Calculate how much we can afford of this item
							int remainingGold = maxAffordableGold - GiveGold;
							int unitPrice = ssi.GetSellPriceFor( resp.Item, barter );
							
							if ( unitPrice <= 0 )
								break;
								
							int affordableAmount = remainingGold / unitPrice;
							
							if ( affordableAmount <= 0 )
								break;
								
							// Reduce the amount to what we can afford
							amount = Math.Min( amount, affordableAmount );
							itemPrice = unitPrice * amount;
						}
						if ( ssi.IsResellable( resp.Item ) )
						{
							bool found = false;

							foreach ( IBuyItemInfo bii in buyInfo )
							{
								if ( bii.Restock( resp.Item, amount ) )
								{
									resp.Item.Consume( amount );
									found = true;

									break;
								}
							}

							if ( !found )
							{
								cont = this.BuyPack;

								if ( amount < resp.Item.Amount )
								{
									Item item = Mobile.LiftItemDupe( resp.Item, resp.Item.Amount - amount );

									if ( item != null )
									{
										item.SetLastMoved();
										cont.DropItem( item );
									}
									else
									{
										resp.Item.SetLastMoved();
										cont.DropItem( resp.Item );
									}
								}
								else
								{
									resp.Item.SetLastMoved();
									cont.DropItem( resp.Item );
								}
							}
						}
						else
						{
							if ( amount < resp.Item.Amount )
								resp.Item.Amount -= amount;
							else
								resp.Item.Delete();
						}

						if ( isBegging && GuildMember == 0 )
						{
							seller.CheckSkill( SkillName.Begging, 0, 125 );
						}

						GiveGold += itemPrice;
						break;
					}
				}
			}

			if ( GiveGold > 0 )
			{
				this.CoinPurse -= GiveGold;

				this.InvalidateProperties();
				
				if(GiveGold > 20000)
				{
					seller.AddToBackpack( new BankCheck( GiveGold ) );
				} 
				else
				{
					seller.AddToBackpack( new Gold( GiveGold ) );
				}

				seller.PlaySound( 0x0037 );//Gold dropping sound

				if ( SupportsBulkOrders( seller ) )
				{
					Item bulkOrder = CreateBulkOrder( seller, false );

					if ( bulkOrder is LargeBOD )
						seller.SendGump( new LargeBODAcceptGump( seller, (LargeBOD)bulkOrder ) );
					else if ( bulkOrder is SmallBOD )
						seller.SendGump( new SmallBODAcceptGump( seller, (SmallBOD)bulkOrder ) );
				}
			}

			return true;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( GetPlayerInfo.GetNPCGuild( this ) != null )
				list.Add( 1072172, "{0}\t{1}", "51C273", GetPlayerInfo.GetNPCGuild( this ) );

			if ( IsActiveBuyer && CoinPurse > 0 && !MySettings.S_RichMerchants )
				list.Add( 1072173, "{0}\t{1}", "FBFF00", "" + CoinPurse + " Gold" );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version == 1 )
			{
				List<SBInfo> sbInfos = this.SBInfos;
				int index;
				while ( ( index = reader.ReadEncodedInt() ) > 0 )
				{
					int doubled = reader.ReadEncodedInt();
					if ( sbInfos != null )
						index -= 1;
				}
			}

			if ( IsParagon )
				IsParagon = false;

			if ( NameHue == 0x35 )
				NameHue = -1;

			Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerStateCallback( Market ), this );
		}

		public static Mobile m_Merchant;

		private void Market( object state )
		{
			BaseVendor v = (BaseVendor)state;
			LoadSBInfo( (Mobile)v );
			v.m_LastRestock = DateTime.Now;

			if ( typeof( PlayerVendor ) == v.GetType() || typeof( PlayerBarkeeper ) == v.GetType() )
				return;

			v.UpdateBlackMarket();
			v.UpdateCoins();
			v.DefaultCoinPurse();
		}

		public override void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( from.Alive && IsActiveVendor )
			{
				bool buysThings = true;

				if ( CoinPurse < 1 && !MySettings.S_RichMerchants )
					buysThings = false;
				else if ( !IsActiveBuyer )
					buysThings = false;

				if ( SupportsBulkOrders( from ) && CheckVendorAccess( from ) )
					list.Add( new BulkOrderInfoEntry( from, this ) );
				
				if ( IsActiveSeller && CheckVendorAccess( from ) )
					list.Add( new VendorBuyEntry( from, this ) );

				if ( buysThings && CheckVendorAccess( from ) )
					list.Add( new VendorSellEntry( from, this ) );

				if ( IsBlackMarket && this.BankBox.TotalItems > 0 && MyServerSettings.BlackMarket() && CheckVendorAccess( from ) )
					list.Add( new BlackMarketEntry( from, this ) );
			}

			if (
				( from.Skills[SkillName.Forensics].Value >= 50 && ( this is Undertaker || this is Witches || this is Necromancer || this is NecromancerGuildmaster ) ) || 
				( from.Skills[SkillName.Druidism].Value >= 50 && ( this is Herbalist || this is DruidTree || this is Druid || this is DruidGuildmaster ) ) || 
				( from.Skills[SkillName.Alchemy].Value >= 50 && ( this is Alchemist || this is AlchemistGuildmaster ) ) || 
				( from.Skills[SkillName.Blacksmith].Value >= 50 && ( this is Blacksmith || this is BlacksmithGuildmaster ) ) || 
				( from.Skills[SkillName.Bowcraft].Value >= 50 && ( this is Bowyer || this is ArcherGuildmaster ) ) || 
				( from.Skills[SkillName.Carpentry].Value >= 50 && ( this is Carpenter || this is CarpenterGuildmaster ) ) || 
				( from.Skills[SkillName.Cartography].Value >= 50 && ( this is Mapmaker || this is CartographersGuildmaster ) ) || 
				( from.Skills[SkillName.Cooking].Value >= 50 && ( this is Cook || this is Baker || this is CulinaryGuildmaster ) ) || 
				( from.Skills[SkillName.Inscribe].Value >= 50 && ( this is Scribe || this is Sage || this is LibrarianGuildmaster ) ) || 
				( from.Skills[SkillName.Tailoring].Value >= 50 && ( this is Weaver || this is Tailor || this is LeatherWorker || this is TailorGuildmaster ) ) || 
				( from.Skills[SkillName.Tinkering].Value >= 50 && ( this is Tinker || this is TinkerGuildmaster ) ) 
			)
			{
				if ( CheckVendorAccess( from ) )
					list.Add( new SetupShoppeEntry( from, this ) );
			}

			if ( VendorRepairs( this ) && CheckVendorAccess( from ) )
				list.Add( new RepairEntry( from, this ) );

			if ( RelicFunctions.VendorDoesID( this ) && CheckVendorAccess( from ) )
				list.Add( new IdentifyEntry( from, this ) );

			if ( MagicRecharge( this ) && CheckVendorAccess( from ) )
				list.Add( new RechargeEntry( from, this ) );

			base.AddCustomContextEntries( from, list );
		}

		public virtual IShopSellInfo[] GetSellInfo()
		{
			return (IShopSellInfo[])m_ArmorSellInfo.ToArray( typeof( IShopSellInfo ) );
		}

		public virtual IBuyItemInfo[] GetBuyInfo()
		{
			return (IBuyItemInfo[])m_ArmorBuyInfo.ToArray( typeof( IBuyItemInfo ) );
		}

		public static bool VendorRepairs( Mobile m )
		{
			if (
				m is ArcherGuildmaster || 
				m is Armorer || 
				m is Bard || 
				m is BardGuildmaster || 
				m is Blacksmith || 
				m is BlacksmithGuildmaster || 
				m is Bowyer || 
				m is Carpenter || 
				m is CarpenterGuildmaster || 
				m is Furtrader || 
				m is Garth || 
				m is IronWorker || 
				m is LeatherWorker || 
				m is Lumberjack || 
				m is Ranger || 
				m is RangerGuildmaster || 
				m is Tailor || 
				m is TailorGuildmaster || 
				m is Tanner || 
				m is Tinker || 
				m is TinkerGuildmaster || 
				m is Undertaker || 
				m is Weaver || 
				m is Weaponsmith )
				return true;

			return false;
		}

		public static bool MagicRecharge( Mobile m )
		{
			if (
				m is Mage || 
				m is MageGuildmaster || 
				m is Roscoe || 
				m is Enchanter )
				return true;

			return false;
		}

		public override bool CanBeDamaged()
		{
			return !IsInvulnerable;
		}

        public void BeginRecharge( Mobile from )
        {
            if ( Deleted || !from.Alive )
                return;

			int cost = 20;

			if ( BeggingPose(from) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) ) // VAMOS VER SE ESTÃO IMPLORANDO
			{
				cost = cost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * cost ); if ( cost < 1 ){ cost = 1; }
				SayTo(from, "Já que você está implorando, qual item devo recarregar, por " + cost.ToString() + " de ouro por carga de círculo?");
			}
			else { SayTo(from, "Qual item devo recarregar, por " + cost.ToString() + " de ouro por carga de círculo?"); }

			from.SendMessage( "" + cost + " de ouro por carga para o primeiro círculo e " + cost*8 + " de ouro por carga para o oitavo círculo." );

            from.Target = new ChargeTarget( this, cost );
        }

        public void BeginIdentify( Mobile from )
        {
            if ( Deleted || !from.Alive )
                return;

			int idCost = 200;

			if ( BeggingPose(from) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) ) // VAMOS VER SE ESTÃO IMPLORANDO
			{
				idCost = idCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * idCost ); if ( idCost < 1 ){ idCost = 1; }
				SayTo(from, "Já que você está implorando, qual item devo examinar, por " + idCost.ToString() + " de ouro?");
			}
			else { SayTo(from, "Qual item devo examinar, por " + idCost.ToString() + " de ouro?"); }

            from.Target = new IDTarget(this);
        }

        public void BeginRepair( Mobile from )
        {
            if  (Deleted || !from.Alive )
                return;

			int cost = 10;

			string pnt = "durabilidade";
			if ( this is Bard || this is BardGuildmaster )
				pnt = "uso";

			if ( BeggingPose(from) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) ) // VAMOS VER SE ESTÃO IMPLORANDO
			{
				cost = cost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * cost ); if ( cost < 1 ){ cost = 1; }
				SayTo(from, "Já que você está implorando, você ainda quer me contratar para consertar algo...pelo menos " + cost.ToString() + " de ouro por " + pnt + "?");
			}
			else { SayTo(from, "Você quer me contratar para consertar o quê por " + cost.ToString() + " de ouro por " + pnt + "?"); }

            from.Target = new RepairTarget( this, cost );
        }

		private class RepairTarget : Target
        {
            private BaseVendor m_Vendor;
			private int m_Cost;

            public RepairTarget( BaseVendor vendor, int cost ) : base(12, false, TargetFlags.None)
            {
                m_Vendor = vendor;
				m_Cost = cost;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
				if ( targeted is Item )
				{
					Item rep = (Item)targeted;
					Container pack = from.Backpack;

					if ( !CanRepairItem( m_Vendor, from, rep ) )
						return;

					int repaired = 0;
					int worn = 0;
					int spent = 0;

					if ( rep is BaseWeapon )
						worn = ((BaseWeapon)rep).MaxHitPoints - ((BaseWeapon)rep).HitPoints - 1;
					else if ( rep is BaseArmor )
						worn = ((BaseArmor)rep).MaxHitPoints - ((BaseArmor)rep).HitPoints - 1;
					else if ( rep is BaseClothing )
						worn = ((BaseClothing)rep).MaxHitPoints - ((BaseClothing)rep).HitPoints - 1;
					else if ( rep is BaseInstrument )
						worn = ((BaseInstrument)rep).InitMinUses - ((BaseInstrument)rep).UsesRemaining;

					if ( worn < 1 )
					{
						m_Vendor.SayTo( from, "Isso não precisa ser consertado." );
						return;
					}

					while ( worn > 0 )
					{
						worn--;
						if ( pack.ConsumeTotal( typeof( Gold ), m_Cost ) )
						{
							spent += m_Cost;
							repaired++;
						}
						else
							worn = 0;
					}

					if ( spent < 1 )
					{
						m_Vendor.SayTo( from, "Você precisará trazer mais ouro se quiser que eu conserte isso." );
					}
					else
					{
						if ( repaired < 1 )
						{
							m_Vendor.SayTo( from, "Não posso consertar isso." );
							return;
						}

						if ( repaired == 1 )
							m_Vendor.SayTo( from, "Consertei 1 vez por " + spent + " de ouro." );
						else
							m_Vendor.SayTo( from, "Consertei " + repaired + " vezes por " + spent + " de ouro." );

						if ( rep is BaseWeapon )
						{
							((BaseWeapon)rep).MaxHitPoints--;
							((BaseWeapon)rep).HitPoints += repaired;
						}
						else if ( rep is BaseArmor )
						{
							((BaseArmor)rep).MaxHitPoints--;
							((BaseArmor)rep).HitPoints += repaired;
						}
						else if ( rep is BaseClothing )
						{
							((BaseClothing)rep).MaxHitPoints--;
							((BaseClothing)rep).HitPoints += repaired;
						}
						else if ( rep is BaseInstrument )
						{
							((BaseInstrument)rep).UsesRemaining += repaired;
						}

						m_Vendor.CoinPurse += spent;
						m_Vendor.InvalidateProperties();
						from.SendMessage( String.Format( "Você pagou {0} de ouro.", spent ) );
						if ( BeggingPose( from ) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) )
							Titles.AwardKarma( from, -BeggingKarma( from ), true );
							// DO ANY KARMA LOSS
					}

				}
				else
					m_Vendor.SayTo( from, "Não posso consertar isso." );
            }
        }

		public static bool CanRepairItem( Mobile m, Mobile from, Item item )
		{
			bool repair = false;

			if ( 
				( item is BaseWeapon && ((BaseWeapon)item).HitPoints >= ((BaseWeapon)item).MaxHitPoints-1 ) || 
				( item is BaseArmor && ((BaseArmor)item).HitPoints >= ((BaseArmor)item).MaxHitPoints-1 ) || 
				( item is BaseInstrument && ((BaseInstrument)item).UsesRemaining >= ((BaseInstrument)item).InitMinUses ) 
			)
			{
				m.SayTo( from, "Isso não precisa de reparos." );
				return false;
			}

			if ( ( item is BaseGiftStave || item is BaseWizardStaff ) && 
				( m is Tinker || m is TinkerGuildmaster ) )
				repair = true;

			else if ( item is BaseClothing && 
				( m is Tailor || m is Weaver || m is TailorGuildmaster ) )
				repair = true;

			else if ( item is BaseRanged && CraftResources.GetType( item.Resource ) == CraftResourceType.Wood &&
				( m is Bowyer || m is Ranger || m is RangerGuildmaster || m is ArcherGuildmaster ) )
				repair = true;

			else if ( item is BaseInstrument && 
				( m is Bard || m is BardGuildmaster ) )
				repair = true;

			else if ( ( item is BaseWeapon || item is BaseArmor ) && CraftResources.GetType( item.Resource ) == CraftResourceType.Wood &&
				( m is Lumberjack || m is Carpenter || m is CarpenterGuildmaster ) )
				repair = true;

			else if ( ( item is BaseWeapon || item is BaseArmor ) && ( CraftResources.GetType( item.Resource ) == CraftResourceType.Leather || CraftResources.GetType( item.Resource ) == CraftResourceType.Skin ) &&
				( m is Tanner || m is Furtrader || m is LeatherWorker || m is TailorGuildmaster ) )
				repair = true;

			else if ( ( item is BaseWeapon || item is BaseArmor ) && CraftResources.GetType( item.Resource ) == CraftResourceType.Skeletal && m is Undertaker )
				repair = true;

			else if ( item is BaseWeapon && 
				( CraftResources.GetType( item.Resource ) == CraftResourceType.Metal || CraftResources.GetType( item.Resource ) == CraftResourceType.Scales || CraftResources.GetType( item.Resource ) == CraftResourceType.Block ) &&
				( m is Weaponsmith ) )
				repair = true;

			else if ( item is BaseArmor && 
				( CraftResources.GetType( item.Resource ) == CraftResourceType.Metal || CraftResources.GetType( item.Resource ) == CraftResourceType.Scales || CraftResources.GetType( item.Resource ) == CraftResourceType.Block ) &&
				( m is Armorer ) )
				repair = true;

			else if ( ( item is BaseWeapon || item is BaseArmor ) && 
				( CraftResources.GetType( item.Resource ) == CraftResourceType.Metal || CraftResources.GetType( item.Resource ) == CraftResourceType.Scales || CraftResources.GetType( item.Resource ) == CraftResourceType.Block ) &&
				( m is Garth || m is Blacksmith || m is IronWorker || m is BlacksmithGuildmaster ) )
				repair = true;

			if ( !repair )
				m.SayTo( from, "Não posso consertar isso." );

			return repair;
		}

        private class IDTarget : Target
        {
            private BaseVendor m_Vendor;

            public IDTarget(BaseVendor vendor) : base(12, false, TargetFlags.None)
            {
                m_Vendor = vendor;
            }

            protected override void OnTarget(Mobile from, object targeted)
			{
				if ( targeted is Item )
				{
					Item examine = (Item)targeted;
					int nCost = 200;

					if ( examine is ScrollClue && ( m_Vendor is Sage || m_Vendor is Scribe || m_Vendor is LibrarianGuildmaster ) )
					{
						Container packs = from.Backpack;
						ScrollClue WhatIsIt = (ScrollClue)examine;

						if ( BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
							nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }

						int toConsume = nCost;

						if ( WhatIsIt.ScrollIntelligence == 0 )
						{
							m_Vendor.SayTo( from, "Isso já foi decifrado por alguém." );
						}
						else if (packs.ConsumeTotal(typeof(Gold), toConsume))
						{
							if ( WhatIsIt.ScrollIntelligence >= 80 ){ WhatIsIt.Name = "pergaminho codificado diabolicamente"; }
							else if ( WhatIsIt.ScrollIntelligence >= 70 ){ WhatIsIt.Name = "pergaminho codificado genialmente"; }
							else if ( WhatIsIt.ScrollIntelligence >= 60 ){ WhatIsIt.Name = "pergaminho codificado traiçoeiramente"; }
							else if ( WhatIsIt.ScrollIntelligence >= 50 ){ WhatIsIt.Name = "pergaminho codificado inteligentemente"; }
							else if ( WhatIsIt.ScrollIntelligence >= 40 ){ WhatIsIt.Name = "pergaminho codificado habilmente"; }
							else if ( WhatIsIt.ScrollIntelligence >= 30 ){ WhatIsIt.Name = "pergaminho codificado especializadamente"; }
							else { WhatIsIt.Name = "pergaminho codificado simplesmente"; }

							WhatIsIt.ScrollIntelligence = 0;
							WhatIsIt.InvalidateProperties();
							from.SendMessage(String.Format("Você pagou {0} de ouro.", toConsume));
							m_Vendor.SayTo(from, "Deixe-me mostrar-lhe o que isto diz...");
							WhatIsIt.ScrollSolved = "Decifrado por " + m_Vendor.Name + " o Escriba";
							from.PlaySound( 0x249 );
							WhatIsIt.InvalidateProperties();
						}
						else
						{
							m_Vendor.SayTo(from, "Custaria {0} de ouro para ter isso decifrado.", toConsume);
							from.SendMessage("Você não tem ouro suficiente.");
						}
					}
					else if ( examine.CoinPrice > 0 && examine.NotIdentified && RelicFunctions.VendorCanID( examine.NotIDSource, m_Vendor ) )
					{
						if ( examine.NotIDSource == Identity.None )
						{
							m_Vendor.SayTo(from, "Não posso realmente dizer muito sobre isso." );
							return;
						}

						Container packs = from.Backpack;

						if ( BeggingPose(from) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) ) // VAMOS VER SE ESTÃO IMPLORANDO
							nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost ); if ( nCost < 1 ){ nCost = 1; }

						int toConsume = nCost;

						if ( packs.ConsumeTotal(typeof(Gold), toConsume) )
						{
							if ( examine is NotIdentified )
								RelicFunctions.IDItem( m_Vendor, from, examine, SkillName.Alchemy );
							else
							{
								from.SendMessage(String.Format("Você pagou {0} de ouro.", toConsume));
								examine.NotIdentified = false;
								m_Vendor.SayTo(from, "Isso vale cerca de " + examine.CoinPrice + " de ouro." );
							}

							m_Vendor.CoinPurse += toConsume;
							m_Vendor.InvalidateProperties();

							if ( BeggingPose( from ) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) )
								Titles.AwardKarma( from, -BeggingKarma( from ), true );
								// DO ANY KARMA LOSS
						}
						else
						{
							m_Vendor.SayTo(from, "Custaria {0} de ouro para eu examinar isso.", toConsume);
							from.SendMessage("Você não tem ouro suficiente.");
						}
					}
					else
					{
						m_Vendor.SayTo(from, "Não posso identificar isso.");
					}
				}
				else
					m_Vendor.SayTo(from, "Não posso identificar isso.");
            }
        }

		private class ChargeTarget : Target
		{
            private BaseVendor m_Vendor;
			private int m_Cost;

            public ChargeTarget( BaseVendor vendor, int cost ) : base(12, false, TargetFlags.None)
            {
                m_Vendor = vendor;
				m_Cost = cost;
            }

            protected override void OnTarget(Mobile from, object targeted)
			{
				if ( targeted is MagicRuneBag )
				{
					m_Vendor.SayTo( from, "Coloque algum ouro na bolsa e eu a recarregarei por esse valor." );
					return;
				}
				else if ( targeted is Item && ((Item)targeted).Enchanted != MagicSpell.None && ((Item)targeted).EnchantUsesMax > 0 )
				{
					Item rep = (Item)targeted;
					Container pack = from.Backpack;

					m_Cost = m_Cost * SpellItems.GetLevel( (int)(rep.Enchanted) );

					int repaired = 0;
					int worn = rep.EnchantUsesMax - rep.EnchantUses;
					int spent = 0;

					if ( worn < 1 )
					{
						m_Vendor.SayTo( from, "Isso não requer meus serviços." );
						return;
					}

					while ( worn > 0 )
					{
						worn--;
						if ( pack.ConsumeTotal( typeof( Gold ), m_Cost ) )
						{
							spent += m_Cost;
							repaired++;
						}
						else
							worn = 0;

						if ( ( repaired + rep.EnchantUses ) >= rep.EnchantUsesMax )
							worn = 0;
					}

					if ( spent < 1 )
					{
						m_Vendor.SayTo( from, "Você precisará trazer mais ouro se quiser que eu recarregue isso." );
					}
					else
					{
						if ( repaired < 1 )
						{
							m_Vendor.SayTo( from, "Não posso recarregar isso." );
							return;
						}

						if ( repaired == 1 )
							m_Vendor.SayTo( from, "Recarreguei 1 vez por " + spent + " de ouro." );
						else
							m_Vendor.SayTo( from, "Recarreguei " + repaired + " vezes por " + spent + " de ouro." );

						rep.EnchantUses += repaired;
                        Effects.PlaySound(from.Location, from.Map, 0x5C1);

						m_Vendor.CoinPurse += spent;
						m_Vendor.InvalidateProperties();

						from.SendMessage( String.Format( "Você pagou {0} de ouro.", spent ) );
						if ( BeggingPose( from ) > 0 && !(typeof( PlayerVendor ) == this.GetType()) && !(typeof( PlayerBarkeeper ) == this.GetType()) )
							Titles.AwardKarma( from, -BeggingKarma( from ), true );
							// DO ANY KARMA LOSS
					}
				}
				else
					m_Vendor.SayTo(from, "Isso não requer meus serviços.");
            }
        }
	}
}

namespace Server.ContextMenus
{
	public class SetupShoppeEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;
		private Mobile m_From;
		
		public SetupShoppeEntry( Mobile from, BaseVendor vendor ) : base( 6164, 12 )
		{
			m_Vendor = vendor;
			m_From = from;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			if ( !m_From.HasGump( typeof( Server.Items.ExplainShopped ) ) )
			{
				m_From.SendGump( new Server.Items.ExplainShopped( m_From, m_Vendor ) );
			}
		}
	}

	public class RepairEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;
		private Mobile m_From;

		public RepairEntry( Mobile from, BaseVendor vendor ) : base( 283, 12 )
		{
			m_Vendor = vendor;
			m_From = from;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_Vendor.BeginRepair( m_From );
		}
	}

	public class IdentifyEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;
		private Mobile m_From;

		public IdentifyEntry( Mobile from, BaseVendor vendor ) : base( 282, 12 )
		{
			m_Vendor = vendor;
			m_From = from;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_Vendor.BeginIdentify( m_From );
		}
	}

	public class RechargeEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;
		private Mobile m_From;

		public RechargeEntry( Mobile from, BaseVendor vendor ) : base( 284, 12 )
		{
			m_Vendor = vendor;
			m_From = from;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_Vendor.BeginRecharge( m_From );
		}
	}

	public class VendorBuyEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;

		public VendorBuyEntry( Mobile from, BaseVendor vendor ) : base( 6103, 12 )
		{
			m_Vendor = vendor;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_Vendor.VendorBuy( this.Owner.From );
		}
	}

	public class VendorSellEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;

		public VendorSellEntry( Mobile from, BaseVendor vendor ) : base( 6104, 12 )
		{
			m_Vendor = vendor;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_Vendor.VendorSell( this.Owner.From );
		}
	}

	public class BlackMarketEntry : ContextMenuEntry
	{
		private BaseVendor m_Vendor;
		private Mobile m_From;

		public BlackMarketEntry( Mobile from, BaseVendor vendor ) : base( 6094, 12 )
		{
			m_Vendor = vendor;
			m_From = from;
			Enabled = vendor.CheckVendorAccess( from );
		}

		public override void OnClick()
		{
			m_From.CloseGump( typeof( BlackMarketGump ) );

			if ( m_Vendor.BankBox.TotalItems > 0 )
			{
				m_From.SendGump( new BlackMarketGump( m_Vendor, m_From, null, 1, 0 ) );
				m_Vendor.SayTo( m_From, "Aqui estão alguns itens especiais que tenho." );
			}
			else
				m_Vendor.SayTo( m_From, "Desculpe, mas não tenho nada disponível." );

		}
	}
}

namespace Server
{
	public interface IShopSellInfo
	{
		//get display name for an item
		string GetNameFor( Item item );

		//get price for an item which the player is selling
		int GetSellPriceFor( Item item, int barter );

		//get price for an item which the player is buying
		int GetBuyPriceFor( Item item );

		//can we sell this item to this vendor?
		bool IsSellable( Item item );

		//What do we sell?
		Type[] Types { get; }

		//does the vendor resell this item?
		bool IsResellable( Item item );
	}

	public interface IBuyItemInfo
	{
		//get a new instance of an object (we just bought it)
		IEntity GetEntity();

		int ControlSlots { get; }

		int PriceScalar { get; set; }

		//display price of the item
		int Price { get; }

		//display name of the item
		string Name { get; }

		//display hue
		int Hue { get; }

		//display id
		int ItemID { get; }

		//amount in stock
		int Amount { get; set; }

		//max amount in stock
		int MaxAmount { get; }

		//Attempt to restock with item, (return true if restock sucessful)
		bool Restock( Item item, int amount );

		//called when its time for the whole shop to restock
		void OnRestock();
	}
}
