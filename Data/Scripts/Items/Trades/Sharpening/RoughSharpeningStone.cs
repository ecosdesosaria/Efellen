using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
	public class RoughSharpeningStone : Item
	{
		private int i_Uses;
		[CommandProperty( AccessLevel.GameMaster )]
		public int Uses { get { return i_Uses; } set { i_Uses = value; InvalidateProperties(); } }

		[Constructable] 
		public RoughSharpeningStone() : this( 5 )
		{
		}

		[Constructable] 
		public RoughSharpeningStone( int uses ) : base( 0x1F14 ) 
		{ 
			Weight = 1.0;
			i_Uses = uses;
			Hue = 0x38C;
			Name = "Rough Sharpening Stone";
		} 

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060584, "{0}\t{1}", i_Uses.ToString(), "Uses" );
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Pode Aumentar Ligeiramente o Dano de uma Arma de Swordsmanship");
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				if ( Uses < 1 )
				{
					Delete();
					from.SendMessage(32, "Isto não tem cargas, então acabou!");
				}
				from.SendMessage("Qual arma você quer tentar afiar?");
				from.Target = new RoughSharpeningStoneTarget(this);
			}
			else
				from.SendMessage("Isto deve estar em sua mochila para usar.");
		}
		
		public void Sharpening(Mobile from, object o)
		{
			if ( o is Item )
			{
				if ( !((Item)o).IsChildOf( from.Backpack ) )
				{
					from.SendMessage(32, "Isto deve estar em sua mochila para afiar");
				}
				else if (o is BaseSword && ((BaseSword)o).IsChildOf(from.Backpack))
				{
					BaseSword weap = o as BaseSword;
					int i_DI = weap.Attributes.WeaponDamage;
					if (weap.Quality == WeaponQuality.Exceptional)
						i_DI += 15;
					if (i_DI >= 60)
					{
						from.SendMessage(32, "Esta arma não pode ser mais afiada");
						return;
					}
					else if (from.Skills[SkillName.Blacksmith].Value < 60.0)
						from.SendMessage(32, "Você precisa de pelo menos 60.0 em Ferraria para afiar armas com esta pedra");
					else if ( !Deleted )
					{
						int bonus = Utility.Random((int)(from.Skills[SkillName.Blacksmith].Value/20));
						if (bonus > 0)
						{
							if (60 < i_DI + bonus)
								bonus = 60 - i_DI;
							weap.Attributes.WeaponDamage += bonus;
							from.SendMessage(88, "Você afiou a arma com {0} de aumento de dano", bonus);
						}
						else
							from.SendMessage(32, "Você falhou ao afiar a arma");
						if (Uses <= 1)
						{
							from.SendMessage(32, "Você usou a pedra de afiação");
							Delete();
						}
						else
						{
							--Uses;
							from.SendMessage(32, "Você tem {0} usos restantes", Uses);
						}
					}
				}
				else if (o is BaseKnife && ((BaseKnife)o).IsChildOf(from.Backpack))
				{
					BaseKnife weap = o as BaseKnife;
					int i_DI = weap.Attributes.WeaponDamage;
					if (weap.Quality == WeaponQuality.Exceptional)
						i_DI += 15;
					if (i_DI >= 60)
					{
						from.SendMessage(32, "Esta arma não pode ser mais afiada");
						return;
					}
					else if (from.Skills[SkillName.Blacksmith].Value < 60.0)
						from.SendMessage(32, "Você precisa de pelo menos 60.0 em Ferraria para afiar armas com esta pedra");
					else if ( !Deleted )
					{
						int bonus = Utility.Random((int)(from.Skills[SkillName.Blacksmith].Value/20));
						if (bonus > 0)
						{
							if (60 < i_DI + bonus)
								bonus = 60 - i_DI;
							weap.Attributes.WeaponDamage += bonus;
							from.SendMessage(88, "Você afiou a arma com {0} de aumento de dano", bonus);
						}
						else
							from.SendMessage(32, "Você falhou ao afiar a arma");
						if (Uses <= 1)
						{
							from.SendMessage(32, "Você usou a pedra de afiação");
							Delete();
						}
						else
						{
							--Uses;
							from.SendMessage(32, "Você tem {0} usos restantes", Uses);
						}
					}
				}
				else if (o is BaseAxe && ((BaseAxe)o).IsChildOf(from.Backpack))
				{
					BaseAxe weap = o as BaseAxe;
					int i_DI = weap.Attributes.WeaponDamage;
					if (weap.Quality == WeaponQuality.Exceptional)
						i_DI += 15;
					if (i_DI >= 60)
					{
						from.SendMessage(32, "Esta arma não pode ser mais afiada");
						return;
					}
					else if (from.Skills[SkillName.Blacksmith].Value < 60.0)
						from.SendMessage(32, "Você precisa de pelo menos 60.0 em Ferraria para afiar armas com esta pedra");
					else if ( !Deleted )
					{
						int bonus = Utility.Random((int)(from.Skills[SkillName.Blacksmith].Value/20));
						if (bonus > 0)
						{
							if (60 < i_DI + bonus)
								bonus = 60 - i_DI;
							weap.Attributes.WeaponDamage += bonus;
							from.SendMessage(88, "Você afiou a arma com {0} de aumento de dano", bonus);
						}
						else
							from.SendMessage(32, "Você falhou ao afiar a arma");
						if (Uses <= 1)
						{
							from.SendMessage(32, "Você usou a pedra de afiação");
							Delete();
						}
						else
						{
							--Uses;
							from.SendMessage(32, "Você tem {0} usos restantes", Uses);
						}
					}
				}
				else
				{
					from.SendMessage(32, "Você só pode aprimorar armas de corte");
				}
			}
			else
			{
				from.SendMessage(32, "Você só pode aprimorar armas de corte");
			}
		}

		public RoughSharpeningStone( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) i_Uses );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			i_Uses = reader.ReadInt();
			if ( version == 0 ) { Serial sr_Owner = reader.ReadInt(); }
		}
	}

	public class RoughSharpeningStoneTarget : Target
	{
		private RoughSharpeningStone sb_Blade;

		public RoughSharpeningStoneTarget(RoughSharpeningStone blade) : base( 18, false, TargetFlags.None )
		{
			sb_Blade = blade;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (sb_Blade.Deleted)
				return;

			sb_Blade.Sharpening(from, targeted);
		}
	}
}