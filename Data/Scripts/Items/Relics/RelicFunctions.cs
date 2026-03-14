using System;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;

namespace Server
{
	public interface IRelic
	{
	}

	public static class RelicIDHelper
		{
		    public static int TryRecursiveIdentify(Mobile from, Item item, IDSkill skillType, SkillName skillName)
		    {
		        int count = 0;

		        if (item == null)
		            return 0;

		        // Check NotIDSkill and NotIdentified properties
		        System.Reflection.PropertyInfo skillProp = item.GetType().GetProperty("NotIDSkill");
		        System.Reflection.PropertyInfo notIdProp = item.GetType().GetProperty("NotIdentified");

		        if (skillProp != null && notIdProp != null)
		        {
		            object skillVal = skillProp.GetValue(item, null);
		            object notIdVal = notIdProp.GetValue(item, null);

		            if (skillVal is IDSkill && ((IDSkill)skillVal) == skillType &&
		                notIdVal is bool && ((bool)notIdVal) == true)
		            {
		                try
		                {
		                    RelicFunctions.IDItem(from, from, item, skillName);
		                    count++;
		                }
		                catch (Exception ex)
		                {
		                    Console.WriteLine("Error identifying item: " + ex.Message);
		                }
		            }
		        }

		        // Recursively id items in containers
		        if (item is Container)
		        {
		            Container c = (Container)item;
		            ArrayList items = new ArrayList(c.Items);

		            for (int i = 0; i < items.Count; ++i)
		            {
		                Item sub = items[i] as Item;
		                if (sub != null)
		                    count += TryRecursiveIdentify(from, sub, skillType, skillName);
		            }
		        }
		        return count;
		    }
		}

	public class RelicFunctions
	{
		public static void IDItem( Mobile m, Mobile from, Item examine, SkillName skills )
		{
			BaseVendor vendor = null;

			if ( m is BaseVendor )
			{
				vendor = (BaseVendor)m;

				if ( !VendorCanID( examine.NotIDSource, m ) )
				{
					vendor.SayTo(from, "Não consigo identificar isso.");
					return;
				}
			}

			SkillName skill = SkillName.Mercantile;
			if ( examine.NotIDSkill == IDSkill.ArmsLore )
				skill = SkillName.ArmsLore;
			else if ( examine.NotIDSkill == IDSkill.Tasting )
				skill = SkillName.Tasting;

			if ( !examine.NotIdentified )
			{
				if ( !(examine is IRelic) && vendor == null && skills != SkillName.Mercantile && from.Skills[SkillName.Mercantile].Value > Utility.Random( 100 ) )
				{
					int gold = ItemInformation.GetBuysPrice( ItemInformation.GetInfo( examine.GetType() ), false, examine, false, false ) * examine.Amount;
					if ( gold > 0 )
					{
						if ( from.Skills[SkillName.Mercantile].Base < 50 && Utility.RandomBool() )
							from.CheckSkill( SkillName.Mercantile, 0, 100 );

						string estimate = "Você talvez consiga";
						switch ( Utility.RandomMinMax( 0, 10 ) )
						{
							case 0: estimate = "Isto poderia talvez render"; break;
							case 1: estimate = "Alguém poderia querer isto por"; break;
							case 2: estimate = "Isto pode valer"; break;
							case 3: estimate = "Vender isto te daria"; break;
							case 4: estimate = "Isto poderia valer"; break;
							case 5: estimate = "Você talvez venda por"; break;
							case 6: estimate = "Isto poderia ser vendido por cerca de"; break;
							case 7: estimate = "Alguém pode levar isto por"; break;
							case 8: estimate = "Isto poderia alcançar um preço de"; break;
							case 9: estimate = "Estes geralmente valem cerca de"; break;
						}
						from.SendMessage( "" + estimate + " " + gold + " ouro." );
						return;
					}
				}
				from.SendMessage( "Isso já está identificado." );
				return;
			}
			else if ( vendor == null && skill != skills )
			{
				from.SendMessage( "Essa não é a habilidade correta para identificar isso." );
				return;
			}
			else if ( vendor == null && examine.NotIDAttempts > 5 )
			{
				from.SendMessage( "Apenas um vendedor pode identificar este item agora, pois muitas tentativas foram feitas." );
				return;
			}

			if ( !examine.Movable && vendor == null )
				from.SendMessage( "Isso não pode ser movido, então você não pode identificá-lo." );
			else if ( !from.InRange( examine.GetWorldLocation(), 3 ) && vendor == null )
				from.SendMessage( "Você precisará se aproximar para identificar isso." );
			else if ( !(examine.IsChildOf( from.Backpack )) && vendor == null ) 
				from.SendMessage( "Isto deve estar em sua mochila para identificar." );
			else if ( examine is Food && examine.NotIDSkill == IDSkill.Tasting && vendor == null )
			{
				Food food = (Food)examine;

				if ( from.CheckTargetSkill( skill, food, 0, 125 ) )
				{
					if ( food.Poison != null )
						food.SendLocalizedMessageTo( from, 1038284 ); // It appears to have poison smeared on it.
					else
						food.SendLocalizedMessageTo( from, 1010600 ); // You detect nothing unusual about this substance.
				}
				else
					food.SendLocalizedMessageTo( from, 502823 ); // You cannot discern anything about this substance.
			}
			else if ( examine.CoinPrice > 0 && examine.NotIdentified )
			{
				if ( vendor != null && examine is NotIdentified )
				{
					string var = NotIdentified.IDVirConItem( (NotIdentified)examine, from );
					vendor.SayTo( from, "Isso parece ser " + var + "." );
				}
				else if ( from.CheckTargetSkill( skill, examine, -5, 125 ) )
				{
					if ( examine is NotIdentified )
					{
						string var = NotIdentified.IDVirConItem( (NotIdentified)examine, from );
						from.SendMessage( "Você identifica " + var + "." );
					}
					else
					{
						examine.NotIdentified = false;

						if ( examine.NotIDSource == Identity.Armor || examine.NotIDSource == Identity.Weapon )
							from.SendMessage( "Isso é muito antigo para ser usado em batalha." );
						else if ( examine.NotIDSource == Identity.Music )
							from.SendMessage( "Isso é muito antigo para ser tocado." );

						from.SendMessage( "Isso provavelmente vale cerca de " + examine.CoinPrice + " ouro." );
					}
				}
				else if ( vendor == null )
				{
					if ( examine is NotIdentified )
					{
						string var = NotIdentified.CannotIDVirConItem( (NotIdentified)examine, from );
						from.SendMessage( var );
					}
					else
					{
						examine.CoinPrice = Utility.RandomMinMax( 5, 25 );
						examine.NotIdentified = false;
						from.SendMessage( "Você não parece conseguir identificá-lo completamente." );

						if ( examine.NotIDSource == Identity.Armor || examine.NotIDSource == Identity.Weapon )
							from.SendMessage( "Isso é muito antigo para ser usado em batalha." );
						else if ( examine.NotIDSource == Identity.Music )
							from.SendMessage( "Isso é muito antigo para ser tocado." );

						from.SendMessage( "Talvez você possa obter " + examine.CoinPrice + " ouro por ele." );
					}
				}
			}
			else
			{
				from.SendMessage( "Esse item não precisa ser identificado." );
			}
		}

