using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;
using System.Collections;
using System.Collections.Generic;
namespace Server.Items
{
	public class Artifact_RodOfResurrection : GiftScepter
	{
		[Constructable]
		public Artifact_RodOfResurrection()
		{
			Name = "Rod Of Resurrection";
			Hue = 0x4AC;
			Attributes.Luck = 125;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Traz os mortos de volta à vida" );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( Parent != from )
			{
				from.SendMessage( "Você precisa estar segurando a varinha para ressuscitar." );
			}
			else
			{
				from.Target = new InternalTarget( from, this );
				from.SendMessage( "Quem você gostaria de ressuscitar?" );
			}
			return;
		}

        public void Target(Mobile m, Mobile from, Artifact_RodOfResurrection rod)
		{
			if (!from.CanSee(m))
			{
				from.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (!from.Alive)
			{
				from.SendLocalizedMessage(501040); // The resurrecter must be alive.
			}
			else if (m == from)
			{
				SoulOrb iOrb = new SoulOrb();

				// Check if backpack is full
				if (m.Backpack == null || !m.Backpack.CheckHold(m, iOrb, false, true, 0, 0))
				{
					iOrb.Delete();
					m.SendMessage("You cannot cast that spell because your inventory is full.");
					return;
				}

				// Remove existing orb(s) from the caster
				ArrayList targets = new ArrayList();
				foreach (Item item in World.Items.Values)
					if (item is SoulOrb)
					{
						SoulOrb myOrb = (SoulOrb)item;
						if (myOrb.m_Owner == m)
						{
							targets.Add(item);
						}
					}
				for (int i = 0; i < targets.Count; ++i)
				{
					Item item = (Item)targets[i];
					item.Delete();
				}

				// Create new orb
				m.PlaySound(0x214);
				m.FixedEffect(0x3039, 10, 16, 1066, 0);
				m.SendMessage("You summon a magical orb of resurrection to protect your soul.");
				iOrb.m_Owner = m;
				iOrb.Hue = 0xB17;
				iOrb.Name = "magical orb of resurrection";
				m.AddToBackpack(iOrb);
				Server.Items.SoulOrb.OnSummoned(m, iOrb);
			}
			else if (m.Alive)
			{
				from.SendLocalizedMessage(501041); // Target is not dead.
			}
			else if (!from.InRange(m, 2))
			{
				from.SendLocalizedMessage(501042); // Target is not close enough.
			}
			else if (m.Map == null || !m.Map.CanFit(m.Location, 16, false, false))
			{
				from.SendLocalizedMessage(501042); // Target can not be resurrected at that location.
				m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
			}
			else if (m is PlayerMobile)
			{
				m.PlaySound(0x214);
				m.FixedEffect(0x376A, 10, 16);

				m.CloseGump(typeof(ResurrectGump));
				m.SendGump(new ResurrectGump(m, from));
			}
			else if (m is BaseCreature)
			{
				BaseCreature pet = (BaseCreature)m;
				Mobile master = pet.GetMaster();
 
                m.PlaySound( 0x214 );
                m.FixedEffect( 0x376A, 10, 16 );
 
                master.CloseGump(typeof(PetResurrectGump));
                master.SendGump(new PetResurrectGump(master, pet));
            }
        }

        public void ItemTarget( Item hench, Mobile from, Artifact_RodOfResurrection rod )
        {
			if ( hench is HenchmanFighterItem )
			{
				HenchmanFighterItem friend = (HenchmanFighterItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "fighter henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					from.PlaySound( 0x214 );
				}
				else
				{
					from.SendMessage("Eles não estão mortos.");
				}
			}
			else if ( hench is HenchmanWizardItem )
			{
				HenchmanWizardItem friend = (HenchmanWizardItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "wizard henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					from.PlaySound( 0x214 );
				}
				else
				{
					from.SendMessage("Eles não estão mortos.");
				}
			}
			else if ( hench is HenchmanArcherItem )
			{
				HenchmanArcherItem friend = (HenchmanArcherItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "archer henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					from.PlaySound( 0x214 );
				}
				else
				{
					from.SendMessage("Eles não estão mortos.");
				}
			}
			else if (hench is HenchmanMonsterItem )
			{
				HenchmanMonsterItem friend = (HenchmanMonsterItem)hench;

				if ( friend.HenchDead > 0 )
				{
					friend.Name = "creature henchman";
					friend.HenchDead = 0;
					friend.InvalidateProperties();
					from.PlaySound( 0x214 );
				}
				else
				{
					from.SendMessage("Eles não estão mortos.");
				}
			}
			else
			{
				from.SendMessage("Este feitiço parece não ter funcionado.");
			}
		}
 
        private class InternalTarget : Target
        {
            private Mobile m_Owner;
            private Artifact_RodOfResurrection m_Rod;
 
            public InternalTarget( Mobile owner, Artifact_RodOfResurrection rod ) : base( 1, false, TargetFlags.Beneficial )
            {
                m_Owner = owner;
				m_Rod = rod;
            }
 
            protected override void OnTarget( Mobile from, object o )
            {
                if ( o is Mobile )
                {
                    m_Rod.Target( (Mobile)o, from, m_Rod );
                }
                else if ( o is Item )
                {
                    m_Rod.ItemTarget( (Item)o, from, m_Rod );
                }
            }
        }

		public Artifact_RodOfResurrection( Serial serial ) : base( serial )
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
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}
