using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectionUI : MonoBehaviour
{
    [Serializable]
    private class Card
    {
        public Button button;
        public TMP_Text title;
        public TMP_Text description;
    }

    [SerializeField] private GameObject panel;
    [SerializeField] private Card[] cards;

    public bool IsOpen => panel.activeSelf;

    public void Show(IReadOnlyList<UpgradeData> options, Action<UpgradeData> onChosen)
    {
        panel.SetActive(true);

        for (int i = 0; i < cards.Length; i++)
        {
            bool hasOption = i < options.Count;
            cards[i].button.gameObject.SetActive(hasOption);
            if (!hasOption) continue;

            UpgradeData option = options[i];   // chép ra biến cục bộ cho lambda, tránh lỗi bắt biến vòng lặp
            cards[i].title.text = option.DisplayName;
            cards[i].description.text = option.Description;
            cards[i].button.onClick.RemoveAllListeners();
            cards[i].button.onClick.AddListener(() => onChosen(option));
        }
    }

    public void Hide() => panel.SetActive(false);
}