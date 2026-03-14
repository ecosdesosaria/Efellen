using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
	public class RoughEnhancementStone : Item
	{
		private int i_Uses;
		[CommandProperty( AccessLevel.GameMaster )]
		public int Uses { get { return i_Uses; } set { i_Uses = value; InvalidateProperties(); } }

		[Constructable] 
		public RoughEnhancementStone() : this( 5 )
		{
		}

		[Constructable] 
		public RoughEnhancementStone( int uses ) : base( 0x1F14 ) 
		{ 
			Weight = 1.0;
			i_Uses = uses;
			Hue = 0x38C;
			Name = "Rough Enhancement Stone";
		} 

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060584, "{0}\t{1}", i_Uses.ToString(), "Uses" );
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
				from.SendMessage("Qual arma você quer tentar aprimorar?");
				from.Target = new RoughEnhancementStoneTarget(this);
			}
			else
				from.SendMessage("Isto deve estar em sua mochila para usar.");
		}
		
        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Pode Aumentar Maravilhosamente o Dano de uma Arma");
        }

		public void Enhancement(Mobile from, object o)
		{
			if ( o is Item )
			{
				if ( !((Item)o).IsChildOf( from.Backpack ) )
				{
					from.SendMessage(32, "Isto deve estar em sua mochila para aprimorar");
				}
				else if (o is BaseWeapon && ((BaseWeapon)o).IsChildOf(from.Backpack))
				{
					BaseWeapon weap = o as BaseWeapon;
					int i_DI = weap.Attributes.WeaponDamage;
					if (weap.Quality == WeaponQuality.Exceptional)
						i_DI += 15;
					if (i_DI >= 60)
					{
						from.SendMessage(32, "Esta arma não pode ser aprimorada mais");
						return;
					}
					else if (from.Skills[SkillName.Blacksmith].Value < 60.0)
						from.SendMessage(32, "Você precisa de pelo menos 60.0 em Ferraria para aprimorar armas com esta pedra");
					else if ( !Deleted )
					{
						int bonus = Utility.Random((int)(from.Skills[SkillName.Blacksmith].Value/10));
						if (bonus > 0)
						{
							if (60 < i_DI + bonus)
								bonus = 60 - i_DI;
							weap.Attributes.WeaponDamage += bonus;
							from.SendMessage(88, "Você aprimorou a arma com {0} de aumento de dano", bonus);
						}
						else
							from.SendMessage(32, "Você falhou ao aprimorar a arma");
						if (Uses <= 1)
						{
							from.SendMessage(32, "Você usou a pedra de aprimoramento");
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
					from.SendMessage(32, "Você não pode aprimorar esse item.");
				}
			}
			else
			{
				from.SendMessage(32, "Você não pode aprimorar isso.");
			}
		}

		public RoughEnhancementStone( Serial serial ) : base( serial )
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

	public class RoughEnhancementStoneTarget : Target
	{
		private RoughEnhancementStone sb_Weapon;

		public RoughEnhancementStoneTarget(RoughEnhancementStone weapon) : base( 18, false, TargetFlags.None )
		{
			sb_Weapon = weapon;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (sb_Weapon.Deleted)
				return;

			sb_Weapon.Enhancement(from, targeted);
		}
	}
}