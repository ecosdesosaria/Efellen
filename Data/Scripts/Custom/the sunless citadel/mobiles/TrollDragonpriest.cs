using System;
using Server;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName( "a Troll Dragonpriest's corpse" )]
    public class TrollDragonpriest : BaseCreature
    {
        private DateTime m_NextRegenTime;
        private const int RegenAmount = 12;
        private const double RegenInterval = 10.0; // seconds

        [Constructable]
        public TrollDragonpriest () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
        {
            Name = "a troll dragonpriest";
            Body = Utility.RandomList( 53, 54, 439 );
            BaseSoundID = 461;
            Hue = 0x25;
            SetStr( 176 );
            SetDex( 46 );
            SetInt( 46 );
            SetHits( 156 );
            SetDamage( 7, 13 );
            SetDamageType( ResistanceType.Physical, 100 );
            SetResistance( ResistanceType.Physical, 35, 45 );
            SetResistance( ResistanceType.Fire, 25 );
            SetResistance( ResistanceType.Cold, 35 );
            SetResistance( ResistanceType.Poison, 45 );
            SetResistance( ResistanceType.Energy, 25 );
            SetSkill( SkillName.MagicResist, 45.1, 60.0 );
            SetSkill( SkillName.Tactics, 50.1, 70.0 );
            SetSkill( SkillName.FistFighting, 50.1, 70.0 );
            Fame = 3500;
            Karma = -3500;
            VirtualArmor = 40;
            
            m_NextRegenTime = DateTime.UtcNow + TimeSpan.FromSeconds( RegenInterval );
        }

        public override void OnThink()
        {
            base.OnThink();

            if ( Alive && DateTime.UtcNow >= m_NextRegenTime )
            {
                if ( Hits < HitsMax )
                {
                    int healAmount = Math.Min( RegenAmount, HitsMax - Hits );
                    Hits += healAmount;
                    
                    this.PublicOverheadMessage( Network.MessageType.Regular, 0x3B2, false, "*regenerates*" );
                }
                
                m_NextRegenTime = DateTime.UtcNow + TimeSpan.FromSeconds( RegenInterval );
            }
        }

        public override void OnDeath( Container c )
        {
            base.OnDeath( c );
            Mobile killer = this.LastKiller;
            if ( killer != null )
            {
                if ( killer is BaseCreature )
                    killer = ((BaseCreature)killer).GetMaster();
                if ( killer is PlayerMobile )
                {
                    if ( GetPlayerInfo.LuckyKiller( killer.Luck ) && this.Body == 53 && Utility.RandomMinMax( 1, 4 ) == 1 )
                    {
                        BaseWeapon axe = new LargeBattleAxe();
                        axe.MinDamage = axe.MinDamage + 2;
                        axe.MaxDamage = axe.MaxDamage + 4;
                        axe.Name = "trollish battle axe";
                        c.DropItem( axe );
                    }
                    else if ( GetPlayerInfo.LuckyKiller( killer.Luck ) && this.Body == 439 && Utility.RandomMinMax( 1, 4 ) == 1 )
                    {
                        BaseWeapon mace = new WarMace();
                        mace.MinDamage = mace.MinDamage + 2;
                        mace.MaxDamage = mace.MaxDamage + 4;
                        mace.DurabilityLevel = WeaponDurabilityLevel.Indestructible;
                        mace.Name = "trollish war mace";
                        c.DropItem( mace );
                    }
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot( LootPack.Average );
			AddLoot( LootPack.Average );
        }

        public override bool CanRummageCorpses{ get{ return true; } }
        public override int TreasureMapLevel{ get{ return 1; } }
        public override int Meat{ get{ return 2; } }
        public override int Skin{ get{ return Utility.Random(3); } }
        public override SkinType SkinType{ get{ return SkinType.Troll; } }
        public override int Skeletal{ get{ return Utility.Random(3); } }
        public override SkeletalType SkeletalType{ get{ return SkeletalType.Troll; } }

        public TrollDragonpriest( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
            
            m_NextRegenTime = DateTime.UtcNow + TimeSpan.FromSeconds( RegenInterval );
        }
    }
}