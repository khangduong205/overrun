using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private List<UpgradeData> allUpgrades = new();
    [SerializeField] private UpgradeSelectionUI selectionUI;

    private const int ChoiceCount = 3;
    private readonly List<UpgradeData> choices = new();
    private int pendingLevelUps;   // lên 2 cấp liên tiếp thì chọn 2 lần, không bỏ sót

    private void OnEnable() => GameEvents.LevelUp += HandleLevelUp;
    private void OnDisable() => GameEvents.LevelUp -= HandleLevelUp;

    private void HandleLevelUp(int newLevel)
    {
        pendingLevelUps++;
        if (!selectionUI.IsOpen) ShowChoices();
    }

    private void ShowChoices()
    {
        PickRandomChoices();
        Time.timeScale = 0f;   // tạm dừng toàn bộ vật lý và mọi thứ dùng Time.deltaTime
        selectionUI.Show(choices, OnUpgradeChosen);
    }

    private void PickRandomChoices()
    {
        choices.Clear();
        // Xáo trộn kiểu Fisher-Yates trên bản sao rồi lấy 3 cái đầu
        var pool = new List<UpgradeData>(allUpgrades);
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }
        for (int i = 0; i < ChoiceCount && i < pool.Count; i++)
            choices.Add(pool[i]);
    }

    private void OnUpgradeChosen(UpgradeData chosen)
    {
        chosen.Apply(stats);
        pendingLevelUps--;
        selectionUI.Hide();

        if (pendingLevelUps > 0) ShowChoices();   // còn lượt chọn thì hiện tiếp
        else Time.timeScale = 1f;
    }
}