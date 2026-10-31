using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Overrun/Shop Catalog", fileName = "ShopCatalog")]
public class ShopCatalog : ScriptableObject
{
    [SerializeField] private List<ShopItemData> items = new();
    public IReadOnlyList<ShopItemData> Items => items;

    // Vũ khí đang trang bị; nếu không hợp lệ thì trả vũ khí mặc định (món vũ khí giá 0)
    public WeaponData GetEquippedWeapon()
    {
        string equippedId = PlayerProfile.EquippedWeaponId;
        WeaponData fallback = null;

        foreach (var item in items)
        {
            if (item.Kind != ShopItemKind.Weapon) continue;
            if (item.Price == 0 && fallback == null) fallback = item.Weapon;
            if (item.Id == equippedId && PlayerProfile.IsOwned(item)) return item.Weapon;
        }
        return fallback;
    }
}