using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private int baseXpToLevel = 5;
    [SerializeField] private float growthPerLevel = 1.35f;

    private int currentXp;
    private int level = 1;
    private int xpToNext;
    public event System.Action OnXpGained;
    public event Action<int, int> OnXpChanged;  
    public int Level => level;

    private void Awake() => xpToNext = baseXpToLevel;

    private void Start() => OnXpChanged?.Invoke(currentXp, xpToNext);   

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