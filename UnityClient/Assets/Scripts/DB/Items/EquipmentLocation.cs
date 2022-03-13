﻿
using System;

[Flags]
public enum EquipmentLocation : int {
    UNEQUIPED = 0,
    HEAD_BOTTOM = 1,
    HEAD_MID = 512,
    HEAD_TOP = 256,
    HAND_RIGHT = 2, //weapon
    HAND_LEFT = 32, //shield
    ARMOR = 16,
    SHOES = 64,
    GARMENT = 4,
    ACCESSORY_RIGHT = 8,
    ACCESSORY_LEFT = 128,
    COSTUME_HEAD_TOP = 1024,
    COSTUME_HEAD_MID = 2048,
    COSTUME_HEAD_LOW = 4096,
    COSTUME_GARMENT = 8192,
    COSTUME_FLOOR = 16384,
    AMMO = 32768,
    SHADOW_ARMOR = 65536,
    SHADOW_WEAPON = 131072,
    SHADOW_SHIELD = 262144,
    SHADOW_SHOES = 524288,
    SHADOW_ACCESSORY_RIGHT = 1048576,
    SHADOW_ACCESSORY_LEFT = 2097152,

    // Combined
    ACCESSORY_RIGHT_AND_LEFT = ACCESSORY_RIGHT | ACCESSORY_LEFT,
    SHADOW_ACCESSORY_RIGHT_AND_LEFT = SHADOW_ACCESSORY_RIGHT | SHADOW_ACCESSORY_LEFT
}