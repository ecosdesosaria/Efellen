using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Commands;
using Server.Targeting;

public class HunterMarkSystem
{
	private static Hashtable _marks = new Hashtable();
	private static Hashtable _cooldowns = new Hashtable();

	private class HunterMarkInfo
	{
		public Mobile Tracker;
		public DateTime Expiration;

		public HunterMarkInfo( Mobile tracker, DateTime expiration )
		{
			Tracker = tracker;
			Expiration = expiration;
		}
	}

	public static void Initialize()
	{
		CommandSystem.Register( "HuntersMark", AccessLevel.Player, new CommandEventHandler( HuntersMark_OnCommand ) );

		_cleanupTimer = new CleanupTimer();
		_cleanupTimer.Start();
	}

	public static bool MarkTarget( Mobile tracker, Mobile target )
	{
		if ( tracker == null || target == null )
			return false;

		if ( tracker.Deleted || target.Deleted )
			return false;

		if ( !tracker.Alive || !target.Alive )
			return false;

		if ( tracker.Map != target.Map )
			return false;

		if ( tracker.Skills[SkillName.Tracking].Value < 8.3 )
		{
			tracker.SendMessage( "Você não é habilidoso o suficiente para caçar presas marcadas." );
			return false;
		}

		DateTime cooldown = GetCooldown( tracker );

		if ( DateTime.Now < cooldown )
		{
			TimeSpan remaining = cooldown - DateTime.Now;

			tracker.SendMessage(
				"Você não está pronto para caçar novamente (recarga restante: " +
				remaining.Seconds.ToString() +
				" segundos)"
			);

			return false;
		}

		HunterMarkInfo oldInfo = _marks[target] as HunterMarkInfo;

		if ( oldInfo != null )
			_marks.Remove( target );

		_marks[target] = new HunterMarkInfo(
			tracker,
			DateTime.Now + TimeSpan.FromSeconds( 60.0 )
		);

		_cooldowns[tracker] = DateTime.Now + TimeSpan.FromMinutes( 1.0 );

		tracker.SendMessage( "Você marcou sua presa para a caçada." );

		target.PublicOverheadMessage(
			MessageType.Emote,
			0x22,
			false,
			"*is marked by " + tracker.Name + "*"
		);

		return true;
	}

	public static double GetDamageBonus( Mobile from, Mobile target )
	{
		if ( from == null || target == null )
			return 1.0;

		if ( from.Deleted || target.Deleted )
			return 1.0;

		HunterMarkInfo info = _marks[target] as HunterMarkInfo;

		if ( info == null )
			return 1.0;

		if ( info.Tracker == null || info.Tracker.Deleted )
		{
			_marks.Remove( target );
			return 1.0;
		}

		if ( info.Tracker != from )
			return 1.0;

		if ( DateTime.Now > info.Expiration )
		{
			_marks.Remove( target );
			return 1.0;
		}

		double tracking = from.Skills[SkillName.Tracking].Value;

		double bonus = tracking / 8.333;

		if ( bonus < 1.0 )
			bonus = 1.0;

		return bonus;
	}

	private static DateTime GetCooldown( Mobile m )
	{
		object obj = _cooldowns[m];

		if ( obj == null )
			return DateTime.MinValue;

		return (DateTime)obj;
	}

	private static Timer _cleanupTimer;

	private class CleanupTimer : Timer
	{
		public CleanupTimer()
			: base( TimeSpan.FromMinutes( 1.0 ), TimeSpan.FromMinutes( 1.0 ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			ArrayList toRemove = new ArrayList();

			foreach ( DictionaryEntry entry in _marks )
			{
				Mobile target = entry.Key as Mobile;
				HunterMarkInfo info = entry.Value as HunterMarkInfo;
				
				if ( target == null )
				{
					toRemove.Add( entry.Key );
					continue;
				}

				if ( target.Deleted || !target.Alive )
				{
					toRemove.Add( target );
					continue;
				}

				if ( info == null )
				{
					toRemove.Add( target );
					continue;
				}

				if ( info.Tracker == null || info.Tracker.Deleted )
				{
					toRemove.Add( target );
					continue;
				}

				if ( DateTime.Now > info.Expiration )
				{
					toRemove.Add( target );
					continue;
				}
			}

			foreach ( object obj in toRemove )
				_marks.Remove( obj );

			toRemove.Clear();

			foreach ( DictionaryEntry entry in _cooldowns )
			{
				Mobile m = entry.Key as Mobile;

				if ( m == null || m.Deleted )
				{
					toRemove.Add( entry.Key );
					continue;
				}

				DateTime cooldown = (DateTime)entry.Value;

				if ( DateTime.Now >= cooldown )
					toRemove.Add( entry.Key );
			}

			foreach ( object obj in toRemove )
				_cooldowns.Remove( obj );
		}
	}

	private static void HuntersMark_OnCommand( CommandEventArgs e )
	{
		Mobile from = e.Mobile;

		if ( from == null || from.Deleted )
			return;

		if ( from.Skills[SkillName.Tracking].Value < 8.3 )
		{
			from.SendMessage( "Você não é habilidoso o suficiente para usar Marca do Caçador." );
			return;
		}

		from.SendMessage( "Choose your prey." );

		from.Target = new HuntersMarkTarget();
	}

	private class HuntersMarkTarget : Target
	{
		public HuntersMarkTarget()
			: base( 12, false, TargetFlags.Harmful )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( from == null || from.Deleted )
				return;

			Mobile m = targeted as Mobile;

			if ( m == null )
			{
				from.SendMessage( "Esse não é um alvo válido." );
				return;
			}

			if ( m == from )
			{
				from.SendMessage( "Você não pode se marcar." );
				return;
			}

			if ( m.Deleted || !m.Alive )
			{
				from.SendMessage( "Esse alvo é inválido." );
				return;
			}

			if ( !from.CanBeHarmful( m ) )
			{
				from.SendMessage( "Você não pode marcar esse alvo." );
				return;
			}

			MarkTarget( from, m );
		}
	}
}