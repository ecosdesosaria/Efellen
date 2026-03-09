using System;
using Server;
using Server.ContextMenus;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using System.Text;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class TradesmanBard : Citizens
	{
		[Constructable]
		public TradesmanBard()
		{
			CitizenType = 12;
			SetupCitizen();
			Blessed = true;
			CantWalk = true;
			AI = AIType.AI_Melee;
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextTalk )
			{
				int seconds = Utility.RandomMinMax( 10, 20 );
				BardHit music = new BardHit();
					music.Delete();

				foreach ( Item instrument in this.GetItemsInRange( 1 ) )
				{
					if ( instrument is BardHit )
					{
						if ( this.FindItemOnLayer( Layer.FirstValid ) != null ){ this.FindItemOnLayer( Layer.TwoHanded ).Delete(); }
						else if ( this.FindItemOnLayer( Layer.OneHanded ) != null ){ this.FindItemOnLayer( Layer.TwoHanded ).Delete(); }
						else if ( this.FindItemOnLayer( Layer.TwoHanded ) != null ){ this.FindItemOnLayer( Layer.TwoHanded ).Delete(); }
						music = (BardHit)instrument;
					}
				}

				if ( music.ItemID == 0x27B3 )
				{
					if ( music.X == X ){ Direction = Direction.South; } //music.Y = Y; }
					else if ( music.Y == Y ){ Direction = Direction.East; } //music.X = X; }
					Server.Items.BardHit.SetInstrument( this, music );
				}
				music.OnDoubleClick( this );
				if ( music.Name != "instrument"){ seconds = Utility.RandomMinMax( 5, 10 ); }

				m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( seconds ));
			}
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			Server.Misc.TavernPatrons.RemoveSomeGear( this, false );
			Server.Misc.MorphingTime.CheckNecromancer( this );
		}

		public TradesmanBard( Serial serial ) : base( serial )
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

namespace Server.Items
{
	public class BardHit : Item
	{
		[Constructable]
		public BardHit() : base( 0x27B3 )
		{
			Name = "instrument";
			Movable = false;
			Weight = -2.0;
			ItemID = 0x27B3;
		}

