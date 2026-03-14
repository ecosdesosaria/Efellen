using System;
using Server;
using Server.Network;
using System.Text;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class VenomSack : Item
	{
		public override string DefaultDescription{ get{ return "Estes sacos de veneno são usados por aqueles proficientes em envenenamento, onde extraem o líquido para uma garrafa."; } }

		[Constructable]
		public VenomSack() : base( 0x23A )
		{
			Name = "venom sack";
			Weight = 1.0;
			Amount = 1;
			Stackable = true;
		}

		public VenomSack( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			int nSkill = 0;
			if ( this.Name == "lesser venom sack" ){ nSkill = -5; }
			else if ( this.Name == "venom sack" ){ nSkill = 15; }
			else if ( this.Name == "greater venom sack" ){ nSkill = 35; }
			else if ( this.Name == "deadly venom sack" ){ nSkill = 55; }
			else { nSkill = 75; }

			if ( from.CheckSkill( SkillName.Poisoning, nSkill, 125 ) )
			{
				if (!from.Backpack.ConsumeTotal(typeof(Bottle), 1))
				{
					from.SendMessage("Você precisa de uma garrafa vazia para drenar o veneno do saco.");
					return;
				}
				else
				{
					from.PlaySound( 0x240 );

					string sPotion = "";

					if ( this.Name == "lesser venom sack" ){ from.AddToBackpack( new LesserPoisonPotion() ); sPotion = "lesser poison"; }
					else if ( this.Name == "venom sack" ){ from.AddToBackpack( new PoisonPotion() ); sPotion = "poison"; }
					else if ( this.Name == "greater venom sack" ){ from.AddToBackpack( new GreaterPoisonPotion() ); sPotion = "greater poison"; }
					else if ( this.Name == "deadly venom sack" ){ from.AddToBackpack( new DeadlyPoisonPotion() ); sPotion = "deadly poison"; }					
					else { from.AddToBackpack( new LethalPoisonPotion() ); sPotion = "lethal poison"; }

					if ( from.CheckSkill( SkillName.Poisoning, nSkill, 125 ) )
					{
						from.SendMessage("Você obtém uma garrafa de " + sPotion + " de alguns dos sacos.");
					}
					else
					{
						from.SendMessage("Você obtém uma garrafa de " + sPotion + " do restante do saco.");
						this.Consume();
					}
					return;
				}
			}
			else
			{
				from.PlaySound( 0x62D );
				if ( Utility.RandomMinMax( 0, 10 ) > 6 )
				{
					from.Say( "Poison!" );

					if ( this.Name == "lesser venom sack" ){ from.ApplyPoison( from, Poison.Lesser ); }
					else if ( this.Name == "venom sack" ){ from.ApplyPoison( from, Poison.Regular ); }
					else if ( this.Name == "greater venom sack" ){ from.ApplyPoison( from, Poison.Greater ); }
					else if ( this.Name == "deadly venom sack" ){ from.ApplyPoison( from, Poison.Deadly ); }
					else { from.ApplyPoison( from, Poison.Lethal ); }

					from.SendMessage( "Veneno!");
				}
				else
				{
					from.SendMessage("Você falha em obter qualquer veneno do saco.");
				}
				this.Consume();
				return;
			}
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Use Para Tentar Extrair Veneno");
			list.Add( 1049644, "Precisa de Uma Garrafa Vazia"); // PARENTHESIS
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
			Stackable = true;
			Hue = 0;
		}
	}
}