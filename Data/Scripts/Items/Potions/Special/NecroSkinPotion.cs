using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class NecroSkinPotion : Item
	{
		public override string DefaultDescription{ get{ return "Jogar este pó em si mesmo deixará sua pele e cabelo brancos fantasmagóricos. Apenas um necromante grão-mestre pode usar isto. Se você já está branco fantasmagórico e usar este pó novamente, sua pele e cabelo retornarão ao que eram."; } }

		public override Catalogs DefaultCatalog{ get{ return Catalogs.Potion; } }

        [Constructable]
        public NecroSkinPotion() : base(0x1006)
		{
            Name = "jar of skull dust";
			Built = true;
        }

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Deixará a Pele e o Cabelo de um Necromante Grão-Mestre Brancos Fantasmagóricos");
			list.Add( 1049644, "Clique Duas Vezes Para Comer o Pó");
        } 

        public override void OnDoubleClick(Mobile from)
		{
			if ( from.RaceID != 0 )
			{
				from.SendMessage( "Você não acha isso realmente útil." );
				return;
			}
			else if ( !IsChildOf( from.Backpack ) ) 
			{
				from.SendMessage( "Isto deve estar na sua mochila para ser usado." );
				return;
			}
			else if ( from.Hue == 0x47E || from.Hue == 0xB70 )
			{
				from.Hue = from.RecordSkinColor;
				from.HairHue = from.RecordHairColor;
				from.FacialHairHue = from.RecordBeardColor;
				from.SendMessage("Seu corpo volta às cores da vida.");
			}
			else if ( from.Skills[SkillName.Necromancy].Base >= 100 )
			{
				from.Hue = 0xB70;
				from.HairHue = Utility.RandomList( 0, 0x497 );
				from.FacialHairHue = from.HairHue;
				from.SendMessage("Seu corpo se transforma em branco fantasmagórico.");
			}
			else
			{
				from.SendMessage("Você come o pó de caveira, deixando sua boca seca.");
				from.Thirst = 0;
			}
			this.Delete();
			from.AddToBackpack( new Jar() );
        }

        public NecroSkinPotion( Serial serial ) : base( serial )
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
			Built = true;
	    }
    }
}