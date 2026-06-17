using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class WizardWand : BaseBashing
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.PsychicAttack; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ZapManaStrike; } }
		public override WeaponAbility ThirdAbility{ get{ return WeaponAbility.ElementalStrike; } }
		public override WeaponAbility FourthAbility{ get{ return WeaponAbility.MagicProtection; } }
		public override WeaponAbility FifthAbility{ get{ return WeaponAbility.MagicProtection2; } }

		public override int AosStrengthReq{ get{ return 20; } }
		public override int AosMinDamage{ get{ return 10; } }
		public override int AosMaxDamage{ get{ return 12; } }
		public override int AosSpeed{ get{ return 44; } }
		public override float MlSpeed{ get{ return 2.50f; } }

		public override int OldStrengthReq{ get{ return 10; } }
		public override int OldMinDamage{ get{ return 8; } }
		public override int OldMaxDamage{ get{ return 24; } }
		public override int OldSpeed{ get{ return 40; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }

		[Constructable]
		public WizardWand() : base( 0x13B4 )
		{
			Weight = 2.0;
			Attributes.SpellChanneling = 1;

			if ( ItemID == 0x13B4 )
			{
				string make = "Varinha";
				ItemID = Utility.RandomList( 0xDF2, 0xDF3, 0xDF4, 0xDF5 );

				if ( Utility.RandomMinMax( 1, 10 ) == 1 ) // 10% ARE SCEPTERS
				{
					Weight = 5.0;
					make = "Cetro";
					ItemID = Utility.RandomList( 0x26BC, 0x26C6 );
				}

				switch ( Utility.RandomMinMax( 0, 14 ) ) 
				{
					case 0: Name = make + " da Feitiçaria";			break;
					case 1: Name = make + " da Bruxaria";			break;
					case 2: Name = make + " do Mágico";		break;
					case 3: Name = make + " do Bruxo";		break;
					case 4: Name = make + " do Mago";			break;
					case 5: Name = make + " do Feiticeiro";		break;
					case 6: Name = make + " dos Magos";			break;
					case 7: Name = make + " dos Feiticeiros";			break;
					case 8: Name = make + " dos Mágicos";			break;
					case 9: Name = make + " dos Bruxos";			break;
					case 10: Name = make + " da Bruxa";			break;
					case 11: Name = make + " das Bruxas";			break;
					case 12: Name = make + " da Magia";			break;
					case 13: Name = make + " dos Magos";				break;
					case 14: Name = make + " dos Magos";			break;
				}
				if(Utility.RandomDouble() < 0.75)
				{
					int maxLowMana = MyServerSettings.LowerMana() > 40 ? 20 : MyServerSettings.LowerMana() / 2;
					Attributes.LowerManaCost = Utility.RandomMinMax( 1, maxLowMana );	
				} 
				else if (Utility.RandomDouble() < 0.50)
				{	
					int maxLowReagentCost = MyServerSettings.LowerReg() > 50 ? 25 : MyServerSettings.LowerReg() / 2;
					Attributes.LowerRegCost = Utility.RandomMinMax( 1, maxLowReagentCost );					
				}
				else
				{
					Attributes.RegenMana = Utility.RandomMinMax(4,6);						
				}
			}
		}

		public override bool OnEquip( Mobile from )
		{
			if (!BaseWeapon.WizardCheck( from ))
				return false;

			return base.OnEquip( from );
		}

		public WizardWand( Serial serial ) : base( serial )
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