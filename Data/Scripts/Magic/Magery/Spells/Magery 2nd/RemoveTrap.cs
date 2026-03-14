using System;
using Server.Targeting;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using System.Collections;
using Server.Misc;

namespace Server.Spells.Second
{
	public class RemoveTrapSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Remove Trap", "An Jux",
				212,
				9001,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public override SpellCircle Circle { get { return SpellCircle.Second; } }

		public RemoveTrapSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Selecione um recipiente armadilhado, ou a si mesmo para invocar um orbe mágico." );
		}

		public void Target( TrapableContainer item )
		{
			if ( !Caster.CanSee( item ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckSequence() )
			{
				int nTrapLevel = item.TrapLevel * 12;

				if ( (int)( Spell.ItemSkillValue( Caster, SkillName.Magery, false ) ) > nTrapLevel )
				{
					Point3D loc = item.GetWorldLocation();

					Effects.SendLocationParticles( EffectItem.Create( loc, item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, PlayerSettings.GetMySpellHue( true, Caster, 0 ), 0, 5015, 0 );
					Effects.PlaySound( loc, item.Map, 0x1F0 );

					Caster.SendMessage( "Quaisquer armadilhas nesse recipiente agora estão desativadas." );

					item.TrapType = TrapType.None;
					item.TrapPower = 0;
					item.TrapLevel = 0;
				}
				else
				{
					Caster.SendMessage( "Essa armadilha parece complicada demais para ser afetada pela sua magia." );
					base.DoFizzle();
				}
			}
			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private RemoveTrapSpell m_Owner;

			public InternalTarget( RemoveTrapSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is TrapableContainer )
				{
					m_Owner.Target( (TrapableContainer)o );
				}
				else if ( from == o )
				{
					if ( m_Owner.CheckSequence() )
					{
						ArrayList targets = new ArrayList();
						foreach ( Item item in World.Items.Values )
						if ( item is TrapWand )
						{
							TrapWand myWand = (TrapWand)item;
							if ( myWand.owner == from )
							{
								targets.Add( item );
							}
						}
						for ( int i = 0; i < targets.Count; ++i )
						{
							Item item = ( Item )targets[ i ];
							item.Delete();
						}

						from.PlaySound( 0x1ED );
						from.FixedParticles( 0x376A, 9, 32, 5008, PlayerSettings.GetMySpellHue( true, from, 0 ), 0, EffectLayer.Waist );
						from.SendMessage( "Você invoca um orbe mágico em sua mochila." );
						Item iWand = new TrapWand(from);
						int nPower = (int)(from.Skills[SkillName.Magery].Value / 2 ) + 25;
						if (nPower > 100){nPower = 100;}
						TrapWand xWand = (TrapWand)iWand;
						xWand.WandPower = nPower;
						from.AddToBackpack( xWand );

						string args = String.Format("{0}", nPower);
						BuffInfo.RemoveBuff( from, BuffIcon.RemoveTrap );
						BuffInfo.AddBuff( from, new BuffInfo( BuffIcon.RemoveTrap, 1063623, 1063624, TimeSpan.FromMinutes( 30.0 ), from, args.ToString(), true ) );
					}
					m_Owner.FinishSequence();
				}
				else
				{
					from.SendMessage( "Este feitiço não tem efeito nisso!" );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}