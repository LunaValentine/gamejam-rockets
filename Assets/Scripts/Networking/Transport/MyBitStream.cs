using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBitStream
{
    byte[] buffer;
    int writeByteIndex = 0;
    int writeBitIndex = 0;

    public int byteIndex = 0;
    int bitIndex = 0;

    public MyBitStream(int size)
    {
        buffer = new byte[size];
    }

    public MyBitStream(byte[] buff)
    {
        buffer = buff;
    }

    public int BufferLength()
    {
        return buffer.Length;
    }

    public byte[] GetUnderlyingArray()
    {
        return buffer;
    }

    //The Easy way, Probably will update this to be more efficient with the middle Bytes
    public void PackInt(int number)
    {
        byte[] bytes = BitConverter.GetBytes(number);
        for (int i = 0; i < bytes.Length; i++)
        {
            PackByte(bytes[i]);
        }
    }

    public int ReadInt()
    {
        byte[] bytes = ReadBytes(4);
        return BitConverter.ToInt32(bytes, 0);
    }

    public void PackFloat(float number)
    {
        byte[] bytes = BitConverter.GetBytes(number);
        for (int i = 0; i < bytes.Length; i++)
        {
            PackByte(bytes[i]);
        }
    }

    public float ReadFloat()
    {
        byte[] bytes = ReadBytes(4);
        return BitConverter.ToSingle(bytes, 0);
    }

    public byte[] ReadBytes(int number)
    {
        byte[] bytes = new byte[number];
        for (int i = 0; i < number; i++)
        {
            bytes[i] = ReadByte();
        }
        return bytes;
    }

    public byte ReadByte()
    {
        byte toReturn = 0;
        int Mask = 255 << bitIndex;
        int InvertMask = (Mask * -1 - 1);

        //First part of the byte
        int num = (buffer[byteIndex] & Mask);
        toReturn = (byte)((buffer[byteIndex] & Mask) >> bitIndex);

        byteIndex++;

        //Second Part
        toReturn += (byte)((buffer[byteIndex] & InvertMask) >> (8 - bitIndex));

        return toReturn;
    }

    public void PackByte(byte b)
    {
        int Mask = 255 << writeBitIndex;
        int InvertMask = (Mask * -1 - 1);

        //First part of the byte
        buffer[writeByteIndex] = (byte)((buffer[writeByteIndex] & InvertMask) + (b << writeBitIndex));

        writeByteIndex++;

        //Second Part
        buffer[writeByteIndex] = (byte)((buffer[writeByteIndex] & Mask) + (b >> 8 - writeBitIndex));
    }

    public bool ReadBool()
    {
        int myBitIndex = bitIndex;
        int myByteIndex = byteIndex;

        bitIndex++;
        if (bitIndex > 7)
        {
            bitIndex = 0;
            byteIndex++;
        }

        return ((buffer[myByteIndex] & (1 << myBitIndex)) == 1 << myBitIndex);
    }

    public void PackBool(bool boo)
    {
        if (boo)
        {
            int Mask = 1 << writeBitIndex;
            buffer[writeByteIndex] = (byte)((buffer[writeByteIndex] & (Mask * -1 - 1)) + Mask);
        }
        //I assume that the bit is already blank (I need to make sure that I clear data)
        writeBitIndex++;
        if (writeBitIndex > 7)
        {
            writeBitIndex = 0;
            writeByteIndex++;
        }
    }

    public byte[] getBytes()
    {
        return buffer;
    }
}