using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Misc;
using System.Collections;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class ArmsLore
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.ArmsLore].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse(Mobile m)
		{
			m.Target = new InternalTarget();

			m.SendLocalizedMessage( 500349 ); // What item do you wish to get information about?

			return TimeSpan.FromSeconds( 1.0 );
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target
		{
			public InternalTarget() : base( 2, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
			   if (targeted is Item)
			   {
			       Item examine = (Item)targeted;
			       int identified = RelicIDHelper.TryRecursiveIdentify(from, examine, IDSkill.ArmsLore, SkillName.ArmsLore);

			       if (examine is Container)
					{
						if (identified == 0)
							from.SendMessage("Não há nada neste recipiente que exija Conhecimento de Armas para identificar.");
						else
							from.SendMessage("Você inspeciona o conteúdo do recipiente usando sua habilidade de Conhecimento de Armas.");
					}
					else
					{
						if (identified == 0)
							from.SendMessage("Esse item não pode ser identificado com Conhecimento de Armas.");
					}
			   }
			}
		}
	}
}