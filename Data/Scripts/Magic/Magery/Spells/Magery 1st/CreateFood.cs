using System;
using Server.Items;

namespace Server.Spells.First
{
	public class CreateFoodSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Create Food", "In Mani Ylem",
				224,
				9011,
				Reagent.Garlic,
				Reagent.Ginseng,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.First; } }

		public CreateFoodSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		private static FoodInfo[] m_Food = new FoodInfo[]
		{
			new FoodInfo( typeof( Grapes ), "um cacho de uvas" ),
			new FoodInfo( typeof( Ham ), "um presunto" ),
			new FoodInfo( typeof( CheeseWedge ), "uma fatia de queijo" ),
			new FoodInfo( typeof( Muffins ), "bolinhos" ),
			new FoodInfo( typeof( FishSteak ), "um bife de peixe" ),
			new FoodInfo( typeof( Ribs ), "corte de costelas" ),
			new FoodInfo( typeof( CookedBird ), "uma ave cozida" ),
			new FoodInfo( typeof( Sausage ), "salsicha" ),
			new FoodInfo( typeof( Apple ), "uma maçã" ),
			new FoodInfo( typeof( Peach ), "um pêssego" )
		};

		public override void OnCast()
		{
		    if (CheckSequence())
		    {
		        Container pack = Caster.Backpack;

		        if (pack == null)
		        {
		            Caster.SendMessage("Você não tem como carregar mais comida.");
		            FinishSequence();
		            return;
		        }

		        if (Server.Items.BaseRace.BloodDrinker(Caster.RaceID))
		        {
		            Item blood = new BloodyDrink();

		            if (!pack.CheckHold(Caster, blood, false, true))
		            {
		                blood.Delete();
		                Caster.SendMessage("Você não tem como carregar mais comida.");
		                FinishSequence();
		                return;
		            }

		            pack.DropItem(blood);
		            Caster.SendMessage("Um pouco de sangue fresco aparece magicamente em sua mochila.");
		        }
		        else if (Server.Items.BaseRace.BrainEater(Caster.RaceID))
		        {
		            Item brain = new FreshBrain();

		            if (!pack.CheckHold(Caster, brain, false, true))
		            {
		                brain.Delete();
		                Caster.SendMessage("Você não tem como carregar mais comida.");
		                FinishSequence();
		                return;
		            }

		            pack.DropItem(brain);
		            Caster.SendMessage("Alguns cérebros frescos aparecem magicamente em sua mochila.");
		        }
		        else
		        {
		            FoodInfo foodInfo = m_Food[Utility.Random(m_Food.Length)];

		            Item food = foodInfo.Create();
		            Item water = new WaterBottle();

		            if (!pack.CheckHold(Caster, food, false, true) ||
		                !pack.CheckHold(Caster, water, false, true))
		            {
		                if (food != null) food.Delete();
		                if (water != null) water.Delete();

		                Caster.SendMessage("Você não tem como carregar mais comida.");
		                FinishSequence();
		                return;
		            }

		            pack.DropItem(food);
		            pack.DropItem(water);

		            Caster.SendMessage("Alguma comida e bebida aparecem magicamente em sua mochila.");
		        }

		        Caster.FixedParticles(0, 10, 5, 2003,
		            Server.Misc.PlayerSettings.GetMySpellHue(true, Caster, 0),
		            0,
		            EffectLayer.RightHand);

		        Caster.PlaySound(0x1E2);
		    }

		    FinishSequence();
		}
	}

	public class FoodInfo
	{
		private Type m_Type;
		private string m_Name;

		public Type Type{ get{ return m_Type; } set{ m_Type = value; } }
		public string Name{ get{ return m_Name; } set{ m_Name = value; } }

		public FoodInfo( Type type, string name )
		{
			m_Type = type;
			m_Name = name;
		}

		public Item Create()
		{
			Item item;

			try
			{
				item = (Item)Activator.CreateInstance( m_Type );
			}
			catch
			{
				item = null;
			}

			return item;
		}
	}
}