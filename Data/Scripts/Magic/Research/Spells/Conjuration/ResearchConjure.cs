using System;
using Server.Targeting;
using Server.Network;
using Server;
using Server.Items;

namespace Server.Spells.Research
{
	public class ResearchConjure : ResearchSpell
	{
		public override int spellIndex { get { return 1; } }
		public override bool alwaysConsume { get{ return bool.Parse( Server.Misc.Research.SpellInformation( spellIndex, 14 )); } }
		public int CirclePower = 1;
		public static int spellID = 1;
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }
		public override double RequiredSkill{ get{ return (double)(Int32.Parse( Server.Misc.Research.SpellInformation( spellIndex, 8 ))); } }
		public override int RequiredMana{ get{ return Int32.Parse( Server.Misc.Research.SpellInformation( spellIndex, 7 )); } }

		private static SpellInfo m_Info = new SpellInfo(
				Server.Misc.Research.SpellInformation( spellID, 2 ),
				Server.Misc.Research.CapsCast( Server.Misc.Research.SpellInformation( spellID, 4 ) ),
				203,
				9041,
				Reagent.MoonCrystal,Reagent.FairyEgg
			);

		public ResearchConjure( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Item item = new Dagger();
				string msg = "Você conjura uma adaga.";

				switch( Utility.RandomMinMax( 1, 27 ) )
				{		
					case 1: item = new Apple(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura algumas maçãs.";
						if ( Server.Items.BaseRace.BloodDrinker( Caster.RaceID ) ){ item = new BloodyDrink(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura sangue fresco."; }
						else if ( Server.Items.BaseRace.BrainEater( Caster.RaceID ) ){ item = new FreshBrain(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura cérebros frescos."; }
					break;
					case 2: item = new Arrow(); item.Amount = Utility.RandomMinMax( 1, 10 ); msg = "Você conjura algumas flechas."; break;
					case 3: item = new Backpack(); msg = "Você conjura uma mochila."; break;
					case 4: item = new Bag(); msg = "Você conjura uma bolsa."; break;
					case 5: item = new Bandage(); item.Amount = Utility.RandomMinMax( 1, 10 ); msg = "Você conjura algumas ataduras."; break;
					case 6: item = new Bedroll(); msg = "Você conjura um rolo de cama."; break;
					case 7: item = new Beeswax(); msg = "Você conjura um pouco de cera de abelha."; break;
					case 8: item = new WritingBook(); msg = "Você conjura um livro."; break;
					case 9: item = new Bolt(); item.Amount = Utility.RandomMinMax( 1, 10 ); msg = "Você conjura alguns virotes de besta."; break;
					case 10: item = new Bottle(); msg = "Você conjura uma garrafa."; break;
					case 11: item = new BreadLoaf(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura um pouco de pão.";
						if ( Server.Items.BaseRace.BloodDrinker( Caster.RaceID ) ){ item = new BloodyDrink(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura sangue fresco."; }
						else if ( Server.Items.BaseRace.BrainEater( Caster.RaceID ) ){ item = new FreshBrain(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura cérebros frescos."; }
					break;
					case 12: item = new Candle(); msg = "Você conjura uma vela."; break;
					case 13: item = new Club(); msg = "Você conjura um porrete."; break;
					case 14: item = new Dagger(); msg = "Você conjura uma adaga."; break;
					case 15: item = new FloppyHat(); msg = "Você conjura um chapéu."; break;
					case 16: item = new Jar(); msg = "Você conjura um jarro."; break;
					case 17: item = new Kindling(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura alguns gravetos."; break;
					case 18: item = new Lantern(); msg = "Você conjura uma lanterna."; break;
					case 19: item = new Lockpick(); msg = "Você conjura uma gazua."; break;
					case 20: item = new OilCloth(); msg = "Você conjura um pano oleoso."; break;
					case 21: item = new Pouch(); msg = "Você conjura uma bolsinha."; break;
					case 22: item = new Robe(); msg = "Você conjura uma vestimenta."; break;
					case 23: item = new Shoes(); msg = "Você conjura alguns sapatos."; break;
					case 24: item = new SpoolOfThread(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura um pouco de linha."; break;
					case 25: item = new TenFootPole(); msg = "Você conjura uma vara de três metros."; break;
					case 26: item = new Torch(); msg = "Você conjura uma tocha."; break;
					case 27: item = new Pitcher( BeverageType.Water ); msg = "Você conjura um jarro de água."; 
						if ( Server.Items.BaseRace.BloodDrinker( Caster.RaceID ) ){ item = new BloodyDrink(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura sangue fresco."; }
						else if ( Server.Items.BaseRace.BrainEater( Caster.RaceID ) ){ item = new FreshBrain(); item.Amount = Utility.RandomMinMax( 1, 5 ); msg = "Você conjura cérebros frescos."; }
					break;
				}

				Caster.SendMessage( msg );

				Caster.AddToBackpack( item );

				Caster.FixedParticles( 0, 10, 5, 2003, Server.Misc.PlayerSettings.GetMySpellHue( true, Caster, 0 ), 0, EffectLayer.RightHand );
				Caster.PlaySound( 0x1E2 );

				Server.Misc.Research.ConsumeScroll( Caster, true, spellIndex, alwaysConsume, Scroll );
			}

			FinishSequence();
		}
	}
}
