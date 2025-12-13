using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class DraconicKey : Item
    {
        private Mobile m_FoundBy;
        private DateTime m_ExpirationTime;
        private Timer m_ExpirationTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile FoundBy
        {
            get { return m_FoundBy; }
            set { m_FoundBy = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime ExpirationTime
        {
            get { return m_ExpirationTime; }
            set { m_ExpirationTime = value; }
        }

        [Constructable]
        public DraconicKey() : base(0x410B)
        {
            Name = "a draconic key";
            Hue = 1157;
            Weight = 1.0;
            LootType = LootType.Blessed;

            m_ExpirationTime = DateTime.UtcNow + TimeSpan.FromHours(24);
            StartExpirationTimer();
        }

        public override string DefaultDescription{ get{ return "This ancient key shimmers with warm and strange energy. It's of an ancient design and looks like its about to crumble into dust."; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (from.Map == Map.Lodor && from.X == 5187 && from.Y == 1000 && from.Z == 0)
            {
                ActivatePortal(from);
            }
            else
            {
                from.SendMessage("You don't see where you could use this key.");
            }
        }

        private void ActivatePortal(Mobile from)
        {
            IPooledEnumerable eable = Map.Lodor.GetItemsInRange(new Point3D(5187, 999, 0), 5);
            Item portcullis = null;

            foreach (Item item in eable)
            {
                if ((item is DraconicPortcullis || item is PortcullisNS) && 
                    item.X == 5187 && item.Y == 999 && item.Z == 0)
                {
                    portcullis = item;
                    break;
                }
            }
            eable.Free();

            Point3D originalLocation = portcullis.Location;
            Map originalMap = portcullis.Map;
            
            portcullis.Internalize();
            
            Teleporter teleporter1 = new Teleporter(new Point3D(6893, 610, 0), Map.Lodor);
            teleporter1.MoveToWorld(new Point3D(5186, 1000, 0), Map.Lodor);

            Teleporter teleporter2 = new Teleporter(new Point3D(6894, 610, 0), Map.Lodor);
            teleporter2.MoveToWorld(new Point3D(5187, 1000, 0), Map.Lodor);

            Teleporter teleporter3 = new Teleporter(new Point3D(6895, 610, 0), Map.Lodor);
            teleporter3.MoveToWorld(new Point3D(5188, 1000, 0), Map.Lodor);

            from.SendMessage("The gate has been opened!");

            Timer.DelayCall(TimeSpan.FromHours(2), delegate()
            {
                if (portcullis != null && !portcullis.Deleted)
                {
                    portcullis.MoveToWorld(originalLocation, originalMap);
                }

                if (teleporter1 != null && !teleporter1.Deleted)
                {
                    teleporter1.Delete();
                }

                if (teleporter2 != null && !teleporter2.Deleted)
                {
                    teleporter2.Delete();
                }

                if (teleporter3 != null && !teleporter3.Deleted)
                {
                    teleporter3.Delete();
                }
            });

            this.Delete();
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is Mobile && m_FoundBy == null)
            {
                m_FoundBy = (Mobile)parent;
                InvalidateProperties();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_FoundBy != null)
            {
                list.Add(1050045, m_FoundBy.Name); // Found by ~1_NAME~
            }

            TimeSpan remaining = m_ExpirationTime - DateTime.UtcNow;
            if (remaining > TimeSpan.Zero)
            {
                list.Add(1060658, "Time Remaining\t{0}", FormatTimeSpan(remaining));
            }
            else
            {
                list.Add(1060658, "Time Remaining\tExpired");
            }
        }

        private string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return string.Format("{0}h {1}m", (int)ts.TotalHours, ts.Minutes);
            else
                return string.Format("{0}m", (int)ts.TotalMinutes);
        }

        private void StartExpirationTimer()
        {
            if (m_ExpirationTimer != null)
                m_ExpirationTimer.Stop();

            TimeSpan delay = m_ExpirationTime - DateTime.UtcNow;
            if (delay > TimeSpan.Zero)
            {
                m_ExpirationTimer = Timer.DelayCall(delay, new TimerCallback(OnExpire));
            }
            else
            {
                OnExpire();
            }
        }

        private void OnExpire()
        {
            if (!Deleted)
            {
                if (m_FoundBy != null)
                {
                    m_FoundBy.SendMessage("Your draconic key has crumbled to dust.");
                }
                Delete();
            }
        }

        public DraconicKey(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_FoundBy);
            writer.Write(m_ExpirationTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_FoundBy = reader.ReadMobile();
            m_ExpirationTime = reader.ReadDateTime();

            StartExpirationTimer();
        }
    }
}