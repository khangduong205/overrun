using UnityEngine;

public enum ShopItemKind { Weapon, AbilityUnlock }

[CreateAssetMenu(menuName = "Overrun/Shop Item", fileName = "ShopItem")]
public class ShopItemData : ScriptableObject
{
    [SerializeField] private string id;   // duy nhất, đã lưu vào save nên ĐỪNG đổi sau khi dùng
    [SerializeField] private string displayName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private ShopItemKind kind;
    [SerializeField] private int price = 100;   // 0 = có sẵn từ đầu

    [Header("Chỉ dùng khi Kind = Weapon")]
    [SerializeField] private WeaponData weapon;

    public string Id => id;
    public string DisplayName => displayName;
    public string Description => description;
    public ShopItemKind Kind => kind;
    public int Price => price;
    public WeaponData Weapon => weapon;
}