using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Spells.Research
{
	public class ResearchGrantPeace : ResearchSpell
	{
		public override int spellIndex { get { return 18; } }
		public int CirclePower = 4;
		public static int spellID = 18;
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2.0 ); } }
		public override double RequiredSkill{ get{ return (double)(Int32.Parse( Server.Misc.Research.SpellInformation( spellIndex, 8 ))); } }
		public override int RequiredMana{ get{ return Int32.Parse( Server.Misc.Research.SpellInformation( spellIndex, 7 )); } }

		private static SpellInfo m_Info = new SpellInfo(
				Server.Misc.Research.SpellInformation( spellID, 2 ),
				Server.Misc.Research.CapsCast( Server.Misc.Research.SpellInformation( spellID, 4 ) ),
				203,
				9031,
				Reagent.PixieSkull,Reagent.GraveDust
			);

		public ResearchGrantPeace( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
			Caster.SendMessage( "Qual criatura morta-viva você deseja banir?" );
		}

		public void Target( Mobile m )
		{
			SlayerEntry undead = SlayerGroup.GetEntryByName( SlayerName.Silver );

			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( m is BaseCreature )
			{
				BaseCreature bc = m as BaseCreature;

				double damage = DamagingSkill( Caster )-30;
					if ( damage > 200 ){ damage = 200.0; }
					if ( damage < 50 ){ damage = 50.0; }

				if (!undead.Slays(m))
				{
					Caster.SendMessage( "Este feitiço não pode ser usado neste tipo de criatura." );
				}
				else if( bc.IsBonded )
				{
					Caster.SendMessage("Este feitiço não pode afetar tal criatura!");
				}
				else if ( CheckHSequence(m) )
				{
					int exChance = (int)(m.Fame/200)+10;
					if ( DamagingSkill( Caster ) >= Utility.RandomMinMax( 1, exChance ) )
					{
						m.Say("Não! Você não pode me enviar em direção à luz! A morte não é eterna!");
						SpellHelper.Turn(Caster, m);
						m.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
						m.PlaySound(0x208);
						new InternalTimer(m).Start();
					}
					else
					{
						Caster.SendMessage( "Você falha em sua tentativa, mas causou algum dano de energia." );
						m.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
						m.PlaySound(0x208);
						SpellHelper.Damage(this, m, damage, 0, 0, 0, 0, 100);
					}
				}
				Server.Misc.Research.ConsumeScroll( Caster, true, spellIndex, alwaysConsume, Scroll );
			}
			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			Mobile m_Owner;

			public InternalTimer( Mobile owner ) : base( TimeSpan.FromSeconds( 1.5 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner != null) 
				{
                    if( m_Owner.CheckAlive() )
					m_Owner.Delete();
				}
			}
		}

		private class InternalTarget : Target
		{
			private ResearchGrantPeace m_Owner;
			public InternalTarget( ResearchGrantPeace owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( m_Owner !=null && o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}