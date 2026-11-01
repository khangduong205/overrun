using System;
using System.IO;
using UnityEngine;

public static class PlayerProfile
{
    private static SaveData data;
    private static string FilePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static event Action Changed;

    public static float BestTime { get { EnsureLoaded(); return data.bestTime; } }
    public static int BestKills { get { EnsureLoaded(); return data.bestKills; } }

    public static int Coins { get { EnsureLoaded(); return data.coins; } }
    public static string EquippedWeaponId { get { EnsureLoaded(); return data.equippedWeaponId; } }

    // retun true if there's new highest score.
    public static bool SubmitRun(float time, int kills)
    {
        EnsureLoaded();
        bool record = false;
        if (time > data.bestTime) { data.bestTime = time; record = true; }
        if (kills > data.bestKills) { data.bestKills = kills; record = true; }
        if (record) Save();
        return record;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() { data = null; Changed = null; }

    private static void EnsureLoaded()
    {
        if (data != null) return;
        try
        {
            data = File.Exists(FilePath)
                ? JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath))
                : new SaveData();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Save bị lỗi, tạo mới: {e.Message}");
            data = new SaveData();
        }
        data ??= new SaveData();
    }

    private static void Save()
    {
        try { File.WriteAllText(FilePath, JsonUtility.ToJson(data)); }
        catch (Exception e) { Debug.LogError($"Không lưu được: {e.Message}"); }
        Changed?.Invoke();
    }

    // Price 0 means owned by default (starter weapon)
    public static bool IsOwned(ShopItemData item)
    {
        EnsureLoaded();
        return item.Price == 0 || data.owned.Contains(item.Id);
    }

    // empty Ability ID means unlocked by default
    public static bool IsOwnedId(string id)
    {
        EnsureLoaded();
        return string.IsNullOrEmpty(id) || data.owned.Contains(id);
    }

    public static void AddCoins(int amount)
    {
        EnsureLoaded();
        data.coins += amount;
        Save();
    }

    public static bool TryPurchase(ShopItemData item)
    {
        EnsureLoaded();
        if (IsOwned(item) || data.coins < item.Price) return false;
        data.coins -= item.Price;
        data.owned.Add(item.Id);
        Save();
        return true;
    }

    public static void Equip(string weaponId)
    {
        EnsureLoaded();
        data.equippedWeaponId = weaponId;
        Save();
    }

    public static void ResetAll()
    {
        data = new SaveData();
        Save();
    }
}