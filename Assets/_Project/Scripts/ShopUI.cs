using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private ShopCatalog catalog;
    [SerializeField] private ShopItemRowUI rowPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private TMP_Text coinsText;

    private readonly List<ShopItemRowUI> rows = new();

    private void Awake()
    {
        foreach (var item in catalog.Items)
        {
            var row = Instantiate(rowPrefab, container);
            row.Bind(item, OnItemClicked);
            rows.Add(row);
        }
        RefreshAll();
    }

    private void OnEnable() => PlayerProfile.Changed += RefreshAll;
    private void OnDisable() => PlayerProfile.Changed -= RefreshAll;

    // Nối vào OnClick của ShopButton và BackButton trong Inspector
    public void Open() { panel.SetActive(true); RefreshAll(); }
    public void Close() => panel.SetActive(false);

    private void OnItemClicked(ShopItemData item)
    {
        if (!PlayerProfile.IsOwned(item))
        {
            if (PlayerProfile.TryPurchase(item) && item.Kind == ShopItemKind.Weapon)
                PlayerProfile.Equip(item.Id);   // mua vũ khí xong tự trang bị
        }
        else if (item.Kind == ShopItemKind.Weapon)
        {
            PlayerProfile.Equip(item.Id);
        }
    }

    private void RefreshAll()
    {
        coinsText.text = $"Coins: {PlayerProfile.Coins}";
        foreach (var row in rows) row.Refresh();
    }

    // Công cụ test: chuột phải tiêu đề component khi đang Play. XÓA trước khi build
    [ContextMenu("DEBUG: Add 500 coins")]
    private void DebugAddCoins() => PlayerProfile.AddCoins(500);

    [ContextMenu("DEBUG: Reset save")]
    private void DebugReset() => PlayerProfile.ResetAll();
}