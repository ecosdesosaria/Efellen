//================================================//
// Created by dracana				  			  //
// Desc: Command for leveling weapons             //
//================================================//
using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;
using Server.Targeting;
using Server.Commands;

namespace Server.Commands
{
	public class ixpCommand
	{
	        public static void Initialize()
	        {
	            CommandSystem.Register("itemexp", AccessLevel.Player, new CommandEventHandler(ixp_OnCommand));
	            CommandSystem.Register("ixp", AccessLevel.Player, new CommandEventHandler(ixp_OnCommand));
	        }

	        [Usage("itemexp")]
	        [Description("Item Experience and Points.")]

	        public static void ixp_OnCommand(CommandEventArgs e)
	        {
			e.Mobile.SendMessage( "Selecione um item para ver a experiência" );
			e.Mobile.Target = new InternalTarget( e.Mobile );
	        }

		private class InternalTarget : Target
		{
			private Mobile m_From;

			public InternalTarget( Mobile from ) :  base ( 2, false, TargetFlags.None )
			{
				m_From = from;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
                if ( targeted is Item)
                {
                    Item item = (Item)targeted;

                    if ( item.Parent != from && item.Parent != from.Backpack )
                    {
                    	from.SendMessage("O item deve estar em sua mochila ou equipado!");
                    	return;
					}

				    if ( item is ILevelable)
				    {
                        if ( item is BaseWeapon || item is BaseArmor || item is BaseTrinket || item is BaseClothing )
                        {
                            from.SendGump( new ItemExperienceGump( from, item, AttributeCategory.Melee ) );
						}
                        else
                        {
                            from.SendMessage("Esse não é um item de nível válido");
                        }
					}
					else
					{
						from.SendMessage("Esse item não pode subir de nível!");
					}
				}
				else
				{
					from.SendMessage("Esse não é um item válido!");
				}

			}
		}
	}
}