using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Experience experience;
    [SerializeField] private GameSession session;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text killText;

    private int lastKills = -1;
    private int lastShownSecond = -1;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealth;
        experience.OnXpChanged += UpdateXp;
        GameEvents.LevelUp += UpdateLevel;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealth;
        experience.OnXpChanged -= UpdateXp;
        GameEvents.LevelUp -= UpdateLevel;
    }

    private void Start()
    {
        UpdateHealth(playerHealth.Current, playerHealth.Max);
        UpdateLevel(experience.Level);
    }

    private void Update()
    {
        int sec = (int)session.Elapsed;
        if (sec != lastShownSecond)
        {
            lastShownSecond = sec;
            timerText.text = $"{sec / 60:00}:{sec % 60:00}";
        }

        if (session.Kills != lastKills)
        {
            lastKills = session.Kills;
            killText.text = $"Kills: {lastKills}";
        }
    }

    private void UpdateHealth(float current, float max)
    {
        healthBar.maxValue = max;
        healthBar.value = current;
    }

    private void UpdateXp(int current, int needed)
    {
        xpBar.maxValue = needed;
        xpBar.value = current;
    }

    private void UpdateLevel(int level) => levelText.text = $"Level {level}";
}