		public BardHit( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from is TradesmanBard )
			{
				string[] song1 = new string[] {"D","L"};
				string[] song2 = new string[] {"a","e","i","o","u","ee","ah","oo"};

				int lyrics = Utility.RandomMinMax( 8, 15 );
				int chords = 0;
				string sing = "";
				bool added = true;

				while ( lyrics > 0 )
				{
					lyrics--;
					chords++;

					if ( chords > 8 && Utility.RandomBool() ){ added = false; }

					if ( added ){ sing = sing + song1[Utility.RandomMinMax( 0, (song1.Length-1) )] + song2[Utility.RandomMinMax( 0, (song2.Length-1) )] + " "; }

					added = true;
				}

				if ( this.Name != "instrument" )
				{
					if ( this.ItemID == 0x64BE || this.ItemID == 0x64BF ){ 		this.Name = "instrument";	from.PlaySound( Utility.RandomList( 0x4C, 0x403, 0x40B, 0x418 ) ); 	if ( Utility.RandomBool() ){ from.Say( sing ); } }		// LUTE
					else if ( this.ItemID == 0x64C0 || this.ItemID == 0x64C1 ){ this.Name = "instrument";	from.PlaySound( 0x504 ); }																									// FLUTE
					else if ( this.ItemID == 0x64C2 || this.ItemID == 0x64C3 ){ this.Name = "instrument";	from.PlaySound( Utility.RandomList( 0x043, 0x045 ) ); 	if ( Utility.RandomBool() ){ from.Say( sing ); } }					// HARP
					else if ( this.ItemID == 0x64C4 || this.ItemID == 0x64C5 ){ this.Name = "instrument";	from.PlaySound( 0x38 ); 	if ( Utility.RandomBool() ){ from.Say( sing ); } }												// DRUM
					else if ( this.ItemID == 0x64C6 || this.ItemID == 0x64C7 ){ this.Name = "instrument";	from.PlaySound( 0x5B1 ); 	if ( Utility.RandomBool() ){ from.Say( sing ); } }												// FIDDLE
					else if ( this.ItemID == 0x64C8 || this.ItemID == 0x64C9 ){ this.Name = "instrument";	from.PlaySound( Utility.RandomList( 0x52, 0x4B5, 0x4B6, 0x4B7 ) ); 	if ( Utility.RandomBool() ){ from.Say( sing ); } }		// TAMBOURINE
					else if ( this.ItemID == 0x64CA || this.ItemID == 0x64CB ){ this.Name = "instrument";	from.PlaySound( Utility.RandomList( 0x3CF, 0x3D0 ) ); } 																	// TRUMPET
					else if ( this.ItemID == 0x64CC || this.ItemID == 0x64CD ){ this.Name = "instrument";	from.PlaySound( 0x5B8 ); }																									// PIPES
				}
				else
				{
					SetInstrument( from, this );

					string[] part1 = new string[] { "Eu escrevi esta", "Eu aprendi esta", "Eu ouvi esta", "Me ensinaram esta", "Aqui está uma", "Esta é uma" };
					string[] part2 = new string[] { "balada", "canção", "melodia", "música" };
					string[] part4 = new string[] { "morte", "destino", "feitos", "coragem", "aventuras", "jornada", "queda", "vitórias", "lenda", "conquistas" };
					string[] part5 = new string[] { "batalha", "ascensão", "destruição", "lenda", "segredo", "sabedoria", "salvador", "campeão", "queda", "conquista" };
					string[] part6 = new string[] { "horrores", "terror", "tesouro", "riquezas", "criaturas", "monstros", "profundezas", "conquista", "descoberta" };

					string ext1 = part1[Utility.RandomMinMax( 0, (part1.Length-1) )] + " ";
					string ext2 = part2[Utility.RandomMinMax( 0, (part2.Length-1) )] + " ";
					string ext3 = "";
					string ext4 = part4[Utility.RandomMinMax( 0, (part4.Length-1) )];
					string ext5 = part5[Utility.RandomMinMax( 0, (part5.Length-1) )];
					string ext6 = part6[Utility.RandomMinMax( 0, (part6.Length-1) )];
					string ext7 = part2[Utility.RandomMinMax( 0, (part2.Length-1) )];

					switch ( Utility.RandomMinMax( 1, 10 ) )
					{
						case 1:
							if ( ext1 == "Aqui está uma " || ext1 == "Esta é uma " ){ ext3 = "de " + RandomThings.GetRandomCity() + "."; }
							else { ext3 = "enquanto eu estava em " + RandomThings.GetRandomCity() + "."; }
						break;
						case 2:		ext3 = "sobre " + RandomThings.GetRandomJobTitle(0) + " e o " + RandomThings.GetRandomThing(0) + "."; break;
						case 3:		ext3 = "sobre " + RandomThings.GetRandomJobTitle(0) + " e a " + RandomThings.GetRandomCreature() + "."; break;
						case 4:		ext3 = "chamada " + RandomThings.GetSongTitle() + "."; break;
						case 5:		ext3 = "que eu chamo de " + RandomThings.GetSongTitle() + "."; break;
						case 6:
							if ( ext1 == "Aqui está uma " || ext1 == "Esta é uma " ){ ext3 = "do " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; }
							else { ext3 = "enquanto viajava pelo " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; }
						break;
						case 7:		ext3 = "sobre a " + ext4 + " de " + NameList.RandomName( "female" ) + " a " + RandomThings.GetBoyGirlJob( 1 ) + "."; break;
						case 8:		ext3 = "sobre a " + ext4 + " de " + NameList.RandomName( "male" ) + " o " + RandomThings.GetBoyGirlJob( 0 ) + "."; break;
						case 9:		ext3 = "sobre a " + ext5 + " de " + RandomThings.GetRandomCity() + "."; break;
						case 10:	ext3 = "sobre os " + ext6 + " de " + RandomThings.MadeUpDungeon() + "."; break;
					}

					string say = ext1 + ext2 + ext3;

					if ( Utility.RandomBool() )
					{
						string job = RandomThings.GetBoyGirlJob(0);
							if ( Utility.RandomBool() ){ job = RandomThings.GetBoyGirlJob(1); }

						string name = RandomThings.GetRandomBoyName();
						string title = " o " + RandomThings.GetBoyGirlJob(0);
						if ( Utility.RandomBool() )
						{
							name = RandomThings.GetRandomGirlName();
							title = " a " + RandomThings.GetBoyGirlJob(1);
						}
						if ( Utility.RandomBool() )
						{
							title = "";
						}

						string dungeon = RandomThings.MadeUpDungeon();
							if ( Utility.RandomBool() ){ dungeon = QuestCharacters.SomePlace( null ); }

						string city = RandomThings.GetRandomCity();	
							if ( Utility.RandomBool() ){ city = RandomThings.MadeUpCity(); }

						string singer = "escrita";	
						switch( Utility.RandomMinMax( 0, 3 ) )
						{
							case 1: singer = "criada"; break;
							case 2: singer = "cantada"; break;
							case 3: singer = "composta"; break;
						}

						string book = "escrita em um pergaminho";	
						switch( Utility.RandomMinMax( 0, 3 ) )
						{
							case 1: book = "entalhada em uma tabuleta"; break;
							case 2: book = "escrita em um livro"; break;
							case 3: book = "rabiscada em uma parede"; break;
						}

						string verb = "encontrada";	
						switch( Utility.RandomMinMax( 0, 3 ) )
						{
							case 1: verb = "descoberta"; break;
							case 2: verb = "dita para ser"; break;
							case 3: verb = "vista"; break;
						}

						switch( Utility.RandomMinMax( 1, 9 ) )
						{
							case 1: say = "Esta " + ext2 + "foi " + singer + " por " + name + title + "."; break;
							case 2: say = "Esta " + ext2 + "foi " + singer + " por " + name + " de " + city + "."; break;
							case 3: say = "Esta " + ext2 + "foi " + singer + " por um " + job + "."; break;
							case 4: say = "Esta " + ext2 + "foi " + singer + " por um " + job + " em " + city + "."; break;
							case 5: say = "Esta " + ext2 + "foi " + book + " " + verb + " em " + dungeon + "."; break;
							case 6: say = "Enquanto explorava " + dungeon + ", esta " + ext2 + "foi " + verb + " " + book + "."; break;
							case 7: say = name + title + " me ensinou esta " + ext2 + "quando eu estava em " + city + "."; break;
							case 8: say = "Um " + job + " me ensinou esta " + ext2 + "quando eu estava em " + city + "."; break;
							case 9: say = "Um " + job + " me ensinou esta " + ext7 + "."; break;
						}
					}

					from.Say( say );
					SetInstrument( from, this );
				}
			}
		}

