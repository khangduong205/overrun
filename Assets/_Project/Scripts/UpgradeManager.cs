using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private List<UpgradeData> allUpgrades = new();
    [SerializeField] private UpgradeSelectionUI selectionUI;

    private const int ChoiceCount = 3;
    private readonly List<UpgradeData> choices = new();
    private int pendingLevelUps;   // queue multiple level-ups to ensure no choice is missed

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
        GameStateMachine.Instance.ChangeState(GameState.LevelUp);  
        selectionUI.Show(choices, OnUpgradeChosen);
    }

    private void PickRandomChoices()
    {
        choices.Clear();
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

        if (pendingLevelUps > 0) ShowChoices();   // keep showing if choices remain
        else GameStateMachine.Instance.ChangeState(GameState.Playing);
    }
}