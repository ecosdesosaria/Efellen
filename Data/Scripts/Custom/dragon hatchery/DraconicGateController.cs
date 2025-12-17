using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;

public class DraconicGateController : Item
{
    private DateTime m_CloseTime;
    private Item m_Portcullis;
    private Teleporter[] m_Teleporters;
    private Timer m_Timer;

    public DraconicGateController(
        Item portcullis,
        Teleporter[] teleporters,
        DateTime closeTime
    ) : base(1)
    {
        Movable = false;
        Visible = false;

        m_Portcullis = portcullis;
        m_Teleporters = teleporters;
        m_CloseTime = closeTime;

        StartTimer();
    }

    private void StartTimer()
    {
        TimeSpan delay = m_CloseTime - DateTime.UtcNow;

        if (delay <= TimeSpan.Zero)
            CloseGate();
        else
            m_Timer = Timer.DelayCall(delay, CloseGate);
    }

    private void CloseGate()
    {
        if (Deleted)
            return;

        if (m_Portcullis != null && !m_Portcullis.Deleted)
            m_Portcullis.MoveToWorld(m_Portcullis.Location, m_Portcullis.Map);

        if (m_Teleporters != null)
        {
            foreach (Teleporter t in m_Teleporters)
                if (t != null && !t.Deleted)
                    t.Delete();
        }

        Delete();
    }

    public DraconicGateController(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0);

        writer.Write(m_CloseTime);
        writer.Write(m_Portcullis);

        writer.Write(m_Teleporters.Length);
        foreach (Teleporter t in m_Teleporters)
            writer.Write(t);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        reader.ReadInt();

        m_CloseTime = reader.ReadDateTime();
        m_Portcullis = reader.ReadItem();

        int count = reader.ReadInt();
        m_Teleporters = new Teleporter[count];

        for (int i = 0; i < count; i++)
            m_Teleporters[i] = reader.ReadItem() as Teleporter;

        StartTimer();
    }
}