		public static void SetInstrument( Mobile from, Item instrument )
		{
			string facing = "east";

			if ( from.Direction == Direction.South ){ facing = "south"; }
			instrument.Hue = 0;

			if ( facing == "south" )
			{
				switch ( Utility.RandomMinMax( 1, 8 ) )
				{
					case 1:	instrument.ItemID = 0x64BF; instrument.Name = "lute"; 		instrument.Z = from.Z + 9;	break;
					case 2:	instrument.ItemID = 0x64C1; instrument.Name = "flute"; 		instrument.Z = from.Z + 11;	break;
					case 3:	instrument.ItemID = 0x64C3; instrument.Name = "harp"; 		instrument.Z = from.Z + 8;	break;
					case 4:	instrument.ItemID = 0x64C5; instrument.Name = "drum"; 		instrument.Z = from.Z + 8;	break;
					case 5:	instrument.ItemID = 0x64C7; instrument.Name = "fiddle"; 	instrument.Z = from.Z + 10;	break;
					case 6:	instrument.ItemID = 0x64C9; instrument.Name = "tambourine"; instrument.Z = from.Z + 9;	break;
					case 7:	instrument.ItemID = 0x64CB; instrument.Name = "trumpet"; 	instrument.Z = from.Z + 9;	instrument.Hue = 0xB61;	break;
					case 8:	instrument.ItemID = 0x64CD; instrument.Name = "pipes"; 		instrument.Z = from.Z + 9;	break;
				}
			}
			else
			{
				switch ( Utility.RandomMinMax( 1, 8 ) )
				{
					case 1:	instrument.ItemID = 0x64BE; instrument.Name = "lute"; 		instrument.Z = from.Z + 9;	break;
					case 2:	instrument.ItemID = 0x64C0; instrument.Name = "flute"; 		instrument.Z = from.Z + 11;	break;
					case 3:	instrument.ItemID = 0x64C2; instrument.Name = "harp"; 		instrument.Z = from.Z + 8;	break;
					case 4:	instrument.ItemID = 0x64C4; instrument.Name = "drum"; 		instrument.Z = from.Z + 8;	break;
					case 5:	instrument.ItemID = 0x64C6; instrument.Name = "fiddle"; 	instrument.Z = from.Z + 10;	break;
					case 6:	instrument.ItemID = 0x64C8; instrument.Name = "tambourine"; instrument.Z = from.Z + 9;	break;
					case 7:	instrument.ItemID = 0x64CA; instrument.Name = "trumpet"; 	instrument.Z = from.Z + 9;	instrument.Hue = 0xB61;	break;
					case 8:	instrument.ItemID = 0x64CC; instrument.Name = "pipes"; 		instrument.Z = from.Z + 9;	break;
				}
			}
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
			Weight = -2.0;
		}
	}
}