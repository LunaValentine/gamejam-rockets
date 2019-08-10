using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct AcknowledgementBundle
{
    public uint LastAck;
    public uint Last32Acks;

    public AcknowledgementBundle(uint LAck, uint L32Ack)
    {
        LastAck = LAck;
        Last32Acks = L32Ack;
    }

    public bool GetAckStatus(uint AckNum)
    {
        if (AckNum == LastAck)
            return true;

        uint offset = LastAck - 1 - AckNum;
        if (offset <= 32)
        {
            uint shifted = Last32Acks >> (int)(offset);
            return ((Last32Acks >> (int)(offset)) % 2 == 1);
        }
        else
        {
            return false;
        }
    }

    public void AddAck(uint number)
    {
        if (SequenceMoreRecent(number, LastAck))
        {
            NewLastAck(number);
        }
        else if (SequenceMoreRecent(number, LastAck - 33))
        {
            uint offset = LastAck - 1 - number;
            uint field = 1;
            field = field << (int)offset;
            Last32Acks = Last32Acks | field;
        }
    }

    private void NewLastAck(uint number)
    {
        uint diff = 0;
        if (number < LastAck)
        {
            //Wrap around
            diff = number - LastAck;
        }
        else
        {
            diff = number - 1 - LastAck;
        }
        if (diff < 32)
        {
            Last32Acks = Last32Acks << (int)diff;
        }
        else
        {
            Last32Acks = 0;
        }
        LastAck = number;
    }

    bool SequenceMoreRecent(uint s1, uint s2)
    {
        return (s1 > s2) && (s1 - s2 <= uint.MaxValue / 2) || (s2 > s1) && (s2 - s1 > uint.MaxValue / 2);
    }
}
