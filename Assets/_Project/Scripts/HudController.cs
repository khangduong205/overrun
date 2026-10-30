using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Experience experience;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text killText;

    private int kills;
    private float elapsed;
    private int lastShownSecond = -1;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealth;
        experience.OnXpChanged += UpdateXp;
        GameEvents.LevelUp += UpdateLevel;
        Enemy.Died += HandleEnemyDied;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealth;
        experience.OnXpChanged -= UpdateXp;
        GameEvents.LevelUp -= UpdateLevel;
        Enemy.Died -= HandleEnemyDied;
    }

    private void Start()
    {
        UpdateHealth(playerHealth.Current, playerHealth.Max);
        UpdateLevel(experience.Level);
        killText.text = "Kills: 0";
    }

    private void Update()
    {
        elapsed += Time.deltaTime;   // dùng deltaTime nên tự dừng khi timeScale = 0
        int sec = (int)elapsed;
        if (sec == lastShownSecond) return;   // chỉ cập nhật chữ khi đổi giây, tránh sinh chuỗi mỗi frame
        lastShownSecond = sec;
        timerText.text = $"{sec / 60:00}:{sec % 60:00}";
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

    private void HandleEnemyDied(Enemy _)
    {
        kills++;
        killText.text = $"Kills: {kills}";
    }
}