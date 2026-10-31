using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int coins;
    public List<string> owned = new();      // id các món đã mua
    public string equippedWeaponId = "";    // rỗng = dùng vũ khí mặc định
}