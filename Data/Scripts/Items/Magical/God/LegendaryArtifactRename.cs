using System;
using Server.Network;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Prompts;

namespace Server.Items
{
	public class LegendaryArtifactRename : Item
	{
		private int m_Charges;
		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get { return m_Charges; }
			set { m_Charges = value; InvalidateProperties(); }
		}

		public Mobile owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get{ return owner; } set{ owner = value; } }

		[Constructable]
		public LegendaryArtifactRename( Mobile from ) : base( 0xFB8 )
		{
			Name = "Legendary Branding Iron";
			m_Charges = 3;
			this.owner = from;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add("{0} Uses Left", Charges);
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties( list );
			list.Add( 1070722, "Rename Legendary Artefacts");
			if ( owner != null ){ list.Add( 1049644, "Belongs to " + owner.Name + "" ); }
        } 

		public override void OnDoubleClick( Mobile from )
		{
			if(!IsChildOf(from.Backpack)) from.SendMessage( "Isto deve estar em sua mochila para usá-lo." );
			else if ( this.owner != from  )
			{
				from.SendMessage( "Este não é seu ferro de marcar." );
				return;
			}
			else if ( m_Charges > 0)
			{
				from.SendMessage( "Escolha o artefato lendário que deseja marcar." );
				from.Target = new InternalTarget( this );
			}
			else
			{
				from.SendMessage( "Esta marca está sem usos." );
				this.Delete();
			}
		}
		private class InternalTarget : Target
		{
			private LegendaryArtifactRename m_LegendaryArtifactRename;
			private Item m_engtarg;

			public InternalTarget( LegendaryArtifactRename engrave ) : base( 1, false, TargetFlags.None )
			{
				m_LegendaryArtifactRename = engrave;
			}
			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is LegendaryArtifactRename )
				{
					LegendaryArtifactRename knife = targeted as LegendaryArtifactRename;
					if (knife != null)
					{
						int knifeuses = knife.Charges;
						m_LegendaryArtifactRename.Charges += knifeuses;
						knife.Delete();
					}
				}
				else if ( targeted is ILevelable )
				{
					m_engtarg = (Item)targeted;
					if(!m_engtarg.IsChildOf(from.Backpack)) from.SendMessage( "Isto deve estar em sua mochila para mudar seu nome." );
					else
					{
						from.SendMessage( "Qual nome você quer marcar no artefato lendário?" );
						m_LegendaryArtifactRename.Charges -= 1 ;
						m_LegendaryArtifactRename.InvalidateProperties();
						from.Prompt = new RenameContPrompt( m_engtarg );
					}
				}
				else from.SendMessage( "Você não pode marcar isso." );
			}
		}

		public LegendaryArtifactRename(Serial serial) : base(serial){}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( (int) m_Charges );
			writer.Write( (Mobile)owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Charges = reader.ReadInt();
			owner = reader.ReadMobile();
		}
	}
}

namespace Server.Prompts
{
	public class RenameContPrompt : Prompt
	{
		private Item m_engtarg;

		public RenameContPrompt( Item rcont )
		{
			m_engtarg = rcont;
		}
		public override void OnResponse( Mobile from, string text )
		{
			m_engtarg.Name = text;
			from.SendMessage( "Você marcou o artefato lendário." );
		}
	}
}