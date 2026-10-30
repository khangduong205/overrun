using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private int baseXpToLevel = 5;
    [SerializeField] private float growthPerLevel = 1.35f;   // mỗi level cần nhiều XP hơn

    private int currentXp;
    private int level = 1;
    private int xpToNext;
    public event System.Action OnXpGained;
    public event Action<int, int> OnXpChanged;   // (xp hiện tại, xp cần)
    public int Level => level;

    private void Awake() => xpToNext = baseXpToLevel;

    private void Start() => OnXpChanged?.Invoke(currentXp, xpToNext);   // cho HUD vẽ giá trị đầu

    public void AddXp(int amount)
    {
        currentXp += amount; 
        OnXpGained?.Invoke();
        while (currentXp >= xpToNext)
        {
            currentXp -= xpToNext;
            level++;
            xpToNext = Mathf.CeilToInt(xpToNext * growthPerLevel);
            GameEvents.RaiseLevelUp(level);
        }
        OnXpChanged?.Invoke(currentXp, xpToNext);
    }
}