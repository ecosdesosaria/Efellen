using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Misc;
using System.Collections;
using Server.Targeting;

namespace Server.Items
{
	public class Tasting
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.Tasting].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.Target = new InternalTarget();

			m.SendLocalizedMessage( 502807 ); // What would you like to taste?

			return TimeSpan.FromSeconds( 1.0 );
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target
		{
			public InternalTarget() :  base ( 2, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					from.SendLocalizedMessage( 502816 ); // You feel that such an action would be inappropriate.
				}
				else if ( targeted is Food )
				{
					Food food = (Food)targeted;

					if ( food.Poison != null )
					{
						if ( from.CheckTargetSkill( SkillName.Tasting, food, 0, 125 ) )
						{
							// It appears to have a bitter taste of poison.
							if ( food.Poison == Poison.Lesser )
								from.SendLocalizedMessage( 1041579 );
							else if ( food.Poison == Poison.Regular )
								from.SendLocalizedMessage( 1041580 );
							else if ( food.Poison == Poison.Greater )
								from.SendLocalizedMessage( 1041581 );
							else if ( food.Poison == Poison.Deadly )
								from.SendLocalizedMessage( 1041582 );
							else if ( food.Poison == Poison.Lethal )
								from.SendLocalizedMessage( 1041583 );
							else
								from.SendLocalizedMessage( 1010600 ); // You detect nothing unusual about this substance.
						}
						else
						{
							food.Eat( from, true );
							from.SendMessage( "Você mordeu um pedaço grande demais!" );
						}
					}
					else
					{
						from.SendMessage( "Esta comida parece segura para comer." );
					}
				}
				else if ( targeted is BaseBeverage )
				{
					BaseBeverage drink = (BaseBeverage)targeted;

					if ( drink.Poison != null )
					{
						if ( from.CheckTargetSkill( SkillName.Tasting, drink, 0, 125 ) )
						{
							// It appears to have a bitter taste of poison.
							if ( drink.Poison == Poison.Lesser )
								from.SendLocalizedMessage( 1041579 );
							else if ( drink.Poison == Poison.Regular )
								from.SendLocalizedMessage( 1041580 );
							else if ( drink.Poison == Poison.Greater )
								from.SendLocalizedMessage( 1041581 );
							else if ( drink.Poison == Poison.Deadly )
								from.SendLocalizedMessage( 1041582 );
							else if ( drink.Poison == Poison.Lethal )
								from.SendLocalizedMessage( 1041583 );
							else
								from.SendLocalizedMessage( 1010600 ); // You detect nothing unusual about this substance.
						}
						else
						{
							from.SendMessage( "Isso parece tentador, mas você não tem certeza se é seguro beber." );
						}
					}
					else
					{
						from.SendMessage( "Este líquido parece seguro para beber." );
					}
				}
				else if ( targeted is Item )
				{
					Item examine = (Item)targeted;
					int identified = RelicIDHelper.TryRecursiveIdentify(from, examine, IDSkill.Tasting, SkillName.Tasting);

       				if (examine is Container)
					{
						if (identified == 0)
							from.SendMessage("Não há nada neste recipiente que exija Degustação para identificar.");
						else
							from.SendMessage("Você inspeciona o conteúdo do recipiente usando sua habilidade de Degustação.");
					}
					else
					{
						if (identified == 0)
							from.SendMessage("Esse item não pode ser identificado com Degustação.");
					}
				}
			}
		}
	}
}