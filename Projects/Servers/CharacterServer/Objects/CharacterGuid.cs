﻿// Copyright (c) Multi-Emu.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Framework.Constants.Object;
using Framework.Objects;

namespace CharacterServer.Objects
{
    public class CharacterGuid : SmartGuid
    {
        public CharacterGuid()
        {
            High = (ulong)GuidType.Player << 58;
        }

        public ushort RealmId
        {
            get { return (ushort)((High >> 42) & 0x1FFF); }
            set { High |= (ulong)value << 42; }
        }

        public ulong CreationBits
        {
            get { return Low & 0xFFFFFFFFFFFFFFFF; }
            set { Low |= value; }
        }
    }
}
