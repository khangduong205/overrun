using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotUI : MonoBehaviour
{
    [SerializeField] private AbilityController controller;
    [SerializeField] private int slotIndex;
    [SerializeField] private Image icon;
    [SerializeField] private Image cooldownOverlay;   // Image Type = Filled, Radial 360
    [SerializeField] private TMP_Text timerText;

    private AbilityData data;
    private int lastShownSecond = -1;

    private void Start()
    {
        if (slotIndex >= controller.SlotCount)
        {
            gameObject.SetActive(false);
            return;
        }

        data = controller.GetData(slotIndex);
        if (data.Icon != null) icon.sprite = data.Icon;
        timerText.text = "";
    }

    private void Update()
    {
        float remaining = controller.GetCooldownRemaining(slotIndex);
        cooldownOverlay.fillAmount = remaining / data.Cooldown;

        // Chỉ đổi chữ khi đổi giây, tránh sinh chuỗi mới mỗi frame
        int sec = remaining > 0f ? Mathf.CeilToInt(remaining) : 0;
        if (sec == lastShownSecond) return;
        lastShownSecond = sec;
        timerText.text = sec > 0 ? sec.ToString() : "";
    }
}