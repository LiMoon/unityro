﻿using System;
using System.IO;

public partial class ZC {

    [PacketHandler(HEADER, "ZC_ACCEPT_ENTER2", SIZE, PacketHandlerAttribute.PacketDirection.In)]
    public class ACCEPT_ENTER2 : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_ACCEPT_ENTER2;
        public const int SIZE = 13;

        public int Tick;
        public int PosX;
        public int PosY;
        public int Dir;
        public short Font;
        public int Sex;

        public PacketHeader GetHeader() {
            return HEADER;
        }

        public bool Read(byte[] data) {
            var br = new BinaryReader(data);
            Tick = br.ReadLong();
            var posDir = br.ReadPos();
            PosX = posDir[0];
            PosY = posDir[1];
            Dir = posDir[2];

            br.Seek(2, SeekOrigin.Current);

            Font = br.ReadShort();
            Sex = br.ReadByte();

            return true;
        }
    }
}
