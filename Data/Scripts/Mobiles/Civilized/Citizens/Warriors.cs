using System;
using Server;
using Server.ContextMenus;
using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class Warriors : Citizens
	{
		[Constructable]
		public Warriors()
		{
			SetStr( 386, 400 );

			Server.Misc.MorphingTime.RemoveMyClothes( this );
			Server.Misc.IntelligentAction.DressUpFighters( this, "", false, false, false );
			if ( Backpack != null ){ Backpack.Delete(); }
			CitizenRumor = "";
			CitizenType = 0;
			CitizenCost = 0;
			CitizenService = 0;
			CitizenPhrase = "";
			CantWalk = true;
			AI = AIType.AI_Melee;
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
		}

		public override bool BleedImmune{ get{ return true; } }

		public override bool IsEnemy( Mobile m )
		{
			if ( m is Warriors && m != this )
				return true;

			return false;
		}

		public override void OnThink()
		{
			if ( Combatant == null )
			{
				foreach ( Mobile man in this.GetMobilesInRange( 1 ) )
				{
					if ( man is Warriors )
					{
						Combatant = man;
					}
				}
			}
			Hits = HitsMax;
			Criminal = false;
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			Server.Misc.IntelligentAction.CryOut( this );
			Server.Misc.MorphingTime.RebuildEquipment( this );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			Server.Misc.MorphingTime.RebuildEquipment( this );
			string name = "";
				if ( Utility.RandomBool() ){ name = ", " + Combatant.Name + ""; }

			base.OnGaveMeleeAttack( defender );

			if ( Server.Misc.Worlds.isOrientalRegion( this ) )
			{
				string jMove = "um macaco";
				switch ( Utility.Random( 8 ))		   
				{
					case 1: jMove = "um furão"; break;
					case 2: jMove = "uma borboleta"; break;
					case 3: jMove = "um coelho"; break;
					case 4: jMove = "um pássaro"; break;
					case 5: jMove = "o vento"; break;
					case 6: jMove = "a brisa"; break;
					case 7: jMove = "as ondas"; break;
				};

				string jHit = "um tigre";
				switch ( Utility.Random( 8 ))		   
				{
					case 1: jHit = "um leão"; break;
					case 2: jHit = "um urso"; break;
					case 3: jHit = "uma pantera"; break;
					case 4: jHit = "um guerreiro"; break;
					case 5: jHit = "uma tempestade"; break;
					case 6: jHit = "um raio"; break;
					case 7: jHit = "uma abelha"; break;
				};

				switch ( Utility.Random( 60 ))		   
				{
					case 0: Say("Você melhorou" + name + "."); break;
					case 1: Say("Você precisará ser mais gracioso que isso" + name + "."); break;
					case 2: Say("Onde você estudou para lutar assim" + name + "?"); break;
					case 3: Say("Movimente-se como " + jMove + ", ataque como " + jHit + "."); break;
					case 4: Say("Você terá que ser melhor que isso" + name + "."); break;
					case 5: Say("Defenda-se!"); break;
					case 6: Say("Olhos abertos e focados em mim" + name + "."); break;
					case 7: Say("Depois disso, refletiremos sobre o que aprendemos."); break;
					case 8: Say("Isto é um bom treino" + name + "."); break;
					case 9: Say("Você precisa aprender a aparar" + name + "."); break;
					case 10: Say("Você não meditou antes do sol nascer?"); break;
				};
			}
			else
			{
				string ale = "cerveja";
				switch ( Utility.Random( 6 ))		   
				{
					case 1: ale = "vinho"; break;
					case 2: ale = "grogue"; break;
					case 3: ale = "hidromel"; break;
					case 4: ale = "cerveja"; break;
					case 5: ale = "cidra"; break;
				};

				switch ( Utility.Random( 60 ))		   
				{
					case 0: Say("Você melhorou" + name + "."); break;
					case 1: Say("Você precisará ser mais rápido que isso" + name + "."); break;
					case 2: Say("Onde você aprendeu a lutar assim" + name + "?"); break;
					case 3: Say("" + Utility.RandomMinMax(10,100) + " de ouro diz que posso vencê-lo" + name + "."); break;
					case 4: Say("Você terá que fazer melhor que isso" + name + "."); break;
					case 5: Say("Em guarda!"); break;
					case 6: Say("Olhos abertos e focados em mim" + name + "."); break;
					case 7: Say("Depois disso, vou lhe comprar " + ale + "."); break;
					case 8: Say("Isto é um bom treino" + name + "."); break;
					case 9: Say("Você precisa aprender a aparar" + name + "."); break;
					case 10: Say("Você bebeu " + ale + " demais ontem à noite" + name + "?"); break;
				};
			}
		}

		public Warriors( Serial serial ) : base( serial )
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

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			return false;
		}
	}
}