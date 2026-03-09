using System; 
using System.Collections; 
using Server.Items; 
using Server.Misc; 
using Server.Network; 
using Server.Mobiles; 
using Server.Multis; 
using Server.Gumps;
using Server.Targeting;
using Server.ContextMenus;
using System.Collections.Generic;

namespace Server.Items 
{ 
	public class StableStone : Item
	{ 
		public override string DefaultDescription{ get{ return "Postes de amarração só podem ser usados por grão-mestres em acampamento. Depois de colocá-lo em sua casa, você pode usá-lo para estabular seus animais de estimação em vez de fazê-lo no estábulo."; } }

		[Constructable] 
		public StableStone() : base( 0x14E7 ) 
		{ 
			Name = "hitching post"; 
			Weight = 20.0;
		} 

		public StableStone( Serial serial ) : base( serial ) 
		{ 
		}

		private class StableEntry : ContextMenuEntry
		{
			private StableStone m_Trainer;
			private Mobile m_From;

			public StableEntry( StableStone trainer, Mobile from ) : base( 6126, 2 )
			{
				m_Trainer = trainer;
				m_From = from;
			}

			public override void OnClick()
			{
				m_Trainer.BeginStable( m_From );
			}
		}

		public class ClaimingGumpEntry : ContextMenuEntry
		{
			private StableStone m_Trainer;
			private Mobile m_From;
			
			public ClaimingGumpEntry( StableStone trainer, Mobile from ) : base( 6165, 3 )
			{
				m_Trainer = trainer;
				m_From = from;
			}

			public override void OnClick()
			{
			    if( !( m_From is PlayerMobile ) )
				return;
				
				PlayerMobile mobile = (PlayerMobile) m_From;
				{
					m_Trainer.BeginClaimList( m_From );
				}
            }
        }

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list  )
		{
			base.GetContextMenuEntries( from, list );
			if ( from.Alive )
			{
				list.Add( new StableEntry( this, from ) );
				list.Add( new ClaimingGumpEntry( this, from ) );

				if ( from.Stabled.Count > 0 )
					list.Add( new ClaimAllEntry( this, from ) );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.ItemID == 0x14E7 ){ this.ItemID = 0x14E8; } else { this.ItemID = 0x14E7; }
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Estabule Seus Animais Em Sua Casa");
			list.Add( 1049644, "Para Grão-Mestres Em Acampamento");
        } 

		private class StableTarget : Target
		{
			private StableStone m_Trainer;

