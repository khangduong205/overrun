using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public float bestTime;
    public int bestKills;
    public int coins;
    public List<string> owned = new();      // purchased item IDs
    public string equippedWeaponId = "";    // empty = use default weapon
}