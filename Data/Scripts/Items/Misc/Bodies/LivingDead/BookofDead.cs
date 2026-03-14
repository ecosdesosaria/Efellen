using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
        public class BookofDead : Item
        {
                public override string DefaultName
                {
                        get { return "Book of the Dead"; }
                }

                [Constructable]
                public BookofDead() : base( 0x1C11 )
                {
                        Weight = 10.0;
                        Hue = 2500;
			LootType = LootType.Blessed;
                }

                public BookofDead( Serial serial ) : base( serial )
                {
                }

                public override void OnDoubleClick( Mobile from )
                {
                        if ( !IsChildOf( from.Backpack ) )
                        {
                                from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
                                return;
                        }

                        double NecroSkill = from.Skills[SkillName.Necromancy].Value;

                        if ( NecroSkill < 80.0 )
                        {
                                from.SendMessage( "Você precisa ter pelo menos 80.0 de habilidade em Necromancia para ressuscitar os mortos." );
                                return;
                        }
                        else if ( (from.Followers + 2) > from.FollowersMax )
                        {
                                from.SendLocalizedMessage( 1049607 ); // You have too many followers to control that creature.
                                return;
                        }

                        double scalar;

                        if ( NecroSkill >= 100.0 )
                                scalar = 1.0;
                        else if ( NecroSkill >= 90.0 )
                                scalar = 0.9;
                        else if ( NecroSkill >= 80.0 )
                                scalar = 0.8;
                        else if ( NecroSkill >= 70.0 )
                                scalar = 0.7;
                        else
                                scalar = 0.6;

                        Container pack = from.Backpack;

                        if ( pack == null )
                                return;

                        int res = pack.ConsumeTotal(
                                new Type[]
                                {
					typeof( DarkHeart ),
                                        typeof( Head ),
					typeof( Torso ),
                                        typeof( RightArm ),
                                        typeof( LeftArm ),
                                        typeof( RightLeg ),
					typeof( LeftLeg )
                                },
                                new int[]
                                {
					1,
                                        1,
                                        1,
                                        1,
                                        1,
					1,
					1,	
                                } );

                        switch ( res )
                        {
				case 0:
                                {
                                from.SendMessage( "Você precisa de um Coração Sombrio para ressuscitar os mortos." );
                                break;
                                }
                                case 1:
                                {
                                from.SendMessage( "Você precisa de uma Cabeça Decapitada para ressuscitar os mortos." );
                                break;
                                }
                                case 2:
                                {
                                from.SendMessage( "Você precisa de um Tronco para ressuscitar os mortos." );
                                break;
                                }
                                case 3:
                                {
                                from.SendMessage( "Você precisa de um Braço Direito para ressuscitar os mortos." );
                                break;
                                }
                                case 4:
                                {
                                from.SendMessage( "Você precisa de um Braço Esquerdo para ressuscitar os mortos." );
                                break;
                                }
                                case 5:
                                {
                                from.SendMessage( "Você precisa de uma Perna Direita para ressuscitar os mortos." );
                                break;
                                }
                                case 6:
                                {
                                from.SendMessage( "Você precisa de uma Perna Esquerda para ressuscitar os mortos." );
                                break;
                                }
                                default:
                                {
                                        corpse z = new corpse( true, scalar );

                                        if ( z.SetControlMaster( from ) )
                                        {
                                                z.MoveToWorld( from.Location, from.Map );
                                                from.PlaySound( 0x754 );
						from.FixedParticles( 0x376A, 10, 30, 5052, EffectLayer.LeftFoot );
						from.Say( "Um Zex Fal Lum" );
                                        }

                                        break;
                                }
                        }
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
                }
        }
} 