			public StableTarget( StableStone trainer ) : base( 12, false, TargetFlags.None )
			{
				m_Trainer = trainer;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseCreature )
					m_Trainer.EndStable( from, (BaseCreature)targeted );
				else
					from.SendMessage ("Você não pode estabular isso!");
			}
		}

		public void BeginStable( Mobile from )
		{
			if ( Deleted || !from.CheckAlive() )
				return;

            if (this.Movable)
			{
				from.SendMessage("Isso deve estar travado em uma casa para usar!");
			}
			else if ( from.Skills[SkillName.Camping].Base < 100 )
			{
				from.SendMessage ("Somente grão-mestres em acampamento podem estabular animais em casa!");
			}
			else if ( from.Stabled.Count >= Server.Mobiles.AnimalTrainer.GetMaxStabled( from ) )
			{
				from.SendMessage ("Você não pode estabular isso! Você tem muitos animais estabulados, atingiu sua quantidade máxima");
			}
			else
			{
				from.SendMessage ("O poste de amarração requer 30 moedas de ouro por animal para cada semana do mundo real para manutenção!");
				from.SendMessage ("o ouro é automaticamente retirado de sua conta bancária");
				from.SendMessage ("Mire no animal que deseja estabular!");
				from.Target = new StableTarget( this );
			}
		}

		public void EndStable( Mobile from, BaseCreature pet )
		{
			if ( Deleted || !from.CheckAlive() )
				return;

			if ( !pet.Controlled || pet.ControlMaster != from )
			{
				from.SendMessage ("Esse não é seu animal de estimação!");
			}
			else if ( pet.IsDeadPet )
			{
				from.SendMessage ("Esse animal está morto e não pode ser estabulado!");
			}
			else if ( pet.Summoned )
			{
				from.SendMessage ("Você não pode estabular criaturas invocadas");
			}
			else if ( pet.Body.IsHuman )
			{
				from.SendMessage ("Esse não é seu animal de estimação!");
			}
			else if ( (pet is PackLlama || pet is PackHorse || pet is Beetle) && (pet.Backpack != null && pet.Backpack.Items.Count > 0) )
			{
				from.SendMessage ("Você precisa descarregar o animal de carga antes de estabulá-lo!");
			}
			else if ( pet.Combatant != null && pet.InRange( pet.Combatant, 12 ) && pet.Map == pet.Combatant.Map )
			{
				from.SendMessage ("Seu animal parece estar ocupado no momento, tente novamente quando não estiver!");
			}
			else if ( from.Stabled.Count >= Server.Mobiles.AnimalTrainer.GetMaxStabled( from ) )
			{
				from.SendMessage ("Você tem muitos animais no estábulo!");
			}
			else
			{
				Container bank = from.BankBox;

				if ( bank != null && bank.ConsumeTotal( typeof( Gold ), 30 ) )
				{
					pet.Language = null;
					pet.ControlTarget = null;
					pet.ControlOrder = OrderType.Stay;
					pet.Internalize();

					pet.SetControlMaster( null );
					pet.SummonMaster = null;

					pet.IsStabled = true;
					from.Stabled.Add( pet );
					from.SendMessage ("Seu animal foi estabulado. Você pode recuperá-lo dizendo 'claim'. Em uma semana do mundo real,");
					from.SendMessage ("se seu animal não for reclamado até lá, ele desaparecerá se não for reclamado!");
				}
				else
				{
					from.SendMessage ("Você não tem os fundos bancários necessários para fazer isso!");
				}
			}
		}

		private class ClaimListGump : Gump
		{
			private StableStone m_Trainer;
			private Mobile m_From;
			private ArrayList m_List;

			public ClaimListGump( StableStone trainer, Mobile from, ArrayList list ) : base( 50, 50 )
			{
				from.SendSound( 0x0EB ); 
				string color = "#bfad7d";

				m_Trainer = trainer;
				m_From = from;
				m_List = list;

				from.CloseGump( typeof( ClaimListGump ) );

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(0, 0, 9590, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddHtml( 12, 12, 376, 20, @"<BODY><BASEFONT Color=" + color + ">ANIMAIS NO ESTÁBULO</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(410, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

				int y = 15;

				for ( int i = 0; i < list.Count; ++i )
				{
					BaseCreature pet = list[i] as BaseCreature;

					if ( pet == null || pet.Deleted )
						continue;

					y = y + 30;

					AddButton(13, y, 4005, 4005, (i+1), GumpButtonType.Reply, 0);
					AddHtml( 50, y, 349, 20, @"<BODY><BASEFONT Color=" + color + ">" + pet.Name + "</BASEFONT></BODY>", (bool)false, (bool)false);
				}
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				m_From.SendSound( 0x0F2 ); 
				int index = info.ButtonID - 1;

				if ( index >= 0 && index < m_List.Count )
					m_Trainer.EndClaimList( m_From, m_List[index] as BaseCreature );
			}
		}
		
		public void BeginClaimList( Mobile from )
		{
			if ( Deleted || !from.CheckAlive() )
				return;

			ArrayList list = new ArrayList();

			for ( int i = 0; i < from.Stabled.Count; ++i )
			{
				BaseCreature pet = from.Stabled[i] as BaseCreature;

				if ( pet == null || pet.Deleted )
				{
					pet.IsStabled = false;
					from.Stabled.RemoveAt( i );
					--i;
					continue;
				}

				list.Add( pet );
			}

            if (this.Movable)
				from.SendMessage("Isso deve estar travado em uma casa para usar!");
			else if ( from.Skills[SkillName.Camping].Base < 100 )
				from.SendMessage ("Somente grão-mestres em acampamento podem estabular animais em casa!");
			else if ( list.Count > 0 )
				from.SendGump( new ClaimListGump( this, from, list ) );
			else
				from.SendMessage ("Mas não tenho nenhum animal estabulado comigo no momento!");
		}

		public void EndClaimList( Mobile from, BaseCreature pet )
		{
			if ( pet == null || pet.Deleted || from.Map != this.Map || !from.Stabled.Contains( pet ) || !from.CheckAlive() )
				return;

			if ( (from.Followers + pet.ControlSlots) <= from.FollowersMax )
			{
				pet.SetControlMaster( from );

				if ( pet.Summoned )
					pet.SummonMaster = from;

				pet.ControlTarget = from;
				pet.ControlOrder = OrderType.Follow;

				pet.MoveToWorld( from.Location, from.Map );

				pet.IsStabled = false;
				from.Stabled.Remove( pet );
				from.SendMessage ("Aqui está...");
			}
			else
			{
				from.SendMessage ( "Esse animal permaneceu no estábulo porque você tem muitos seguidores");
			}
		}
      	
		public override bool HandlesOnSpeech{ get{ return true; } } 

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( ( !e.Handled && e.HasKeyword( 0x0008 ) ) && ( e.Mobile.InRange( this, 2 ) ) )
			{
				e.Handled = true;
				BeginStable( e.Mobile );
			}
			else if ( ( !e.Handled && e.HasKeyword( 0x0009 ) ) && ( e.Mobile.InRange( this, 2 ) ) )
			{
				e.Handled = true;

				if ( !Insensitive.Equals( e.Speech, "Claim" ) )
					BeginClaimList( e.Mobile );
				else
					BeginClaimList( e.Mobile );
			}
			else
			{
				base.OnSpeech( e );
			}
		}

		private class ClaimAllEntry : ContextMenuEntry
		{
			private StableStone m_Trainer;
			private Mobile m_From;

			public ClaimAllEntry( StableStone trainer, Mobile from ) : base( 6127, 12 )
			{
				m_Trainer = trainer;
				m_From = from;
			}

			public override void OnClick()
			{
				m_Trainer.Claim( m_From );
			}
		}

		public void Claim( Mobile from )
		{
			Claim( from, null );
		}

		public void Claim( Mobile from, string petName )
		{
			if ( Deleted || !from.CheckAlive() )
				return;

			bool claimed = false;
			int stabled = 0;
			
			bool claimByName = ( petName != null );

			for ( int i = 0; i < from.Stabled.Count; ++i )
			{
				BaseCreature pet = from.Stabled[i] as BaseCreature;

				if ( pet == null || pet.Deleted )
				{
					pet.IsStabled = false;
					from.Stabled.RemoveAt( i );
					--i;
					continue;
				}

				++stabled;

				if ( claimByName && !Insensitive.Equals( pet.Name, petName ) )
					continue;

				if ( CanClaim( from, pet ) )
				{
					DoClaim( from, pet );

					from.Stabled.RemoveAt( i );
					--i;

					claimed = true;
				}
				else
				{
					from.SendMessage ("Esse animal permaneceu no estábulo porque você tem muitos seguidores");
				}
			}

			if ( claimed )
				from.SendMessage ("Aqui está...");
			else if ( stabled == 0 )
				from.SendMessage ("Mas não tenho nenhum animal estabulado comigo no momento!");
			else if ( claimByName )
				BeginClaimList( from );
		}

		public bool CanClaim( Mobile from, BaseCreature pet )
		{
			return ((from.Followers + pet.ControlSlots) <= from.FollowersMax);
		}

		private void DoClaim( Mobile from, BaseCreature pet )
		{
			pet.SetControlMaster( from );

			if ( pet.Summoned )
				pet.SummonMaster = from;

			pet.Language = null;
			pet.ControlTarget = from;
			pet.ControlOrder = OrderType.Follow;

			pet.MoveToWorld( from.Location, from.Map );

			pet.IsStabled = false;

			if ( Core.SE )
				pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully Happy
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