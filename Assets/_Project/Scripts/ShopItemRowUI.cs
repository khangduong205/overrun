using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    private ShopItemData item;

    public void Bind(ShopItemData data, Action<ShopItemData> onClick)
    {
        item = data;
        nameText.text = data.DisplayName;
        descText.text = data.Description;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(data));
        Refresh();
    }

    // 4 states: not purchased, can/can't afford, owned, equipped
    public void Refresh()
    {
        bool owned = PlayerProfile.IsOwned(item);

        if (!owned)
        {
            buttonText.text = $"Buy {item.Price}";
            button.interactable = PlayerProfile.Coins >= item.Price;
            return;
        }

        if (item.Kind == ShopItemKind.AbilityUnlock)
        {
            buttonText.text = "Owned";
            button.interactable = false;
            return;
        }

        bool equipped = PlayerProfile.EquippedWeaponId == item.Id
                        || (string.IsNullOrEmpty(PlayerProfile.EquippedWeaponId) && item.Price == 0);
        buttonText.text = equipped ? "Equipped" : "Equip";
        button.interactable = !equipped;
    }
}