		public static bool VendorDoesID( Mobile m )
		{
			if ( 
			m is Alchemist || 
			m is AlchemistGuildmaster || 
			m is ArcherGuildmaster || 
			m is Armorer || 
			m is Banker || 
			m is Bard || 
			m is BardGuildmaster || 
			m is Blacksmith || 
			m is BlacksmithGuildmaster || 
			m is Bowyer || 
			m is Carpenter || 
			m is CarpenterGuildmaster || 
			m is Enchanter || 
			m is Fighter || 
			m is Furtrader || 
			m is Garth || 
			m is Herbalist || 
			m is Jeweler || 
			m is LeatherWorker || 
			m is LibrarianGuildmaster || 
			m is Lumberjack || 
			m is Mage || 
			m is MageGuildmaster || 
			m is Merchant || 
			m is MerchantGuildmaster || 
			m is Minter || 
			m is Provisioner || 
			m is Ranger || 
			m is RangerGuildmaster || 
			m is Roscoe || 
			m is Sage || 
			m is Scribe || 
			m is Tailor || 
			m is TailorGuildmaster || 
			m is Tanner || 
			m is VarietyDealer || 
			m is WarriorGuildmaster || 
			m is Weaponsmith || 
			m is Weaver )
				return true;

			return false;
		}

		public static bool VendorCanID( Identity id, Mobile m )
		{
			if ( id == Identity.Archer )
				return ( m is Bowyer || m is Ranger || m is RangerGuildmaster || m is ArcherGuildmaster );
			else if ( id == Identity.Armor )
				return ( m is Armorer || m is Blacksmith || m is Fighter || m is Garth || m is WarriorGuildmaster || m is BlacksmithGuildmaster );
			else if ( id == Identity.Artifact )
				return ( m is Sage );
			else if ( id == Identity.Book )
				return ( m is Scribe || m is LibrarianGuildmaster );
			else if ( id == Identity.Clothing )
				return ( m is Tailor || m is Weaver || m is TailorGuildmaster );
			else if ( id == Identity.Coins )
				return ( m is Banker || m is Minter );
			else if ( id == Identity.Jewelry )
				return ( m is Jeweler );
			else if ( id == Identity.Leather )
				return ( m is Furtrader || m is LeatherWorker || m is Tanner );
			else if ( id == Identity.Magic )
				return ( m is Sage );
			else if ( id == Identity.Music )
				return ( m is Bard || m is BardGuildmaster );
			else if ( id == Identity.Potion )
				return ( m is Alchemist || m is AlchemistGuildmaster );
			else if ( id == Identity.Reagent )
				return ( m is Herbalist );
			else if ( id == Identity.Scroll )
				return ( m is Scribe || m is LibrarianGuildmaster );
			else if ( id == Identity.Wand )
				return ( m is Enchanter || m is Mage || m is Roscoe || m is MageGuildmaster );
			else if ( id == Identity.Weapon )
				return ( m is Blacksmith || m is Fighter || m is Garth || m is Weaponsmith || m is WarriorGuildmaster || m is BlacksmithGuildmaster );
			else if ( id == Identity.Wood )
				return ( m is Carpenter || m is Lumberjack || m is CarpenterGuildmaster );
			else if ( id == Identity.Merchant )
				return ( m is Provisioner || m is Merchant || m is VarietyDealer || m is MerchantGuildmaster );

			return false;
		}
	}
}